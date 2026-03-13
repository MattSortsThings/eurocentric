using Eurocentric.Components.DataAccess.Common;
using Microsoft.EntityFrameworkCore;

namespace Eurocentric.Components.DataAccess.EfCore;

/// <summary>
///     Represents a session with the application database.
/// </summary>
public sealed class AppDbContext : DbContext
{
    /// <summary>
    ///     Initializes a new <see cref="AppDbContext" /> instance with the provided options.
    /// </summary>
    /// <param name="options">The database context options.</param>
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        modelBuilder.HasDefaultSchema(DboSchemaConstants.SchemaName);
    }
}
