using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace Eurocentric.Components.OpenApi;

/// <summary>
///     Appends the server URL with the common URI segments for the API release and trims them from all paths.
/// </summary>
public abstract class ServerAndPathsTransformerBase : IOpenApiDocumentTransformer
{
    /// <summary>
    ///     The common URI segments for the API release.
    /// </summary>
    /// <example>"/admin/api/v0.1"</example>
    protected abstract string ReleaseUriSegments { get; }

    public Task TransformAsync(
        OpenApiDocument document,
        OpenApiDocumentTransformerContext context,
        CancellationToken cancellationToken
    )
    {
        ReplaceServer(document);
        ReplacePaths(document);

        return Task.CompletedTask;
    }

    private void ReplacePaths(OpenApiDocument document)
    {
        OpenApiPaths updatedPaths = new();

        foreach ((string path, IOpenApiPathItem item) in document.Paths)
        {
            updatedPaths.Add(path.Replace(ReleaseUriSegments, string.Empty), item);
        }

        document.Paths = updatedPaths;
    }

    private void ReplaceServer(OpenApiDocument document)
    {
        string updatedServerUrl = document.Servers?.FirstOrDefault() is { } existing
            ? existing.Url?.TrimEnd('/') + "/" + ReleaseUriSegments.TrimStart('/')
            : ReleaseUriSegments;

        updatedServerUrl = EnsureStartsWithHttps(updatedServerUrl);

        document.Servers = new List<OpenApiServer> { new() { Url = updatedServerUrl } };
    }

    private static string EnsureStartsWithHttps(string serverUrl) => "https://" + serverUrl.Split("://").Last();
}
