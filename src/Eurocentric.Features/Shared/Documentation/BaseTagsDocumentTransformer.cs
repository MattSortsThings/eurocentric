using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace Eurocentric.Features.Shared.Documentation;

internal abstract class BaseTagsDocumentTransformer : IOpenApiDocumentTransformer
{
    public Task TransformAsync(OpenApiDocument document,
        OpenApiDocumentTransformerContext context,
        CancellationToken cancellationToken)
    {
        document.Tags = GetTagData().Select(datum => new OpenApiTag { Name = datum.Name, Description = datum.Description })
            .ToList();

        return Task.CompletedTask;
    }

    private protected abstract IEnumerable<TagDatum> GetTagData();

    private protected sealed record TagDatum(string Name, string Description);
}
