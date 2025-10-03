using System;
using System.Collections.Generic;

namespace CommunityTracker.Models
{
    public class Report
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty; // e.g., Roads, Utilities, Garbage, Lighting
        public string LocationText { get; set; } = string.Empty; // manually entered address
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string Status { get; set; } = "Submitted"; // Submitted → Reviewed → Resolved
        public int CreatedByUserId { get; set; }
        public User CreatedByUser { get; set; } = new User(); // make sure User has a default constructor
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public List<Media> Media { get; set; } = new();
        public List<ReportTag> ReportTags { get; set; } = new();
    }
}
