using Eurocentric.Features.AdminApi.V1.Common.Constants;
using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.AdminApi.V1.Common.Dtos;

public sealed record Award(Guid VotingCountryId, int PointsValue) : IExampleProvider<Award>
{
    public static Award CreateExample() => new(ExampleValues.CountryId2Of3, 12);
}
