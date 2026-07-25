using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NordesteFoodAPI.Modules.Payments.Application.UseCases;
using NordesteFoodAPI.Modules.Payments.Domain.DTOs;
using NordesteFoodAPI.Shared.API.Responses;
using NordesteFoodAPI.Shared.Common.Results;

namespace NordesteFoodAPI.Modules.Payments.API
{
    [Route("[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly CreatePaymentUseCase _createPaymentUseCase;

        public PaymentController(CreatePaymentUseCase createPaymentUseCase)
        {
            _createPaymentUseCase = createPaymentUseCase;
        }

        [HttpPost]
        [Authorize]
        [Route("create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateAsync([FromBody] CreatePaymentRequestDTO createPaymentRequestDTO)
        {
            var result = await _createPaymentUseCase.CreateAsync(createPaymentRequestDTO);

            if (!result.IsSuccess)
            {
                var statusCode = result.ErrorType switch
                {
                    ErrorType.ValidationError => StatusCodes.Status400BadRequest,
                    ErrorType.Conflict => StatusCodes.Status409Conflict,
                    ErrorType.NotFound => StatusCodes.Status404NotFound,
                    ErrorType.DatabaseError => StatusCodes.Status500InternalServerError,
                    _ => StatusCodes.Status500InternalServerError
                };

                return StatusCode(statusCode, new ApiResponse
                {
                    Status = statusCode,
                    Message = result.ErrorMessage ?? "Ocorreu um erro inesperado ao tentar criar o pagamento"
                });
            }

            return StatusCode(StatusCodes.Status201Created, new ApiResponse<PaymentResponseDTO>
            {
                Status = StatusCodes.Status201Created,
                Data = result.Value,
                Message = "O pagamento foi criado com sucesso"
            });
        }
    }
}
