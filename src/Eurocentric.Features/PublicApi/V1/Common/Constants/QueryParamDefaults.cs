using Eurocentric.Features.PublicApi.V1.Common.Enums;

namespace Eurocentric.Features.PublicApi.V1.Common.Constants;

internal static class QueryParamDefaults
{
    internal const QueryableContestStage ContestStage = QueryableContestStage.Any;
    internal const QueryableVotingMethod VotingMethod = QueryableVotingMethod.Any;
    internal const int PageIndex = 0;
    internal const int PageSize = 10;
    internal const bool Descending = false;
}
