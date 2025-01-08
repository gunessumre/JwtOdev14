using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using JwtOdev14.Data;
using JwtOdev14.Dtos;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JwtOdev14.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly JwtDbContext _context;
        private readonly IConfiguration _config;

        public AuthController(JwtDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _context.Users.ToList();
            return Ok(result);
        }

        [HttpPost]
        public IActionResult Login(UserDto dto) 
        { 
            
            var user =_context.Users.FirstOrDefault(u => u.Email == dto.Email && u.Password == dto.Password);

            if (user == null)
            {
                return Unauthorized("Email veya sifre hatali");
            }

            
            var claims = new[]
            {
               new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), 
               new Claim(ClaimTypes.Email, user.Email),
               new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) 
            };

            
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials 
                );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo 
            });
        }

        [Authorize]
        [HttpGet("Bilgi")]
        public IActionResult GetUserInfo()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
           

            return Ok(new
            {
                Email = email
            });
        }
    }
}
