using Eurocentric.Apis.Admin.V1.Enums;

namespace Eurocentric.Apis.Admin.V1.Features.Contests;

public sealed record CreateContestRequest
{
    public required ContestRules ContestRules { get; init; }

    public required int ContestYear { get; init; }

    public required string CityName { get; init; }

    public Guid? GlobalTelevoteVotingCountryId { get; init; }

    public required CreateParticipantRequest[] Participants { get; init; }
}
