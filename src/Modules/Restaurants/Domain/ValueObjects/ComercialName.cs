using NordesteFoodAPI.Modules.Restaurants.Domain.Exceptions;
using System.Security.AccessControl;

namespace NordesteFoodAPI.Modules.Restaurants.Domain.ValueObjects
{
    public sealed class ComercialName
    {
        public string Value { get; private set; } = null!;

        private ComercialName() { }

        private ComercialName(string value) => Value = value;

        public static ComercialName Create(string comercialName)
        {
            //Criar regra para verificar duplicidade entre as unidades de um restaurante, ou seja, não pode existir duas unidades com o mesmo nome comercial.

            if (comercialName.Length < 8 || comercialName.Length > 35)
            {
                throw new RestaurantLayerException("O nome do restaurante deve conter de 8 a 35 caracteres.");
            }

            if (string.IsNullOrEmpty(comercialName))
            {
                throw new RestaurantLayerException("O nome do restaurante não pode ser nulo ou vazio.");
            }

            return new ComercialName(comercialName);
        }

        public override bool Equals(object? obj) =>
            obj is ComercialName other && Value == other.Value;

        public override int GetHashCode() => Value.GetHashCode();
    }
}
