using Eurocentric.Apis.Public.V1.Enums;
using Eurocentric.Components.OpenApi;

namespace Eurocentric.Apis.Public.V1.Dtos.VotingCountryRankings;

public sealed record VotingCountryPointsAverageMetadata : ISchemaExampleProvider<VotingCountryPointsAverageMetadata>
{
    public string CompetingCountryCode { get; init; } = string.Empty;

    public int? MinYear { get; init; }

    public int? MaxYear { get; init; }

    public ContestStageFilter? ContestStage { get; init; }

    public VotingMethodFilter? VotingMethod { get; init; }

    public int PageIndex { get; init; }

    public int PageSize { get; init; }

    public bool Descending { get; init; }

    public int TotalItems { get; init; }

    public int TotalPages { get; init; }

    public static VotingCountryPointsAverageMetadata CreateExample() =>
        new()
        {
            CompetingCountryCode = "GB",
            MinYear = 2022,
            MaxYear = 2023,
            ContestStage = ContestStageFilter.Any,
            VotingMethod = VotingMethodFilter.Any,
            PageIndex = 0,
            PageSize = 10,
            Descending = true,
            TotalItems = 35,
            TotalPages = 4,
        };
}
