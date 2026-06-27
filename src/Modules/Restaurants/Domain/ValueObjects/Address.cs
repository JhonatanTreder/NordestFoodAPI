using NordesteFoodAPI.Modules.Restaurants.Domain.Exceptions;

namespace NordesteFoodAPI.Modules.Restaurants.Domain.ValueObjects
{
    public sealed class Address
    {
        public string Value { get; private set; } = null!;
        private Address() { }
        private Address(string value) => Value = value;

        public static Address Create(string address)
        {
            if (string.IsNullOrEmpty(address))
            {
                throw new RestaurantLayerException("O endereço não pode ser nulo ou vazio.");
            }

            if (address.Length > 250)
            {
                throw new RestaurantLayerException("O endereço não pode conter mais que 250 caracteres.");
            }

            return new Address(address);
        }

        public override bool Equals(object? obj) =>
            obj is Address other && Value == other.Value;

        public override int GetHashCode() => Value.GetHashCode();
    }
}
