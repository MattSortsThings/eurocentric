using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.AdminApi.V0.Contests.Common;

public sealed record Contest(Guid Id, int ContestYear, string CityName, ContestFormat ContestFormat) : IExampleProvider<Contest>
{
    public static Contest CreateExample() => new(Guid.Parse("ff0c1d46-8031-42e8-8b7d-d33552623957"),
        2025,
        "Basel",
        ContestFormat.Liverpool);
}
