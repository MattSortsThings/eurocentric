using Eurocentric.Features.AdminApi.V1.Common.Constants;
using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.AdminApi.V1.Common.OpenApi;

internal sealed class V1TagsDocumentTransformer : TagsDocumentTransformer
{
    private protected override IEnumerable<TagDatum> GetTagData()
    {
        yield return new TagDatum(Endpoints.Countries.Tag,
            """
            Endpoints using the **Country** resource.

            A Country resource represents a single real-world country (and the "Rest of the World" pseudo-country). Use the
            Countries endpoints to create a new country in the system so that it can participate in contests.
            """);
    }
}
