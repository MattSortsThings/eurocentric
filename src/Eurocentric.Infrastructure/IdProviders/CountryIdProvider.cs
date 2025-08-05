using Eurocentric.Domain.Aggregates.Countries;
using Eurocentric.Domain.Identifiers;

namespace Eurocentric.Infrastructure.IdProviders;

internal sealed class CountryIdProvider(TimeProvider timeProvider) : ICountryIdProvider
{
    public CountryId CreateSingle() => CountryId.FromValue(Guid.CreateVersion7(timeProvider.GetUtcNow()));
}
