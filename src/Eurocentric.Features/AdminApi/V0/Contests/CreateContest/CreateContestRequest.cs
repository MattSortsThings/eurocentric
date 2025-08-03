using Eurocentric.Features.AdminApi.V0.Common.Constants;
using Eurocentric.Features.AdminApi.V0.Common.Enums;
using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.AdminApi.V0.Contests.CreateContest;

public sealed record CreateContestRequest : IExampleProvider<CreateContestRequest>
{
    public required int ContestYear { get; init; }

    public required string CityName { get; init; }

    public required ContestFormat ContestFormat { get; init; }

    public required Guid[] ParticipatingCountryIds { get; init; }

    public static CreateContestRequest CreateExample() => new()
    {
        ContestYear = 2025,
        CityName = "Basel",
        ContestFormat = ContestFormat.Liverpool,
        ParticipatingCountryIds =
        [
            ExampleValues.CountryId
        ]
    };
}
