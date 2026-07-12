using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xunit;

namespace DCTMRestAPI.IntegrationTests
{
    /// <summary>
    /// Runs against the restored SQL Server DCTrack database. Skipped automatically when the
    /// database is not reachable (e.g. CI without the restore). These cover what the InMemory
    /// suite cannot: real SQL translation and stored-procedure execution.
    /// </summary>
    public class SqlServerReadTests : IClassFixture<SqlServerWebApplicationFactory>
    {
        private readonly SqlServerWebApplicationFactory _factory;

        public SqlServerReadTests(SqlServerWebApplicationFactory factory) => _factory = factory;

        private System.Net.Http.HttpClient AuthedClient()
        {
            var client = _factory.CreateHttpsClient();
            var token = TestJwt.Create(CustomWebApplicationFactory.TestSigningKey);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return client;
        }

        [SkippableFact]
        public async Task Countries_returns_real_data()
        {
            Skip.IfNot(SqlServerWebApplicationFactory.DatabaseAvailable(), "DCTrack SQL Server not available.");
            var client = AuthedClient();

            var response = await client.GetAsync("/api/Countries");

            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();
            Assert.StartsWith("[", body.TrimStart());
            Assert.Contains("CountryName", body, System.StringComparison.OrdinalIgnoreCase);
        }

        [SkippableFact]
        public async Task Hosts_by_id_translates_ToLower_rewrite_to_sql()
        {
            // Exercises 'where g.HostId.ToString().ToLower() == HostId.ToString().ToLower()'.
            // On EF Core 10 an untranslatable predicate throws (500); a 2xx proves it translated.
            Skip.IfNot(SqlServerWebApplicationFactory.DatabaseAvailable(), "DCTrack SQL Server not available.");
            var client = AuthedClient();

            var response = await client.GetAsync("/api/Hosts/1");

            response.EnsureSuccessStatusCode();
        }

        [SkippableFact]
        public async Task Assets_stored_procedure_endpoint_executes()
        {
            // GET /api/assets calls stored proc iAssetTrack_sp_AssetExcel_Data_API via FromSqlRaw.
            Skip.IfNot(SqlServerWebApplicationFactory.DatabaseAvailable(), "DCTrack SQL Server not available.");
            var client = AuthedClient();

            var response = await client.GetAsync("/api/assets");

            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();
            Assert.StartsWith("[", body.TrimStart());
        }
    }
}
