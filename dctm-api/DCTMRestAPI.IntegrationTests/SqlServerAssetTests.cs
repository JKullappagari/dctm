using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Xunit;

namespace DCTMRestAPI.IntegrationTests
{
    /// <summary>
    /// Asset endpoints exercised against the real DCTrack asset data, including the
    /// stored-procedure update path (iAssetTrack_Sp_Asset_UpdateNew) which the InMemory
    /// provider cannot run. The update test captures the original value and restores it.
    /// </summary>
    public class SqlServerAssetTests : IClassFixture<SqlServerWebApplicationFactory>
    {
        // Seed values known to exist in the restored backup.
        private const int AssetId = 1;
        private const string RealUser = "Admin";

        private readonly SqlServerWebApplicationFactory _factory;

        public SqlServerAssetTests(SqlServerWebApplicationFactory factory) => _factory = factory;

        private HttpClient Client(string username = "tester")
        {
            var client = _factory.CreateHttpsClient();
            var token = TestJwt.Create(CustomWebApplicationFactory.TestSigningKey, username: username);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return client;
        }

        [SkippableFact]
        public async Task GetById_returns_real_asset()
        {
            Skip.IfNot(SqlServerWebApplicationFactory.DatabaseAvailable(), "DCTrack SQL Server not available.");

            var response = await Client().GetAsync($"/api/assets/{AssetId}");

            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();
            Assert.StartsWith("[", body.TrimStart());
            Assert.Contains("\"assetId\":1", body.Replace(" ", ""), StringComparison.OrdinalIgnoreCase);
        }

        [SkippableFact]
        public async Task GetPaged_returns_page_and_pagination_header()
        {
            Skip.IfNot(SqlServerWebApplicationFactory.DatabaseAvailable(), "DCTrack SQL Server not available.");

            var response = await Client().GetAsync("/api/assets/pages?PageNumber=1&PageSize=5");

            response.EnsureSuccessStatusCode();
            Assert.True(response.Headers.Contains("X-Pagination"), "X-Pagination header missing");
            var body = await response.Content.ReadAsStringAsync();
            Assert.Contains("totalItems", body, StringComparison.OrdinalIgnoreCase);
        }

        [SkippableFact]
        public async Task Patch_runs_asset_update_stored_procedure()
        {
            // Drives the full stored-procedure update path: auth -> AutoMapper (TblAsset->AssetDTO)
            // -> 35-parameter command -> raw ADO.NET ExecuteReader -> output params. Success (200)
            // proves the proc ran and returned @pIntResult != -1; the bumped LastModifiedDate proves
            // it actually updated the row. (The proc only writes ExternalId on insert, not update,
            // so LastModifiedDate is the reliable signal for an existing asset.)
            Skip.IfNot(SqlServerWebApplicationFactory.DatabaseAvailable(), "DCTrack SQL Server not available.");

            var origExternalId = ScalarString("SELECT ExternalId FROM dbo.tblAsset WHERE AssetId = @id", AssetId);
            var modifiedBefore = ScalarDateTime("SELECT LastModifiedDate FROM dbo.tblAsset WHERE AssetId = @id", AssetId);
            var newValue = "ITEST-" + Guid.NewGuid().ToString("N").Substring(0, 12);

            try
            {
                var patch = $"[{{\"op\":\"replace\",\"path\":\"/externalid\",\"value\":\"{newValue}\"}}]";
                using var request = new HttpRequestMessage(HttpMethod.Patch, $"/api/assets/{AssetId}")
                {
                    Content = new StringContent(patch, Encoding.UTF8, "application/json-patch+json"),
                };

                var response = await Client(RealUser).SendAsync(request);

                var responseBody = await response.Content.ReadAsStringAsync();
                Assert.True(response.IsSuccessStatusCode,
                    $"PATCH returned {(int)response.StatusCode}: {responseBody}");

                var modifiedAfter = ScalarDateTime("SELECT LastModifiedDate FROM dbo.tblAsset WHERE AssetId = @id", AssetId);
                Assert.True(modifiedAfter > modifiedBefore,
                    $"proc did not update the row (LastModifiedDate before={modifiedBefore:o}, after={modifiedAfter:o})");
            }
            finally
            {
                // ExternalId is untouched by the update path, but restore defensively.
                Execute("UPDATE dbo.tblAsset SET ExternalId = @v WHERE AssetId = @id",
                    ("@v", (object)origExternalId ?? DBNull.Value), ("@id", AssetId));
            }
        }

        // --- direct-SQL helpers ---

        private static string ScalarString(string sql, int id)
        {
            using var conn = new SqlConnection(SqlServerWebApplicationFactory.ConnectionString);
            conn.Open();
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);
            var result = cmd.ExecuteScalar();
            return result == null || result is DBNull ? null : result.ToString();
        }

        private static DateTime ScalarDateTime(string sql, int id)
        {
            using var conn = new SqlConnection(SqlServerWebApplicationFactory.ConnectionString);
            conn.Open();
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);
            var result = cmd.ExecuteScalar();
            return result == null || result is DBNull ? DateTime.MinValue : Convert.ToDateTime(result);
        }

        private static void Execute(string sql, params (string Name, object Value)[] parameters)
        {
            using var conn = new SqlConnection(SqlServerWebApplicationFactory.ConnectionString);
            conn.Open();
            using var cmd = new SqlCommand(sql, conn);
            foreach (var (name, value) in parameters)
                cmd.Parameters.AddWithValue(name, value);
            cmd.ExecuteNonQuery();
        }
    }
}
