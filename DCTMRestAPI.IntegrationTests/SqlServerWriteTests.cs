using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Xunit;

namespace DCTMRestAPI.IntegrationTests
{
    /// <summary>
    /// Write-path coverage against the real SQL Server DCTrack database. Each test inserts a
    /// uniquely-keyed row and deletes it again in a finally block, leaving the database pristine.
    /// These exercise paths the InMemory provider cannot: real EF inserts and the
    /// SET IDENTITY_INSERT + explicit-transaction path.
    /// </summary>
    public class SqlServerWriteTests : IClassFixture<SqlServerWebApplicationFactory>
    {
        private readonly SqlServerWebApplicationFactory _factory;

        public SqlServerWriteTests(SqlServerWebApplicationFactory factory) => _factory = factory;

        private HttpClient MobileClient()
        {
            var client = _factory.CreateHttpsClient();
            var token = TestJwt.Create(CustomWebApplicationFactory.TestSigningKey, roles: "Mobile");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return client;
        }

        [SkippableFact]
        public async Task CheckOutPurposes_Post_persists_via_ef_insert()
        {
            Skip.IfNot(SqlServerWebApplicationFactory.DatabaseAvailable(), "DCTrack SQL Server not available.");

            var id = Guid.NewGuid();
            var payload = new[]
            {
                new
                {
                    Id = id,
                    CheckoutPurposeId = 987654,
                    CheckoutSessionId = Guid.NewGuid(),
                    TotalCoutItems = 0,
                    CinItems = 0,
                    LastUpdatedTime = 123L,
                },
            };

            try
            {
                var response = await MobileClient().PostAsJsonAsync("/api/CheckOutPurposes", payload);

                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                Assert.DoesNotContain("errormessage", body.ToLowerInvariant());
                Assert.Equal(1, ScalarCount(
                    "SELECT COUNT(*) FROM dbo.tblCheckoutPurpose WHERE Id = @p", ("@p", id)));
            }
            finally
            {
                Execute("DELETE FROM dbo.tblCheckoutPurpose WHERE Id = @p", ("@p", id));
            }
        }

        [SkippableFact]
        public async Task StockTakeSessions_Post_uses_identity_insert_and_transaction()
        {
            Skip.IfNot(SqlServerWebApplicationFactory.DatabaseAvailable(), "DCTrack SQL Server not available.");

            const long stockTakeSessionId = 990001; // well above the current max
            var id = Guid.NewGuid();
            var payload = new[]
            {
                new
                {
                    Id = id,
                    StockTakeSessionId = stockTakeSessionId,
                    StockTakeLocationId = 1,
                    StockTakeUserId = 1,
                    WorkflowInstanceId = "integration-test",
                    StockTakeDateTime = DateTime.UtcNow,
                    DeviceId = (int?)null,
                    LastUpdatedTime = 1L,
                },
            };

            try
            {
                var response = await MobileClient().PostAsJsonAsync("/api/StockTakeSessions", payload);

                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                Assert.DoesNotContain("errormessage", body.ToLowerInvariant());
                Assert.Equal(1, ScalarCount(
                    "SELECT COUNT(*) FROM dbo.tblStockTakeSession WHERE StockTakeSessionId = @p",
                    ("@p", stockTakeSessionId)));
            }
            finally
            {
                Execute("DELETE FROM dbo.tblStockTakeSession WHERE StockTakeSessionId = @p",
                    ("@p", stockTakeSessionId));
            }
        }

        // --- direct-SQL helpers for verification and cleanup (independent of the app) ---

        private static int ScalarCount(string sql, (string Name, object Value) parameter)
        {
            using var conn = new SqlConnection(SqlServerWebApplicationFactory.ConnectionString);
            conn.Open();
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue(parameter.Name, parameter.Value);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        private static void Execute(string sql, (string Name, object Value) parameter)
        {
            using var conn = new SqlConnection(SqlServerWebApplicationFactory.ConnectionString);
            conn.Open();
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue(parameter.Name, parameter.Value);
            cmd.ExecuteNonQuery();
        }
    }
}
