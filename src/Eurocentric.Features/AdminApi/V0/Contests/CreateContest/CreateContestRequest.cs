using Eurocentric.Features.AdminApi.V0.Common.Enums;

namespace Eurocentric.Features.AdminApi.V0.Contests.CreateContest;

public sealed record CreateContestRequest
{
    public required int ContestYear { get; init; }

    public required string CityName { get; init; }

    public required ContestFormat ContestFormat { get; init; }

    public required Guid[] ParticipatingCountryIds { get; init; }
}
