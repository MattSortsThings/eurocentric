using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.Analytics.Listings;

/// <summary>
///     Required listings query filtering parameters.
/// </summary>
public interface IRequiredBroadcastFiltering
{
    /// <summary>
    ///     Filters voting data by contest year.
    /// </summary>
    int ContestYear { get; }

    /// <summary>
    ///     Filters voting data by contest stage.
    /// </summary>
    ContestStage ContestStage { get; }
}
