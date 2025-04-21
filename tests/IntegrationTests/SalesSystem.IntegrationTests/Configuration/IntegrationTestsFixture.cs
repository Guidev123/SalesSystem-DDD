using Bogus;
using Bogus.Extensions.Brazil;
using Microsoft.AspNetCore.Mvc.Testing;
using SalesSystem.Registers.Application.Commands.Authentication.SignIn;
using System.Net.Http.Json;
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

        public Guid UserId;
        public string UserPassword = string.Empty;
        public string UserEmail = string.Empty;
        public string UserDocument = string.Empty;
        public string UserName = string.Empty;
        public DateTime UserAge;

        public IntegrationTestsFixture()
        {
            var clientOptions = new WebApplicationFactoryClientOptions
            {
                BaseAddress = new Uri("http://localhost"),
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

        public async Task SignInAsync()
        {
            var response = await HttpClient.PostAsJsonAsync("/api/v1/registers/signin", new SignInUserCommand("teste@teste.com", "Teste@123"));
            response.EnsureSuccessStatusCode();
            var result = await GetResponse<Response<SignInUserResponse>>(response);
            TestsExtensions.SetJsonWebToken(HttpClient, result.Data?.AccessToken ?? string.Empty);
        }

        public async Task<T> GetResponse<T>(HttpResponseMessage response)
            => JsonSerializer.Deserialize<T>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions
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
    public record Response<T>(bool? IsSuccess, List<string?>? Errors, string? Message, T? Data);
}
