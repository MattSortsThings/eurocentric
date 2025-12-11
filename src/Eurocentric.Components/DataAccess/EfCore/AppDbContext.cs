using Eurocentric.Components.DataAccess.Common;
using Microsoft.EntityFrameworkCore;
using V0Broadcast = Eurocentric.Domain.Aggregates.V0.Broadcast;
using V0Contest = Eurocentric.Domain.Aggregates.V0.Contest;
using V0Country = Eurocentric.Domain.Aggregates.V0.Country;

namespace Eurocentric.Components.DataAccess.EfCore;

/// <summary>
///     Represents a session with the application database.
/// </summary>
public sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<V0Broadcast> V0Broadcasts => Set<V0Broadcast>();

    public DbSet<V0Contest> V0Contests => Set<V0Contest>();

    public DbSet<V0Country> V0Countries => Set<V0Country>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(DbSchemas.Dbo);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
