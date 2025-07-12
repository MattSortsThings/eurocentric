using Eurocentric.Features.AdminApi.V1.Common.Contracts;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Comparers;

internal static class ContestEquality
{
    internal static bool Compare(Contest a, Contest b) => a.Id == b.Id
                                                          && a.ContestYear == b.ContestYear
                                                          && a.CityName == b.CityName
                                                          && a.Completed == b.Completed
                                                          && a.ContestFormat == b.ContestFormat
                                                          && a.ChildBroadcasts.SequenceEqual(b.ChildBroadcasts)
                                                          && a.Participants.SequenceEqual(b.Participants);
}
