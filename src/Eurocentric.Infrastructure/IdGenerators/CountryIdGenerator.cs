using Eurocentric.Domain.Aggregates.Countries;
using Eurocentric.Domain.Identifiers;

namespace Eurocentric.Infrastructure.IdGenerators;

/// <summary>
///     Generates <see cref="CountryId" /> value objects.
/// </summary>
/// <param name="timeProvider">Provides the system time.</param>
internal sealed class CountryIdGenerator(TimeProvider timeProvider) : ICountryIdGenerator
{
    /// <inheritdoc />
    public CountryId CreateSingle() => CountryId.Create(timeProvider.GetUtcNow());
}
