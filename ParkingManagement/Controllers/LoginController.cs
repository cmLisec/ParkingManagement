using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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

        [HttpPost("login")]
        public IActionResult Login(LoginModel model)
        {
            // Here you would normally validate the username and password against your data source (e.g., a database)
            if (IsValidUser(model.email, model.Password))
            {
                var token = GenerateJwtToken(model.email);
                return Ok(new { token });
            }

            return Unauthorized();
        }

        private bool IsValidUser(string username, string password)
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
                expires: DateTime.Now.AddHours(1), // Token expiration time
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
