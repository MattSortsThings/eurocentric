using CSharpFunctionalExtensions;
using Eurocentric.Domain.Core;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.Analytics.Rankings.Common;

public static class RankingsInvariants
{
    public static UnitResult<IDomainError> LegalPaginationOverrides(IOptionalPaginationOverrides overrides)
    {
        UnitResult<IDomainError> result = UnitResult.Success<IDomainError>();

        if (overrides.PageIndex is { } pageIndex and < 0)
        {
            result = RankingsErrors.IllegalPageIndexValue(pageIndex);
        }
        else if (overrides.PageSize is { } pageSize and (< 1 or > 100))
        {
            result = RankingsErrors.IllegalPageSizeValue(pageSize);
        }

        return result;
    }

    public static UnitResult<IDomainError> LegalBroadcastFiltering(IOptionalBroadcastFiltering filtering)
    {
        return filtering is { MinYear: { } minYear, MaxYear: { } maxYear } && maxYear < minYear
            ? RankingsErrors.IllegalContestYearRange(minYear, maxYear)
            : UnitResult.Success<IDomainError>();
    }

    public static UnitResult<IDomainError> LegalVotingCountryFiltering(IOptionalVotingCountryFiltering filtering)
    {
        return
            filtering.VotingCountryCode is { } votingCountryCode
            && ValueObjectInvariants.LegalCountryCodeValue(votingCountryCode).IsFailure
            ? RankingsErrors.IllegalVotingCountryCodeValue(votingCountryCode)
            : UnitResult.Success<IDomainError>();
    }

    public static UnitResult<IDomainError> LegalCompetingCountryFiltering(IOptionalCompetingCountryFiltering filtering)
    {
        return
            filtering.CompetingCountryCode is { } competingCountryCode
            && ValueObjectInvariants.LegalCountryCodeValue(competingCountryCode).IsFailure
            ? RankingsErrors.IllegalCompetingCountryCodeValue(competingCountryCode)
            : UnitResult.Success<IDomainError>();
    }

    public static UnitResult<IDomainError> LegalCompetingCountryFiltering(IRequiredCompetingCountryFiltering filtering)
    {
        return ValueObjectInvariants.LegalCountryCodeValue(filtering.CompetingCountryCode).IsFailure
            ? RankingsErrors.IllegalCompetingCountryCodeValue(filtering.CompetingCountryCode)
            : UnitResult.Success<IDomainError>();
    }

    public static UnitResult<IDomainError> LegalPointsValueRange(IRequiredPointsValueRange range)
    {
        return range is { MinPoints: var minPoints, MaxPoints: var maxPoints } && maxPoints < minPoints
            ? RankingsErrors.IllegalPointsValueRange(minPoints, maxPoints)
            : UnitResult.Success<IDomainError>();
    }
}
