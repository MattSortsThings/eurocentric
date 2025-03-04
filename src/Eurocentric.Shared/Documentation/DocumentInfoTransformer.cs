using Asp.Versioning;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace Eurocentric.Shared.Documentation;

internal sealed class DocumentInfoTransformer(string title, string description, ApiVersion version) : IOpenApiDocumentTransformer
{
    public Task TransformAsync(OpenApiDocument document,
        OpenApiDocumentTransformerContext context,
        CancellationToken cancellationToken)
    {
        document.Info.Title = title;
        document.Info.Description = description;
        document.Info.Version = $"v{version}";

        return Task.CompletedTask;
    }
}
