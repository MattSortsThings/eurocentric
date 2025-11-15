using Eurocentric.Domain.Core;

namespace Eurocentric.Domain.Analytics.Listings;

public static class ListingsErrors
{
    public static UnprocessableError IllegalCompetingCountryCodeValue(string competingCountryCode)
    {
        return new UnprocessableError
        {
            Title = "Illegal competing country code value",
            Detail = "Competing country code value must be a string of 2 upper-case letters.",
            Extensions = new Dictionary<string, object?> { { nameof(competingCountryCode), competingCountryCode } },
        };
    }
}
