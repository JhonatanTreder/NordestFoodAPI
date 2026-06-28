using NordesteFoodAPI.Modules.Products.Domain.Exceptions;

namespace NordesteFoodAPI.Modules.Products.Domain.ValueObjects
{
    public sealed class Price
    {
        public decimal Value { get; }

        private Price() { }

        private Price(decimal value) => Value = value;

        public static Price Create(decimal value)
        {
            if (value <= 0)
                throw new ProductsLayerException("O preço unitário deve ser maior que zero.");

            return new Price(value);
        }

        public override bool Equals(object? obj) =>
            obj is Price other && Value == other.Value;

        public override int GetHashCode() => Value.GetHashCode();
    }
}
