using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.Placeholders;

public sealed record Contest(Guid Id, int ContestYear, string CityName, ContestFormat ContestFormat)
{
    public static Contest CreateStockholmFormat(int contestYear, string cityName) =>
        new(Guid.NewGuid(), contestYear, cityName, ContestFormat.Stockholm);

    public static Contest CreateLiverpoolFormat(int contestYear, string cityName) =>
        new(Guid.NewGuid(), contestYear, cityName, ContestFormat.Liverpool);
}
