using Eurocentric.Components.DataAccess.EfCore;
using Eurocentric.Domain.Aggregates.Countries;

namespace Eurocentric.Components.Repositories;

internal sealed class CountryWriteRepository(AppDbContext dbContext) : ICountryWriteRepository
{
    public void Add(Country country) => dbContext.Countries.Add(country);

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default) =>
        await dbContext.SaveChangesAsync(cancellationToken);
}
