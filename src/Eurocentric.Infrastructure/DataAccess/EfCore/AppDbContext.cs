using Microsoft.EntityFrameworkCore;
using Contest = Eurocentric.Domain.Aggregates.Contests.Contest;
using Country = Eurocentric.Domain.Aggregates.Countries.Country;
using V0Country = Eurocentric.Domain.V0Entities.Country;
using V0Contest = Eurocentric.Domain.V0Entities.Contest;
using V0Broadcast = Eurocentric.Domain.V0Entities.Broadcast;

namespace Eurocentric.Infrastructure.DataAccess.EfCore;

/// <summary>
///     Represents a session with a database.
/// </summary>
public sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Contest> Contests => Set<Contest>();

    public DbSet<Country> Countries => Set<Country>();

    public DbSet<V0Broadcast> V0Broadcasts => Set<V0Broadcast>();

    public DbSet<V0Contest> V0Contests => Set<V0Contest>();

    public DbSet<V0Country> V0Countries => Set<V0Country>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
