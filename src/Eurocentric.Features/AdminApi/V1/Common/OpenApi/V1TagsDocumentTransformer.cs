using Eurocentric.Features.AdminApi.V1.Common.Constants;
using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.AdminApi.V1.Common.OpenApi;

internal sealed class V1TagsDocumentTransformer : TagsDocumentTransformer
{
    private protected override IEnumerable<TagDatum> GetTagData()
    {
        yield return new TagDatum(Endpoints.Contests.Tag,
            """
            Endpoints using the **Contest** resource.

            A Contest resource represents a single year's edition of the Eurovision Song Contest. Use the Contest endpoints to
            create a new contest using countries that exist in the system, and to initialize the contest's child broadcasts.
            """);

        yield return new TagDatum(Endpoints.Countries.Tag,
            """
            Endpoints using the **Country** resource.

            A Country resource represents a single real-world country (and the "Rest of the World" pseudo-country). Use the
            Countries endpoints to create a new country in the system so that it can participate in contests.
            """);
    }
}
