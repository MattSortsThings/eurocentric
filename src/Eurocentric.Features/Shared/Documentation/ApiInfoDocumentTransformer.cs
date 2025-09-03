using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace Eurocentric.Features.Shared.Documentation;

internal abstract class ApiInfoDocumentTransformer : IOpenApiDocumentTransformer
{
    private protected abstract string Title { get; }

    private protected abstract string Description { get; }

    private protected abstract string ApiVersion { get; }

    public Task TransformAsync(OpenApiDocument document,
        OpenApiDocumentTransformerContext context,
        CancellationToken cancellationToken)
    {
        document.Info.Title = Title;
        document.Info.Description = Description;
        document.Info.Version = ApiVersion;
        document.Info.Contact = new OpenApiContact { Name = "Matt Tantony" };

        return Task.CompletedTask;
    }
}
