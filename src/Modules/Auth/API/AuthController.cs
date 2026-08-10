using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NordesteFoodAPI.Modules.Auth.Application.UseCases;
using NordesteFoodAPI.Modules.Auth.Domain.DTOs.Login;
using NordesteFoodAPI.Modules.Auth.Domain.DTOs.Register;
using NordesteFoodAPI.Shared.API.Responses;
using NordesteFoodAPI.Shared.Common.Results;
using System.Threading.Tasks;

namespace NordesteFoodAPI.Modules.Auth.API
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly LoginUseCase _loginUseCase;
        private readonly RegisterUseCase _registerUseCase;

        public AuthController(LoginUseCase loginUseCase, RegisterUseCase registerUseCase)
        {
            _loginUseCase = loginUseCase;
            _registerUseCase = registerUseCase;
        }

        [HttpPost]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequest)
        {
            var loginResponse = await _loginUseCase.Login(loginRequest);

            if (!loginResponse.IsSuccess)
            {
                var statusCode = loginResponse.ErrorType switch
                {
                    ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
                    ErrorType.UnexpectedFailure => StatusCodes.Status500InternalServerError,
                    _ => StatusCodes.Status500InternalServerError
                };

                return StatusCode(statusCode, new ApiResponse
                {
                    Status = statusCode,
                    Message = loginResponse.ErrorMessage ?? "Ocorreu um erro inesperado ao tentar realizar o login do usuário"
                });
            }

            return StatusCode(StatusCodes.Status200OK, new ApiResponse<LoginResponseDTO>
            {
                Status = StatusCodes.Status200OK,
                Data = loginResponse.Value,
                Message = "Login realizado com sucesso"
            });
        }

        [HttpPost]
        [Route("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register(RegisterUserRequestDTO registerRequest)
        {
            var result = await _registerUseCase.Register(registerRequest);

            if (!result.IsSuccess)
            {
                var statusCode = result.ErrorType switch
                {
                    ErrorType.Conflict => StatusCodes.Status409Conflict,
                    ErrorType.CreateConflict => StatusCodes.Status400BadRequest,
                    ErrorType.UnexpectedFailure => StatusCodes.Status500InternalServerError,
                    _ => StatusCodes.Status500InternalServerError
                };

                return StatusCode(statusCode, new ApiResponse
                {
                    Status = statusCode,
                    Message = result.ErrorMessage!
                });
            }

            return StatusCode(StatusCodes.Status201Created, new ApiResponse<Guid>
            {
                Status = StatusCodes.Status201Created,
                Data = result.Value,
                Message = "Usuário criado com sucesso"
            });
        }
    }
}
