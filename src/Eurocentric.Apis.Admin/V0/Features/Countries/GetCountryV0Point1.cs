using ErrorOr;
using Eurocentric.Apis.Admin.V0.Constants;
using Eurocentric.Apis.Admin.V0.Dtos;
using Eurocentric.Infrastructure.DataAccess.EfCore;
using Eurocentric.Infrastructure.Messaging;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using SlimMessageBus;

namespace Eurocentric.Apis.Admin.V0.Features.Countries;

public static class GetCountryV0Point1
{
    internal static IEndpointRouteBuilder MapGetCountryV0Point1(this IEndpointRouteBuilder builder)
    {
        builder.MapGet("v0.1/countries/{countryId:guid}", ExecuteAsync)
            .WithName("AdminApi.V0.1.GetCountry")
            .WithTags(V0Group.Countries.Tag)
            .Produces<Response>()
            .ProducesProblem(StatusCodes.Status404NotFound);

        return builder;
    }

    private static async Task<IResult> ExecuteAsync(Guid countryId,
        IRequestResponseBus bus,
        CancellationToken cancellationToken = default)
    {
        ErrorOr<Response> errorsOrResponse = await bus.Send(new Query(countryId), cancellationToken: cancellationToken);

        return TypedResults.Ok(errorsOrResponse.Value);
    }

    public sealed record Response(Country Country);

    internal sealed record Query(Guid CountryId) : IQuery<Response>;

    [UsedImplicitly]
    internal sealed class QueryHandler(AppDbContext dbContext) : IQueryHandler<Query, Response>
    {
        public async Task<ErrorOr<Response>> OnHandle(Query query, CancellationToken cancellationToken)
        {
            Guid countryId = query.CountryId;

            Country? country = await dbContext.V0Countries.AsNoTracking()
                .Where(country => country.Id == countryId)
                .Select(OutboundMapping.CountryToCountryDto())
                .SingleOrDefaultAsync(cancellationToken);

            return country is null ? CountryNotFound(countryId) : new Response(country);
        }

        private static Error CountryNotFound(Guid countryId) => Error.NotFound("Country not found",
            "No country exists with the provided country ID.",
            new Dictionary<string, object> { { nameof(countryId), countryId } });
    }
}
