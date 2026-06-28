using NordesteFoodAPI.Modules.UnitProducts.Domain.Exceptions;

namespace NordesteFoodAPI.Modules.UnitProducts.Domain.ValueObjects
{
    public sealed class UnitPrice
    {
        public decimal Value { get; private set; } = default;

        private UnitPrice() { }

        private UnitPrice(decimal value) => Value = value;

        public static UnitPrice Create(decimal unitPrice)
        {
            if (unitPrice <= 0)
            {
                throw new UnitProductsLayerException("O preço da unidade do produto não pode ser menor ou igual a zero");
            }

            return new UnitPrice(unitPrice);
        }

        public override bool Equals(object? obj) =>
          obj is UnitPrice other && Value == other.Value;

        public override int GetHashCode() => Value.GetHashCode();
    }
}
