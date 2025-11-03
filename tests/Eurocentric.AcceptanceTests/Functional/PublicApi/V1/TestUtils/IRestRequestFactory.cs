using RestSharp;

namespace Eurocentric.AcceptanceTests.Functional.PublicApi.V1.TestUtils;

public interface IRestRequestFactory
{
    IQueryablesEndpoints Queryables { get; }

    interface IQueryablesEndpoints
    {
        RestRequest GetQueryableCountries();
    }
}
