using System.Data.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using UrlShortner.Data;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;

namespace UrlShortner.Infrastructure;

public interface ILongUrlContext
{
    Microsoft.EntityFrameworkCore.DbSet<LongUrl> longUrls { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    EntityEntry Add(object entity);
}

public class LongUrlContext : DbContext, ILongUrlContext
{
    public LongUrlContext(DbContextOptions<LongUrlContext> options) : base(options)
    {
    }

    public Microsoft.EntityFrameworkCore.DbSet<LongUrl> longUrls { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<LongUrl>().ToTable("URL");
        builder.Entity<LongUrl>().HasKey(p => p.Id);
        builder.Entity<LongUrl>().Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<LongUrl>().Property(p => p.Url).IsRequired();
    }
}