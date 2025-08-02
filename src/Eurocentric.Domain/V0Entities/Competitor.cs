namespace Eurocentric.Domain.V0Entities;

public sealed record Competitor
{
    public Guid CompetingCountryId { get; init; }

    public int FinishingPosition { get; init; }

    public int RunningOrderPosition { get; init; }

    public IList<JuryAward> JuryAwards { get; init; } = [];

    public IList<TelevoteAward> TelevoteAwards { get; init; } = [];
}
