using Bogus;
using Bogus.Extensions.Brazil;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
namespace SalesSystem.IntegrationTests.Configuration
{
    [CollectionDefinition(nameof(IntegrationApiTestsFixtureCollection))]
    public class IntegrationApiTestsFixtureCollection : ICollectionFixture<IntegrationTestsFixture<Program>> { }

    public class IntegrationTestsFixture<TProgram> : IDisposable where TProgram : class
    {
        public readonly SalesAppFactory<TProgram> Factory;
        public HttpClient HttpClient;

        public string UserPassword = string.Empty;
        public string UserEmail = string.Empty;
        public string UserDocument = string.Empty;
        public string UserName = string.Empty;
        public DateTime UserAge;

        public IntegrationTestsFixture()
        {
            var clientOptions = new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = true,
                BaseAddress = new Uri("http://localhost"),
                HandleCookies = true,
                MaxAutomaticRedirections = 7
            };

            Factory = new SalesAppFactory<TProgram>();
            HttpClient = Factory.CreateClient(clientOptions);
        }

        public void GenerateUserData()
        {
            var faker = new Faker();
            UserEmail = faker.Internet.Email().ToLower();
            UserPassword = faker.Internet.Password(8, false, "", "@1Ab_");
            UserDocument = Regex.Replace(faker.Person.Cpf(), "[^0-9]", "");
            UserAge = DateTime.Now.AddYears(-faker.Random.Number(18, 80));
            UserName = faker.Person.FullName.ToLower();
        }

        public StringContent GetContent<T>(T command)
            => new(
                JsonSerializer.Serialize(command),
                Encoding.UTF8,
                "application/json");

        public async Task<Response> GetResponse(HttpResponseMessage response)
            => JsonSerializer.Deserialize<Response>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;

        public void Dispose()
        {
            Factory.Dispose();
            HttpClient.Dispose();
            GC.SuppressFinalize(this);
        }
    }
    public record Response(bool? IsSuccess, List<string?>? Errors, string? Message);
}
