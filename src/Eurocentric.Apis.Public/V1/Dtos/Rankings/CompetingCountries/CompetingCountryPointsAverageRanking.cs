using Eurocentric.Components.OpenApi;

namespace Eurocentric.Apis.Public.V1.Dtos.Rankings.CompetingCountries;

public sealed record CompetingCountryPointsAverageRanking : ISchemaExampleProvider<CompetingCountryPointsAverageRanking>
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
            CountryCode = "AT",
            CountryName = "Austria",
            PointsAverage = 7.0m,
            TotalPoints = 28,
            PointsAwards = 4,
            Broadcasts = 2,
            Contests = 2,
            VotingCountries = 1,
        };
}
