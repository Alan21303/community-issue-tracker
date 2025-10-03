// File: Controllers/ReportsController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CommunityTracker.Data;
using CommunityTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace CommunityTracker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;

        public ReportsController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        // ====== Create Report (Users Only) ======
        [HttpPost]
        [Authorize(Roles="User")]
        public async Task<IActionResult> CreateReport([FromForm] ReportCreateDto dto)
        {
            var userId = int.Parse(User.Claims.First(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value);

            var report = new Report
            {
                Title = dto.Title,
                Description = dto.Description,
                Category = dto.Category,
                LocationText = dto.LocationText,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                CreatedByUserId = userId
            };

            _db.Reports.Add(report);
            await _db.SaveChangesAsync();

            // Handle tags
            var tagNames = dto.Tags.Split(',', StringSplitOptions.RemoveEmptyEntries)
                       .Select(t => t.Trim().ToLower())
                       .Distinct();
            foreach(var tName in tagNames)
            {
                var tag = await _db.Tags.FirstOrDefaultAsync(x => x.Name == tName)
                          ?? new Tag { Name = tName };
                if(tag.Id == 0) _db.Tags.Add(tag);
                report.ReportTags.Add(new ReportTag { Report = report, Tag = tag });
            }
            await _db.SaveChangesAsync();

            // Handle media files
            if(dto.Files != null && dto.Files.Count > 0)
            {
                foreach(var file in dto.Files)
                {
                    var filename = $"{Guid.NewGuid()}_{file.FileName}";
                    var path = Path.Combine(_env.WebRootPath, "uploads", filename);
                    using var fs = new FileStream(path, FileMode.Create);
                    await file.CopyToAsync(fs);

                    _db.Media.Add(new Media
                    {
                        ReportId = report.Id,
                        FilePath = $"/uploads/{filename}",
                        MediaType = file.ContentType.StartsWith("video") ? "video" : "image"
                    });
                }
                await _db.SaveChangesAsync();
            }

            return Ok(new { report.Id });
        }

        // ====== Get Reports (All Users) ======
        [HttpGet]
        [Authorize] // all logged in users
[HttpGet]
[Authorize]
public async Task<IActionResult> GetReports([FromQuery] string? status=null, [FromQuery] string? category=null, [FromQuery] string? tag=null)
{
    var query = _db.Reports
                .Include(r => r.Media)
                .Include(r => r.ReportTags).ThenInclude(rt => rt.Tag)
                .Include(r => r.CreatedByUser)
                .AsQueryable();

    if(!string.IsNullOrEmpty(status)) query = query.Where(r => r.Status == status);
    if(!string.IsNullOrEmpty(category)) query = query.Where(r => r.Category == category);
    if(!string.IsNullOrEmpty(tag))
    {
        var tagLower = tag.ToLower();
        query = query.Where(r => r.ReportTags.Any(rt => rt.Tag.Name.Contains(tagLower)));
    }

    var reports = await query.OrderByDescending(r => r.CreatedAt).ToListAsync();
    return Ok(reports);
}

        // ====== Update Report Status (Authorities Only) ======
        [HttpPut("{id}/status")]
        [Authorize(Roles="Authority")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] StatusUpdateDto dto)
        {
            var report = await _db.Reports.FindAsync(id);
            if(report == null) return NotFound("Report not found.");

            report.Status = dto.Status; // Submitted → Reviewed → Resolved
            await _db.SaveChangesAsync();
            return Ok(report);
        }
    }

    // ====== DTOs ======
public class ReportCreateDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string LocationText { get; set; } = string.Empty;
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public string Tags { get; set; } = string.Empty; // comma-separated
    public List<IFormFile> Files { get; set; } = new List<IFormFile>();
}
    public class StatusUpdateDto
    {
        public string Status { get; set; } = string.Empty;
    }
}
