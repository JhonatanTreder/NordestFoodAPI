using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NordesteFoodAPI.Modules.UnitProducts.Application.UseCases;
using NordesteFoodAPI.Modules.UnitProducts.Domain.DTOs;
using NordesteFoodAPI.Shared.API.Responses;
using NordesteFoodAPI.Shared.Common.Results;

namespace NordesteFoodAPI.Modules.UnitProducts.API
{
    [Route("[controller]")]
    [ApiController]
    public class UnitProductsController : ControllerBase
    {
        private readonly CreateUnitProductUseCase _createUnitProductUseCase;
        private readonly GetRestaurantMenuUseCase _getrestaurantMenuUseCase;

        public UnitProductsController(CreateUnitProductUseCase createUnitProductUseCase, GetRestaurantMenuUseCase getrestaurantMenuUseCase)
        {
            _createUnitProductUseCase = createUnitProductUseCase;
            _getrestaurantMenuUseCase = getrestaurantMenuUseCase;
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateAsync(CreateUnitProductRequestDTO unitProductRequestDTO)
        {
            var result = await _createUnitProductUseCase.CreateAsync(unitProductRequestDTO);

            if (!result.IsSuccess)
            {
                var statusCode = result.ErrorType switch
                {
                    ErrorType.NotFound => StatusCodes.Status404NotFound,
                    _ => StatusCodes.Status500InternalServerError
                };

                return StatusCode(statusCode, new ApiResponse
                {
                    Status = statusCode,
                    Message = result.ErrorMessage ?? "Ocorreu um erro inesperado ao tentar criar um produto de uma unidade"
                });
            }

            return StatusCode(StatusCodes.Status201Created, new ApiResponse<UnitProductResponseDTO>
            {
                Status = StatusCodes.Status201Created,
                Data = result.Value,
                Message = "O produto da unidade foi criado com sucesso"
            });
        }

        [HttpGet]
        [Route("restaurant/{restaurantId}/menu")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMenuAsync(Guid restaurantId)
        {
            var result = await _getrestaurantMenuUseCase.GetMenuAsync(restaurantId);

            if (!result.IsSuccess)
            {
                var statusCode = result.ErrorType switch
                {
                    ErrorType.NotFound => StatusCodes.Status404NotFound,
                    _ => StatusCodes.Status500InternalServerError
                };

                return StatusCode(statusCode, new ApiResponse
                {
                    Status = statusCode,
                    Message = result.ErrorMessage ?? $"Ocorreu um erro inesperado ao buscar pelo menu do restaurante de Id '{restaurantId}'"
                });
            }

            return StatusCode(StatusCodes.Status201Created, new ApiResponse<IEnumerable<UnitProductResponseDTO>>
            {
                Status = StatusCodes.Status201Created,
                Data = result.Value,
                Message = "O menu foi encontrado com sucesso"
            });
        }
    }
}
