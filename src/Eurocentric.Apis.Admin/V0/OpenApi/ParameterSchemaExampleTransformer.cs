using System.Text.Json.Nodes;
using Eurocentric.Apis.Admin.V0.Config;
using Eurocentric.Components.OpenApi;

namespace Eurocentric.Apis.Admin.V0.OpenApi;

internal sealed class ParameterSchemaExampleTransformer : ParameterSchemaExampleTransformerBase
{
    protected override IReadOnlyDictionary<string, JsonNode?> Examples { get; } =
        new Dictionary<string, JsonNode?> { { "countryId", V0ExampleIds.Country.ToString() } };
}
