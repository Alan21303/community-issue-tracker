// File: Data/AppDbContext.cs
using Microsoft.EntityFrameworkCore;
using CommunityTracker.Models;

namespace CommunityTracker.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Media> Media { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<ReportTag> ReportTags { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ReportTag>()
                .HasKey(rt => new { rt.ReportId, rt.TagId });

            modelBuilder.Entity<Tag>()
                .HasIndex(t => t.Name)
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}
