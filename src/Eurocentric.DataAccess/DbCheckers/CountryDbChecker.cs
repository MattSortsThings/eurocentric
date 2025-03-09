using Eurocentric.DataAccess.EfCore;
using Eurocentric.Domain.Countries;
using Eurocentric.Domain.Rules.External.DbCheckers;
using Microsoft.EntityFrameworkCore;

namespace Eurocentric.DataAccess.DbCheckers;

internal sealed class CountryDbChecker(AppDbContext dbContext) : ICountryDbChecker
{
    public bool CountryCodeIsNotUnique(Country country) =>
        dbContext.Countries.AsNoTracking().Any(existingCountry => existingCountry.CountryCode.Equals(country.CountryCode));
}
