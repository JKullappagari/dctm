using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xunit;

namespace DCTMRestAPI.IntegrationTests
{
    public class SmokeTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;

        public SmokeTests(CustomWebApplicationFactory factory) => _factory = factory;

        [Fact]
        public async Task Swagger_document_is_served()
        {
            var client = _factory.CreateHttpsClient();

            var response = await client.GetAsync("/swagger/v1.2/swagger.json");

            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();
            Assert.Contains("\"openapi\"", body);
            Assert.Contains("/api/Countries", body);
        }

        [Fact]
        public async Task Protected_endpoint_without_token_returns_401()
        {
            var client = _factory.CreateHttpsClient();

            var response = await client.GetAsync("/api/Countries");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Protected_endpoint_with_invalid_token_returns_401()
        {
            var client = _factory.CreateHttpsClient();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", "not-a-real-token");

            var response = await client.GetAsync("/api/Countries");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
