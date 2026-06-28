using NordesteFoodAPI.Modules.Products.Domain.Exceptions;
using NordesteFoodAPI.Modules.Restaurants.Domain.ValueObjects;

namespace NordesteFoodAPI.Modules.Products.Domain.ValueObjects
{
    public class ProductName
    {
        public string Value { get; private set; } = null!;

        private ProductName() { }

        private ProductName(string value) => Value = value;

        public static ProductName Create(string productName)
        {
            if (string.IsNullOrEmpty(productName))
            {
                throw new ProductsLayerException("O nome do produto não pode ser nulo ou vazio");
            }

            if (productName.Length < 3 || productName.Length > 100)
            {
                throw new ProductsLayerException("O nome do produto deve conter de 3 a 100 caracteres");
            }

            return new ProductName(productName);
        }

        public override bool Equals(object? obj) =>
          obj is ProductName other && Value == other.Value;

        public override int GetHashCode() => Value.GetHashCode();
    }
}
