using CSharpFunctionalExtensions;
using Eurocentric.Components.DataAccess.EfCore;
using Eurocentric.Domain.Aggregates.Countries;
using Eurocentric.Domain.Core;
using Eurocentric.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Eurocentric.Components.Repositories;

internal sealed class CountryReadRepository(AppDbContext dbContext) : ICountryReadRepository
{
    public async Task<Country[]> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext
            .Countries.AsNoTracking()
            .OrderBy(country => country.CountryCode)
            .ToArrayAsync(cancellationToken);
    }

    public async Task<Result<Country, IDomainError>> GetByIdAsync(
        CountryId countryId,
        CancellationToken cancellationToken = default
    )
    {
        Country? country = await dbContext
            .Countries.AsNoTracking()
            .SingleOrDefaultAsync(country => country.Id.Equals(countryId), cancellationToken);

        return country is not null ? country : CountryErrors.CountryNotFound(countryId);
    }

    public IQueryable<Country> GetAsQueryable() => dbContext.Countries.AsNoTracking();
}
