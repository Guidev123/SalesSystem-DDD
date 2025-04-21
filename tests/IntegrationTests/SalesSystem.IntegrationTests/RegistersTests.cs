using SalesSystem.IntegrationTests.Configuration;
using SalesSystem.Registers.Application.Commands.Authentication.SignIn;
using SalesSystem.Registers.Application.Commands.Authentication.SignUp;
using SalesSystem.Registers.Application.Commands.Customers.AddAddress;

namespace SalesSystem.IntegrationTests
{
    [TestCaseOrderer("SalesSystem.IntegrationTests.Configuration.PriorityOrderer", "SalesSystem.IntegrationTests")]
    [Collection(nameof(IntegrationApiTestsFixtureCollection))]
    public class RegistersTests(IntegrationTestsFixture<Program> testsFixture)
    {
        private static string CURRENT_JWT = string.Empty;

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
            var result = await testsFixture.GetResponse<Response<SignUpUserResponse>>(response);
            SetUserId(result.Data?.Id ?? Guid.Empty);
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
            var result = await testsFixture.GetResponse<Response<SignInUserResponse>>(response);
            SetCurrentJWT(result.Data?.AccessToken ?? string.Empty);
            Assert.True(response.IsSuccessStatusCode);
            Assert.True(result.IsSuccess);
            Assert.Empty(result.Errors ?? []);
        }

        [Fact(DisplayName = "Should Add Address to Customer With Success"), TestPriority(4)]
        [Trait("Registers", "Integration Tests")]
        public async Task Registers_AddAddress_ShouldAddAddressToCustomerWithSuccess()
        {
            // Arrange
            var command = new AddAddressCommand(
                "123 Maple Street",
                "456",
                "Apartment 12B",
                "Downtown",
                "12345-678",
                "Springfield",
                "IL"
            );

            TestsExtensions.SetJsonWebToken(testsFixture.HttpClient, CURRENT_JWT);

            // Act
            var response = await testsFixture.HttpClient.PostAsync($"/api/v1/registers/address", testsFixture.GetContent(command));

            // Assert
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact(DisplayName = "Should Delete User With Success"), TestPriority(500)]
        [Trait("Registers", "Integration Tests")]
        public async Task Registers_DeleteUser_ShouldExecuteWithSuccess()
        {
            // Arrange
            var userId = testsFixture.UserId;

            // Act
            var response = await testsFixture.HttpClient.DeleteAsync($"/api/v1/registers/{userId}");

            // Assert
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact(DisplayName = "Sign Up With Weak Password Should Return Error")]
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

        #region Private Methods

        private static void SetCurrentJWT(string jwt)
            => CURRENT_JWT = jwt;

        private void SetUserId(Guid userId)
            => testsFixture.UserId = userId;

        #endregion
    }
}
