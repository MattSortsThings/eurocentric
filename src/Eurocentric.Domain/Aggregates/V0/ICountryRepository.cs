using CSharpFunctionalExtensions;
using Eurocentric.Domain.Abstractions.Errors;

namespace Eurocentric.Domain.Aggregates.V0;

public interface ICountryRepository : ICountryReadRepository
{
    void Add(Country country);

    void Update(Country country);

    void Remove(Country country);

    Task<Result<Country, IDomainError>> GetTrackedAsync(Guid countryId, CancellationToken cancellationToken = default);
}
