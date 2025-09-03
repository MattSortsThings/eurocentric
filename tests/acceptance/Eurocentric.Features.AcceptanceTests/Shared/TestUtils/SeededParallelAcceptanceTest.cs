using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.Shared.TestUtils;

[Category("acceptance")]
public abstract class SeededParallelAcceptanceTest
{
    [ClassDataSource<SeededWebAppFixture>(Shared = SharedType.PerAssembly)]
    public required SeededWebAppFixture SystemUnderTest { get; init; }

    private protected static RestRequest GetRequest(string route) => new(route);

    private protected static RestRequest PostRequest(string route) => new(route, Method.Post);
}
