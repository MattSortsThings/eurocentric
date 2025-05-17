using ErrorOr;
using Eurocentric.Features.AdminApi.V1.Common.Constants;
using Eurocentric.Features.AdminApi.V1.Common.Dtos;
using Eurocentric.Features.Shared.ErrorHandling;
using Eurocentric.Features.Shared.Messaging;
using Eurocentric.Infrastructure.EfCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using SlimMessageBus;

namespace Eurocentric.Features.AdminApi.V1.Countries;

public sealed record GetCountriesResponse(Country[] Countries);

internal static class GetCountries
{
    internal static IEndpointRouteBuilder MapGetCountries(this IEndpointRouteBuilder apiVersionGroup)
    {
        apiVersionGroup.MapGet("countries", Endpoint.HandleAsync)
            .WithName(RouteIds.Countries.GetCountries)
            .HasApiVersion(1, 0)
            .WithSummary("Get all countries")
            .WithDescription("Retrieves all existing countries, ordered by country code.")
            .WithTags(EndpointTags.Countries)
            .Produces<GetCountriesResponse>();

        return apiVersionGroup;
    }

    internal sealed record Query : IQuery<GetCountriesResponse>;

    internal sealed class Handler(AppDbContext dbContext) : IQueryHandler<Query, GetCountriesResponse>
    {
        public async Task<ErrorOr<GetCountriesResponse>> OnHandle(Query request, CancellationToken cancellationToken)
        {
            Country[] countries = await dbContext.Countries.AsNoTracking()
                .OrderBy(country => country.CountryCode.Value)
                .Select(country => country.ToCountryDto())
                .ToArrayAsync(cancellationToken);

            return ErrorOrFactory.From(new GetCountriesResponse(countries));
        }
    }

    private static class Endpoint
    {
        internal static async Task<Results<Ok<GetCountriesResponse>, ProblemHttpResult>> HandleAsync(IRequestResponseBus bus,
            CancellationToken cancellationToken = default) => await InitializeQuery()
            .ThenAsync(query => bus.Send(query, cancellationToken: cancellationToken))
            .ToResultOrProblemAsync(TypedResults.Ok);

        private static ErrorOr<Query> InitializeQuery() => ErrorOrFactory.From(new Query());
    }
}
