using Eurocentric.Components.DataAccess.Common;
using Microsoft.EntityFrameworkCore;
using Contest = Eurocentric.Domain.Aggregates.Contests.Contest;
using Country = Eurocentric.Domain.Aggregates.Countries.Country;
using V0Broadcast = Eurocentric.Domain.V0.Aggregates.Broadcasts.Broadcast;
using V0Contest = Eurocentric.Domain.V0.Aggregates.Contests.Contest;
using V0Country = Eurocentric.Domain.V0.Aggregates.Countries.Country;

namespace Eurocentric.Components.DataAccess.EfCore;

/// <summary>
///     Represents a session with the application database.
/// </summary>
public sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<Contest> Contests => Set<Contest>();

    public DbSet<Country> Countries => Set<Country>();

    public DbSet<V0Broadcast> V0Broadcasts => Set<V0Broadcast>();

    public DbSet<V0Contest> V0Contests => Set<V0Contest>();

    public DbSet<V0Country> V0Countries => Set<V0Country>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema(Schemas.Dbo);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
