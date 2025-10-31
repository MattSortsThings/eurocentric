using CSharpFunctionalExtensions;
using Eurocentric.Components.DataAccess.EfCore;
using Eurocentric.Domain.Aggregates.Countries;
using Eurocentric.Domain.Core;
using Eurocentric.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using SlimMessageBus;

namespace Eurocentric.Components.Repositories;

internal sealed class CountryWriteRepository : BaseWriteRepository, ICountryWriteRepository
{
    public CountryWriteRepository(IPublishBus publishBus, AppDbContext dbContext)
        : base(publishBus, dbContext) { }

    public void Add(Country country) => DbContext.Countries.Add(country);

    public void Update(Country country) => DbContext.Countries.Update(country);

    public async Task<Result<Country, IDomainError>> GetByIdAsync(
        CountryId countryId,
        CancellationToken cancellationToken = default
    )
    {
        Country? country = await DbContext.Countries.SingleOrDefaultAsync(
            country => country.Id.Equals(countryId),
            cancellationToken
        );

        return country is not null ? country : CountryErrors.CountryNotFound(countryId);
    }
}
