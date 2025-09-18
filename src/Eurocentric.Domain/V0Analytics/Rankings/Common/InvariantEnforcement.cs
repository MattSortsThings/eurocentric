using ErrorOr;

namespace Eurocentric.Domain.V0Analytics.Rankings.Common;

public static class InvariantEnforcement
{
    public static ErrorOr<T> FailOnIllegalPagination<T>(this ErrorOr<T> errorsOrQuery)
        where T : IPaginatedQuery
    {
        if (errorsOrQuery.IsError)
        {
            return errorsOrQuery;
        }

        T query = errorsOrQuery.Value;

        if (query.PageIndex is var x and < 0)
        {
            return Error.Failure("Illegal page index value",
                "Page index value must not be negative.",
                new Dictionary<string, object> { { "pageIndex", x } });
        }

        if (query.PageSize is var y and < 1)
        {
            return Error.Failure("Illegal page size value",
                "Page size value must be greater than zero.",
                new Dictionary<string, object> { { "pageSize", y } });
        }

        return errorsOrQuery;
    }

    public static ErrorOr<T> FailOnIllegalPointsRange<T>(this ErrorOr<T> errorsOrQuery)
        where T : IPointsInRangeQuery
    {
        if (errorsOrQuery.IsError)
        {
            return errorsOrQuery;
        }

        T query = errorsOrQuery.Value;

        if (query.MinPoints is var x && query.MaxPoints is var y && y < x)
        {
            return Error.Failure("Illegal points range values",
                "Maximum points value must be greater than or equal to minimum points value.",
                new Dictionary<string, object> { { "minPoints", x }, { "maxPoints", y } });
        }

        return errorsOrQuery;
    }

    public static ErrorOr<T> FailOnIllegalYearRange<T>(this ErrorOr<T> errorsOrQuery)
        where T : IYearRangeFilter
    {
        if (errorsOrQuery.IsError)
        {
            return errorsOrQuery;
        }

        T query = errorsOrQuery.Value;

        if (query.MinYear is var x && query.MaxYear is var y && y < x)
        {
            return Error.Failure("Illegal year range values",
                "Maximum year value must be greater than or equal to minimum points value.",
                new Dictionary<string, object> { { "minyear", x }, { "maxYear", y } });
        }

        return errorsOrQuery;
    }

    public static ErrorOr<T> FailOnIllegalVotingCountryCode<T>(this ErrorOr<T> errorsOrQuery)
        where T : IVotingCountryFilter
    {
        if (errorsOrQuery.IsError)
        {
            return errorsOrQuery;
        }

        T query = errorsOrQuery.Value;

        if (query.VotingCountryCode is { } countryCode && countryCode.Length != 2)
        {
            return Error.Failure("Illegal voting country code",
                "Voting country code value must be a string of 2 characters",
                new Dictionary<string, object> { { "votingCountryCode", countryCode } });
        }

        return errorsOrQuery;
    }
}
