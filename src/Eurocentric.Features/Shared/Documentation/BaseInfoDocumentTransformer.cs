using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace Eurocentric.Features.Shared.Documentation;

internal abstract class BaseInfoDocumentTransformer : IOpenApiDocumentTransformer
{
    private protected abstract string Title { get; }

    private protected abstract string Description { get; }

    private protected abstract string Version { get; }

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
