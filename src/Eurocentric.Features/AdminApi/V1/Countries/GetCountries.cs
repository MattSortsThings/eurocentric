using ErrorOr;
using Eurocentric.Features.AdminApi.V1.Common.Constants;
using Eurocentric.Features.AdminApi.V1.Common.Contracts;
using Eurocentric.Features.Shared.ErrorHandling;
using Eurocentric.Features.Shared.Messaging;
using Eurocentric.Infrastructure.DataAccess.EfCore;
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
    internal static IEndpointRouteBuilder MapGetCountries(this IEndpointRouteBuilder v1Group)
    {
        v1Group.MapGet("countries", ExecuteAsync)
            .WithName(EndpointConstants.Names.Countries.GetCountries)
            .WithSummary("Get all countries")
            .WithDescription("Retrieves all existing countries in country code order.")
            .WithTags(EndpointConstants.Tags.Countries)
            .HasApiVersion(1, 0)
            .Produces<GetCountriesResponse>();

        return v1Group;
    }

    private static async Task<Results<ProblemHttpResult, Ok<GetCountriesResponse>>> ExecuteAsync(IRequestResponseBus bus,
        CancellationToken cancellationToken = default) => await InitializeQuery()
        .ThenAsync(query => bus.Send(query, cancellationToken: cancellationToken))
        .ToProblemOrResponseAsync(TypedResults.Ok);

    private static ErrorOr<Query> InitializeQuery() => ErrorOrFactory.From(new Query());

    internal sealed record Query : IQuery<GetCountriesResponse>;

    internal sealed class Handler(AppDbContext dbContext) : IQueryHandler<Query, GetCountriesResponse>
    {
        public async Task<ErrorOr<GetCountriesResponse>> OnHandle(Query _, CancellationToken cancellationToken)
        {
            Country[] countries = await dbContext.Countries.AsNoTracking()
                .OrderBy(country => country.CountryCode)
                .Select(Projections.CountryToCountryDto)
                .ToArrayAsync(cancellationToken);

            return new GetCountriesResponse(countries);
        }
    }
}
