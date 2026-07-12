using System;
using System.Collections.Generic;
using System.Linq;
using DCTMRestAPI.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DCTMRestAPI.IntegrationTests
{
    /// <summary>
    /// Boots the real application in-memory (TestServer), but swaps the SQL Server
    /// <see cref="DCTrackContext"/> for the EF Core InMemory provider and supplies a
    /// test JWT signing key so authentication can be exercised.
    ///
    /// NOTE: the InMemory provider does not translate SQL, run stored procedures, or
    /// support SET IDENTITY_INSERT / transactions. Endpoints that rely on those
    /// (raw-SQL / stored-proc write paths) cannot be covered here and require a run
    /// against a real SQL Server instance.
    /// </summary>
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        // Test-only signing key (>= 32 bytes). Not a secret; never used in production.
        public const string TestSigningKey =
            "integration-test-signing-key-do-not-use-in-production-0123456789";

        // Shared root so the seeding context and the app's contexts see the same store.
        private static readonly InMemoryDatabaseRoot DbRoot = new InMemoryDatabaseRoot();

        static CustomWebApplicationFactory()
        {
            // The app reads Jwt:SigningKey eagerly during ConfigureServices (before the host is
            // built), so it must be present when WebApplication.CreateBuilder runs. Environment
            // variables are read eagerly by CreateBuilder, unlike WAF's build-time config overrides.
            Environment.SetEnvironmentVariable("Jwt__SigningKey", TestSigningKey);
            Environment.SetEnvironmentVariable("ConnectionStrings__DCTrackDatabase", "data source=(test);");
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            // "Testing" (not Development) so user secrets are NOT loaded and the test signing key
            // above is the single authoritative source.
            builder.UseEnvironment("Testing");

            builder.ConfigureTestServices(services =>
            {
                RemoveDbContextRegistrations(services);

                services.AddDbContext<DCTrackContext>(options =>
                    options.UseInMemoryDatabase("DCTMTestDb", DbRoot));

                using var provider = services.BuildServiceProvider();
                using var scope = provider.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<DCTrackContext>();
                db.Database.EnsureCreated();
                Seed(db);
            });
        }

        private static void RemoveDbContextRegistrations(IServiceCollection services)
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
        }

        private static void Seed(DCTrackContext db)
        {
            if (!db.TblCountry.Any())
            {
                db.TblCountry.AddRange(
                    new TblCountry { CountryId = 1, CountryName = "Testland", LastUpdatedTime = 100 },
                    new TblCountry { CountryId = 2, CountryName = "Sampleistan", LastUpdatedTime = 200 });
                db.SaveChanges();
            }

            if (!db.TblCheckoutPurpose.Any())
            {
                db.TblCheckoutPurpose.Add(new TblCheckoutPurpose
                {
                    Id = Guid.NewGuid(),
                    CheckoutPurposeId = 42,
                    CheckoutSessionId = Guid.NewGuid(),
                    LastUpdatedTime = 500,
                });
                db.SaveChanges();
            }
        }

        /// <summary>Client whose requests look like HTTPS so the HTTPS-redirect rewriter and RequireHttps filter pass.</summary>
        public System.Net.Http.HttpClient CreateHttpsClient() =>
            CreateClient(new WebApplicationFactoryClientOptions
            {
                BaseAddress = new Uri("https://localhost"),
                AllowAutoRedirect = false,
            });
    }
}
