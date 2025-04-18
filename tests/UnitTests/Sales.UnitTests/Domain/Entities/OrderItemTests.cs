using SalesSystem.Sales.Domain.Entities;
using SalesSystem.SharedKernel.DomainObjects;

namespace Sales.UnitTests.Domain.Entities
{
    public class OrderItemTests
    {
        [Fact(DisplayName = "Order Item Validations")]
        [Trait("Sales Domain", "Order Item Tests")]
        public void OrderItem_Validate_ValidationShouldThrowExceptions()
        {
            Assert.Throws<DomainException>(() =>
            {
                new OrderItem(Guid.NewGuid(), "Test", Order.MAX_ITEM_QUANTITY + 1, 12);
            });

            Assert.Throws<DomainException>(() =>
            {
                new OrderItem(Guid.NewGuid(), "Test", Order.MIN_ITEM_QUANTITY - 1, 12);
            });

            Assert.Throws<DomainException>(() =>
            {
                new OrderItem(Guid.NewGuid(), "Test", Order.MIN_ITEM_QUANTITY, 0);
            });

            Assert.Throws<DomainException>(() =>
            {
                new OrderItem(Guid.Empty, "Test", Order.MIN_ITEM_QUANTITY, 0);
            });

            Assert.Throws<DomainException>(() =>
            {
                new OrderItem(Guid.NewGuid(), string.Empty, Order.MIN_ITEM_QUANTITY, 0);
            });
        }
    }
}
