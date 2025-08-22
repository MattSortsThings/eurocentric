using Eurocentric.Features.PublicApi.V1.Common.Constants;
using Eurocentric.Features.PublicApi.V1.Common.Enums;
using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.PublicApi.V1.Rankings.VotingCountries.GetVotingCountryPointsAverageRankings;

public sealed record VotingCountryPointsAverageQueryMetadata : IExampleProvider<VotingCountryPointsAverageQueryMetadata>
{
    public required QueryableContestStage ContestStage { get; init; }

    public int? MinYear { get; init; }

    public int? MaxYear { get; init; }

    public required string CompetingCountryCode { get; init; }

    public required QueryableVotingMethod VotingMethod { get; init; }

    public static VotingCountryPointsAverageQueryMetadata CreateExample() => new()
    {
        ContestStage = QueryParamDefaults.ContestStage,
        MinYear = null,
        MaxYear = null,
        CompetingCountryCode = ExampleValues.CompetingCountryCode,
        VotingMethod = QueryParamDefaults.VotingMethod
    };
}
