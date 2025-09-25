namespace Eurocentric.Domain.V0.Views;

public sealed record QueryableContest
{
    public int ContestYear { get; init; }

    public string CityName { get; init; } = string.Empty;

    public int Participants { get; init; }

    public bool UsesRestOfWorldTelevote { get; init; }
}
