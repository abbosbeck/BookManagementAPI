using BookManagement.Core.Services.AuthService;
using BookManagement.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BookManagement.API.Controllers
{

    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly TokenService _tokenService;

        public AuthController(TokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequestViewModel request)
        {
            if (request.Username == "admin" && request.Password == "password")
            {
                var token = _tokenService.GenerateToken(request.Username);
                
                return Ok(new { Token = token });
            }

            return Unauthorized();
        }
    }
}
