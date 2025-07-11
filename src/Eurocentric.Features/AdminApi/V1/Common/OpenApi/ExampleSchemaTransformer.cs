using Eurocentric.Features.AdminApi.V1.Common.Contracts;
using Eurocentric.Features.AdminApi.V1.Countries;
using Eurocentric.Features.Shared.Documentation;
using Microsoft.OpenApi.Any;

namespace Eurocentric.Features.AdminApi.V1.Common.OpenApi;

internal sealed class ExampleSchemaTransformer : BaseExampleSchemaTransformer
{
    private protected override IReadOnlyDictionary<Type, IOpenApiAny> SchemaExamples { get; } =
        new Dictionary<Type, IOpenApiAny>
        {
            [typeof(BroadcastMemo)] = BroadcastMemo.CreateExample().ToOpenApiAny(),
            [typeof(CreateCountryRequest)] = CreateCountryRequest.CreateExample().ToOpenApiAny(),
            [typeof(CreateCountryResponse)] = CreateCountryResponse.CreateExample().ToOpenApiAny(),
            [typeof(Contest)] = Contest.CreateExample().ToOpenApiAny(),
            [typeof(Country)] = Country.CreateExample().ToOpenApiAny(),
            [typeof(Participant)] = Participant.CreateExample().ToOpenApiAny()
        };
}
