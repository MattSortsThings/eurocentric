using System.Text.Json.Nodes;
using Eurocentric.Apis.Admin.V1.Config;
using Eurocentric.Components.OpenApi;

namespace Eurocentric.Apis.Admin.V1.OpenApi;

internal sealed class ParameterSchemaExampleTransformer : ParameterSchemaExampleTransformerBase
{
    protected override IReadOnlyDictionary<string, JsonNode?> Examples { get; } =
        new Dictionary<string, JsonNode?>
        {
            { "broadcastId", V1ExampleIds.Broadcast.ToString() },
            { "contestId", V1ExampleIds.Contest.ToString() },
            { "countryId", V1ExampleIds.CountryA.ToString() },
        };
}
