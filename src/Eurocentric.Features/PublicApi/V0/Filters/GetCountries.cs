using ErrorOr;
using Eurocentric.Features.PublicApi.V0.Common.Constants;
using Eurocentric.Features.PublicApi.V0.Common.Contracts;
using Eurocentric.Features.PublicApi.V0.Common.Extensions;
using Eurocentric.Features.Shared.Messaging;
using Eurocentric.Infrastructure.DataAccess.EfCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using SlimMessageBus;

namespace Eurocentric.Features.PublicApi.V0.Filters;

public sealed record GetCountriesResponse(Country[] Countries);

internal static class GetCountries
{
    internal static IEndpointRouteBuilder MapGetCountries(this IEndpointRouteBuilder apiGroup)
    {
        apiGroup.MapGet("filters/countries", ExecuteAsync)
            .WithName(EndpointNames.Filters.GetCountries)
            .WithSummary("Get countries")
            .WithDescription("Retrieves a list of all the queryable countries in country code order.")
            .WithTags(EndpointTags.Filters)
            .HasApiVersion(0, 1)
            .HasApiVersion(0, 2)
            .Produces<GetCountriesResponse>();

        return apiGroup;
    }

    private static async Task<Results<ProblemHttpResult, Ok<GetCountriesResponse>>> ExecuteAsync(
        IRequestResponseBus bus,
        CancellationToken cancellationToken = default)
    {
        ErrorOr<GetCountriesResponse> errorsOrResponse = await bus.Send(new Query(), cancellationToken: cancellationToken);

        return TypedResults.Ok(errorsOrResponse.Value);
    }

    internal sealed record Query : IQuery<GetCountriesResponse>;

    internal sealed class Handler(AppDbContext dbContext) : IQueryHandler<Query, GetCountriesResponse>
    {
        public async Task<ErrorOr<GetCountriesResponse>> OnHandle(Query request, CancellationToken cancellationToken)
        {
            Country[] countries = await dbContext.PlaceholderQueryableCountries.AsNoTracking()
                .OrderBy(country => country.CountryCode)
                .Select(Projections.ProjectToCountryDto)
                .ToArrayAsync(cancellationToken);

            return ErrorOrFactory.From(new GetCountriesResponse(countries));
        }
    }
}
