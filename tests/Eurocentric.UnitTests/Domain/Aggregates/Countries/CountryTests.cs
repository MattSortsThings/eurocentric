using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.UnitTests.Domain.Aggregates.Countries;

public sealed partial class CountryTests
{
    private static readonly CountryId DefaultCountryId = CountryId.FromValue(
        Guid.Parse("fe901f2e-35ab-4c3a-a828-8d0fd019a182")
    );

    private static readonly CountryCode DefaultCountryCode = CountryCode.FromValue("AA").GetValueOrDefault();
    private static readonly CountryName DefaultCountryName = CountryName.FromValue("CountryName").GetValueOrDefault();
}
