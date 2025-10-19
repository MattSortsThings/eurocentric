using CSharpFunctionalExtensions;
using Eurocentric.Domain.Functional;

namespace Eurocentric.Domain.V0.Queries.Rankings.Common;

public static class RankingsQueryRules
{
    public static UnitResult<IDomainError> LegalPaginationSettings(IOptionalPaginationSettings settings)
    {
        if (settings.PageIndex is { } pageIndex and < 0)
        {
            return RankingsQueryErrors.IllegalPageIndexValue(pageIndex);
        }

        if (settings.PageSize is { } pageSize and (< 1 or > 100))
        {
            return RankingsQueryErrors.IllegalPageSizeValue(pageSize);
        }

        return UnitResult.Success<IDomainError>();
    }

    public static UnitResult<IDomainError> LegalBroadcastFiltering(IOptionalBroadcastFiltering filtering)
    {
        if (filtering is { MinYear: { } minYear, MaxYear: { } maxYear } && maxYear < minYear)
        {
            return RankingsQueryErrors.IllegalContestYearRange(minYear, maxYear);
        }

        return UnitResult.Success<IDomainError>();
    }

    public static UnitResult<IDomainError> LegalVotingCountryFiltering(IOptionalVotingCountryFiltering filtering)
    {
        if (filtering.VotingCountryCode is { } votingCountryCode && !LegalCountryCodeValue(votingCountryCode))
        {
            return RankingsQueryErrors.IllegalVotingCountryCodeValue(votingCountryCode);
        }

        return UnitResult.Success<IDomainError>();
    }

    private static bool LegalCountryCodeValue(string countryCode) =>
        countryCode.Length == 2 && countryCode.All(char.IsAsciiLetterUpper);
}
