using Microsoft.EntityFrameworkCore;
using UrlShortner.Data;

namespace UrlShortner.Infrastructure;

public class LongUrlContext : DbContext
{
    public LongUrlContext(DbContextOptions<LongUrlContext> options) : base(options)
    {
    }

    public DbSet<LongUrl> longUrls { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<LongUrl>().ToTable("URL");
        builder.Entity<LongUrl>().HasKey(p => p.Id);
        builder.Entity<LongUrl>().Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<LongUrl>().Property(p => p.Url).IsRequired();
    }
}