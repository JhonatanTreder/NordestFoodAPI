using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NordesteFoodAPI.Modules.Feedbacks.Application.UseCases;
using NordesteFoodAPI.Modules.Feedbacks.Domain.DTOs;
using NordesteFoodAPI.Shared.API.Responses;
using NordesteFoodAPI.Shared.Common.Results;

namespace NordesteFoodAPI.Modules.Feedbacks.API
{
    [Route("[controller]")]
    [ApiController]
    public class FeedbacksController : ControllerBase
    {
        private readonly CreateFeedbackUseCase _createFeedbackUseCase;

        public FeedbacksController(CreateFeedbackUseCase createFeedbackUseCase)
        {
            _createFeedbackUseCase = createFeedbackUseCase;
        }

        [HttpPost]
        [Authorize]
        [Route("create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateAsync([FromBody] CreateFeedbackRequestDTO createFeedbackRequestDTO)
        {
            var result = await _createFeedbackUseCase.CreateAsync(createFeedbackRequestDTO);

            if (!result.IsSuccess)
            {
                var statusCode = result.ErrorType switch
                {
                    ErrorType.CreateConflict => StatusCodes.Status409Conflict,
                    ErrorType.NotFound => StatusCodes.Status404NotFound,
                    ErrorType.ValidationError => StatusCodes.Status400BadRequest,
                    ErrorType.DatabaseError => StatusCodes.Status500InternalServerError,
                    _ => StatusCodes.Status500InternalServerError
                };

                return StatusCode(statusCode, new ApiResponse
                {
                    Status = statusCode,
                    Message = result.ErrorMessage ?? $"Ocorreu um erro inesperado ao tentar criar o feedback para o pedido de Id '{createFeedbackRequestDTO.OrderId}'"
                });
            }

            return StatusCode(StatusCodes.Status201Created, new ApiResponse<FeedbackResponseDTO>
            {
                Status = StatusCodes.Status201Created,
                Data = result.Value,
                Message = $"O feedback foi criado com sucesso para o pedido de Id '{createFeedbackRequestDTO.OrderId}'"
            });
        }
    }
}
