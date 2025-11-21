using Eurocentric.Apis.Admin.V1.Config;
using Eurocentric.Apis.Admin.V1.Enums;
using Eurocentric.Components.OpenApi;

namespace Eurocentric.Apis.Admin.V1.Features.Contests;

public sealed record CreateContestRequest : IDtoSchemaExampleProvider<CreateContestRequest>
{
    public required ContestRules ContestRules { get; init; }

    public required int ContestYear { get; init; }

    public required string CityName { get; init; }

    public Guid? GlobalTelevoteVotingCountryId { get; init; }

    public required CreateParticipantRequest[] Participants { get; init; }

    public static CreateContestRequest CreateExample() =>
        new()
        {
            ContestYear = 2025,
            ContestRules = ContestRules.Liverpool,
            CityName = "Basel",
            GlobalTelevoteVotingCountryId = V1ExampleIds.CountryC,
            Participants = [CreateParticipantRequest.CreateExample()],
        };
}
