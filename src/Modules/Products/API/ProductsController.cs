using Microsoft.AspNetCore.Mvc;
using NordesteFoodAPI.Modules.Products.Application.UseCases;
using NordesteFoodAPI.Modules.Products.Domain.DTOs;
using NordesteFoodAPI.Shared.API.Responses;
using NordesteFoodAPI.Shared.Common.Results;

namespace NordesteFoodAPI.Modules.Products.API
{
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly CreateProductUseCase _createProductUseCase;
        private readonly GetProductByIdUseCase _getProductByIdUseCase;

        public ProductsController(CreateProductUseCase createProductUseCase, GetProductByIdUseCase getProductByIdUseCase)
        {
            _createProductUseCase = createProductUseCase;
            _getProductByIdUseCase = getProductByIdUseCase;
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] CreateProductRequestDTO productRequestDTO)
        {
            var result = await _createProductUseCase.CreateAsync(productRequestDTO);

            if (!result.IsSuccess)
            {
                var statusCodes = result.ErrorType switch
                {
                    ErrorType.CreateConflict => StatusCodes.Status409Conflict,
                    _ => StatusCodes.Status500InternalServerError
                };

                return StatusCode(statusCodes, new ApiResponse
                {
                    Status = statusCodes,
                    Message = result.ErrorMessage!
                });
            }

            return StatusCode(StatusCodes.Status201Created, new ApiResponse<ProductResponseDTO>
            {
                Status = StatusCodes.Status201Created,
                Data = result.Value,
                Message = "Produto criado com sucesso"
            });
        }

        [HttpGet]
        [Route("search/id{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var result = await _getProductByIdUseCase.FindByIdAsync(id);

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
                    Message = result.ErrorMessage ?? $"Erro interno ao buscar pelo produto de Id '{id}'"
                });
            }

            return Ok(new ApiResponse<ProductResponseDTO>
            {
                Status = StatusCodes.Status200OK,
                Data = result.Value,
                Message = $"O produto de Id '{id}' foi encontrado com sucesso"
            });
        }
    }
}
