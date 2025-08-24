using Eurocentric.Features.PublicApi.V1.Common.Constants;
using Eurocentric.Features.PublicApi.V1.Common.Enums;
using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.PublicApi.V1.Rankings.CompetingCountries.GetCompetingCountryPointsConsensusRankings;

public sealed record
    CompetingCountryPointsConsensusQueryMetadata : IExampleProvider<CompetingCountryPointsConsensusQueryMetadata>
{
    public required QueryableContestStage ContestStage { get; init; }

    public int? MinYear { get; init; }

    public int? MaxYear { get; init; }

    public string? VotingCountryCode { get; init; }

    public static CompetingCountryPointsConsensusQueryMetadata CreateExample() => new()
    {
        ContestStage = QueryParamDefaults.ContestStage, MinYear = null, MaxYear = null, VotingCountryCode = null
    };
}
