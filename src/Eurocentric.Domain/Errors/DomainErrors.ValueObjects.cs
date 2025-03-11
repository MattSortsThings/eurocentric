using ErrorOr;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.Errors;

public static partial class DomainErrors
{
    public static class ValueObjects
    {
        public static class CountryCodes
        {
            public static Error InvalidCountryCode(CountryCode countryCode) => Error.Failure("Invalid country code",
                "Country code value must be a string of 2 upper-case letters.",
                new Dictionary<string, object> { [nameof(countryCode)] = countryCode.Value });
        }

        public static class CountryNames
        {
            public static Error InvalidCountryName(CountryName countryName) => Error.Failure("Invalid country name",
                "Country name value must be a non-empty, non-white-space string of no more than 200 characters.",
                new Dictionary<string, object> { [nameof(countryName)] = countryName.Value });
        }
    }
}
