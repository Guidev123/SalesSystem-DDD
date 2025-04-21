using SalesSystem.IntegrationTests.Configuration;
using SalesSystem.Registers.Application.Commands.Authentication.SignIn;
using SalesSystem.Registers.Application.Commands.Authentication.SignUp;

namespace SalesSystem.IntegrationTests
{
    [TestCaseOrderer("SalesSystem.IntegrationTests.Configuration.PriorityOrderer", "SalesSystem.IntegrationTests")]
    [Collection(nameof(IntegrationApiTestsFixtureCollection))]
    public class RegistersTests(IntegrationTestsFixture<Program> testsFixture)
    {
        [Fact(DisplayName = "Sign Up With Success"), TestPriority(1)]
        [Trait("Registers", "Integration Tests")]
        public async Task Registers_SignUp_ShouldExecuteWithSuccess()
        {
            // Arrange
            testsFixture.GenerateUserData();
            var command = new SignUpUserCommand(
                testsFixture.UserName, testsFixture.UserDocument,
                testsFixture.UserEmail, testsFixture.UserPassword,
                testsFixture.UserPassword, testsFixture.UserAge
                );

            // Act
            var response = await testsFixture.HttpClient.PostAsync("/api/v1/registers", testsFixture.GetContent(command));

            // Assert
            var result = await testsFixture.GetResponse<Response>(response);
            Assert.True(response.IsSuccessStatusCode);
            Assert.True(result.IsSuccess);
            Assert.Null(result.Errors);
        }

        [Fact(DisplayName = "Sign In With Success"), TestPriority(2)]
        [Trait("Registers", "Integration Tests")]
        public async Task Registers_SignIn_ShouldExecuteWithSuccess()
        {
            // Arrange
            var command = new SignInUserCommand(testsFixture.UserEmail, testsFixture.UserPassword);

            // Act
            var response = await testsFixture.HttpClient.PostAsync("/api/v1/registers/signin", testsFixture.GetContent(command));

            // Assert
            var result = await testsFixture.GetResponse<Response>(response);
            Assert.True(response.IsSuccessStatusCode);
            Assert.True(result.IsSuccess);
            Assert.Empty(result.Errors ?? []);
        }

        [Fact(DisplayName = "Sign Up With Weak Password Should Return Error"), TestPriority(3)]
        [Trait("Registers", "Integration Tests")]
        public async Task Registers_SignUp_ShouldFailWhenPasswordIsWeak()
        {
            // Arrange
            testsFixture.GenerateUserData();
            var command = new SignUpUserCommand(
                testsFixture.UserName, testsFixture.UserDocument,
                testsFixture.UserEmail, "WEAKPASSWORD",
                "WEAKPASSWORD", testsFixture.UserAge
                );

            // Act
            var response = await testsFixture.HttpClient.PostAsync("/api/v1/registers", testsFixture.GetContent(command));

            // Assert
            var result = await testsFixture.GetResponse<Response>(response);
            Assert.False(response.IsSuccessStatusCode);
            Assert.False(result.IsSuccess);
            Assert.NotNull(result.Errors);
            Assert.Equal(3, result.Errors?.Count);
        }
    }
}
