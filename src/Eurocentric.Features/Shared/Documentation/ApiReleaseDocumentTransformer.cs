using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace Eurocentric.Features.Shared.Documentation;

internal abstract class ApiReleaseDocumentTransformer : IOpenApiDocumentTransformer
{
    private protected abstract string ReleaseUriSegments { get; }

    public Task TransformAsync(OpenApiDocument document,
        OpenApiDocumentTransformerContext context,
        CancellationToken cancellationToken)
    {
        ReplaceServer(document);
        ReplacePaths(document);

        return Task.CompletedTask;
    }

    private void ReplacePaths(OpenApiDocument document)
    {
        OpenApiPaths updatedPaths = new();

        foreach ((string path, OpenApiPathItem item) in document.Paths)
        {
            updatedPaths.Add(path.Replace(ReleaseUriSegments, string.Empty), item);
        }

        document.Paths = updatedPaths;
    }

    private void ReplaceServer(OpenApiDocument document)
    {
        OpenApiServer existingServer = document.Servers.First();

        string updatedServerUrl = existingServer.Url.TrimEnd('/') + "/" + ReleaseUriSegments.TrimStart('/');

        document.Servers.Clear();
        document.Servers.Add(new OpenApiServer { Url = updatedServerUrl });
    }
}
