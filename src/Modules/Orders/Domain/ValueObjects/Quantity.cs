namespace NordesteFoodAPI.Modules.Orders.Domain.ValueObjects
{
    public class Quantity
    {
        public int Value { get; private set; }

        private Quantity() { }

        private Quantity(int value) => Value = value;

        public static Quantity Create(int quanitty)
        {


            return new Quantity(quanitty);
        }
    }
}
