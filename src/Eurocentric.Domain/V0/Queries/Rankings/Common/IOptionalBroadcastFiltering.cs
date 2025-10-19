using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.V0.Queries.Rankings.Common;

public interface IOptionalBroadcastFiltering
{
    int? MinYear { get; }

    int? MaxYear { get; }

    ContestStageFilter? ContestStage { get; }
}
