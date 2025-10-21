using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace Eurocentric.Apis.Admin.V0.OpenApi;

internal abstract class InfoTransformerBase : IOpenApiDocumentTransformer
{
    private protected abstract string ApiVersion { get; }

    public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken _)
    {
        document.Info = new OpenApiInfo
        {
            Title = "Eurocentric Admin API",
            Description = "A web API for modelling the Eurovision Song Contest, 2016-2025.",
            Version = ApiVersion,
        };

        return Task.CompletedTask;
    }
}
