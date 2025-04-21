using SalesSystem.IntegrationTests.Configuration;
using SalesSystem.Sales.Application.Commands.Orders.AddOrderItem;

namespace SalesSystem.IntegrationTests
{
    [TestCaseOrderer("SalesSystem.IntegrationTests.Configuration.PriorityOrderer", "SalesSystem.IntegrationTests")]
    [Collection(nameof(IntegrationApiTestsFixtureCollection))]
    public class SalesTests(IntegrationTestsFixture<Program> testsFixture)
    {
        [Fact(DisplayName = "Add Order Item to new Order Should Return With Success"), TestPriority(1)]
        [Trait("Sales", "Integration Tests")]
        public async Task Sales_AddOrderItem_ShouldReturnSuccess()
        {
            // Arrange
            var productId = new Guid("62a718f3-4525-495d-a4a6-a4ae66af60c3");
            var command = new AddOrderItemCommand(
                productId, "Monitor 27 4K", 2, 2399);

            await testsFixture.SignInAsync();

            // Act
            var response = await testsFixture.HttpClient.PostAsync("/api/v1/sales/cart/item", testsFixture.GetContent(command));

            // Assert
            var result = await testsFixture.GetResponse<Response>(response);
            Assert.True(response.IsSuccessStatusCode);
            Assert.True(result.IsSuccess);
            Assert.Null(result.Errors);
        }

        [Fact(DisplayName = "Remove Order Item From Existent Order Should Return With Success"), TestPriority(2)]
        [Trait("Sales", "Integration Tests")]
        public async Task Sales_RemoveOrderItem_ShouldReturnSuccess()
        {
            // Arrange
            var productId = new Guid("62a718f3-4525-495d-a4a6-a4ae66af60c3");

            await testsFixture.SignInAsync();

            // Act
            var response = await testsFixture.HttpClient.DeleteAsync($"/api/v1/sales/cart/item/{productId}");

            // Assert
            Assert.True(response.IsSuccessStatusCode);
        }
    }
}
