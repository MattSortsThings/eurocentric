namespace Eurocentric.Features.AdminApi.V0.Common.Dtos;

public sealed record Participant
{
    public Guid ParticipatingCountryId { get; init; }

    public int ParticipantGroup { get; init; }

    public string? ActName { get; init; }

    public string? SongTitle { get; init; }
}
