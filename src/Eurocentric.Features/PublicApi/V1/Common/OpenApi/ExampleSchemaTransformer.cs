using Eurocentric.Features.PublicApi.V1.Common.Dtos;
using Eurocentric.Features.PublicApi.V1.Common.Enums;
using Eurocentric.Features.Shared.Documentation;
using Microsoft.OpenApi.Any;

namespace Eurocentric.Features.PublicApi.V1.Common.OpenApi;

internal sealed class ExampleSchemaTransformer : BaseExampleSchemaTransformer
{
    private protected override IReadOnlyDictionary<Type, IOpenApiAny> SchemaExamples { get; } =
        new Dictionary<Type, IOpenApiAny>
        {
            [typeof(QueryableContestStage[])] = Enum.GetValues<QueryableContestStage>().ToOpenApiAny(),
            [typeof(QueryableCountry)] = QueryableCountry.CreateExample().ToOpenApiAny(),
            [typeof(QueryableVotingMethod[])] = Enum.GetValues<QueryableVotingMethod>().ToOpenApiAny()
        };
}
