using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.Analytics.Rankings.Common;

/// <summary>
///     Optional rankings query filtering parameters.
/// </summary>
public interface IOptionalBroadcastFiltering
{
    /// <summary>
    ///     Filters voting data by inclusive minimum contest year when not <see langword="null" />.
    /// </summary>
    int? MinYear { get; }

    /// <summary>
    ///     Filters voting data by inclusive maximum contest year when not <see langword="null" />.
    /// </summary>
    int? MaxYear { get; }

    /// <summary>
    ///     Filters voting data by contest stage when not <see langword="null" />.
    /// </summary>
    ContestStageFilter? ContestStage { get; }
}
