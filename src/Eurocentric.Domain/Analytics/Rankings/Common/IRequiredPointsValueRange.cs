namespace Eurocentric.Domain.Analytics.Rankings.Common;

/// <summary>
///     Required points in range rankings query values.
/// </summary>
public interface IRequiredPointsValueRange
{
    /// <summary>
    ///     Specifies the inclusive minimum points value.
    /// </summary>
    int MinPoints { get; }

    /// <summary>
    ///     Specifies the inclusive maximum points value.
    /// </summary>
    int MaxPoints { get; }
}
