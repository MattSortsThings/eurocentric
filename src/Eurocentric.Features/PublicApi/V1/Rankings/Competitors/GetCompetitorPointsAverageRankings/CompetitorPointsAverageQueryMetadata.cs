using Eurocentric.Features.PublicApi.V1.Common.Constants;
using Eurocentric.Features.PublicApi.V1.Common.Enums;
using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.PublicApi.V1.Rankings.Competitors.GetCompetitorPointsAverageRankings;

public sealed record CompetitorPointsAverageQueryMetadata : IExampleProvider<CompetitorPointsAverageQueryMetadata>
{
    public required QueryableContestStage ContestStage { get; init; }

    public int? MinYear { get; init; }

    public int? MaxYear { get; init; }

    public required QueryableVotingMethod VotingMethod { get; init; }

    public static CompetitorPointsAverageQueryMetadata CreateExample() => new()
    {
        ContestStage = QueryParamDefaults.ContestStage,
        MinYear = null,
        MaxYear = null,
        VotingMethod = QueryParamDefaults.VotingMethod
    };
}
