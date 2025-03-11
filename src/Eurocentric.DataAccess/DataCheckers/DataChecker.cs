using Eurocentric.DataAccess.EfCore;
using Eurocentric.Domain.Rules.DataCheckers;
using Eurocentric.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Eurocentric.DataAccess.DataCheckers;

internal sealed class DataChecker(AppDbContext dbContext) : IDataChecker
{
    public bool CountryExistsWithCountryCode(CountryCode countryCode) =>
        dbContext.Countries.AsNoTracking().Any(country => country.CountryCode == countryCode);
}
