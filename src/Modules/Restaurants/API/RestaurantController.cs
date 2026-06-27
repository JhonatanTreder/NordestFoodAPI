using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NordesteFoodAPI.Modules.Restaurants.Application.UseCases;
using NordesteFoodAPI.Modules.Restaurants.Domain.DTOs;
using NordesteFoodAPI.Shared.API.Responses;
using NordesteFoodAPI.Shared.Infraestructure.Results;

namespace NordesteFoodAPI.Modules.Restaurants.API
{
    [Route("[controller]")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {
        private readonly CreateRestaurantUseCase _createRestaurantUseCase;

        public RestaurantController(CreateRestaurantUseCase createRestaurantUseCase)
        {
            _createRestaurantUseCase = createRestaurantUseCase;
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateRestaurantAsync([FromBody] CreateRestaurantRequestDTO createRestaurantDTO)
        {
            var result = await _createRestaurantUseCase.Create(createRestaurantDTO);

            if (!result.IsSuccess)
            {
                var statusCodes = result.ErrorType switch
                {
                    ErrorType.CreateConflict => StatusCodes.Status409Conflict,
                    ErrorType.ValidationError => StatusCodes.Status400BadRequest,
                    _ => StatusCodes.Status500InternalServerError,
                };

                return StatusCode(statusCodes, new ApiResponse
                {
                    Status = statusCodes,
                    Message = result.ErrorMessage!
                });
            }

            return StatusCode(StatusCodes.Status201Created, new ApiResponse<CreateRestaurantResponseDTO?>
            {
                Status = StatusCodes.Status201Created,
                Data = result.Value,
                Message = "Restaurante criado com sucesso"
            });
        }
    }
}
