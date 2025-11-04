using CSharpFunctionalExtensions;
using Eurocentric.Domain.Core;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.Analytics.Rankings.Common;

public static class RankingsInvariants
{
    public static UnitResult<IDomainError> LegalPaginationSettings(IOptionalPaginationSettings settings)
    {
        UnitResult<IDomainError> result = UnitResult.Success<IDomainError>();

        if (settings.PageIndex is { } pageIndex and < 0)
        {
            result = RankingsErrors.IllegalPageIndexValue(pageIndex);
        }
        else if (settings.PageSize is { } pageSize and (< 1 or > 100))
        {
            result = RankingsErrors.IllegalPageSizeValue(pageSize);
        }

        return result;
    }

    public static UnitResult<IDomainError> LegalBroadcastFiltering(IOptionalBroadcastFiltering filtering)
    {
        if (filtering is { MinYear: { } minYear, MaxYear: { } maxYear } && maxYear < minYear)
        {
            return RankingsErrors.IllegalContestYearRange(minYear, maxYear);
        }

        return UnitResult.Success<IDomainError>();
    }

    public static UnitResult<IDomainError> LegalVotingCountryFiltering(IOptionalVotingCountryFiltering filtering)
    {
        if (
            filtering.VotingCountryCode is { } votingCountryCode
            && ValueObjectInvariants.LegalCountryCodeValue(votingCountryCode).IsFailure
        )
        {
            return RankingsErrors.IllegalVotingCountryCodeValue(votingCountryCode);
        }

        return UnitResult.Success<IDomainError>();
    }

    public static UnitResult<IDomainError> LegalCompetingCountrySettings(IRequiredCompetingCountryFiltering filtering)
    {
        if (
            filtering.CompetingCountryCode is { } competingCountryCode
            && ValueObjectInvariants.LegalCountryCodeValue(competingCountryCode).IsFailure
        )
        {
            return RankingsErrors.IllegalCompetingCountryCodeValue(competingCountryCode);
        }

        return UnitResult.Success<IDomainError>();
    }
}
