// File: Controllers/TagsController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CommunityTracker.Data;
using CommunityTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace CommunityTracker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TagsController : ControllerBase
    {
        private readonly AppDbContext _db;
        public TagsController(AppDbContext db) { _db = db; }

        // ====== Autocomplete/Search Tags ======
        [HttpGet]
        [Authorize] // all logged-in users can search tags
        public async Task<IActionResult> GetTags([FromQuery] string q)
        {
            if (string.IsNullOrEmpty(q))
                return BadRequest("Query cannot be empty.");

            var tags = await _db.Tags
                        .Where(t => t.Name.Contains(q))
                        .OrderBy(t => t.Name)
                        .Take(10)
                        .Select(t => new { t.Id, t.Name })
                        .ToListAsync();

            return Ok(tags);
        }

        // ====== Add Tag (if not exists) ======
        [HttpPost]
        [Authorize(Roles="User,Authority")] // any authenticated user can create new tags
        public async Task<IActionResult> AddTag([FromBody] TagCreateDto dto)
        {
            var existing = await _db.Tags.FirstOrDefaultAsync(t => t.Name.ToLower() == dto.Name.ToLower());
            if (existing != null) return Conflict("Tag already exists.");

            var tag = new Tag { Name = dto.Name.Trim().ToLower() };
            _db.Tags.Add(tag);
            await _db.SaveChangesAsync();
            return Ok(tag);
        }
    }

    // ====== DTO ======
    public class TagCreateDto
    {
        public string Name { get; set; }= string.Empty;
    }
}
