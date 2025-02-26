using SalesSystem.Catalog.Domain.Entities;
using SalesSystem.Catalog.Domain.ValueObjects;
using SalesSystem.SharedKernel.DomainObjects;

namespace SalesSystem.Catalog.Domain.Tests
{
    public class ProductTests
    {
        [Fact]
        public void Product_Validate_ValidationShouldThrowExceptions()
        {
            var ex = Assert.Throws<DomainException>(() =>
            {
                new Product(string.Empty, "Description", 100, "ImageUrl", Guid.NewGuid(), new Dimensions(1, 1, 1));
            });

            Assert.Equal("The field 'Name' cannot be empty. Please provide a valid product name.", ex.Message);

            ex = Assert.Throws<DomainException>(() =>
            {
                new Product("Name", string.Empty, 100, "ImageUrl", Guid.NewGuid(), new Dimensions(1, 1, 1));
            });

            Assert.Equal("The field 'Description' cannot be empty. Please provide a valid product description.", ex.Message);

            ex = Assert.Throws<DomainException>(() =>
            {
                new Product("Name", "Description", 0, "ImageUrl", Guid.NewGuid(), new Dimensions(1, 1, 1));
            });

            Assert.Equal("The field 'Price' must be greater than $0. Please provide a valid price.", ex.Message);

            ex = Assert.Throws<DomainException>(() =>
            {
                new Product("Name", "Description", 1, string.Empty, Guid.NewGuid(), new Dimensions(1, 1, 1));
            });

            Assert.Equal("The field 'ImageUrl' cannot be empty. Please provide a valid image URL.", ex.Message);

            ex = Assert.Throws<DomainException>(() =>
            {
                new Product("Name", "Description", 1, "Description", Guid.Empty, new Dimensions(1, 1, 1));
            });

            Assert.Equal("The field 'CategoryId' cannot be empty. Please provide a valid category ID.", ex.Message);
        }

        [Fact]
        public void Dimensions_Validate_ValidationShouldThrowExceptions()
        {
            var ex = Assert.Throws<DomainException>(() =>
            {
                new Dimensions(0, 1, 1);
            });

            Assert.Equal("The 'Height' field must be equal or greater than 1.", ex.Message);

            ex = Assert.Throws<DomainException>(() =>
            {
                new Dimensions(1, 0, 1);
            });

            Assert.Equal("The 'Width' field must be equal or greater than 1.", ex.Message);

            ex = Assert.Throws<DomainException>(() =>
            {
                new Dimensions(1, 1, 0);
            });

            Assert.Equal("The 'Depth' field must be equal or greater than 1.", ex.Message);
        }
    }
}
