using Eurocentric.Features.AdminApi.V0.Common.Enums;
using Eurocentric.Features.AdminApi.V0.Common.OpenApi;
using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.AdminApi.V0.Common.Dtos;

public sealed record Contest(Guid Id, int ContestYear, string CityName, ContestFormat ContestFormat) : IExampleProvider<Contest>
{
    public static Contest CreateExample() => new(ExampleValues.ContestId, 2025, "Basel", ContestFormat.Liverpool);
}
