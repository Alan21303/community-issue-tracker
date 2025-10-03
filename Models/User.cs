

// File: Models/User.cs
using System;

namespace CommunityTracker.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = "User"; // "User", "Authority", "Admin"
        public bool IsVerified { get; set; } = true; // Authorities will be set to false at registration
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}