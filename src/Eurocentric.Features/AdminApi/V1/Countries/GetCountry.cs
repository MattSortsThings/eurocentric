using ErrorOr;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.ValueObjects;
using Eurocentric.Features.AdminApi.V1.Common.Constants;
using Eurocentric.Features.AdminApi.V1.Countries.Common;
using Eurocentric.Features.Shared.ErrorHandling;
using Eurocentric.Features.Shared.Messaging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SlimMessageBus;
using Country = Eurocentric.Features.AdminApi.V1.Countries.Common.Country;

namespace Eurocentric.Features.AdminApi.V1.Countries;

public sealed record GetCountryResponse(Country Country);

internal static class GetCountry
{
    internal static IEndpointRouteBuilder MapGetCountry(this IEndpointRouteBuilder apiVersionGroup)
    {
        apiVersionGroup.MapGet("countries/{countryId:guid}", Endpoint.HandleAsync)
            .WithName("AdminApi.V1.GetCountry")
            .HasApiVersion(1, 0)
            .WithSummary("Get a country")
            .WithDescription("Retrieves a single country.")
            .WithTags(EndpointTags.Countries)
            .Produces<GetCountryResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound);

        return apiVersionGroup;
    }

    internal sealed record Query(Guid CountryId) : IQuery<GetCountryResponse>;

    internal sealed class Handler : IQueryHandler<Query, GetCountryResponse>
    {
        public Task<ErrorOr<GetCountryResponse>> OnHandle(Query query, CancellationToken cancellationToken)
        {
            Country country = Domain.Countries.Country.Create()
                .WithCountryCode(CountryCode.FromValue("GB"))
                .WithName(CountryName.FromValue("United Kingdom"))
                .Build(() => CountryId.FromValue(query.CountryId))
                .Value.ToCountryDto();

            return Task.FromResult(ErrorOrFactory.From(new GetCountryResponse(country)));
        }
    }

    private static class Endpoint
    {
        internal static async Task<Results<Ok<GetCountryResponse>, ProblemHttpResult>> HandleAsync(
            [FromRoute(Name = "countryId")] Guid countryId,
            IRequestResponseBus bus,
            CancellationToken cancellationToken = default) => await InitializeQuery(countryId)
            .ThenAsync(query => bus.Send(query, cancellationToken: cancellationToken))
            .ToResultOrProblemAsync(TypedResults.Ok);

        private static ErrorOr<Query> InitializeQuery(Guid countryId) => ErrorOrFactory.From(new Query(countryId));
    }
}
