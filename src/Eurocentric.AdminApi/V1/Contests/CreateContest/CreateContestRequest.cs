using Eurocentric.AdminApi.V1.Contests.Models;

namespace Eurocentric.AdminApi.V1.Contests.CreateContest;

public sealed record CreateContestRequest
{
    public required int ContestYear { get; init; }

    public required string HostCityName { get; init; }

    public required VotingRules VotingRules { get; init; }
}
