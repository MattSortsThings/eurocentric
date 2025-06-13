using Eurocentric.Domain.Abstractions;
using Eurocentric.Domain.Broadcasts;
using Eurocentric.Domain.Contests;
using Eurocentric.Domain.Countries;
using Microsoft.EntityFrameworkCore;

namespace Eurocentric.Infrastructure.EFCore;

/// <summary>
///     Represents a session with the application database.
/// </summary>
public sealed class AppDbContext : DbContext
{
    private readonly PublishDomainEventsInterceptor _publishDomainEventsInterceptor;

    public AppDbContext(DbContextOptions<AppDbContext> options, PublishDomainEventsInterceptor publishDomainEventsInterceptor)
        : base(options)
    {
        _publishDomainEventsInterceptor = publishDomainEventsInterceptor;
    }

    public DbSet<Broadcast> Broadcasts => Set<Broadcast>();

    public DbSet<Contest> Contests => Set<Contest>();

    public DbSet<Country> Countries => Set<Country>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        modelBuilder.HasDefaultSchema(DbConstants.SchemaName);
        modelBuilder.Ignore<IReadOnlyList<IDomainEvent>>();

        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_publishDomainEventsInterceptor);
        base.OnConfiguring(optionsBuilder);
    }
}
