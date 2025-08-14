using Eurocentric.Features.PublicApi.V1.Common.Enums;

namespace Eurocentric.Features.PublicApi.V1.Rankings.Common.Queries;

internal interface IContestStageFilteringQuery
{
    public QueryableContestStage ContestStage { get; }
}
