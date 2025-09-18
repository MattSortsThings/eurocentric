namespace Eurocentric.Domain.V0Analytics.Scoreboard;

public readonly record struct Scoreboard(ScoreboardRow[] Rows, ScoreboardMetadata Metadata);
