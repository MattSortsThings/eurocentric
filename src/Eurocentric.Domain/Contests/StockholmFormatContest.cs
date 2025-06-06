using Eurocentric.Domain.Enums;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.Contests;

/// <summary>
///     Represents a contest aggregate using the <see cref="Enums.ContestFormat.Stockholm" /> contest format.
/// </summary>
public sealed class StockholmFormatContest : Contest
{
    private StockholmFormatContest()
    {
    }

    private StockholmFormatContest(ContestId id,
        ContestYear contestYear,
        CityName cityName,
        List<Participant> participants) :
        base(id, contestYear, cityName, participants)
    {
    }

    /// <inheritdoc />
    public override ContestFormat ContestFormat { get; private protected init; } = ContestFormat.Stockholm;

    public static StockholmFormatContest Create(ContestYear contestYear,
        CityName cityName,
        IEnumerable<CountryId> group1CountryIds,
        IEnumerable<CountryId> group2CountryIds)
    {
        List<Participant> participants = new();

        foreach (CountryId id in group1CountryIds)
        {
            participants.Add(Participant.CreateInGroup1(id,
                ActName.FromValue("Act " + id.Value),
                SongTitle.FromValue("Song " + id.Value)).Value);
        }

        foreach (CountryId id in group2CountryIds)
        {
            participants.Add(Participant.CreateInGroup2(id,
                ActName.FromValue("Act " + id.Value),
                SongTitle.FromValue("Song " + id.Value)).Value);
        }

        return new StockholmFormatContest(ContestId.Create(DateTimeOffset.UtcNow),
            contestYear,
            cityName, participants);
    }
}
