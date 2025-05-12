using ErrorOr;
using Eurocentric.Features.AdminApi.V1.Common.Constants;
using Eurocentric.Features.AdminApi.V1.Common.Enums;
using Eurocentric.Features.AdminApi.V1.Countries.Common;
using Eurocentric.Features.Shared.ErrorHandling;
using Eurocentric.Features.Shared.Messaging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SlimMessageBus;

namespace Eurocentric.Features.AdminApi.V1.Countries;

public static class GetCountry
{
    internal static IEndpointRouteBuilder MapGetCountry(this IEndpointRouteBuilder apiVersionGroup)
    {
        apiVersionGroup.MapGet("countries/{countryId:guid}", Endpoint.HandleAsync)
            .WithName("AdminApi.V1.GetCountry")
            .HasApiVersion(1, 0)
            .WithSummary("Get a country")
            .WithDescription("Retrieves a single country.")
            .WithTags(EndpointTags.Countries)
            .Produces<Response>()
            .ProducesProblem(StatusCodes.Status404NotFound);

        return apiVersionGroup;
    }

    public sealed record Response(Country Country);

    internal sealed record Query(Guid CountryId) : IQuery<Response>;

    internal sealed class Handler : IQueryHandler<Query, Response>
    {
        public Task<ErrorOr<Response>> OnHandle(Query request, CancellationToken cancellationToken)
        {
            Country country = new(request.CountryId, "GB", "United Kingdom",
                [new ContestMemo(Guid.NewGuid(), ContestStatus.Completed)]);

            return Task.FromResult(ErrorOrFactory.From(new Response(country)));
        }
    }

    private static class Endpoint
    {
        internal static async Task<Results<Ok<Response>, ProblemHttpResult>> HandleAsync(
            [FromRoute(Name = "countryId")] Guid countryId,
            IRequestResponseBus bus,
            CancellationToken cancellationToken = default) => await InitializeQuery(countryId)
            .ThenAsync(query => bus.Send(query, cancellationToken: cancellationToken))
            .ToResultOrProblemAsync(TypedResults.Ok);

        private static ErrorOr<Query> InitializeQuery(Guid countryId) => ErrorOrFactory.From(new Query(countryId));
    }
}
