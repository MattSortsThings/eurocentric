using Eurocentric.Components.OpenApi;

namespace Eurocentric.Apis.Public.V1.Dtos.Rankings.VotingCountries;

public sealed record VotingCountryPointsAverageRanking : ISchemaExampleProvider<VotingCountryPointsAverageRanking>
{
    public int Rank { get; init; }

    public string CountryCode { get; init; } = string.Empty;

    public string CountryName { get; init; } = string.Empty;

    public decimal PointsAverage { get; init; }

    public int TotalPoints { get; init; }

    public int PointsAwards { get; init; }

    public int Broadcasts { get; init; }

    public int Contests { get; init; }

    public static VotingCountryPointsAverageRanking CreateExample() =>
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
        };
}
