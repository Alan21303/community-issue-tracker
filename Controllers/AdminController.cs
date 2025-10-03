// File: Controllers/AdminController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CommunityTracker.Data;
using Microsoft.EntityFrameworkCore;

namespace CommunityTracker.Controllers
{
    [ApiController]
    [Route("api/admin")]
    [Authorize(Roles="Admin")]
    public class AdminController : ControllerBase
    {
        private readonly AppDbContext _db;
        public AdminController(AppDbContext db) { _db = db; }

        [HttpGet("pending-authorities")]
        public async Task<IActionResult> PendingAuthorities()
        {
            var pending = await _db.Users
                .Where(u => u.Role == "Authority" && !u.IsVerified)
                .Select(u => new { u.Id, u.Name, u.Email })
                .ToListAsync();

            return Ok(pending);
        }

        [HttpPut("verify/{id}")]
        public async Task<IActionResult> VerifyAuthority(int id)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == id && u.Role=="Authority");
            if (user == null) return NotFound("Authority not found.");

            user.IsVerified = true;
            await _db.SaveChangesAsync();
            return Ok("Authority verified successfully.");
        }
    }
}
