using Eurocentric.Domain.Aggregates.Countries;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.UnitTests.Utils;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.UnitTests.Aggregates.Countries;

public sealed partial class CountryTests : UnitTest
{
    private static readonly CountryCode ArbitraryCountryCode = CountryCode.FromValue("AA").Value;
    private static readonly CountryName ArbitraryCountryName = CountryName.FromValue("CountryName").Value;
    private static readonly CountryId FixedCountryId = CountryId.FromValue(Guid.Parse("24c201bc-709a-4b37-a31e-f193d9832244"));

    private static Country CreateCountry() => Country.Create()
        .WithCountryCode(ArbitraryCountryCode)
        .WithCountryName(ArbitraryCountryName)
        .Build(() => FixedCountryId).Value;
}
