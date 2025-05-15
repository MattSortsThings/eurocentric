using Eurocentric.Domain.Countries;
using Microsoft.EntityFrameworkCore;

namespace Eurocentric.Infrastructure.EfCore;

/// <summary>
///     Application database context.
/// </summary>
public sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Country> Countries => Set<Country>();

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
}
