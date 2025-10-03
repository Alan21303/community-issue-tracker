// File: Controllers/AuthController.cs
using Microsoft.AspNetCore.Mvc;
using CommunityTracker.Data;
using CommunityTracker.Models;
using CommunityTracker.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using BCrypt.Net;

namespace CommunityTracker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IConfiguration _config;

        public AuthController(AppDbContext db, IConfiguration config)
        {
            _db = db;
            _config = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (await _db.Users.AnyAsync(u => u.Email == dto.Email))
                return BadRequest("Email already registered.");

            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = dto.Role,
                IsVerified = dto.Role == "Authority" ? false : true
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            if (dto.Role == "Authority")
                return Ok("Authority registered. Wait for admin verification.");
            
            return Ok("User registered successfully.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return Unauthorized("Invalid credentials.");

            if (user.Role == "Authority" && !user.IsVerified)
                return Unauthorized("Authority not verified by admin yet.");

            var token = GenerateJwt(user);
            return Ok(new { token });
        }

        private string GenerateJwt(User user)
        {
            var jwtSettings = _config.GetSection("Jwt");

            var keyString = jwtSettings["Key"];
            if (string.IsNullOrEmpty(keyString))
                throw new Exception("JWT Key is missing in appsettings.json");

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(keyString));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiryStr = jwtSettings["ExpiryMinutes"];
            if (!int.TryParse(expiryStr, out var expiryMinutes))
                expiryMinutes = 60;

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
