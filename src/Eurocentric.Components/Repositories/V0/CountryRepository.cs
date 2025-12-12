using CSharpFunctionalExtensions;
using Eurocentric.Components.DataAccess.EfCore;
using Eurocentric.Domain.Abstractions.Errors;
using Eurocentric.Domain.Aggregates.V0;
using Microsoft.EntityFrameworkCore;

namespace Eurocentric.Components.Repositories.V0;

internal sealed class CountryRepository(AppDbContext dbContext) : ICountryRepository
{
    public async Task<Result<Country, IDomainError>> GetUntrackedAsync(
        Guid countryId,
        CancellationToken cancellationToken = default
    )
    {
        Country? country = await dbContext
            .V0Countries.AsNoTracking()
            .SingleOrDefaultAsync(country => country.Id == countryId, cancellationToken)
            .ConfigureAwait(false);

        return country is not null ? country : CountryErrors.CountryNotFound(countryId);
    }

    public async Task<List<Country>> GetAllUntrackedAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext
            .V0Countries.AsNoTracking()
            .OrderBy(country => country.CountryCode)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    public IQueryable<Country> GetUntrackedQueryable() => dbContext.V0Countries.AsNoTracking().AsQueryable();

    public void Add(Country country) => dbContext.V0Countries.Add(country);

    public void Update(Country country) => dbContext.V0Countries.Update(country);

    public void Remove(Country country) => dbContext.V0Countries.Remove(country);

    public async Task<Result<Country, IDomainError>> GetTrackedAsync(
        Guid countryId,
        CancellationToken cancellationToken = default
    )
    {
        Country? country = await dbContext
            .V0Countries.SingleOrDefaultAsync(country => country.Id == countryId, cancellationToken)
            .ConfigureAwait(false);

        return country is not null ? country : CountryErrors.CountryNotFound(countryId);
    }
}
