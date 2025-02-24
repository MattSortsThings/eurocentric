using Asp.Versioning;
using Eurocentric.Shared.ApiRegistration;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace Eurocentric.Shared.Documentation;

internal sealed class DocumentInfoTransformer : IOpenApiDocumentTransformer
{
    private readonly ApiVersion _apiVersion;
    private readonly string _description;
    private readonly string _title;

    public DocumentInfoTransformer(IApiInfo apiInfo, ApiVersion apiVersion)
    {
        _title = apiInfo.OpenApiDocumentTitle;
        _description = apiInfo.OpenApiDocumentDescription;
        _apiVersion = apiVersion;
    }


    public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context,
        CancellationToken cancellationToken)
    {
        document.Info.Title = _title;
        document.Info.Description = _description;
        document.Info.Version = $"v{_apiVersion.MajorVersion}.{_apiVersion.MinorVersion}";

        return Task.CompletedTask;
    }
}
