using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.V0Analytics.Rankings.Common;

public interface IContestStagesFilter
{
    ContestStage[]? ContestStages { get; }
}
