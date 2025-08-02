using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.V0Entities;

public sealed record Contest
{
    public Guid Id { get; init; }

    public int ContestYear { get; init; }

    public string CityName { get; init; } = string.Empty;

    public ContestFormat ContestFormat { get; init; }

    public bool Completed { get; init; }

    public IList<ChildBroadcast> ChildBroadcasts { get; init; } = [];

    public IList<Participant> Participants { get; init; } = [];

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
            ContestFormat = ContestFormat.Stockholm,
            Participants = participants,
            Completed = false,
            ChildBroadcasts = []
        };
    }
}
