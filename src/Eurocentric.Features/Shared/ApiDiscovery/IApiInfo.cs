namespace Eurocentric.Features.Shared.ApiDiscovery;

public interface IApiInfo
{
    public string Name { get; }

    public string UrlPrefix { get; }

    public IReadOnlyList<ApiRelease> Releases { get; }

    public string OpenApiDocumentTitle { get; }

    public string OpenApiDocumentDescription { get; }

    public string AuthorizationPolicyName { get; }
}
