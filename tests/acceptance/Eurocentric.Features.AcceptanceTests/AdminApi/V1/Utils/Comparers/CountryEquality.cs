using Eurocentric.Features.AdminApi.V1.Common.Contracts;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Comparers;

internal static class CountryEquality
{
    internal static bool Compare(Country a, Country b) => a.Id == b.Id
                                                          && a.CountryCode.Equals(b.CountryCode, StringComparison.Ordinal)
                                                          && a.CountryName.Equals(b.CountryName, StringComparison.Ordinal)
                                                          && a.ParticipatingContestIds.SequenceEqual(b.ParticipatingContestIds);
}
