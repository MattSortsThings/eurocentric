using CSharpFunctionalExtensions;
using Eurocentric.Components.DataAccess.EfCore;
using Eurocentric.Domain.Functional;
using Eurocentric.Domain.V0.Aggregates.Countries;
using Microsoft.EntityFrameworkCore;

namespace Eurocentric.Components.Repositories.V0;

internal sealed class CountryReadRepository(AppDbContext dbContext) : ICountryReadRepository
{
    public async Task<Country[]> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext
            .V0Countries.AsNoTracking()
            .OrderBy(country => country.CountryCode)
            .ToArrayAsync(cancellationToken);
    }

    public async Task<Result<Country, IDomainError>> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        Country? country = await dbContext
            .V0Countries.AsNoTracking()
            .SingleOrDefaultAsync(country => country.Id == id, cancellationToken);

        return country is not null ? country : CountryErrors.CountryNotFound(id);
    }

    public IQueryable<Country> GetQueryable() => dbContext.V0Countries.AsNoTracking();
}
