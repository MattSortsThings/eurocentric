using Eurocentric.Domain.V0Entities;
using Microsoft.EntityFrameworkCore;

namespace Eurocentric.Infrastructure.DataAccess.EfCore;

/// <summary>
///     Represents a session with a database.
/// </summary>
public sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Broadcast> V0Broadcasts => Set<Broadcast>();

    public DbSet<Contest> V0Contests => Set<Contest>();

    public DbSet<Country> V0Countries => Set<Country>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
