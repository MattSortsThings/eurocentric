using CSharpFunctionalExtensions;
using Eurocentric.Domain.Core;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.Analytics.Listings;

public static class ListingsInvariants
{
    public static UnitResult<IDomainError> LegalCompetingCountryFiltering(IRequiredCompetingCountryFiltering filtering)
    {
        return
            filtering.CompetingCountryCode is { } competingCountryCode
            && ValueObjectInvariants.LegalCountryCodeValue(competingCountryCode).IsFailure
            ? ListingsErrors.IllegalCompetingCountryCodeValue(competingCountryCode)
            : UnitResult.Success<IDomainError>();
    }
}
