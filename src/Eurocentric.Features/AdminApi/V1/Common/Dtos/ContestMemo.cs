using Eurocentric.Features.AdminApi.V1.Common.Constants;
using Eurocentric.Features.AdminApi.V1.Common.Enums;
using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.AdminApi.V1.Common.Dtos;

public sealed record ContestMemo(Guid ContestId, ContestStatus Status) : IExampleProvider<ContestMemo>
{
    public static ContestMemo CreateExample() => new(ExampleValues.ContestId, ContestStatus.InProgress);
}
