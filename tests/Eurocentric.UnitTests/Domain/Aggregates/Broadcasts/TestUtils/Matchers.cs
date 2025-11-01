using Eurocentric.Domain.Aggregates.Broadcasts;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.UnitTests.Domain.Aggregates.Broadcasts.TestUtils;

public static class Matchers
{
    public static CompetitorMatcher Competitor() => new();

    public static JuryMatcher Jury() => new();

    public static TelevoteMatcher Televote() => new();

    public sealed class CompetitorMatcher
    {
        private Func<Competitor, bool> Predicate { get; set; } = _ => true;

        public bool Match(Competitor competitor) => Predicate(competitor);

        public CompetitorMatcher HasCompetingCountryId(CountryId countryId)
        {
            Func<Competitor, bool> existingPredicate = Predicate;

            Predicate = competitor => competitor.CompetingCountryId.Equals(countryId) && existingPredicate(competitor);

            return this;
        }

        public CompetitorMatcher HasNoJuryAwards()
        {
            Func<Competitor, bool> existingPredicate = Predicate;

            Predicate = competitor => competitor.JuryAwards.Count == 0 && existingPredicate(competitor);

            return this;
        }

        public CompetitorMatcher HasNoTelevoteAwards()
        {
            Func<Competitor, bool> existingPredicate = Predicate;

            Predicate = competitor => competitor.TelevoteAwards.Count == 0 && existingPredicate(competitor);

            return this;
        }

        public CompetitorMatcher HasTelevoteAward(CountryId votingCountryId, PointsValue pointsValue)
        {
            Func<Competitor, bool> existingPredicate = Predicate;

            Predicate = competitor =>
                competitor.TelevoteAwards.Any(award =>
                    award.VotingCountryId.Equals(votingCountryId) && award.PointsValue == pointsValue
                ) && existingPredicate(competitor);

            return this;
        }

        public CompetitorMatcher HasSingleTelevoteAward(CountryId votingCountryId)
        {
            Func<Competitor, bool> existingPredicate = Predicate;

            Predicate = competitor =>
                competitor.TelevoteAwards.Single() is var singleAward
                && singleAward.VotingCountryId.Equals(votingCountryId)
                && existingPredicate(competitor);

            return this;
        }

        public CompetitorMatcher HasSingleTelevoteAward(CountryId votingCountryId, PointsValue pointsValue)
        {
            Func<Competitor, bool> existingPredicate = Predicate;

            Predicate = competitor =>
                competitor.TelevoteAwards.Single() is var singleAward
                && singleAward.VotingCountryId.Equals(votingCountryId)
                && singleAward.PointsValue == pointsValue
                && existingPredicate(competitor);

            return this;
        }

        public CompetitorMatcher HasRunningOrderSpot(int value)
        {
            Func<Competitor, bool> existingPredicate = Predicate;

            Predicate = competitor => competitor.RunningOrderSpot.Value == value && existingPredicate(competitor);

            return this;
        }

        public CompetitorMatcher HasFinishingPosition(int value)
        {
            Func<Competitor, bool> existingPredicate = Predicate;

            Predicate = competitor => competitor.FinishingPosition.Value == value && existingPredicate(competitor);

            return this;
        }
    }

    public sealed class JuryMatcher
    {
        private Func<Jury, bool> Predicate { get; set; } = _ => true;

        public bool Match(Jury voter) => Predicate(voter);

        public JuryMatcher HasVotingCountryId(CountryId votingCountryId)
        {
            Func<Jury, bool> existingPredicate = Predicate;

            Predicate = jury => jury.VotingCountryId.Equals(votingCountryId) && existingPredicate(jury);

            return this;
        }

        public JuryMatcher PointsAwarded()
        {
            Func<Jury, bool> existingPredicate = Predicate;

            Predicate = jury => jury.PointsAwarded && existingPredicate(jury);

            return this;
        }

        public JuryMatcher PointsNotAwarded()
        {
            Func<Jury, bool> existingPredicate = Predicate;

            Predicate = jury => !jury.PointsAwarded && existingPredicate(jury);

            return this;
        }
    }

    public sealed class TelevoteMatcher
    {
        private Func<Televote, bool> Predicate { get; set; } = _ => true;

        public bool Match(Televote voter) => Predicate(voter);

        public TelevoteMatcher HasVotingCountryId(CountryId votingCountryId)
        {
            Func<Televote, bool> existingPredicate = Predicate;

            Predicate = televote => televote.VotingCountryId.Equals(votingCountryId) && existingPredicate(televote);

            return this;
        }

        public TelevoteMatcher PointsAwarded()
        {
            Func<Televote, bool> existingPredicate = Predicate;

            Predicate = televote => televote.PointsAwarded && existingPredicate(televote);

            return this;
        }

        public TelevoteMatcher PointsNotAwarded()
        {
            Func<Televote, bool> existingPredicate = Predicate;

            Predicate = televote => !televote.PointsAwarded && existingPredicate(televote);

            return this;
        }
    }
}
