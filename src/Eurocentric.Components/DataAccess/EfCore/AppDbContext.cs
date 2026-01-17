using Eurocentric.Components.DataAccess.Common;
using Microsoft.EntityFrameworkCore;

namespace Eurocentric.Components.DataAccess.EFCore;

/// <summary>
///     Represents a session with the application database.
/// </summary>
public sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(DbSchemas.Dbo);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
