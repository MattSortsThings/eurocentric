using Eurocentric.Features.AdminApi.V1.Common.Constants;
using Eurocentric.Features.AdminApi.V1.Common.Enums;
using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.AdminApi.V1.Common.Dtos;

public sealed record ContestMemo(Guid ContestId, ContestStatus ContestStatus) : IExampleProvider<ContestMemo>
{
    public static ContestMemo CreateExample() => new(ExampleIds.Contests.Basel2025, ContestStatus.InProgress);
}
