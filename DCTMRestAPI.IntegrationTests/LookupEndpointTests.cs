using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xunit;

namespace DCTMRestAPI.IntegrationTests
{
    /// <summary>
    /// Exercises the async / AsNoTracking read paths end-to-end: route -> auth -> async EF read -> JSON.
    /// </summary>
    public class LookupEndpointTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;

        public LookupEndpointTests(CustomWebApplicationFactory factory) => _factory = factory;

        private System.Net.Http.HttpClient AuthedClient()
        {
            var client = _factory.CreateHttpsClient();
            var token = TestJwt.Create(CustomWebApplicationFactory.TestSigningKey);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return client;
        }

        [Fact]
        public async Task GetAll_returns_seeded_rows()
        {
            var client = AuthedClient();

            var response = await client.GetAsync("/api/Countries");

            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();
            Assert.Contains("Testland", body);
            Assert.Contains("Sampleistan", body);
        }

        [Fact]
        public async Task GetById_returns_only_matching_row()
        {
            var client = AuthedClient();

            var response = await client.GetAsync("/api/Countries/1");

            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();
            Assert.Contains("Testland", body);
            Assert.DoesNotContain("Sampleistan", body);
        }

        [Fact]
        public async Task GetUpdatedSince_filters_by_last_updated_time()
        {
            var client = AuthedClient();

            // Only rows with LastUpdatedTime > 150 => "Sampleistan" (200), not "Testland" (100).
            var response = await client.GetAsync("/api/Countries/updated/150");

            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();
            Assert.Contains("Sampleistan", body);
            Assert.DoesNotContain("Testland", body);
        }

        [Fact]
        public async Task WriteControllerGet_returns_seeded_rows()
        {
            // Exercises a converted GET on a write controller (CheckOutPurposes).
            var client = AuthedClient();

            var response = await client.GetAsync("/api/CheckOutPurposes");

            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();
            Assert.Contains("checkoutpurposeid", body.ToLowerInvariant());
            Assert.Contains("42", body);
        }
    }
}
