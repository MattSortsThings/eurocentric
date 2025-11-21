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

    public bool Equals(CreateContestRequest? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return ContestRules == other.ContestRules
            && ContestYear == other.ContestYear
            && CityName == other.CityName
            && Nullable.Equals(GlobalTelevoteVotingCountryId, other.GlobalTelevoteVotingCountryId)
            && Participants
                .OrderBy(participant => participant.ParticipatingCountryId)
                .SequenceEqual(other.Participants.OrderBy(participant => participant.ParticipatingCountryId));
    }

    public override int GetHashCode() =>
        HashCode.Combine((int)ContestRules, ContestYear, CityName, GlobalTelevoteVotingCountryId, Participants);
}
