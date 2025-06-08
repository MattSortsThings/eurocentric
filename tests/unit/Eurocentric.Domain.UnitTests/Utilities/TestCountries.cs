using Eurocentric.Domain.Countries;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.UnitTests.Utilities;

public static class TestCountries
{
    public static Country At => Create("AT", "Austria", TestCountryIds.At);

    public static Country Be => Create("BE", "Belgium", TestCountryIds.Be);

    public static Country Cz => Create("CZ", "Czechia", TestCountryIds.Cz);

    public static Country Dk => Create("DK", "Denmark", TestCountryIds.Dk);

    public static Country Ee => Create("EE", "Estonia", TestCountryIds.Ee);

    public static Country Fi => Create("FI", "Finland", TestCountryIds.Fi);

    public static Country Gb => Create("GB", "United Kingdom", TestCountryIds.Gb);

    public static Country Xx => Create("XX", "Rest of the World", TestCountryIds.Xx);

    private static Country Create(string countryCode, string countryName, CountryId id)
    {
        FixedCountryIdGenerator idGenerator = new(id);

        return Country.Create()
            .WithCountryCode(CountryCode.FromValue(countryCode))
            .WithCountryName(CountryName.FromValue(countryName))
            .Build(idGenerator)
            .Value;
    }
}
