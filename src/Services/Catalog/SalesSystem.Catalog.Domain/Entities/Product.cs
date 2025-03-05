using SalesSystem.Catalog.Domain.ValueObjects;
using SalesSystem.SharedKernel.DomainObjects;

namespace SalesSystem.Catalog.Domain.Entities
{
    public class Product : Entity, IAggregateRoot
    {
        public Product(string name, string description, decimal price, string imageUrl, Guid categoryId, Dimensions dimensions)
        {
            Name = name;
            Description = description;
            Price = price;
            ImageUrl = imageUrl;
            CategoryId = categoryId;
            Dimensions = dimensions;
            CreatedAt = DateTime.Now;
            Active = true;
            Validate();
        }

        protected Product() { }

        public string Name { get; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public string ImageUrl { get; private set; } = string.Empty;
        public bool Active { get; private set; }
        public decimal Price { get; private set; }
        public int QuantityInStock { get; private set; }
        public Dimensions Dimensions { get; private set; } = null!;
        public DateTime CreatedAt { get; }
        public Guid CategoryId { get; private set; }
        public Category Category { get; private set; } = null!;

        public void Activate() => Active = true;

        public void Deactivate() => Active = false;

        public void UpdateCategory(Category category)
        {
            AssertionConcern.EnsureDifferent(category.Id, Guid.Empty, "The field 'CategoryId' cannot be empty. Please provide a valid category ID.");
            Category = category;
            CategoryId = category.Id;
        }

        public void UpdateDescription(string description)
        {
            AssertionConcern.EnsureNotEmpty(description, "The field 'Description' cannot be empty. Please provide a valid product description.");
            Description = description;
        }

        public void UpdatePrice(decimal price)
        {
            AssertionConcern.EnsureGreaterThan(Price, 0, "The field 'Price' must be greater than $0. Please provide a valid price.");
            Price = price;
        }

        public void UpdateImage(string imageUrl)
        {
            AssertionConcern.EnsureNotEmpty(ImageUrl, "The field 'ImageUrl' cannot be empty. Please provide a valid image URL.");
            ImageUrl = imageUrl;
        }

        public void DebitStock(int quantity)
        {
            if (quantity < 0) quantity *= -1;
            AssertionConcern.EnsureTrue(HasStock(quantity), "Insufficient stock");
            QuantityInStock -= quantity;
        }

        public void AddStock(int quantity)
        {
            AssertionConcern.EnsureGreaterThan(quantity, 0, "The 'Quantity' must be greater than 0");
            QuantityInStock += quantity;
        }

        public bool HasStock(int quantity) => QuantityInStock >= quantity;  

        public override void Validate()
        {
            AssertionConcern.EnsureNotEmpty(Name, "The field 'Name' cannot be empty. Please provide a valid product name.");
            AssertionConcern.EnsureNotEmpty(Description, "The field 'Description' cannot be empty. Please provide a valid product description.");
            AssertionConcern.EnsureDifferent(CategoryId, Guid.Empty, "The field 'CategoryId' cannot be empty. Please provide a valid category ID.");
            AssertionConcern.EnsureGreaterThan(Price, 0, "The field 'Price' must be greater than $0. Please provide a valid price.");
            AssertionConcern.EnsureNotEmpty(ImageUrl, "The field 'ImageUrl' cannot be empty. Please provide a valid image URL.");
            AssertionConcern.EnsureTrue(Active, "The product must be active. Please ensure the product is marked as active.");
        }
    }
}
