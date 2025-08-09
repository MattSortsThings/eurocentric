using Eurocentric.Features.AdminApi.V1.Common.Constants;
using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.AdminApi.V1.Broadcasts.AwardTelevotePoints;

public sealed record AwardTelevotePointsRequest : IExampleProvider<AwardTelevotePointsRequest>
{
    public required Guid VotingCountryId { get; init; }

    public required Guid[] RankedCompetingCountryIds { get; init; }

    public static AwardTelevotePointsRequest CreateExample() => new()
    {
        VotingCountryId = ExampleValues.CountryId3Of3,
        RankedCompetingCountryIds = [ExampleValues.CountryId3Of3, ExampleValues.CountryId2Of3]
    };
}
