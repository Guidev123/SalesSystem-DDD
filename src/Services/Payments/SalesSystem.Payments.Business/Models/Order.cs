namespace SalesSystem.Payments.Business.Models
{
    public class Order
    {
        public Order(decimal value, string customerEmail, string orderCode, Guid customerId)
        {
            Id = Guid.NewGuid();
            Value = value;
            CustomerEmail = customerEmail;
            OrderCode = orderCode;
            CustomerId = customerId;
        }

        public Guid Id { get; private set; }
        public decimal Value { get; private set; }
        public string CustomerEmail { get; private set; } = string.Empty;
        public string OrderCode { get; private set; } = string.Empty;
        public Guid CustomerId { get; private set; }
        public List<Product> Products { get; private set; } = [];
    }
}
