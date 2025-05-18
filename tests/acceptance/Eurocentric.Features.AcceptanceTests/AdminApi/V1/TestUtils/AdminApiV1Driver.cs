using Eurocentric.Features.AcceptanceTests.TestUtils;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.TestUtils;

public sealed partial class AdminApiV1Driver
{
    private readonly string _apiVersion;
    private readonly ITestClient _client;

    private AdminApiV1Driver(ITestClient client, string apiVersion)
    {
        _client = client;
        _apiVersion = apiVersion;
    }

    public static AdminApiV1Driver Create(ITestClient client, int majorVersion, int minorVersion)
    {
        string routePrefix = $"{majorVersion}.{minorVersion}";

        return new AdminApiV1Driver(client, routePrefix);
    }
}
