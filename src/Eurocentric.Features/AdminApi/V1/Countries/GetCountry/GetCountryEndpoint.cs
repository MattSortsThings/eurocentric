using ErrorOr;
using Eurocentric.Features.AdminApi.Common;
using Eurocentric.Features.Shared.ApiDiscovery;
using Eurocentric.Features.Shared.ErrorHandling;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SlimMessageBus;

namespace Eurocentric.Features.AdminApi.V1.Countries.GetCountry;

internal sealed record GetCountryEndpoint : IEndpointInfo
{
    public string Name => nameof(GetCountryEndpoint);

    public HttpMethod HttpMethod => HttpMethod.Get;

    public string Route => "countries/{countryId:guid}";

    public Delegate Handler => static async ([FromRoute] Guid countryId,
        IRequestResponseBus bus,
        CancellationToken cancellationToken = default) => await ErrorOrFactory.From(new GetCountryQuery(countryId))
        .ThenAsync(query => bus.Send(query, cancellationToken: cancellationToken))
        .ToResultOrProblemAsync(TypedResults.Ok);

    public string Summary => "Get a country";

    public string Description => "Retrieves a single country. The country ID must be supplied as a route parameter.";

    public string Tag => AdminApiInfo.Tags.Countries;

    public IEnumerable<int> ProblemStatusCodes
    {
        get
        {
            yield return StatusCodes.Status404NotFound;
        }
    }

    public string ApiName => AdminApiInfo.ApiName;

    public int MajorApiVersion => 1;

    public int MinorApiVersion => 0;
}
