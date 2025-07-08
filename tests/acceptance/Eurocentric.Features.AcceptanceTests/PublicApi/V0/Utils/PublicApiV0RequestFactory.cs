using Eurocentric.Features.PublicApi.V0.Rankings;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.PublicApi.V0.Utils;

public sealed class PublicApiV0RequestFactory : IPublicApiV0RequestFactory, IPublicApiV0RequestFactory.IFiltersEndpoints,
    IPublicApiV0RequestFactory.IRankingsEndpoints
{
    private readonly string _apiVersion;

    public PublicApiV0RequestFactory(string apiVersion)
    {
        _apiVersion = apiVersion;
    }

    public RestRequest GetContestStages()
    {
        RestRequest request = new("/public/api/{apiVersion}/filters/contest-stages");

        request.AddUrlSegment("apiVersion", _apiVersion);

        return request;
    }

    public RestRequest GetCountries()
    {
        RestRequest request = new("/public/api/{apiVersion}/filters/countries");

        request.AddUrlSegment("apiVersion", _apiVersion);

        return request;
    }

    public RestRequest GetVotingMethods()
    {
        RestRequest request = new("/public/api/{apiVersion}/filters/voting-methods");

        request.AddUrlSegment("apiVersion", _apiVersion);

        return request;
    }

    public IPublicApiV0RequestFactory.IFiltersEndpoints Filters => this;

    public IPublicApiV0RequestFactory.IRankingsEndpoints Rankings => this;

    public RestRequest GetCompetingCountryPointsAverageRankings(GetCompetingCountryPointsAverageRankingsRequest query)
    {
        RestRequest request = new("/public/api/{apiVersion}/rankings/competing-countries/points-average");

        request.AddUrlSegment("apiVersion", _apiVersion);

        if (query.ContestStage is { } contestStage)
        {
            request.AddQueryParameter("contestStage", contestStage);
        }

        if (query.MaxYear is { } maxYear)
        {
            request.AddQueryParameter("maxYear", maxYear);
        }

        if (query.MinYear is { } minYear)
        {
            request.AddQueryParameter("minYear", minYear);
        }

        if (query.VotingCountryCode is { } votingCountryCode)
        {
            request.AddQueryParameter("votingCountryCode", votingCountryCode);
        }

        if (query.VotingMethod is { } votingMethod)
        {
            request.AddQueryParameter("votingMethod", votingMethod);
        }

        if (query.PageIndex is { } pageIndex)
        {
            request.AddQueryParameter("pageIndex", pageIndex);
        }

        if (query.PageSize is { } pageSize)
        {
            request.AddQueryParameter("pageSize", pageSize);
        }

        if (query.Descending is { } descending)
        {
            request.AddQueryParameter("descending", descending);
        }

        return request;
    }

    public RestRequest GetCompetingCountryPointsInRangeRankings(GetCompetingCountryPointsInRangeRankingsRequest query)
    {
        RestRequest request = new("/public/api/{apiVersion}/rankings/competing-countries/points-in-range");

        request.AddUrlSegment("apiVersion", _apiVersion)
            .AddQueryParameter("minPoints", query.MinPoints)
            .AddQueryParameter("maxPoints", query.MaxPoints);

        if (query.ContestStage is { } contestStage)
        {
            request.AddQueryParameter("contestStage", contestStage);
        }

        if (query.MaxYear is { } maxYear)
        {
            request.AddQueryParameter("maxYear", maxYear);
        }

        if (query.MinYear is { } minYear)
        {
            request.AddQueryParameter("minYear", minYear);
        }

        if (query.VotingCountryCode is { } votingCountryCode)
        {
            request.AddQueryParameter("votingCountryCode", votingCountryCode);
        }

        if (query.VotingMethod is { } votingMethod)
        {
            request.AddQueryParameter("votingMethod", votingMethod);
        }

        if (query.PageIndex is { } pageIndex)
        {
            request.AddQueryParameter("pageIndex", pageIndex);
        }

        if (query.PageSize is { } pageSize)
        {
            request.AddQueryParameter("pageSize", pageSize);
        }

        if (query.Descending is { } descending)
        {
            request.AddQueryParameter("descending", descending);
        }

        return request;
    }
}
