using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NordesteFoodAPI.Modules.Stocks.Application.UseCases;
using NordesteFoodAPI.Modules.Stocks.Domain.DTOs;
using NordesteFoodAPI.Shared.API.Responses;
using NordesteFoodAPI.Shared.Common.Results;

namespace NordesteFoodAPI.Modules.Stocks.API
{
    [Route("[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IncreaseStockUseCase _increaseStockUseCase;
        private readonly DecreaseStockUseCase _decreaseStockUseCase;

        public StockController(IncreaseStockUseCase increaseStockUseCase, DecreaseStockUseCase decreaseStockUseCase)
        {
            _increaseStockUseCase = increaseStockUseCase;
            _decreaseStockUseCase = decreaseStockUseCase;
        }

        [HttpPatch]
        [Authorize]
        [Route("{stockId}/increase")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> IncreaseAsync([FromRoute] Guid stockId, [FromBody] UpdateStockQuantityDTO increaseStockDTO)
        {
            var result = await _increaseStockUseCase.IncreaseAsync(stockId, increaseStockDTO.Quantity);

            if (!result.IsSuccess)
            {
                var statusCodes = result.ErrorType switch
                {
                    ErrorType.NotFound => StatusCodes.Status404NotFound,
                    ErrorType.BussinessError => StatusCodes.Status400BadRequest,
                    ErrorType.DatabaseError or ErrorType.UnexpectedFailure => StatusCodes.Status500InternalServerError,
                    _ => StatusCodes.Status500InternalServerError
                };

                return StatusCode(statusCodes, new ApiResponse
                {
                    Status = statusCodes,
                    Message = result.ErrorMessage ?? $"Ocorreu um erro inesperado ao tentar incrementar o estoque de Id '{stockId}'."
                });
            }

            return NoContent();
        }

        [HttpPatch]
        [Authorize]
        [Route("{stockId}/decrease")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DecreaseAsync([FromRoute] Guid stockId, [FromBody] UpdateStockQuantityDTO decreaseStockDTO)
        {
            var result = await _decreaseStockUseCase.DecreaseAsync(stockId, decreaseStockDTO.Quantity);

            if (!result.IsSuccess)
            {
                var statusCodes = result.ErrorType switch
                {
                    ErrorType.NotFound => StatusCodes.Status404NotFound,
                    ErrorType.BussinessError => StatusCodes.Status400BadRequest,
                    ErrorType.DatabaseError or ErrorType.UnexpectedFailure => StatusCodes.Status500InternalServerError,
                    _ => StatusCodes.Status500InternalServerError
                };

                return StatusCode(statusCodes, new ApiResponse
                {
                    Status = statusCodes,
                    Message = result.ErrorMessage ?? $"Ocorreu um erro inesperado ao tentar decrementar o estoque de Id '{stockId}'."
                });
            }

            return NoContent();
        }
    }
}
