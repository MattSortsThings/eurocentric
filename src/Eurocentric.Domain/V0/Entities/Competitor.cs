namespace Eurocentric.Domain.V0.Entities;

public sealed record Competitor
{
    public Guid CompetingCountryId { get; init; }

    public int RunningOrderPosition { get; init; }

    public int FinishingPosition { get; init; }

    public List<JuryAward> JuryAwards { get; init; } = [];

    public List<TelevoteAward> TelevoteAwards { get; init; } = [];
}
