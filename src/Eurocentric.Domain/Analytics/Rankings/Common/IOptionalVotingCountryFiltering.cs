namespace Eurocentric.Domain.Analytics.Rankings.Common;

/// <summary>
///     Optional rankings query filtering parameters.
/// </summary>
public interface IOptionalVotingCountryFiltering
{
    /// <summary>
    ///     Filters voting data by voting country ISO 3166-1 alpha-2 country code when not <see langword="null" />.
    /// </summary>
    string? VotingCountryCode { get; }
}
