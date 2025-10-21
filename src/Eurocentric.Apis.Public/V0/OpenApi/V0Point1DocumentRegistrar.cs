using Eurocentric.Apis.Public.V0.Config;
using Eurocentric.Components.OpenApi;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.OpenApi;

namespace Eurocentric.Apis.Public.V0.OpenApi;

public sealed class V0Point1DocumentRegistrar : IOpenApiDocumentRegistrar
{
    public string DocumentName => "public-api-v0.1";

    public void Configure(OpenApiOptions options)
    {
        options.ShouldInclude = static apiDescription =>
            apiDescription.GroupName == EndpointConstants.GroupName
            && apiDescription.GetApiVersion() is { MajorVersion: 0, MinorVersion: 1 };

        options
            .AddDocumentTransformer<ApiKeySecurityTransformer>()
            .AddDocumentTransformer<InfoTransformer>()
            .AddDocumentTransformer<ServerAndPathsTransformer>()
            .AddOperationTransformer<ProblemDetailsResponseExampleTransformer>();
    }

    private sealed class InfoTransformer : InfoTransformerBase
    {
        private protected override string ApiVersion => "v0.1";
    }

    private sealed class ServerAndPathsTransformer : ServerAndPathsTransformerBase
    {
        protected override string ReleaseUriSegments => "/public/api/v0.1";
    }
}
