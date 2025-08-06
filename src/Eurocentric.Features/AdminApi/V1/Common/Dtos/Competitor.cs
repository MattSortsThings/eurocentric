using Eurocentric.Features.AdminApi.V1.Common.Constants;
using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.AdminApi.V1.Common.Dtos;

public sealed record Competitor : IExampleProvider<Competitor>
{
    public required Guid CompetingCountryId { get; init; }

    public required int RunningOrderPosition { get; init; }

    public required int FinishingPosition { get; init; }

    public required Award[] JuryAwards { get; init; }

    public required Award[] TelevoteAwards { get; init; }

    public static Competitor CreateExample()
    {
        Award[] awards = [Award.CreateExample()];

        return new Competitor
        {
            CompetingCountryId = ExampleValues.CountryId1Of3,
            RunningOrderPosition = 1,
            FinishingPosition = 1,
            JuryAwards = awards,
            TelevoteAwards = awards
        };
    }
}
