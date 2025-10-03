
// File: Models/ReportTag.cs
namespace CommunityTracker.Models
{
    public class ReportTag
    {
        public int ReportId { get; set; }
        public Report Report { get; set; } = new Report(); // default value to satisfy non-nullable
        public int TagId { get; set; }
        public Tag Tag { get; set; } = new Tag(); // default value
    }
}
