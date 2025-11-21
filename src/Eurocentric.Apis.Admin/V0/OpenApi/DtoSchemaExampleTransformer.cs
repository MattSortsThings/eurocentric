using Eurocentric.Components.OpenApi;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Options;

namespace Eurocentric.Apis.Admin.V0.OpenApi;

internal sealed class DtoSchemaExampleTransformer(IOptions<JsonOptions> jsonOptions)
    : DtoSchemaExampleTransformerBase(jsonOptions)
{
    private const string Namespace = "Eurocentric.Apis.Admin.V0";

    protected override string RootNamespace => Namespace;
}
