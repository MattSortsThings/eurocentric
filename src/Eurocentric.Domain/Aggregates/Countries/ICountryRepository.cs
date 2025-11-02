namespace Eurocentric.Domain.Aggregates.Countries;

/// <summary>
///     Repository for <see cref="Country" /> aggregates.
/// </summary>
public interface ICountryRepository : ICountryReadRepository, ICountryWriteRepository;
