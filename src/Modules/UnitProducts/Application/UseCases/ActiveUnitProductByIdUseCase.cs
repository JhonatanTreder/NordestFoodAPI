using NordesteFoodAPI.Modules.UnitProducts.Domain.Contracts;
using NordesteFoodAPI.Modules.UnitProducts.Domain.DTOs;
using NordesteFoodAPI.Shared.Common.Results;

namespace NordesteFoodAPI.Modules.UnitProducts.Application.UseCases
{
    public class ActiveUnitProductByIdUseCase
    {
        private readonly IUnitProductRepository _unitProductRepsitory;

        public ActiveUnitProductByIdUseCase(IUnitProductRepository unitProductRepsitory)
        {
            _unitProductRepsitory = unitProductRepsitory;
        }

        public async Task<Result> ActiveAsync(Guid unitProductId)
        {
            var result = await _unitProductRepsitory.ActiveUnitProductByIdAsync(unitProductId);

            if (!result.IsSuccess)
            {
                return Result<UnitProductResponseDTO>.Failure(
                    result.ErrorMessage ?? $"Ocorreu um ero ao tentar ativar o produto de Id '{unitProductId}' de um restaurante",
                    result.ErrorType
                );
            }

            return Result.Success();
        }
    }
}
