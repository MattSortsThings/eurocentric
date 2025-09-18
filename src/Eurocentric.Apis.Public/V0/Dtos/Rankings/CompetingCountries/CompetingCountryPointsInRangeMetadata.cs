using Eurocentric.Apis.Public.V0.Dtos.Rankings.Common;
using Eurocentric.Apis.Public.V0.Enums;

namespace Eurocentric.Apis.Public.V0.Dtos.Rankings.CompetingCountries;

public sealed record CompetingCountryPointsInRangeMetadata : PaginatedMetadata
{
    public int MinPoints { get; init; }

    public int MaxPoints { get; init; }

    public int? MinYear { get; init; }

    public int? MaxYear { get; init; }

    public ContestStage[]? ContestStages { get; init; }

    public VotingMethod? VotingMethod { get; init; }

    public string? VotingCountryCode { get; init; }
}
