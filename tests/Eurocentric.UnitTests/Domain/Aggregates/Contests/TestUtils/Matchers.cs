using Eurocentric.Domain.Aggregates.Broadcasts;
using Eurocentric.Domain.Aggregates.Contests;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.UnitTests.Domain.Aggregates.Contests.TestUtils;

public static class Matchers
{
    public static Func<Participant, bool> Participant(
        string songTitle = "",
        string actName = "",
        SemiFinalDraw semiFinalDraw = default,
        CountryId participatingCountryId = null!
    )
    {
        return participant =>
            participant.ParticipatingCountryId.Equals(participatingCountryId)
            && participant.SemiFinalDraw == semiFinalDraw
            && participant.ActName.Value.Equals(actName, StringComparison.Ordinal)
            && participant.SongTitle.Value.Equals(songTitle, StringComparison.Ordinal);
    }

    public static Func<Competitor, bool> CompetitorWithNoPointsAwards(
        int finishingPosition = 0,
        int runningOrderSpot = 0,
        CountryId competingCountryId = null!
    )
    {
        return competitor =>
            competitor.RunningOrderSpot.Value == runningOrderSpot
            && competitor.FinishingPosition.Value == finishingPosition
            && competitor.CompetingCountryId.Equals(competingCountryId)
            && competitor.JuryAwards.Count == 0
            && competitor.TelevoteAwards.Count == 0;
    }

    public static Func<Jury, bool> Jury(bool pointsAwarded = true, CountryId votingCountryId = null!) =>
        jury => jury.VotingCountryId.Equals(votingCountryId) && jury.PointsAwarded == pointsAwarded;

    public static Func<Televote, bool> Televote(bool pointsAwarded = true, CountryId votingCountryId = null!) =>
        televote => televote.VotingCountryId.Equals(votingCountryId) && televote.PointsAwarded == pointsAwarded;
}
