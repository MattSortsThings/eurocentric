namespace Eurocentric.Apis.Public.V0.Contracts.Queryables.Dtos;

public sealed record QueryableContest
{
    public int ContestYear { get; init; }

    public string CityName { get; init; } = string.Empty;

    public int Participants { get; init; }

    public bool UsesRestOfWorldTelevote { get; init; }
}
