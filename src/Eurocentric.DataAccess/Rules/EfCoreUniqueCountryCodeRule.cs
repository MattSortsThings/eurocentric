using Eurocentric.DataAccess.EfCore;
using Eurocentric.Domain.Rules;
using Eurocentric.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Eurocentric.DataAccess.Rules;

internal sealed class EfCoreUniqueCountryCodeRule(AppDbContext dbContext) : UniqueCountryCodeRule
{
    protected override bool Exists(CountryCode countryCode) =>
        dbContext.Countries.AsNoTracking().Any(country => country.CountryCode.Equals(countryCode));
}
