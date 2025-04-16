using Bogus;
using SalesSystem.Sales.Domain.Entities;
using static SalesSystem.Sales.Domain.Entities.Order;

namespace Sales.UnitTests.Domain.Entities
{
    public class OrderTests
    {
        private readonly Faker _faker = new();

        [Fact(DisplayName = "Add Item to New Order")]
        [Trait("Sales Domain", "Order Tests")]
        public void Order_AddItem_ShouldUpdateOrderPriceWhenProductIsAdd()
        {
            // Arrange
            var order = GetValidNewDraftOrder();
            var orderItem = GetValidOrderItem();

            // Act
            order.AddItem(orderItem);

            // Assert
            Assert.Equal(orderItem.Quantity * orderItem.UnitPrice, order.Price);
        }

        [Fact(DisplayName = "Add Existent Item to Existent Order")]
        [Trait("Sales Domain", "Order Tests")]
        public void Order_AddItem_ShouldIncrementPriceWhenOrderItemAlreadyExists()
        {
            // Arrange
            var order = GetValidNewDraftOrder();
            var productId = Guid.NewGuid();
            var orderItem = new OrderItem(productId, "Test Product", 2, 100);
            var orderItem2 = new OrderItem(productId, "Test Product", 1, 100);
            order.AddItem(orderItem);

            // Act
            order.AddItem(orderItem2);

            // Assert
            Assert.Equal(300, order.Price);
            Assert.Single(order.OrderItems);
            Assert.Equal(3, order.OrderItems?.FirstOrDefault(oi => oi.ProductId == productId)?.Quantity);
        }

        private OrderItem GetValidOrderItem()
            => new(Guid.NewGuid(), _faker.Commerce.ProductName(), _faker.Random.Int(1, 10), _faker.Random.Decimal(1, 1000));

        private Order GetValidNewDraftOrder()
            => OrderFactory.NewDraftOrder(Guid.NewGuid());
    }
}
