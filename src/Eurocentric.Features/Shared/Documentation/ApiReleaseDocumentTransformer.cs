using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace Eurocentric.Features.Shared.Documentation;

internal sealed class ApiReleaseDocumentTransformer(string releaseUriSegments) : IOpenApiDocumentTransformer
{
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
            updatedPaths.Add(path.Replace(releaseUriSegments, string.Empty), item);
        }

        document.Paths = updatedPaths;
    }

    private void ReplaceServer(OpenApiDocument document)
    {
        OpenApiServer existingServer = document.Servers.First();

        string updatedServerUrl = existingServer.Url.TrimEnd('/') + "/" + releaseUriSegments.TrimStart('/');

        document.Servers.Clear();
        document.Servers.Add(new OpenApiServer { Url = updatedServerUrl });
    }
}
