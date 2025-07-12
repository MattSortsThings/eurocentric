using Eurocentric.Domain.Aggregates.Countries;
using Eurocentric.Domain.PlaceholderEntities;
using Microsoft.EntityFrameworkCore;
using PlaceholderContest = Eurocentric.Domain.PlaceholderEntities.Contest;
using DomainContest = Eurocentric.Domain.Aggregates.Contests.Contest;

namespace Eurocentric.Infrastructure.DataAccess.EfCore;

/// <summary>
///     Represents a session with the application database.
/// </summary>
public sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<DomainContest> Contests => Set<DomainContest>();

    public DbSet<Country> Countries => Set<Country>();

    public DbSet<PlaceholderContest> PlaceholderContests => Set<PlaceholderContest>();

    public DbSet<QueryableCountry> PlaceholderQueryableCountries => Set<QueryableCountry>();

    public DbSet<QueryableJuryAward> PlaceholderQueryableJuryAwards => Set<QueryableJuryAward>();

    public DbSet<QueryableTelevoteAward> PlaceholderQueryableTelevoteAwards => Set<QueryableTelevoteAward>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
