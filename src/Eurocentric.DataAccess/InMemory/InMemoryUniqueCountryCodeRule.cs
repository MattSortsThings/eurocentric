using Eurocentric.Domain.Rules;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.DataAccess.InMemory;

public class InMemoryUniqueCountryCodeRule(InMemoryRepository repository) : UniqueCountryCodeRule
{
    protected override bool Exists(CountryCode countryCode) =>
        repository.Countries.Any(country => country.CountryCode == countryCode);
}
