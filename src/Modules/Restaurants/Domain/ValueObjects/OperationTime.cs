using NordesteFoodAPI.Modules.Restaurants.Domain.Exceptions;

namespace NordesteFoodAPI.Modules.Restaurants.Domain.ValueObjects
{
    public sealed class OperationTime
    {
        public TimeOnly OpeningTime { get; private set; }
        public TimeOnly ClosingTime { get; private set; }

        private OperationTime() { }

        private OperationTime(TimeOnly opensAt, TimeOnly closesAt)
        {
            OpeningTime = opensAt;
            ClosingTime = closesAt;
        }

        public static OperationTime Create(TimeOnly opensAt, TimeOnly closesAt)
        {
            if (opensAt >= closesAt)
            {
                throw new RestaurantLayerException("O horário de abertura não pode ser maior que o horário do fechamento.");
            }

            return new OperationTime(opensAt, closesAt);
        }
    }
}
