using Bogus;
using SalesSystem.Sales.Domain.Entities;
using SalesSystem.Sales.Domain.Enums;
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

        [Fact(DisplayName = "Remove Inexistent Order Item")]
        [Trait("Sales Domain", "Order Tests")]
        public void Order_RemoveItem_ShouldThrowDomainExceptionIfOrderItemDoesNotExists()
        {
            // Arrange
            var order = GetValidNewDraftOrder();
            var orderItem = GetValidOrderItem();

            // Act & Assert
            Assert.Throws<DomainException>(() => order.RemoveItem(orderItem));
        }

        [Fact(DisplayName = "Remove Order Item Should Update Order Price")]
        [Trait("Sales Domain", "Order Tests")]
        public void Order_RemoveItem_ShouldUpdateOrderPrice()
        {
            // Arrange
            var order = GetValidNewDraftOrder();
            var orderItem = GetValidOrderItem();
            var orderItem2 = GetValidOrderItem();

            order.AddItem(orderItem);
            order.AddItem(orderItem2);

            var totalPrice = orderItem.CalculatePrice() + orderItem2.CalculatePrice();

            // Act
            order.RemoveItem(orderItem);

            // Assert
            Assert.NotEqual(totalPrice, order.Price);
        }

        [Fact(DisplayName = "Apply Voucher To Order Should be Successfully When Voucher Is Valid")]
        [Trait("Sales Domain", "Order Tests")]
        public void Order_ApplyVoucher_ShouldReturnWithouErrorsWhenVoucherIsValid()
        {
            // Arrange
            var order = GetValidNewDraftOrder();
            var voucher = GetValidValueVoucher();

            order.AddItem(GetValidOrderItem());

            // Act
            var result = order.ApplyVoucher(voucher);

            // Assert
            Assert.True(result.IsValid);
            Assert.True(order.VoucherIsUsed);
        }

        [Fact(DisplayName = "Apply Value Voucher Discount Type")]
        [Trait("Sales Domain", "Order Tests")]
        public void Order_ApplyVoucher_ShouldUpdatePriceWhenValidValueVoucherIsApplied()
        {
            // Arrange
            var order = GetValidNewDraftOrder();

            var orderItem1 = GetValidOrderItem();
            var orderItem2 = GetValidOrderItem();

            var voucher = GetValidValueVoucher();

            order.AddItem(orderItem1);
            order.AddItem(orderItem2);

            var priceWithDiscount = order.Price - voucher.Value;

            // Act
            order.ApplyVoucher(voucher);

            // Assert
            Assert.Equal(priceWithDiscount, order.Price);
            Assert.Equal(voucher.Value, order.Discount);
        }

        [Fact(DisplayName = "Apply Percentual Voucher Discount Type")]
        [Trait("Sales Domain", "Order Tests")]
        public void Order_ApplyVoucher_ShouldUpdatePriceWhenValidPercentualVoucherIsApplied()
        {
            // Arrange
            var order = GetValidNewDraftOrder();

            var orderItem1 = GetValidOrderItem();
            var orderItem2 = GetValidOrderItem();

            var voucher = GetValidPercentualVoucher();

            order.AddItem(orderItem1);
            order.AddItem(orderItem2);

            var discount = (order.Price * voucher.Percentual) / 100;
            var priceWithDiscount = order.Price - discount;

            // Act
            order.ApplyVoucher(voucher);

            // Assert
            Assert.Equal(priceWithDiscount, order.Price);
            Assert.Equal(discount, order.Discount);
        }

        [Fact(DisplayName = "When Voucher Discount Is Greater Than Order Price")]
        [Trait("Sales Domain", "Order Tests")]
        public void Order_ApplyVoucher_WhenVoucherDiscountIsGreaterThanOrderPriceMustResetOrderPrice()
        {
            // Arrange
            var order = GetValidNewDraftOrder();

            var orderItem = new OrderItem(Guid.NewGuid(), "Product Test", 1, 100);

            var voucher = GetValidValueVoucher();

            order.AddItem(orderItem);

            // Act
            order.ApplyVoucher(voucher);

            // Assert
            Assert.Equal(0, order.Price);
        }

        [Fact(DisplayName = "Apply Price Recap on Order Modification")]
        [Trait("Sales Domain", "Order Tests")]
        public void Order_ApplyVoucher_ApplyPriceRecapOnOrderModification()
        {
            // Arrange
            var order = GetValidNewDraftOrder();

            var orderItem1 = GetValidOrderItem();
            order.AddItem(orderItem1);

            var voucher = GetValidValueVoucher();
            order.ApplyVoucher(voucher);

            var orderItem2 = GetValidOrderItem();
            
            // Act
            order.AddItem(orderItem2);

            // Assert
            var price = order.OrderItems.Sum(oi => oi.CalculatePrice()) - voucher.Value;
            Assert.Equal(price, order.Price);
        }

        private OrderItem GetValidOrderItem()
            => new(Guid.NewGuid(), _faker.Commerce.ProductName(), _faker.Random.Int(MIN_ITEM_QUANTITY, MAX_ITEM_QUANTITY), _faker.Random.Decimal(1, 1000));

        private Voucher GetValidValueVoucher()
            => new(Guid.NewGuid().ToString("N"), null, 150, 100, EVoucherType.Value, DateTime.Now.AddDays(1));

        private Voucher GetValidPercentualVoucher()
            => new(Guid.NewGuid().ToString("N"), 50, null, 100, EVoucherType.Percentual, DateTime.Now.AddDays(1));

        private Order GetValidNewDraftOrder()
            => OrderFactory.NewDraftOrder(Guid.NewGuid());
    }
}
