using ErrorOr;
using Eurocentric.Domain.Countries;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Features.AdminApi.V1.Common.Constants;
using Eurocentric.Features.AdminApi.V1.Countries.Common;
using Eurocentric.Features.Shared.ErrorHandling;
using Eurocentric.Features.Shared.Messaging;
using Eurocentric.Infrastructure.EfCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using SlimMessageBus;
using Country = Eurocentric.Features.AdminApi.V1.Countries.Common.Country;

namespace Eurocentric.Features.AdminApi.V1.Countries;

public sealed record GetCountryResponse(Country Country);

internal static class GetCountry
{
    internal const string RouteId = "AdminApi.V1.GetCountry";

    internal static IEndpointRouteBuilder MapGetCountry(this IEndpointRouteBuilder apiVersionGroup)
    {
        apiVersionGroup.MapGet("countries/{countryId:guid}", Endpoint.HandleAsync)
            .WithName(RouteId)
            .HasApiVersion(1, 0)
            .WithSummary("Get a country")
            .WithDescription("Retrieves a single country.")
            .WithTags(EndpointTags.Countries)
            .Produces<GetCountryResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound);

        return apiVersionGroup;
    }

    internal sealed record Query(Guid CountryId) : IQuery<GetCountryResponse>;

    internal sealed class Handler(AppDbContext dbContext) : IQueryHandler<Query, GetCountryResponse>
    {
        public async Task<ErrorOr<GetCountryResponse>> OnHandle(Query query, CancellationToken cancellationToken)
        {
            CountryId targetId = CountryId.FromValue(query.CountryId);

            Country? country = await dbContext.Countries
                .Where(c => c.Id == targetId)
                .Select(c => c.ToCountryDto())
                .FirstOrDefaultAsync(cancellationToken);

            return country is not null ? new GetCountryResponse(country) : CountryErrors.CountryNotFound(targetId);
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
