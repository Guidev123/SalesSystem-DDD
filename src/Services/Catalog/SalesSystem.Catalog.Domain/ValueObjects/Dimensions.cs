using SalesSystem.SharedKernel.DomainObjects;

namespace SalesSystem.Catalog.Domain.ValueObjects
{
    public record Dimensions
    {
        public Dimensions(decimal height, decimal width, decimal depth)
        {
            Height = height;
            Width = width;
            Depth = depth;
            Validate();
        }

        public decimal Height { get; }
        public decimal Width { get; }
        public decimal Depth { get; }

        private void Validate()
        {
            AssertionConcern.EnsureGreaterThan(Height, 0, "The 'Height' field must be equal or greater than 1.");
            AssertionConcern.EnsureGreaterThan(Width, 0, "The 'Width' field must be equal or greater than 1.");
            AssertionConcern.EnsureGreaterThan(Depth, 0, "The 'Depth' field must be equal or greater than 1.");
        }

        public string FormatDescription() => $"WxHxD = {Width} x {Height} x {Depth}";
        public override string ToString() => FormatDescription();
    }
}
