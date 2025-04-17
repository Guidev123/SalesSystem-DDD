using Bogus;
using SalesSystem.Sales.Domain.Entities;
using SalesSystem.SharedKernel.DomainObjects;
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

        [Fact(DisplayName = "Add More Order Items Than Max Quantity Allowed")]
        [Trait("Sales Domain", "Order Tests")]
        public void Order_AddItem_ShouldReturnDomainExceptionIfOrderItemQuantityIsMoreThanQuantityAllowed()
        {
            // Arrange
            var order = GetValidNewDraftOrder();
            var productId = Guid.NewGuid();
            var orderItem = new OrderItem(productId, "Test Product", 1, 100);
            var orderItem2 = new OrderItem(productId, "Test Product", MAX_ITEM_QUANTITY, 100);
            order.AddItem(orderItem);

            // Act & Assert
            Assert.Throws<DomainException>(() => order.AddItem(orderItem2));
        }

        [Fact(DisplayName = "Upate Inexistent Order Item")]
        [Trait("Sales Domain", "Order Tests")]
        public void Order_UpdateItem_ShouldReturnDomainExceptionIfOrderItemDoesNotExists()
        {
            // Arrange
            var order = GetValidNewDraftOrder();
            var orderItemUpdated = GetValidOrderItem();

            // Act & Assert
            Assert.Throws<DomainException>(() => order.UpdateItem(orderItemUpdated));
        }

        [Fact(DisplayName = "Upate Order Item Quantity")]
        [Trait("Sales Domain", "Order Tests")]
        public void Order_UpdateItem_ShouldUpdateQuantityWhenOrderitemIsValid()
        {
            // Arrange
            var order = GetValidNewDraftOrder();
            var productId = Guid.NewGuid();
            var orderItem = new OrderItem(productId, "Test Product", 1, 100);
            order.AddItem(orderItem);
            var orderItemUpdated = new OrderItem(productId, "Test Product", 1, 100);
            var newQuantity = orderItemUpdated.Quantity;

            // Act
            order.UpdateItem(orderItemUpdated);

            // Assert
            Assert.Equal(newQuantity, order.OrderItems?.FirstOrDefault(oi => oi.ProductId == productId)?.Quantity);
        }

        [Fact(DisplayName = "Upate Order Item Quantity And Calculate Total Price")]
        [Trait("Sales Domain", "Order Tests")]
        public void Order_UpdateItem_ShouldUpdatePriceWhenOrderitemIsValid()
        {
            // Arrange
            var order = GetValidNewDraftOrder();
            var productId = Guid.NewGuid();
            var orderItem = new OrderItem(productId, "Test Product", 1, 100);
            order.AddItem(orderItem);
            var orderItemUpdated = new OrderItem(productId, "Test Product", 2, 100);
            var orderPriceBeforeUpdate = order.Price;   

            // Act
            order.UpdateItem(orderItemUpdated);

            // Assert
            Assert.True(order.Price > orderPriceBeforeUpdate);
        }

        [Fact(DisplayName = "Upate Order Item Quantity And Calculate Total Price")]
        [Trait("Sales Domain", "Order Tests")]
        public void Order_UpdateItem_ShouldUpdatePriceWhenDiferentOrderItemsAreAdded()
        {
            // Arrange
            var order = GetValidNewDraftOrder();
            var productId = Guid.NewGuid();
            var orderItemExistent = new OrderItem(Guid.NewGuid(), "Test Product", 2, 100);
            var orderItemExistent2 = new OrderItem(productId, "Test Product", 3, 15);
            order.AddItem(orderItemExistent);
            order.AddItem(orderItemExistent2);

            var updatedOrderItem = new OrderItem(productId, "Test Product", 5, 15);
            var totalPrice = orderItemExistent.Quantity * orderItemExistent.UnitPrice +
                             updatedOrderItem.Quantity * updatedOrderItem.UnitPrice;

            // Act
            order.UpdateItem(updatedOrderItem);

            // Assert
            Assert.Equal(totalPrice, order.Price);
        }

        private OrderItem GetValidOrderItem()
            => new(Guid.NewGuid(), _faker.Commerce.ProductName(), _faker.Random.Int(MIN_ITEM_QUANTITY, MAX_ITEM_QUANTITY), _faker.Random.Decimal(1, 1000));

        private Order GetValidNewDraftOrder()
            => OrderFactory.NewDraftOrder(Guid.NewGuid());
    }
}
