using Eurocentric.Features.PublicApi.V0.Common.Enums;
using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.PublicApi.V0.Queryables.GetQueryableContestStages;

public sealed record GetQueryableContestStagesResponse(QueryableContestStage[] QueryableContestStages)
    : IExampleProvider<GetQueryableContestStagesResponse>
{
    public static GetQueryableContestStagesResponse CreateExample() => new(Enum.GetValues<QueryableContestStage>());
}
