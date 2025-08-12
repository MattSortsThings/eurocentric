using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.PublicApi.V1.Utils;

public interface IRestRequestFactory
{
    public IQueryablesEndpoints Queryables { get; }

    public interface IQueryablesEndpoints
    {
        public RestRequest GetQueryableVotingMethods();
    }
}
