using Eurocentric.Features.AdminApi.V0.Common.Contracts;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V0.Utils.Comparers;

public static class ContestEquality
{
    public static bool Compare(Contest a, Contest b) => a.Id == b.Id
                                                        && a.ContestYear == b.ContestYear
                                                        && a.ContestFormat == b.ContestFormat
                                                        && a.CityName.Equals(b.CityName, StringComparison.Ordinal);
}
