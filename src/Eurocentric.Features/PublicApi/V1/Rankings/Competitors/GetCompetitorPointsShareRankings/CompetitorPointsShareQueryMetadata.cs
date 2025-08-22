using Eurocentric.Features.PublicApi.V1.Common.Constants;
using Eurocentric.Features.PublicApi.V1.Common.Enums;
using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.PublicApi.V1.Rankings.Competitors.GetCompetitorPointsShareRankings;

public sealed record CompetitorPointsShareQueryMetadata : IExampleProvider<CompetitorPointsShareQueryMetadata>
{
    public required QueryableContestStage ContestStage { get; init; }

    public int? MinYear { get; init; }

    public int? MaxYear { get; init; }

    public required QueryableVotingMethod VotingMethod { get; init; }

    public static CompetitorPointsShareQueryMetadata CreateExample() => new()
    {
        ContestStage = QueryParamDefaults.ContestStage,
        MinYear = null,
        MaxYear = null,
        VotingMethod = QueryParamDefaults.VotingMethod
    };
}
