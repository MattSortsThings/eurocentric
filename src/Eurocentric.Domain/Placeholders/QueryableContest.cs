namespace Eurocentric.Domain.Placeholders;

public sealed record QueryableContest
{
    public int ContestYear { get; init; }

    public string CityName { get; init; } = string.Empty;
}
