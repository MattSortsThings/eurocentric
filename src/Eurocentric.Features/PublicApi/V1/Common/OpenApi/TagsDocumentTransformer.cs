using Eurocentric.Features.PublicApi.V1.Common.Constants;
using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.PublicApi.V1.Common.OpenApi;

internal sealed class TagsDocumentTransformer : BaseTagsDocumentTransformer
{
    private protected override IEnumerable<TagDatum> GetTagData()
    {
        yield return new TagDatum(EndpointNames.Tags.Queryables,
            """
            Endpoints for accessing lists of **Queryables**.

            Use these endpoints to obtain lists of query parameter values that can be used in rankings queries.
            """);

        yield return new TagDatum(EndpointNames.Tags.CompetitorRankings,
            """
            Endpoints for **Competitor Rankings** queries.

            Each endpoint ranks every single competitor in every contest broadcast by a named metric calculated from the points
            awards the competitor received in the broadcast in which they competed.

            A range of query parameters allows the client to filter the queried data and control the pagination of results.
            """);

        yield return new TagDatum(EndpointNames.Tags.CompetingCountryRankings,
            """
            Endpoints for **Competing Country Rankings** queries.

            Each endpoint ranks every competing country by a named metric calculated from the points awards the country has
            received across the contest broadcasts in which the country competed.

            A range of query parameters allows the client to filter the queried data and control the pagination of results.
            """);

        yield return new TagDatum(EndpointNames.Tags.VotingCountryRankings,
            """
            Endpoints for **Voting Country Rankings** queries.

            Each endpoint ranks every voting country by a named metric calculated from the points awards the country has given
            across the contest broadcasts in which the country voted.

            A range of query parameters allows the client to filter the queried data and control the pagination of results.
            """);
    }
}
