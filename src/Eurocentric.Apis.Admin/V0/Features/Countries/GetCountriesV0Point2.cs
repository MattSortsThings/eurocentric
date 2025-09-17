using ErrorOr;
using Eurocentric.Apis.Admin.V0.Constants;
using Eurocentric.Apis.Admin.V0.Dtos;
using Eurocentric.Infrastructure.DataAccess.EfCore;
using Eurocentric.Infrastructure.Messaging;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using SlimMessageBus;

namespace Eurocentric.Apis.Admin.V0.Features.Countries;

public static class GetCountriesV0Point2
{
    internal static IEndpointRouteBuilder MapGetCountriesV0Point2(this IEndpointRouteBuilder builder)
    {
        builder.MapGet("v0.2/countries", ExecuteAsync)
            .WithName("AdminApi.V0.2.GetCountries")
            .WithTags(V0Group.Countries.Tag)
            .Produces<Response>();

        return builder;
    }

    private static async Task<IResult> ExecuteAsync(
        [FromServices] IRequestResponseBus bus,
        CancellationToken cancellationToken = default)
    {
        ErrorOr<Response> errorsOrResponse = await bus.Send(new Query(), cancellationToken: cancellationToken);

        return TypedResults.Ok(errorsOrResponse.Value);
    }

    public sealed record Response(Country[] Countries);

    internal sealed record Query : IQuery<Response>;

    [UsedImplicitly]
    internal sealed class QueryHandler(AppDbContext dbContext) : IQueryHandler<Query, Response>
    {
        public async Task<ErrorOr<Response>> OnHandle(Query _, CancellationToken cancellationToken)
        {
            Country[] countries = await dbContext.V0Countries.AsNoTracking()
                .OrderBy(country => country.CountryCode)
                .Select(OutboundMapping.CountryToCountryDto)
                .ToArrayAsync(cancellationToken);

            return new Response(countries);
        }
    }
}
