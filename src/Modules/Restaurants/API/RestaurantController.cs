using Microsoft.AspNetCore.Mvc;
using NordesteFoodAPI.Modules.Restaurants.Application.UseCases;
using NordesteFoodAPI.Modules.Restaurants.Domain.DTOs;
using NordesteFoodAPI.Shared.API.Responses;
using NordesteFoodAPI.Shared.Common.Results;

namespace NordesteFoodAPI.Modules.Restaurants.API
{
    [Route("[controller]")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {
        private readonly CreateRestaurantUseCase _createRestaurantUseCase;
        private readonly GetRestaurantByIdUseCase _getRestaurantByIdUseCase;
        private readonly GetRestaurantByNameUseCase _getRestaurantByNameUseCase;
        private readonly GetAllRestaurantsUseCase _getAllRestaurantsUseCase;

        public RestaurantController(
            CreateRestaurantUseCase createRestaurantUseCase,
            GetRestaurantByIdUseCase getRestaurantByIdUseCase,
            GetRestaurantByNameUseCase getRestaurantByNameUseCase,
            GetAllRestaurantsUseCase getAllRestaurantsUseCase
        )
        {
            _createRestaurantUseCase = createRestaurantUseCase;
            _getRestaurantByIdUseCase = getRestaurantByIdUseCase;
            _getRestaurantByNameUseCase = getRestaurantByNameUseCase;
            _getAllRestaurantsUseCase = getAllRestaurantsUseCase;
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateRestaurantAsync([FromBody] CreateRestaurantRequestDTO createRestaurantDTO)
        {
            var result = await _createRestaurantUseCase.CreateAsync(createRestaurantDTO);

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

        [HttpGet]
        [Route("search/comercial-name/{comercialName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetByName(string comercialName)
        {
            var result = await _getRestaurantByNameUseCase.Get(comercialName);

            if (!result.IsSuccess)
            {
                var statusCodes = result.ErrorType switch
                {
                    ErrorType.NotFound => StatusCodes.Status404NotFound,
                    _ => StatusCodes.Status500InternalServerError,
                };

                return StatusCode(statusCodes, new ApiResponse
                {
                    Status = statusCodes,
                    Message = result.ErrorMessage!
                });
            }

            return StatusCode(StatusCodes.Status200OK, new ApiResponse<RestaurantResponseDTO?>
            {
                Status = StatusCodes.Status200OK,
                Data = result.Value,
                Message = "Restaurante encontrado com sucesso"
            });
        }

        [HttpGet]
        [Route("search/id/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _getRestaurantByIdUseCase.FindByIdAsync(id);

            if (!result.IsSuccess)
            {
                var statusCodes = result.ErrorType switch
                {
                    ErrorType.NotFound => StatusCodes.Status404NotFound,
                    _ => StatusCodes.Status500InternalServerError,
                };

                return StatusCode(statusCodes, new ApiResponse
                {
                    Status = statusCodes,
                    Message = result.ErrorMessage!
                });
            }

            return StatusCode(StatusCodes.Status200OK, new ApiResponse<RestaurantResponseDTO?>
            {
                Status = StatusCodes.Status200OK,
                Data = result.Value,
                Message = "Restaurante encontrado com sucesso"
            });
        }

        [HttpGet]
        [Route("search/all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetByName()
        {
            var result = await _getAllRestaurantsUseCase.GetAll();

            if (!result.IsSuccess)
            {
                var statusCodes = result.ErrorType switch
                {
                    ErrorType.NotFound => StatusCodes.Status404NotFound,
                    _ => StatusCodes.Status500InternalServerError,
                };

                return StatusCode(statusCodes, new ApiResponse
                {
                    Status = statusCodes,
                    Message = result.ErrorMessage!
                });
            }

            return StatusCode(StatusCodes.Status200OK, new ApiResponse<IEnumerable<RestaurantResponseDTO?>>
            {
                Status = StatusCodes.Status200OK,
                Data = result.Value,
                Message = "Restaurantes encontrados com sucesso"
            });
        }
    }
}
