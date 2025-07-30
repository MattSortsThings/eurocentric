using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.V0Entities;

public sealed record Contest
{
    public required Guid Id { get; init; }

    public required int ContestYear { get; init; }

    public required string CityName { get; init; }

    public required ContestFormat ContestFormat { get; init; }

    public required bool Completed { get; init; }

    public required IList<ChildBroadcast> ChildBroadcasts { get; init; }

    public required IList<Participant> Participants { get; init; }

    public static Contest CreateLiverpoolFormat(int contestYear, string cityName, IList<Guid> participatingCountryIds)
    {
        Participant[] participants = participatingCountryIds.Skip(1)
            .Select((id, index) => new Participant
            {
                ParticipatingCountryId = id,
                ParticipantGroup = index % 2 == 0 ? ParticipantGroup.Two : ParticipantGroup.One,
                ActName = "ActName",
                SongTitle = "SongTitle"
            }).Prepend(new Participant
            {
                ParticipatingCountryId = participatingCountryIds[0],
                ParticipantGroup = ParticipantGroup.Zero,
                ActName = null,
                SongTitle = null
            }).ToArray();

        return new Contest
        {
            Id = Guid.NewGuid(),
            ContestYear = contestYear,
            CityName = cityName,
            ContestFormat = ContestFormat.Liverpool,
            Participants = participants,
            Completed = false,
            ChildBroadcasts = []
        };
    }

    public static Contest CreateStockholmFormat(int contestYear, string cityName, IList<Guid> participatingCountryIds)
    {
        Participant[] participants = participatingCountryIds
            .Select((id, index) => new Participant
            {
                ParticipatingCountryId = id,
                ParticipantGroup = index % 2 == 0 ? ParticipantGroup.Two : ParticipantGroup.One,
                ActName = "ActName",
                SongTitle = "SongTitle"
            }).ToArray();

        return new Contest
        {
            Id = Guid.NewGuid(),
            ContestYear = contestYear,
            CityName = cityName,
            ContestFormat = ContestFormat.Liverpool,
            Participants = participants,
            Completed = false,
            ChildBroadcasts = []
        };
    }
}
