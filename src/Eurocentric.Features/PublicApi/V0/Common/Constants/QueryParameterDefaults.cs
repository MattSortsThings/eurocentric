using Eurocentric.Features.PublicApi.V0.Common.Contracts;

namespace Eurocentric.Features.PublicApi.V0.Common.Constants;

internal static class QueryParameterDefaults
{
    internal const int PageIndex = 0;
    internal const int PageSize = 10;
    internal const bool Descending = false;
    internal const ContestStageFilter ContestStage = ContestStageFilter.Any;
    internal const VotingMethodFilter VotingMethod = VotingMethodFilter.Any;
}
