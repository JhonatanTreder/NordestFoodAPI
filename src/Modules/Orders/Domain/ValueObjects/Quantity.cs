using NordesteFoodAPI.Shared.Exceptions;

namespace NordesteFoodAPI.Modules.Orders.Domain.ValueObjects
{
    public class Quantity
    {
        public int Value { get; private set; }

        private Quantity() { }

        private Quantity(int value) => Value = value;

        public static Quantity Create(int quantity)
        {
            if (quantity <= 0)
                throw new DomainLayerException("A quantidade deve ser maior que 0.");

            return new Quantity(quantity);
        }
    }
}
