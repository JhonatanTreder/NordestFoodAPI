using NordesteFoodAPI.Shared.Exceptions;

namespace NordesteFoodAPI.Modules.Stocks.Domain.ValueObjects
{
    public class Quantity
    {
        public int Value { get; private set; }

        private Quantity() { }

        private Quantity(int value) => Value = value;

        public static Quantity Create(int value)
        {
            if (value < 0)
            {
                throw new DomainLayerException("A quantidade do estoque não pode ser menor que 0.");
            }

            return new Quantity(value);
        }
    }
}
