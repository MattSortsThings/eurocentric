using RestSharp;

namespace Eurocentric.WebApp.AcceptanceTests.PublicApi.V0.Utils;

public interface IRestRequestFactory
{
    IQueryablesEndpoints Queryables { get; }

    interface IQueryablesEndpoints
    {
        RestRequest GetQueryableBroadcasts();
    }
}
