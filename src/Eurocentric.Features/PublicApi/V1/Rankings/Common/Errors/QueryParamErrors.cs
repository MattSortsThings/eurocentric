using ErrorOr;

namespace Eurocentric.Features.PublicApi.V1.Rankings.Common.Errors;

internal static class QueryParamErrors
{
    internal static Error PageIndexOutOfRange(int pageIndex) => Error.Validation("Page index out of range",
        "Query parameter 'pageIndex' value must be an integer greater than or equal to 0.",
        new Dictionary<string, object> { { "pageIndex", pageIndex } });

    internal static Error PageSizeOutOfRange(int pageSize) => Error.Validation("Page size out of range",
        "Query parameter 'pageSize' value must be an integer between 1 and 100.",
        new Dictionary<string, object> { { "pageSize", pageSize } });

    internal static Error InvalidVotingCountryCode(string votingCountryCode) => Error.Validation("Invalid voting country code",
        "Query parameter 'votingCountryCode' value must be a string of 2 upper-case letters.",
        new Dictionary<string, object> { { "votingCountryCode", votingCountryCode } });
}
