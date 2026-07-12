using System;
using System.Linq;
using DCTMRestAPI.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DCTMRestAPI.IntegrationTests
{
    /// <summary>
    /// Boots the app against the REAL restored SQL Server DCTrack database, so tests can exercise
    /// what the InMemory provider cannot: SQL translation of LINQ, stored procedures (FromSqlRaw),
    /// and real data. Connection string is overridable via the DCTM_TEST_SQL environment variable.
    /// </summary>
    public class SqlServerWebApplicationFactory : WebApplicationFactory<Program>
    {
        public static readonly string ConnectionString =
            Environment.GetEnvironmentVariable("DCTM_TEST_SQL")
            ?? @"Server=.\SQLEXPRESS;Database=DCTrack;Trusted_Connection=True;TrustServerCertificate=True;Encrypt=False;";

        static SqlServerWebApplicationFactory()
        {
            // Same eager-read reasoning as CustomWebApplicationFactory: the app resolves the JWT
            // signing key during ConfigureServices, before the host is built.
            Environment.SetEnvironmentVariable("Jwt__SigningKey", CustomWebApplicationFactory.TestSigningKey);
            // Enable rehash-on-login so the auth test can observe the legacy -> v2 upgrade.
            Environment.SetEnvironmentVariable("Security__UpgradePasswordHashesOnLogin", "true");
        }

        /// <summary>True if the restored DCTrack database is reachable (used to skip tests otherwise).</summary>
        public static bool DatabaseAvailable()
        {
            try
            {
                var csb = new SqlConnectionStringBuilder(ConnectionString) { ConnectTimeout = 3 };
                using var conn = new SqlConnection(csb.ConnectionString);
                conn.Open();
                return true;
            }
            catch
            {
                return false;
            }
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");

            builder.ConfigureTestServices(services =>
            {
                var toRemove = services.Where(d =>
                    d.ServiceType == typeof(DbContextOptions<DCTrackContext>) ||
                    d.ServiceType == typeof(DbContextOptions) ||
                    d.ServiceType == typeof(DCTrackContext) ||
                    (d.ServiceType.IsGenericType &&
                     d.ServiceType.GetGenericTypeDefinition().Name.Contains("IDbContextOptionsConfiguration") &&
                     d.ServiceType.GenericTypeArguments.Contains(typeof(DCTrackContext))))
                    .ToList();
                foreach (var descriptor in toRemove)
                    services.Remove(descriptor);

                services.AddDbContext<DCTrackContext>(options => options.UseSqlServer(ConnectionString));
            });
        }

        public System.Net.Http.HttpClient CreateHttpsClient() =>
            CreateClient(new WebApplicationFactoryClientOptions
            {
                BaseAddress = new Uri("https://localhost"),
                AllowAutoRedirect = false,
            });
    }
}
