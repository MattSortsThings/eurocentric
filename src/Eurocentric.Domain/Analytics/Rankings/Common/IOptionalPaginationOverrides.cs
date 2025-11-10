namespace Eurocentric.Domain.Analytics.Rankings.Common;

/// <summary>
///     Optional rankings query pagination overrides.
/// </summary>
public interface IOptionalPaginationOverrides
{
    /// <summary>
    ///     Sets the zero-based pagination page index when not <see langword="null" />.
    /// </summary>
    int? PageIndex { get; }

    /// <summary>
    ///     Sets the pagination page size Sets the zero-based pagination page index when not <see langword="null" />.
    /// </summary>
    int? PageSize { get; }

    /// <summary>
    ///     Sets the pre-pagination initial sort to descending rank (<see langword="true" />) or ascending rank (
    ///     <see langword="false" />) when not <see langword="null" />.
    /// </summary>
    bool? Descending { get; }
}
