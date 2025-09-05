using Eurocentric.Domain.Enums;
using Eurocentric.Domain.ValueObjects;
using JetBrains.Annotations;

namespace Eurocentric.Domain.Aggregates.Contests;

/// <summary>
///     Represents a "Liverpool" format contest.
/// </summary>
public sealed class LiverpoolFormatContest : Contest
{
    [UsedImplicitly(Reason = "EF Core")]
    private LiverpoolFormatContest()
    {
        ContestFormat = ContestFormat.Liverpool;
    }

    public LiverpoolFormatContest(ContestId id,
        ContestYear contestYear,
        CityName cityName,
        List<Participant> participants,
        GlobalTelevote globalTelevote) : base(id, contestYear, cityName, participants, globalTelevote)
    {
        ContestFormat = ContestFormat.Liverpool;
    }

    /// <inheritdoc />
    public override ContestFormat ContestFormat { get; private protected init; }

    public static LiverpoolFormatContest CreateDummyContest(Guid contestId)
    {
        ContestYear contestYear = ContestYear.FromValue(2025).Value;
        CityName cityName = CityName.FromValue("Basel").Value;

        GlobalTelevote televote = new(CountryId.FromValue(Guid.NewGuid()));

        List<Participant> participants = Enumerable.Range(0, 6)
            .Select(_ => Guid.NewGuid())
            .Select((id, index) =>
            {
                CountryId countryId = CountryId.FromValue(id);
                SemiFinalDraw semiFinalDraw = index % 2 == 0 ? SemiFinalDraw.SemiFinal1 : SemiFinalDraw.SemiFinal2;
                ActName actName = ActName.FromValue(id + " act").Value;
                SongTitle songTitle = SongTitle.FromValue(id + " song").Value;

                return new Participant(countryId, semiFinalDraw, actName, songTitle);
            }).ToList();

        return new LiverpoolFormatContest(ContestId.FromValue(contestId), contestYear, cityName, participants, televote);
    }
}
