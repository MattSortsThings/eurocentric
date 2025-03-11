using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.Rules.DataCheckers;

public interface IDataChecker
{
    public bool CountryExistsWithCountryCode(CountryCode countryCode);
}
