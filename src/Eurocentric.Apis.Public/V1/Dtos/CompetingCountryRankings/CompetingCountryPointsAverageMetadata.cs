using Eurocentric.Apis.Public.V1.Enums;
using Eurocentric.Components.OpenApi;

namespace Eurocentric.Apis.Public.V1.Dtos.CompetingCountryRankings;

public sealed record CompetingCountryPointsAverageMetadata
    : ISchemaExampleProvider<CompetingCountryPointsAverageMetadata>
{
    public int? MinYear { get; init; }

    public int? MaxYear { get; init; }

    public ContestStageFilter? ContestStage { get; init; }

    public string? VotingCountryCode { get; init; }

    public VotingMethodFilter? VotingMethod { get; init; }

    public int PageIndex { get; init; }

    public int PageSize { get; init; }

    public bool Descending { get; init; }

    public int TotalItems { get; init; }

    public int TotalPages { get; init; }

    public static CompetingCountryPointsAverageMetadata CreateExample() =>
        new()
        {
            MinYear = 2022,
            MaxYear = 2023,
            ContestStage = ContestStageFilter.Any,
            VotingMethod = VotingMethodFilter.Any,
            VotingCountryCode = "GB",
            PageIndex = 0,
            PageSize = 10,
            Descending = true,
            TotalItems = 35,
            TotalPages = 4,
        };
}
