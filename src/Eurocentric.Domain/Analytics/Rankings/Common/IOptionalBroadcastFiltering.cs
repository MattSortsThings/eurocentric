using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.Analytics.Rankings.Common;

public interface IOptionalBroadcastFiltering
{
    int? MinYear { get; }

    int? MaxYear { get; }

    ContestStageFilter? ContestStage { get; }
}
