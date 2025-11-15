namespace Eurocentric.Domain.Analytics.Listings;

/// <summary>
///     Required listings query filtering parameters.
/// </summary>
public interface IRequiredCompetingCountryFiltering
{
    /// <summary>
    ///     Filters voting data by competing country ISO 3166-1 alpha-2 country code.
    /// </summary>
    string CompetingCountryCode { get; }
}
