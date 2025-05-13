using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace Eurocentric.Features.Shared.Documentation;

internal sealed class InfoDocumentTransformer : IOpenApiDocumentTransformer
{
    public InfoDocumentTransformer(string title = "OpenAPI Document Title", string description = "OpenAPI document description.",
        string version = "v1.0")
    {
        Title = title;
        Description = description;
        Version = version;
    }

    private string Title { get; }

    private string Description { get; }

    private string Version { get; }

    public Task TransformAsync(OpenApiDocument document,
        OpenApiDocumentTransformerContext context,
        CancellationToken cancellationToken)
    {
        document.Info.Title = Title;
        document.Info.Description = Description;
        document.Info.Version = Version;

        return Task.CompletedTask;
    }
}
