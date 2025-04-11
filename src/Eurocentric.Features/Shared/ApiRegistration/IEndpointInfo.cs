namespace Eurocentric.Features.Shared.ApiRegistration;

public interface IEndpointInfo
{
    public string Name { get; }

    public HttpMethod HttpMethod { get; }

    public string Route { get; }

    public Delegate Handler { get; }

    public string Summary { get; }

    public string Description { get; }

    public string Tag { get; }

    public IEnumerable<int> ProblemStatusCodes { get; }

    public string ApiName { get; }

    public int MajorApiVersion { get; }

    public int MinorApiVersion { get; }
}
