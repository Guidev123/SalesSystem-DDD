using Bogus;
using SalesSystem.Sales.Application.Commands.Orders.AddOrderItem;
using Xunit;

namespace Sales.UnitTests.Application.Commands.AddOrderItem
{
    public class AddOrderItemCommandTests
    {
        private readonly Faker _faker = new();

        [Fact(DisplayName = "Valid Add Order Item Command")]
        [Trait("Sales Application", "Commands Tests")]
        public void AddOrderItemCommand_IsValid_WhenCommandIsValidMustPassValidation()
        {
            // Arrange
            var command = CreateValidAddOrderItemCommand(Guid.NewGuid());

            // Act
            var result = command.IsValid();

            // Assert
            Assert.True(result);
        }

        [Fact(DisplayName = "Invalid Add Order Item Command")]
        [Trait("Sales Application", "Commands Tests")]
        public void AddOrderItemCommand_IsValid_WhenCommandIsInvalidMustReturnFalse()
        {
            // Arrange
            var command = CreateInvalidAddOrderItemCommand();

            // Act
            var result = command.IsValid();

            // Assert
            Assert.False(result);
            Assert.Equal(5, command.GetErrorMessages().Count);
        }

        [Fact(DisplayName = "Add Order Item Command Without CustomerId")]
        [Trait("Sales Application", "Commands Tests")]
        public void AddOrderItemCommand_IsValid_WhenCommandIsWithoutCustomerIdMustReturnFalse()
        {
            // Arrange
            var command = CreateAddOrderItemCommandWithoutCustomerId();

            // Act
            var result = command.IsValid();

            // Assert
            Assert.False(result);
        }

        private AddOrderItemCommand CreateValidAddOrderItemCommand(Guid customerId)
        {
            var command = new AddOrderItemCommand(Guid.NewGuid(), _faker.Commerce.ProductName(), _faker.Random.Int(1, 10), _faker.Random.Decimal(1, 100));
            command.SetCustomerId(customerId);
            return command;
        }

        private AddOrderItemCommand CreateAddOrderItemCommandWithoutCustomerId()
            => new(Guid.NewGuid(), _faker.Commerce.ProductName(), _faker.Random.Int(1, 10), _faker.Random.Decimal(1, 100));

        private AddOrderItemCommand CreateInvalidAddOrderItemCommand()
        {
            var command = new AddOrderItemCommand(Guid.Empty, string.Empty, 16, 0);
            command.SetCustomerId(Guid.Empty);
            return command;
        }
    }
}
