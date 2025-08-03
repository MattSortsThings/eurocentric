using Eurocentric.Features.PublicApi.V0.Common.Constants;
using Eurocentric.Features.PublicApi.V0.Common.Enums;
using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.PublicApi.V0.Rankings.GetCompetingCountryPointsAverageRankings;

public sealed record CompetingCountryPointsAverageFilters : IExampleProvider<CompetingCountryPointsAverageFilters>
{
    public QueryableVotingMethod VotingMethod { get; init; }

    public QueryableContestStage ContestStage { get; init; }

    public int? MinYear { get; init; }

    public int? MaxYear { get; init; }

    public string? VotingCountryCode { get; init; }

    public static CompetingCountryPointsAverageFilters CreateExample() => new()
    {
        ContestStage = QueryParamDefaults.ContestStage, VotingMethod = QueryParamDefaults.VotingMethod
    };
}
