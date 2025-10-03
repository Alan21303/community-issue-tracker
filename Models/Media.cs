// File: Models/Media.cs
namespace CommunityTracker.Models
{
    public class Media
    {
        public int Id { get; set; }
        public int ReportId { get; set; }
        public Report Report { get; set; } = new Report(); // default value
        public string FilePath { get; set; } = string.Empty; // default value
        public string MediaType { get; set; } = string.Empty; // default value, e.g., "image" or "video"
    }
}
