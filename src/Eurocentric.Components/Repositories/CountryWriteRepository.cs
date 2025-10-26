using Eurocentric.Components.DataAccess.EfCore;
using Eurocentric.Domain.Aggregates.Countries;

namespace Eurocentric.Components.Repositories;

internal sealed class CountryWriteRepository : BaseWriteRepository, ICountryWriteRepository
{
    public CountryWriteRepository(AppDbContext dbContext)
        : base(dbContext) { }

    public void Add(Country country) => DbContext.Countries.Add(country);
}
