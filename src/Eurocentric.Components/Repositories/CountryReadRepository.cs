using Eurocentric.Components.DataAccess.EfCore;
using Eurocentric.Domain.Aggregates.Countries;
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
}
