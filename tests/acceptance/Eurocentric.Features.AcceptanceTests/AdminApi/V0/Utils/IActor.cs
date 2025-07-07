using Eurocentric.Features.AcceptanceTests.Utils;
using Eurocentric.Features.AdminApi.V0.Common.Contracts;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V0.Utils;

public interface IActor
{
    public IWebAppFixtureBackDoor BackDoor { get; }

    public IWebAppFixtureRestClient RestClient { get; }

    public string ApiVersion { get; }

    public RestRequest? Request { get; set; }

    public List<Contest> GivenContests { get; }

    public Task When_I_send_my_request();

    public void Then_my_request_should_succeed_with_status_code(int statusCode);

    public void Then_my_request_should_fail_with_status_code(int statusCode);
}
