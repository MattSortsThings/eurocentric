using Eurocentric.Features.AdminApi.V1.Common.Constants;
using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.AdminApi.V1.Common.Dtos;

public sealed record Participant : IExampleProvider<Participant>
{
    public required Guid ParticipatingCountryId { get; init; }

    public required int ParticipantGroup { get; init; }

    public string? ActName { get; init; }

    public string? SongTitle { get; init; }

    public static Participant CreateExample() => new()
    {
        ParticipatingCountryId = ExampleValues.CountryId, ParticipantGroup = 2, ActName = "JJ", SongTitle = "Wasted Love"
    };
}
