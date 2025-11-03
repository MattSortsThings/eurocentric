using Eurocentric.Apis.Public.V1.Config;
using Eurocentric.Components.OpenApi;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.OpenApi;

namespace Eurocentric.Apis.Public.V1.OpenApi;

public sealed class V1Point0DocumentRegistrar : IOpenApiDocumentRegistrar
{
    public string DocumentName => "public-api-v1.0";

    public void Configure(OpenApiOptions options)
    {
        options.ShouldInclude = static apiDescription =>
            apiDescription.GroupName == V1Group.Name
            && apiDescription.GetApiVersion() is { MajorVersion: 1, MinorVersion: 0 };

        options
            .AddDocumentTransformer<ApiKeySecurityTransformer>()
            .AddDocumentTransformer<InfoTransformer>()
            .AddDocumentTransformer<ServerAndPathsTransformer>()
            .AddOperationTransformer<ProblemDetailsResponseExampleTransformer>()
            .AddSchemaTransformer<SchemaExampleTransformer>();
    }

    private sealed class InfoTransformer : InfoTransformerBase
    {
        private protected override string ApiVersion => "v1.0";
    }

    private sealed class ServerAndPathsTransformer : ServerAndPathsTransformerBase
    {
        protected override string ReleaseUriSegments => "/public/api/v1.0";
    }
}
