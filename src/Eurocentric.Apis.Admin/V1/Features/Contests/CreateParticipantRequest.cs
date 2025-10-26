using Eurocentric.Apis.Admin.V1.Enums;

namespace Eurocentric.Apis.Admin.V1.Features.Contests;

public sealed record CreateParticipantRequest
{
    public required Guid ParticipatingCountryId { get; init; }

    public required SemiFinalDraw SemiFinalDraw { get; init; }

    public required string ActName { get; init; }

    public required string SongTitle { get; init; }

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
