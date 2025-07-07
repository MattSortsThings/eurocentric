using Eurocentric.Features.AdminApi.V0.Common.Contracts;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.Contests.Utils;

public static class ContestEquality
{
    public static bool Compare(Contest a, Contest b) =>
        a.Id == b.Id
        && a.ContestYear == b.ContestYear
        && a.CityName.Equals(b.CityName, StringComparison.InvariantCulture)
        && a.ContestFormat == b.ContestFormat;
}
