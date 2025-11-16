namespace Eurocentric.Domain.Analytics.Listings;

/// <summary>
///     Parameters for a competing country result listings query.
/// </summary>
public abstract record CompetingCountryResultQuery : IRequiredCompetingCountryFiltering
{
    /// <inheritdoc />
    public required string CompetingCountryCode { get; init; }
}
