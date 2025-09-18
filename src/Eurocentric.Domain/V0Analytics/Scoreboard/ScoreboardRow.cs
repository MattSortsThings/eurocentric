namespace Eurocentric.Domain.V0Analytics.Scoreboard;

public sealed record ScoreboardRow
{
    public int FinishingPosition { get; init; }

    public int RunningOrderSpot { get; init; }

    public string CountryCode { get; init; } = string.Empty;

    public string CountryName { get; init; } = string.Empty;

    public string ActName { get; init; } = string.Empty;

    public string SongTitle { get; init; } = string.Empty;

    public int OverallPoints { get; init; }

    public int? JuryPoints { get; init; }

    public int TelevotePoints { get; init; }
}
