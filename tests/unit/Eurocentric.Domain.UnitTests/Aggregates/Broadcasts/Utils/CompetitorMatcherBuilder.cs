using Eurocentric.Domain.Aggregates.Broadcasts;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.Identifiers;

namespace Eurocentric.Domain.UnitTests.Aggregates.Broadcasts.Utils;

public sealed class CompetitorMatcherBuilder
{
    private readonly List<Func<Competitor, bool>> _predicates = [];

    public CompetitorMatcherBuilder HasCompetingCountryId(CountryId countryId)
    {
        _predicates.Add(competitor => competitor.CompetingCountryId == countryId);

        return this;
    }

    public CompetitorMatcherBuilder HasRunningOrderPosition(int runningOrderPosition)
    {
        _predicates.Add(competitor => competitor.RunningOrderPosition == runningOrderPosition);

        return this;
    }

    public CompetitorMatcherBuilder HasFinishingPosition(int finishingPosition)
    {
        _predicates.Add(competitor => competitor.FinishingPosition == finishingPosition);

        return this;
    }

    public CompetitorMatcherBuilder HasNoTelevoteAwards()
    {
        _predicates.Add(competitor => competitor.TelevoteAwards.Count == 0);

        return this;
    }

    public CompetitorMatcherBuilder HasSingleJuryAward(CountryId votingCountryId, PointsValue pointsValue)
    {
        _predicates.Add(competitor => competitor.JuryAwards.Single() is { } award && award.PointsValue == pointsValue &&
                                      award.VotingCountryId == votingCountryId);

        return this;
    }

    public CompetitorMatcherBuilder HasNoJuryAwards()
    {
        _predicates.Add(competitor => competitor.JuryAwards.Count == 0);

        return this;
    }

    public CompetitorMatcherBuilder HasSingleTelevoteAward(CountryId votingCountryId, PointsValue pointsValue)
    {
        _predicates.Add(competitor => competitor.TelevoteAwards.Single() is { } award && award.PointsValue == pointsValue &&
                                      award.VotingCountryId == votingCountryId);

        return this;
    }

    public Func<Competitor, bool> Build() => competitor => _predicates.All(predicate => predicate(competitor));
}
