using Microsoft.AspNetCore.Mvc;
using NordesteFoodAPI.Modules.Products.Application.UseCases;
using NordesteFoodAPI.Modules.Products.Domain.DTOs;
using NordesteFoodAPI.Shared.API.Responses;

namespace NordesteFoodAPI.Modules.Products.API
{
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly CreateProductUseCase _createProductUseCase;

        public ProductsController(CreateProductUseCase createProductUseCase)
        {
            _createProductUseCase = createProductUseCase;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] CreateProductRequestDTO productRequestDTO)
        {
            var result = await _createProductUseCase.CreateAsync(productRequestDTO);

            return StatusCode(StatusCodes.Status201Created, new ApiResponse<ProductResponseDTO>
            {
                Status = StatusCodes.Status201Created,
                Data = result.Value,
                Message = "Produto criado com sucesso"
            });
        }
    }
}
