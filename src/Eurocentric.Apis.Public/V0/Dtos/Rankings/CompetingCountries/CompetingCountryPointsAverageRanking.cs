using Eurocentric.Components.OpenApi;

namespace Eurocentric.Apis.Public.V0.Dtos.Rankings.CompetingCountries;

public sealed record CompetingCountryPointsAverageRanking
    : IDtoSchemaExampleProvider<CompetingCountryPointsAverageRanking>
{
    public int Rank { get; init; }

    public string CountryCode { get; init; } = string.Empty;

    public string CountryName { get; init; } = string.Empty;

    public decimal PointsAverage { get; init; }

    public int TotalPoints { get; init; }

    public int PointsAwards { get; init; }

    public int Broadcasts { get; init; }

    public int Contests { get; init; }

    public int VotingCountries { get; init; }

    public static CompetingCountryPointsAverageRanking CreateExample() =>
        new()
        {
            Rank = 1,
            CountryCode = "AA",
            CountryName = "CountryName",
            PointsAverage = 9.5m,
            TotalPoints = 1900,
            PointsAwards = 200,
            Broadcasts = 4,
            Contests = 2,
            VotingCountries = 50,
        };
}
