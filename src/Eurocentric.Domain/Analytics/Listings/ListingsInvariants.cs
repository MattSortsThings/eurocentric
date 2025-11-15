using CSharpFunctionalExtensions;
using Eurocentric.Domain.Core;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.Analytics.Listings;

public static class ListingsInvariants
{
    public static UnitResult<IDomainError> LegalCompetingCountryFiltering(IRequiredCompetingCountryFiltering filtering)
    {
        return ValueObjectInvariants.LegalCountryCodeValue(filtering.CompetingCountryCode).IsFailure
            ? ListingsErrors.IllegalCompetingCountryCodeValue(filtering.CompetingCountryCode)
            : UnitResult.Success<IDomainError>();
    }

    public static UnitResult<IDomainError> LegalVotingCountryFiltering(IRequiredVotingCountryFiltering filtering)
    {
        return ValueObjectInvariants.LegalCountryCodeValue(filtering.VotingCountryCode).IsFailure
            ? ListingsErrors.IllegalVotingCountryCodeValue(filtering.VotingCountryCode)
            : UnitResult.Success<IDomainError>();
    }
}
