using Eurocentric.Components.OpenApi;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Options;

namespace Eurocentric.Apis.Public.V1.OpenApi;

internal sealed class DtoSchemaExampleTransformer(IOptions<JsonOptions> jsonOptions)
    : DtoSchemaExampleTransformerBase(jsonOptions)
{
    private const string Namespace = "Eurocentric.Apis.Public.V1";

    protected override string RootNamespace => Namespace;
}
