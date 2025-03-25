namespace SalesSystem.Payments.Business.Models
{
    public class Product
    {
        public Product(string name, int quantity, decimal value)
        {
            Name = name;
            Quantity = quantity;
            Value = value;
        }

        public string Name { get; private set; } = string.Empty;
        public int Quantity { get; private set; }
        public decimal Value { get; private set; }
    }
}
