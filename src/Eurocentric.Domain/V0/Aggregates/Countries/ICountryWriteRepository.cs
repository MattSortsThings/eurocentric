using CSharpFunctionalExtensions;
using Eurocentric.Domain.Functional;

namespace Eurocentric.Domain.V0.Aggregates.Countries;

public interface ICountryWriteRepository
{
    void Add(Country country);

    void Remove(Country country);

    Task<Result<Country, IDomainError>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
