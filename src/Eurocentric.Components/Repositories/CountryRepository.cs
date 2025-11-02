using System.Linq.Expressions;
using CSharpFunctionalExtensions;
using Eurocentric.Components.DataAccess.EfCore;
using Eurocentric.Domain.Aggregates.Countries;
using Eurocentric.Domain.Core;
using Eurocentric.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Eurocentric.Components.Repositories;

internal sealed class CountryRepository(AppDbContext dbContext) : ICountryRepository
{
    public async Task<Result<Country, IDomainError>> GetUntrackedAsync(
        CountryId id,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(id);

        Country? country = await dbContext
            .Countries.AsNoTracking()
            .SingleOrDefaultAsync(country => country.Id == id, cancellationToken);

        return country is not null ? country : CountryErrors.CountryNotFound(id);
    }

    public async Task<Country[]> GetAllUntrackedAsync<TKey>(
        Expression<Func<Country, TKey>> sortKey,
        CancellationToken cancellationToken = default
    ) => await dbContext.Countries.AsNoTracking().OrderBy(sortKey).ToArrayAsync(cancellationToken);

    public IQueryable<Country> GetUntrackedQueryable() => dbContext.Countries.AsNoTracking();

    public void Add(Country aggregate)
    {
        ArgumentNullException.ThrowIfNull(aggregate);

        dbContext.Countries.Add(aggregate);
    }

    public async Task<Result<Country, IDomainError>> GetTrackedAsync(
        CountryId id,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(id);

        Country? country = await dbContext.Countries.SingleOrDefaultAsync(
            country => country.Id == id,
            cancellationToken
        );

        return country is not null ? country : CountryErrors.CountryNotFound(id);
    }

    public void Remove(Country aggregate)
    {
        ArgumentNullException.ThrowIfNull(aggregate);

        dbContext.Countries.Remove(aggregate);
    }

    public void Update(Country aggregate)
    {
        ArgumentNullException.ThrowIfNull(aggregate);

        dbContext.Countries.Update(aggregate);
    }
}
