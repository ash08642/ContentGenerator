using ContentGenerator.Models;
using Microsoft.EntityFrameworkCore;

namespace ContentGenerator.Data;

public class ContentDbContext(DbContextOptions<ContentDbContext> options) : DbContext(options)
{
    public DbSet<Content> Contents => Set<Content>();

    /*protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Content>().HasData(
            new Content
            {
                Id = Guid.NewGuid(),
                AudioPath = "/temp/music",
                Text = "Hello My name is tester",
                Type = ContentType.Conversational
            }
        );
    }*/
}