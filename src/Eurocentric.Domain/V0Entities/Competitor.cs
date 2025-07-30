namespace Eurocentric.Domain.V0Entities;

public sealed record Competitor
{
    public required Guid CompetingCountryId { get; init; }

    public required int FinishingPosition { get; init; }

    public required int RunningOrderPosition { get; init; }

    public required IList<JuryAward> JuryAwards { get; init; }

    public required IList<TelevoteAward> TelevoteAwards { get; init; }
}
