using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NordesteFoodAPI.Modules.Orders.Application.UseCases;
using NordesteFoodAPI.Modules.Orders.Domain.DTOs.Order;
using NordesteFoodAPI.Shared.API.Responses;
using NordesteFoodAPI.Shared.Common.Results;
using System.Security.Claims;

namespace NordesteFoodAPI.Modules.Orders.API
{
    [Route("[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly CreateOrderUseCase _createOrderUseCase;
        private readonly GetOrderByIdUseCase _getOrderByIdUseCase;

        public OrderController(CreateOrderUseCase createOrderUseCase, GetOrderByIdUseCase getOrderByIdUseCase)
        {
            _createOrderUseCase = createOrderUseCase;
            _getOrderByIdUseCase = getOrderByIdUseCase;
        }

        [HttpPost]
        [Authorize]
        [Route("create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateAsync([FromBody] CreateOrderRequestDTO createOrderDTO)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var result = await _createOrderUseCase.CreateAsync(createOrderDTO, userId);

            if (!result.IsSuccess)
            {
                var statusCodes = result.ErrorType switch 
                {
                    ErrorType.ValidationError => StatusCodes.Status400BadRequest,
                    ErrorType.NotFound => StatusCodes.Status404NotFound,
                    ErrorType.Conflict => StatusCodes.Status409Conflict,
                    ErrorType.UnexpectedFailure => StatusCodes.Status500InternalServerError,
                    _ => StatusCodes.Status500InternalServerError
                };

                return StatusCode(statusCodes, new ApiResponse
                {
                    Status = statusCodes,
                    Message = result.ErrorMessage ?? "Ocorreu um erro inesperado ao tentar criar o pedido"
                });

            }

            return StatusCode(StatusCodes.Status201Created, new ApiResponse<OrderResponseDTO>
            {
                Status = StatusCodes.Status201Created,
                Data = result.Value,
                Message = "O pedido foi criado com sucesso"
            });
        }

        [HttpGet]
        [Authorize]
        [Route("search/id/{orderId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> FindByIdAsync(Guid orderId)
        {
            var result = await _getOrderByIdUseCase.GetAsync(orderId);

            if (!result.IsSuccess)
            {
                var statusCode = result.ErrorType switch 
                {
                    ErrorType.NotFound => StatusCodes.Status404NotFound,
                    ErrorType.DatabaseError => StatusCodes.Status500InternalServerError,
                    _ => StatusCodes.Status500InternalServerError
                };

                return StatusCode(statusCode, new ApiResponse
                {
                    Status = statusCode,
                    Message = result.ErrorMessage ?? $"Ocorreu um erro inesperado ao tentar buscar pelo pedido de Id '{orderId}'"
                });
            }

            return Ok(new ApiResponse<OrderResponseDTO>
            {
                Status = StatusCodes.Status200OK,
                Data = result.Value,
                Message = $"O pedido de Id '{orderId}' foi encontrado com sucesso"
            });
        }
    }
}
