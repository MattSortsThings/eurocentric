using CSharpFunctionalExtensions;

namespace Eurocentric.Domain.Aggregates.Placeholders;

public interface ICountryWriteRepository
{
    void Add(Country country);

    void Remove(Country country);

    Task<Maybe<Country>> GetTrackedAsync(Guid id, CancellationToken cancellationToken = default);
}
