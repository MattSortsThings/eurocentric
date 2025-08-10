using Eurocentric.Features.AdminApi.V1.Common.Constants;
using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.AdminApi.V1.Broadcasts.DisqualifyCompetitor;

public sealed record DisqualifyCompetitorRequest : IExampleProvider<DisqualifyCompetitorRequest>
{
    public required Guid CompetingCountryId { get; init; }

    public static DisqualifyCompetitorRequest CreateExample() => new() { CompetingCountryId = ExampleValues.CountryId1Of3 };
}
