using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ParkingManagement.Domain.DTO;
using ParkingManagement.Domain.Dtos;
using ParkingManagement.Domain.Services.v1;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ParkingManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly LoginService _service;
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration, LoginService service)
        {
            _configuration = configuration;
            _service = service;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login(LoginModel model)
        {
            var user = IsValidUser(model.email, model.Password);
            if (user != null)
            {
                var token = GenerateJwtToken(model.email);
                return Ok(new LoginResponseDTO() { User = user, Token = token });
            }

            return Unauthorized();
        }

        private UserDto IsValidUser(string username, string password)
        {
            return _service.IsValidUser(username, password);
        }

        private string GenerateJwtToken(string username)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username)
                // Add additional claims if needed
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddDays(2), // Token expiration time
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    public class LoginModel
    {
        public string email { get; set; }
        public string Password { get; set; }
    }
}
