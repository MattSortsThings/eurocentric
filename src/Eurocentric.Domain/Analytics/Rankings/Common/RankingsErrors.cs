using Eurocentric.Domain.Core;

namespace Eurocentric.Domain.Analytics.Rankings.Common;

public static class RankingsErrors
{
    public static UnprocessableError IllegalPageIndexValue(int pageIndex)
    {
        return new UnprocessableError
        {
            Title = "Illegal page index value",
            Detail = "Page index value must be a non-negative integer.",
            Extensions = new Dictionary<string, object?> { { nameof(pageIndex), pageIndex } },
        };
    }

    public static UnprocessableError IllegalPageSizeValue(int pageSize)
    {
        return new UnprocessableError
        {
            Title = "Illegal page size value",
            Detail = "Page size value must be an integer between 1 and 100.",
            Extensions = new Dictionary<string, object?> { { nameof(pageSize), pageSize } },
        };
    }

    public static UnprocessableError IllegalCompetingCountryCodeValue(string competingCountryCode)
    {
        return new UnprocessableError
        {
            Title = "Illegal competing country code value",
            Detail = "Competing country code value must be a string of 2 upper-case letters.",
            Extensions = new Dictionary<string, object?> { { nameof(competingCountryCode), competingCountryCode } },
        };
    }

    public static UnprocessableError IllegalVotingCountryCodeValue(string votingCountryCode)
    {
        return new UnprocessableError
        {
            Title = "Illegal voting country code value",
            Detail = "Voting country code value must be a string of 2 upper-case letters.",
            Extensions = new Dictionary<string, object?> { { nameof(votingCountryCode), votingCountryCode } },
        };
    }

    public static UnprocessableError IllegalContestYearRange(int minYear, int maxYear)
    {
        return new UnprocessableError
        {
            Title = "Illegal contest year range",
            Detail = "Maximum contest year must be greater than or equal to minimum contest year.",
            Extensions = new Dictionary<string, object?> { { nameof(minYear), minYear }, { nameof(maxYear), maxYear } },
        };
    }

    public static UnprocessableError IllegalPointsValueRange(int minPoints, int maxPoints)
    {
        return new UnprocessableError
        {
            Title = "Illegal points value range",
            Detail = "Maximum points value must be greater than or equal to minimum points value.",
            Extensions = new Dictionary<string, object?>
            {
                { nameof(minPoints), minPoints },
                { nameof(maxPoints), maxPoints },
            },
        };
    }
}
