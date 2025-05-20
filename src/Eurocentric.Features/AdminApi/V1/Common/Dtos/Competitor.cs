using Eurocentric.Features.AdminApi.V1.Common.Constants;
using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.AdminApi.V1.Common.Dtos;

public sealed record Competitor : IExampleProvider<Competitor>
{
    public required Guid CompetingCountryId { get; init; }

    public required int RunningOrderPosition { get; init; }

    public required int FinishingPosition { get; init; }

    public required Award[] TelevoteAwards { get; init; }

    public required Award[] JuryAwards { get; init; }

    public static Competitor CreateExample() => new()
    {
        CompetingCountryId = ExampleValues.CountryId1Of3,
        RunningOrderPosition = 9,
        FinishingPosition = 1,
        TelevoteAwards = [Award.CreateExample()],
        JuryAwards = [Award.CreateExample()]
    };
}
