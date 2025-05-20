using Eurocentric.Features.AdminApi.V1.Common.Constants;
using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.AdminApi.V1.Common.Dtos;

public sealed record Vote(Guid VotingCountryId, bool PointsAwarded) : IExampleProvider<Vote>
{
    public static Vote CreateExample() => new(ExampleValues.CountryId2Of3, true);
}
