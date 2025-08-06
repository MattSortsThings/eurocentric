using Eurocentric.Features.AdminApi.V1.Common.Constants;
using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.AdminApi.V1.Common.Dtos;

public sealed record Voter : IExampleProvider<Voter>
{
    public required Guid VotingCountryId { get; init; }

    public required bool PointsAwarded { get; init; }

    public static Voter CreateExample() => new() { VotingCountryId = ExampleValues.CountryId2Of3, PointsAwarded = true };
}
