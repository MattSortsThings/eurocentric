using Eurocentric.Features.AdminApi.V1.Common.Constants;
using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.AdminApi.V1.Common.Dtos;

public sealed record Voter(Guid VotingCountryId, bool PointsAwarded) : IExampleProvider<Voter>
{
    public static Voter CreateExample() => new(ExampleValues.CountryId2Of3, true);
}
