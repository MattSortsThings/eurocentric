using Eurocentric.Domain.Aggregates.Countries;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Components.IdFactories;

internal sealed class CountryIdFactory(TimeProvider timeProvider) : ICountryIdFactory
{
    public CountryId Create() => CountryId.FromValue(Guid.CreateVersion7(timeProvider.GetUtcNow()));
}
