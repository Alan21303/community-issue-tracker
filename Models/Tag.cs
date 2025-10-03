// File: Models/Tag.cs
using System.Collections.Generic;

namespace CommunityTracker.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<ReportTag> ReportTags { get; set; } = new();
    }
}
