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
    ///     Specifies descending rank (<see langword="true" />) or ascending rank (<see langword="false" />) initial sort
    ///     before pagination when not <see langword="null" />.
    /// </summary>
    bool? Descending { get; }
}
