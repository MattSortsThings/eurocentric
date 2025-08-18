using Eurocentric.Features.PublicApi.V1.Common.Constants;
using Eurocentric.Features.PublicApi.V1.Common.Enums;
using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.PublicApi.V1.Rankings.GetCompetingCountryPointsConsensusRankings;

public sealed record CompetingCountryPointsConsensusFilteringMetadata :
    IExampleProvider<CompetingCountryPointsConsensusFilteringMetadata>
{
    public required QueryableContestStage ContestStage { get; init; }

    public int? MinYear { get; init; }

    public int? MaxYear { get; init; }

    public string? VotingCountryCode { get; init; }


    public static CompetingCountryPointsConsensusFilteringMetadata CreateExample() => new()
    {
        ContestStage = QueryParamDefaults.ContestStage, MinYear = null, MaxYear = null, VotingCountryCode = null
    };
}
