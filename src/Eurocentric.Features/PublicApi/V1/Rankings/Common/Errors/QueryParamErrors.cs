using ErrorOr;

namespace Eurocentric.Features.PublicApi.V1.Rankings.Common.Errors;

internal static class QueryParamErrors
{
    internal static Error PageIndexOutOfRange(int pageIndex) => Error.Validation("Page index out of range",
        "Query parameter 'pageIndex' value must be greater than or equal to 0.",
        new Dictionary<string, object> { { "pageIndex", pageIndex } });

    internal static Error PageSizeOutOfRange(int pageSize) => Error.Validation("Page size out of range",
        "Query parameter 'pageSize' value must be greater than or equal to 1.",
        new Dictionary<string, object> { { "pageSize", pageSize } });

    internal static Error InvalidVotingCountryCode(string votingCountryCode) => Error.Validation("Invalid voting country code",
        "Query parameter 'votingCountryCode' value must be a string of 2 upper-case letters.",
        new Dictionary<string, object> { { "votingCountryCode", votingCountryCode } });

    internal static Error InvalidContestYearRange(int minYear, int maxYear) => Error.Validation("Invalid contest year range",
        "Query parameter 'minYear' value must be less than or equal to query parameter 'maxYear' value.",
        new Dictionary<string, object> { { "minYear", minYear }, { "maxYear", maxYear } });
}
