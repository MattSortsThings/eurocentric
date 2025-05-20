using Eurocentric.Features.AdminApi.V1.Common.Dtos;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Contests.TestUtils;

internal static class EqualityComparers
{
    internal static bool Contest(Contest a, Contest b) => a.Id == b.Id
                                                          && a.Year == b.Year
                                                          && a.CityName == b.CityName
                                                          && a.Format == b.Format
                                                          && a.Status == b.Status
                                                          && a.BroadcastMemos.SequenceEqual(b.BroadcastMemos)
                                                          && a.Participants.SequenceEqual(b.Participants);
}
