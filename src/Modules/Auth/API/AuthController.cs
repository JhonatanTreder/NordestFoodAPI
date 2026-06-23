using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NordesteFoodAPI.Modules.Auth.Application.UseCases;
using NordesteFoodAPI.Modules.Auth.Domain.DTOs.Login;
using NordesteFoodAPI.Modules.Auth.Domain.DTOs.Register;
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

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequest)
        {
            var loginResponse = await _loginUseCase.Login(loginRequest);

            return Ok(loginResponse);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserRequestDTO registerRequest)
        {
            var registerResponse = await _registerUseCase.Register(registerRequest);

            return Ok(new
            {
                Message = "Usuário registrado com sucesso",
                UserId = registerResponse.ToString()
            });
        }
    }
}
