using SalesSystem.SharedKernel.DomainObjects;

namespace SalesSystem.Catalog.Domain.Entities
{
    public class Product : Entity, IAggregateRoot
    {
        public Product(string name, string description, decimal price, string imageUrl, Guid categoryId)
        {
            Name = name;
            Description = description;
            Price = price;
            ImageUrl = imageUrl;
            CategoryId = categoryId;
            CreatedAt = DateTime.Now;
            Active = true;
        }
        protected Product() { }

        public string Name { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public string ImageUrl { get; private set; } = string.Empty;
        public bool Active { get; private set; }
        public decimal Price { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public int QuantityInStock { get; private set; }
        public Guid CategoryId { get; private set; }
        public Category Category { get; private set; } = null!;

        public void Activate() => Active = true;

        public void Deactivate() => Active = false;

        public void UpdateCategory(Category category)
        {
            Category = category;
            CategoryId = category.Id;
        }

        public void UpdateDescription(string description)
        {
            Description = description;
        }

        public void DebitStock(int quantity)
        {
            if (quantity < 0) quantity *= -1;
            QuantityInStock -= quantity;
        }

        public void AddStock(int quantity)
        {
            QuantityInStock += quantity;
        }

        public bool HasStock(int quantity) => QuantityInStock >= quantity;  

        public void Validate()
        {

        }
    }
}
