using Eurocentric.AdminApi.V0.Contests.Models;

namespace Eurocentric.AdminApi.V0.Contests.CreateContest;

public sealed record CreateContestRequest
{
    public required int ContestYear { get; init; }

    public required string HostCityName { get; init; }

    public required VotingRules VotingRules { get; init; }
}
