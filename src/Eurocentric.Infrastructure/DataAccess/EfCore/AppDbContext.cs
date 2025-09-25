using Microsoft.EntityFrameworkCore;
using V0Broadcast = Eurocentric.Domain.V0.Aggregates.Broadcasts.Broadcast;
using V0Contest = Eurocentric.Domain.V0.Aggregates.Contests.Contest;
using V0Country = Eurocentric.Domain.V0.Aggregates.Countries.Country;

namespace Eurocentric.Infrastructure.DataAccess.EfCore;

/// <summary>
///     Represents a session with the application database.
/// </summary>
public sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<V0Broadcast> Broadcasts => Set<V0Broadcast>();

    public DbSet<V0Contest> Contests => Set<V0Contest>();

    public DbSet<V0Country> Countries => Set<V0Country>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}
