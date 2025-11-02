using Eurocentric.Domain.Core;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.Aggregates.Countries;

/// <summary>
///     Read-write repository for <see cref="Country" /> aggregates.
/// </summary>
public interface ICountryWriteRepository : IWriteRepository<Country, CountryId>;
