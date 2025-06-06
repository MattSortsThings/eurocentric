using Eurocentric.Domain.Countries;
using Eurocentric.Domain.Identifiers;

namespace Eurocentric.Infrastructure.IdGenerators;

/// <summary>
///     Generates a <see cref="CountryId" /> instance on demand.
/// </summary>
internal sealed class CountryIdGenerator(TimeProvider timeProvider) : ICountryIdGenerator
{
    /// <inheritdoc />
    public CountryId Generate() => CountryId.Create(timeProvider.GetUtcNow());
}
