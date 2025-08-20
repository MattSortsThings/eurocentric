using Eurocentric.Features.PublicApi.V1.Common.Constants;
using Eurocentric.Features.PublicApi.V1.Common.Enums;
using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.PublicApi.V1.Rankings.Competitors.GetCompetitorPointsConsensusRankings;

public sealed record CompetitorPointsConsensusFilteringMetadata : IExampleProvider<CompetitorPointsConsensusFilteringMetadata>
{
    public required QueryableContestStage ContestStage { get; init; }

    public int? MinYear { get; init; }

    public int? MaxYear { get; init; }

    public static CompetitorPointsConsensusFilteringMetadata CreateExample() => new()
    {
        ContestStage = QueryParamDefaults.ContestStage, MinYear = null, MaxYear = null
    };
}
