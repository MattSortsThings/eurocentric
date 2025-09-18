namespace Eurocentric.Apis.Public.V0.Dtos.Scoreboards;

public sealed record ScoreboardRow
{
    public int FinishingPosition { get; init; }

    public int RunningOrderSpot { get; init; }

    public string CountryCode { get; init; } = string.Empty;

    public string CountryName { get; init; } = string.Empty;

    public string ActName { get; init; } = string.Empty;

    public string SongTitle { get; init; } = string.Empty;

    public int? JuryPoints { get; init; }

    public int TelevotePoints { get; init; }

    public int OverallPoints { get; init; }
}
