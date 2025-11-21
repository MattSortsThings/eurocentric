using Eurocentric.Apis.Admin.V1.Config;
using Eurocentric.Apis.Admin.V1.Enums;
using Eurocentric.Components.OpenApi;

namespace Eurocentric.Apis.Admin.V1.Features.Contests;

public sealed record CreateParticipantRequest : IDtoSchemaExampleProvider<CreateParticipantRequest>
{
    public required Guid ParticipatingCountryId { get; init; }

    public required SemiFinalDraw SemiFinalDraw { get; init; }

    public required string ActName { get; init; }

    public required string SongTitle { get; init; }

    public static CreateParticipantRequest CreateExample() =>
        new()
        {
            ParticipatingCountryId = V1ExampleIds.CountryA,
            ActName = "JJ",
            SongTitle = "Wasted Love",
            SemiFinalDraw = SemiFinalDraw.SemiFinal2,
        };

    public bool Equals(CreateParticipantRequest? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return ParticipatingCountryId.Equals(other.ParticipatingCountryId)
            && SemiFinalDraw == other.SemiFinalDraw
            && ActName == other.ActName
            && SongTitle == other.SongTitle;
    }

    public override int GetHashCode() =>
        HashCode.Combine(ParticipatingCountryId, (int)SemiFinalDraw, ActName, SongTitle);

    public void Deconstruct(
        out Guid participatingCountryId,
        out SemiFinalDraw semiFinalDraw,
        out string actName,
        out string songTitle
    )
    {
        participatingCountryId = ParticipatingCountryId;
        semiFinalDraw = SemiFinalDraw;
        actName = ActName;
        songTitle = SongTitle;
    }
}
