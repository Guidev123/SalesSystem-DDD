using SalesSystem.SharedKernel.DomainObjects;

namespace SalesSystem.Sales.Domain.Entities
{
    public class OrderItem : Entity
    {
        public OrderItem(Guid productId, string productName, int quantity, decimal unitPrice)
        {
            ProductId = productId;
            ProductName = productName;
            Quantity = quantity;
            UnitPrice = unitPrice;
            Validate();
        }
        protected OrderItem() { }   

        public Guid OrderId { get; private set; }
        public Guid ProductId { get; private set; }
        public string ProductName { get; private set; } = string.Empty;
        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }
        public Order? Order { get; private set; }

        internal void AssociateOrder(Guid orderId)
        {
            AssertionConcern.EnsureDifferent(orderId, Guid.Empty, "OrderId cannot be empty.");
            OrderId = orderId;
        }

        public decimal CalculatePrice() => Quantity * UnitPrice;
        internal void AddUnities(int quantity)
        {
            AssertionConcern.EnsureGreaterThan(quantity, 0, "Quantity must be greater than 0.");
            Quantity += quantity;
        }

        internal void UpdateUnities(int quantity)
        {
            AssertionConcern.EnsureGreaterThan(quantity, 0, "Quantity must be greater than 0.");
            Quantity = quantity;
        }

        public override void Validate()
        {
            AssertionConcern.EnsureNotNull(ProductId, "The product ID cannot be null.");
            AssertionConcern.EnsureNotEmpty(ProductName, "The product name cannot be empty.");
            AssertionConcern.EnsureLengthInRange(ProductName, 1, 255, "The product name must be between 1 and 255 characters.");
            AssertionConcern.EnsureGreaterThan(Quantity, 0, "The quantity must be greater than zero.");
            AssertionConcern.EnsureGreaterThan(UnitPrice, 0, "The unit price must be greater than zero.");
        }
    }
}
