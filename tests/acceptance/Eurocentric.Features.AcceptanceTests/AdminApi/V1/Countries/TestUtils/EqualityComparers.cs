using Eurocentric.Features.AdminApi.V1.Countries.Common;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Countries.TestUtils;

public static class EqualityComparers
{
    public static bool Country(Country a, Country b) => a.Id == b.Id
                                                        && a.CountryCode == b.CountryCode
                                                        && a.Name == b.Name
                                                        && a.ContestMemos.SequenceEqual(b.ContestMemos);
}
