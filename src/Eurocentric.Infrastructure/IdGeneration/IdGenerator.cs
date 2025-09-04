using Eurocentric.Domain.Aggregates.Countries;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Infrastructure.IdGeneration;

/// <summary>
///     Creates aggregate IDs based on the system time.
/// </summary>
internal sealed class IdGenerator(TimeProvider timeProvider) : ICountryIdGenerator
{
    /// <inheritdoc />
    public CountryId CreateSingle() => CountryId.FromValue(Guid.CreateVersion7(timeProvider.GetUtcNow()));
}
