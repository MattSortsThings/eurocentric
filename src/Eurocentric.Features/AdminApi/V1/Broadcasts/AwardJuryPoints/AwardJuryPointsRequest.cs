using Eurocentric.Features.AdminApi.V1.Common.Constants;
using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.AdminApi.V1.Broadcasts.AwardJuryPoints;

public sealed record AwardJuryPointsRequest : IExampleProvider<AwardJuryPointsRequest>
{
    public required Guid VotingCountryId { get; init; }

    public required Guid[] RankedCompetingCountryIds { get; init; }

    public static AwardJuryPointsRequest CreateExample() => new()
    {
        VotingCountryId = ExampleValues.CountryId3Of3,
        RankedCompetingCountryIds = [ExampleValues.CountryId3Of3, ExampleValues.CountryId2Of3]
    };
}
