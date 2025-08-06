using Eurocentric.Features.AdminApi.V1.Common.Constants;
using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.AdminApi.V1.Common.Dtos;

public sealed record Award : IExampleProvider<Award>
{
    public required Guid VotingCountryId { get; init; }

    public required int PointsValue { get; init; }

    public static Award CreateExample() => new() { VotingCountryId = ExampleValues.CountryId2Of3, PointsValue = 12 };
}
