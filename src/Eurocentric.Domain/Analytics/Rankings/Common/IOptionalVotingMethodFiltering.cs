using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.Analytics.Rankings.Common;

/// <summary>
///     Optional rankings query filtering parameters.
/// </summary>
public interface IOptionalVotingMethodFiltering
{
    /// <summary>
    ///     Filters voting data by voting method when not <see langword="null" />.
    /// </summary>
    VotingMethodFilter? VotingMethod { get; }
}
