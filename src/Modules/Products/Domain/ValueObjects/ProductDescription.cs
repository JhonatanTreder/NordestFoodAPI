using NordesteFoodAPI.Modules.Products.Domain.Exceptions;

namespace NordesteFoodAPI.Modules.Products.Domain.ValueObjects
{
    public class ProductDescription
    {
        public string Value { get; private set; } = null!;

        private ProductDescription() { }

        private ProductDescription(string value) => Value = value;

        public static ProductDescription Create(string value)
        {
            if (value.Length > 270)
            {
                throw new ProductsLayerException("A descrição do produto não pode conter mais que 270 caracteres.");
            }

            return new ProductDescription(value);
        }

        public override bool Equals(object? obj) =>
          obj is ProductDescription other && Value == other.Value;

        public override int GetHashCode() => Value.GetHashCode();
    }
}