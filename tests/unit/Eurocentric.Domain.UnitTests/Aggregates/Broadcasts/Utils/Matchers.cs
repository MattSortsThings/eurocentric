namespace Eurocentric.Domain.UnitTests.Aggregates.Broadcasts.Utils;

public static class Matchers
{
    public static CompetitorMatcherBuilder Competitor() => new();

    public static VoterMatcherBuilder Televote() => new();
}
