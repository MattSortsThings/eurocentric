using Eurocentric.Domain.Countries;
using Eurocentric.Domain.Identifiers;

namespace Eurocentric.Domain.UnitTests.Utilities;

public sealed class FixedCountryIdGenerator(CountryId fixedId) : ICountryIdGenerator
{
    public CountryId Generate() => fixedId;
}
