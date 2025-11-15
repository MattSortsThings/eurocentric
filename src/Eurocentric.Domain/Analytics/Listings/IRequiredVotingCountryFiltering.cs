namespace Eurocentric.Domain.Analytics.Listings;

/// <summary>
///     Required listings query filtering parameters.
/// </summary>
public interface IRequiredVotingCountryFiltering
{
    /// <summary>
    ///     Filters voting data by voting country ISO 3166-1 alpha-2 country code.
    /// </summary>
    string VotingCountryCode { get; }
}
