using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using DCTMRestAPI.Types;
using Microsoft.Data.SqlClient;
using Xunit;

namespace DCTMRestAPI.IntegrationTests
{
    /// <summary>
    /// End-to-end auth against the real DCTrack database, covering the security migration:
    ///  - Phase 3: password sent as plaintext over TLS (X-Password-Encoding: plain).
    ///  - Phase 2: a legacy fixed-salt hash still authenticates and is upgraded to v2 on login.
    /// Seeds and removes its own throwaway user.
    /// </summary>
    public class SqlServerAuthTests : IClassFixture<SqlServerWebApplicationFactory>
    {
        private readonly SqlServerWebApplicationFactory _factory;

        public SqlServerAuthTests(SqlServerWebApplicationFactory factory) => _factory = factory;

        [SkippableFact]
        public async Task Login_with_legacy_hash_succeeds_and_rehashes_to_v2()
        {
            Skip.IfNot(SqlServerWebApplicationFactory.DatabaseAvailable(), "DCTrack SQL Server not available.");

            var login = "pwtest_" + Guid.NewGuid().ToString("N").Substring(0, 8);
            const string password = "TestPass123";
            var legacyHash = LegacyHash(password);

            Execute(
                "INSERT INTO dbo.tblUser (LoginName, FirstName, Password, CreatedBy, Status) VALUES (@l, 'Test', @p, 1, 1)",
                ("@l", login), ("@p", legacyHash));

            try
            {
                var client = _factory.CreateHttpsClient();
                // Phase 3: plaintext over TLS is the default (no encoding header needed).
                var response = await client.PostAsJsonAsync(
                    "/api/auth/Token", new { UserName = login, Password = password, DeviceID = "" });

                // Phase 2: legacy hash verified -> login succeeds.
                var body = await response.Content.ReadAsStringAsync();
                Assert.True(response.IsSuccessStatusCode, $"login -> {(int)response.StatusCode}: {body}");
                Assert.Contains("token", body.ToLowerInvariant());

                // Phase 2: rehash-on-login upgraded the stored hash to v2.
                var stored = ScalarString("SELECT Password FROM dbo.tblUser WHERE LoginName = @l", ("@l", login));
                Assert.StartsWith("v2:", stored);
            }
            finally
            {
                Execute("DELETE FROM dbo.tblUser WHERE LoginName = @l", ("@l", login));
            }
        }

        [SkippableFact]
        public async Task Login_with_wrong_password_is_rejected()
        {
            Skip.IfNot(SqlServerWebApplicationFactory.DatabaseAvailable(), "DCTrack SQL Server not available.");

            var login = "pwtest_" + Guid.NewGuid().ToString("N").Substring(0, 8);
            Execute(
                "INSERT INTO dbo.tblUser (LoginName, FirstName, Password, CreatedBy, Status) VALUES (@l, 'Test', @p, 1, 1)",
                ("@l", login), ("@p", LegacyHash("TestPass123")));
            try
            {
                var client = _factory.CreateHttpsClient();
                var response = await client.PostAsJsonAsync(
                    "/api/auth/Token", new { UserName = login, Password = "WrongPass", DeviceID = "" });

                Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
            }
            finally
            {
                Execute("DELETE FROM dbo.tblUser WHERE LoginName = @l", ("@l", login));
            }
        }

        [SkippableFact]
        public async Task Login_with_aes_encrypted_password_works_via_opt_in_header()
        {
            Skip.IfNot(SqlServerWebApplicationFactory.DatabaseAvailable(), "DCTrack SQL Server not available.");

            var login = "pwtest_" + Guid.NewGuid().ToString("N").Substring(0, 8);
            const string password = "TestPass123";
            Execute(
                "INSERT INTO dbo.tblUser (LoginName, FirstName, Password, CreatedBy, Status) VALUES (@l, 'Test', @p, 1, 1)",
                ("@l", login), ("@p", LegacyHash(password)));
            try
            {
                var client = _factory.CreateHttpsClient();
                // Legacy client: AES-encrypt the password and opt in with the header.
                var encrypted = CryptographyUtil.Encrypt(password);
                using var request = new HttpRequestMessage(HttpMethod.Post, "/api/auth/Token")
                {
                    Content = JsonContent.Create(new { UserName = login, Password = encrypted, DeviceID = "" }),
                };
                request.Headers.Add("X-Password-Encoding", "aes");

                var response = await client.SendAsync(request);

                var body = await response.Content.ReadAsStringAsync();
                Assert.True(response.IsSuccessStatusCode, $"login -> {(int)response.StatusCode}: {body}");
                Assert.Contains("token", body.ToLowerInvariant());
            }
            finally
            {
                Execute("DELETE FROM dbo.tblUser WHERE LoginName = @l", ("@l", login));
            }
        }

        private static string LegacyHash(string password)
        {
            byte[] salt = BitConverter.GetBytes(652);
            if (BitConverter.IsLittleEndian) Array.Reverse(salt);
            return CryptographyUtil.ComputeHash(password, "SHA256", salt);
        }

        private static string ScalarString(string sql, (string Name, object Value) parameter)
        {
            using var conn = new SqlConnection(SqlServerWebApplicationFactory.ConnectionString);
            conn.Open();
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue(parameter.Name, parameter.Value);
            var result = cmd.ExecuteScalar();
            return result == null || result is DBNull ? null : result.ToString();
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
