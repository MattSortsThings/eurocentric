using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.Placeholders;

public sealed record QueryableCompetitor
{
    public int ContestYear { get; init; }

    public ContestStage ContestStage { get; init; }

    public int RunningOrderPosition { get; init; }

    public string CompetingCountryCode { get; init; } = string.Empty;

    public string ActName { get; init; } = string.Empty;

    public string SongTitle { get; init; } = string.Empty;

    public int FinishingPosition { get; init; }
}
