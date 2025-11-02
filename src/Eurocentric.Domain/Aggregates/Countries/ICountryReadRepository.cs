using Eurocentric.Domain.Core;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.Aggregates.Countries;

/// <summary>
///     Read-only repository for <see cref="Country" /> aggregates.
/// </summary>
public interface ICountryReadRepository : IReadRepository<Country, CountryId>;
