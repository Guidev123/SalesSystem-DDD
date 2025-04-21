using System.Net.Http.Headers;

namespace SalesSystem.IntegrationTests.Configuration
{
    public static class TestsExtensions
    {
        public static void SetJsonWebToken(this HttpClient client, string jwt)
            => client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
    }
}
