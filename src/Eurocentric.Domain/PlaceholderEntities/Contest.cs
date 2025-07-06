using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.PlaceholderEntities;

public sealed record Contest
{
    private Contest() { }

    public required Guid Id { get; init; }

    public required int ContestYear { get; init; }

    public required string CityName { get; init; }

    public required ContestFormat ContestFormat { get; init; }

    public static Contest CreateStockholmFormat(int contestYear, string cityName) => new()
    {
        Id = Guid.NewGuid(), ContestYear = contestYear, CityName = cityName, ContestFormat = ContestFormat.Stockholm
    };

    public static Contest CreateLiverpoolFormat(int contestYear, string cityName) => new()
    {
        Id = Guid.NewGuid(), ContestYear = contestYear, CityName = cityName, ContestFormat = ContestFormat.Liverpool
    };
}
