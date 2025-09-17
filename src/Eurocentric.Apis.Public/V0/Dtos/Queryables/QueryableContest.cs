namespace Eurocentric.Apis.Public.V0.Dtos.Queryables;

public sealed record QueryableContest
{
    public int ContestYear { get; init; }

    public string CityName { get; init; } = string.Empty;

    public int ParticipatingCountries { get; init; }

    public bool HasGlobalTelevote { get; init; }
}
