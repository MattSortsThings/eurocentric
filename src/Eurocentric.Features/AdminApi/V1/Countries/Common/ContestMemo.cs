using Eurocentric.Features.AdminApi.V1.Common.Enums;
using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.AdminApi.V1.Countries.Common;

public sealed record ContestMemo(Guid ContestId, ContestStatus Status) : IExampleProvider<ContestMemo>
{
    public static ContestMemo CreateExample() =>
        new(Guid.Parse("ff0c1d46-8031-42e8-8b7d-d33552623957"), ContestStatus.InProgress);
}
