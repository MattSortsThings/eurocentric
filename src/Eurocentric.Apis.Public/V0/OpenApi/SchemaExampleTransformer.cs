using Eurocentric.Components.OpenApi;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Options;

namespace Eurocentric.Apis.Public.V0.OpenApi;

internal sealed class SchemaExampleTransformer(IOptions<JsonOptions> jsonOptions)
    : SchemaExampleTransformerBase(jsonOptions)
{
    private const string Namespace = "Eurocentric.Apis.Public.V0";

    protected override string RootNamespace => Namespace;
}
