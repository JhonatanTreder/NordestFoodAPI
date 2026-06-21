using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NordesteFoodAPI.Modules.Auth.API
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost]
        public IActionResult Login()
        {
            return Ok("Login successful");
        }

        [HttpPost]
        public IActionResult Register()
        {
            return Ok("Register successful");
        }
    }
}
