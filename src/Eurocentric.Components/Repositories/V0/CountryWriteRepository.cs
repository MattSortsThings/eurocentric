using CSharpFunctionalExtensions;
using Eurocentric.Components.DataAccess.EfCore;
using Eurocentric.Domain.Functional;
using Eurocentric.Domain.V0.Aggregates.Countries;
using Microsoft.EntityFrameworkCore;

namespace Eurocentric.Components.Repositories.V0;

internal sealed class CountryWriteRepository(AppDbContext dbContext) : ICountryWriteRepository
{
    public void Add(Country country) => dbContext.V0Countries.Add(country);

    public void Remove(Country country) => dbContext.V0Countries.Remove(country);

    public async Task<Result<Country, IDomainError>> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        Country? country = await dbContext.V0Countries.SingleOrDefaultAsync(
            country => country.Id == id,
            cancellationToken
        );

        return country is not null ? country : CountryErrors.CountryNotFound(id);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default) =>
        await dbContext.SaveChangesAsync(cancellationToken);
}
