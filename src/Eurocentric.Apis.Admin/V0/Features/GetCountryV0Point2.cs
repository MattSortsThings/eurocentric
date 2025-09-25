using ErrorOr;
using Eurocentric.Apis.Admin.V0.Constants;
using Eurocentric.Apis.Admin.V0.Contracts.Countries;
using Eurocentric.Apis.Admin.V0.Contracts.Mapping;
using Eurocentric.Domain.V0.Aggregates.Countries;
using Eurocentric.Infrastructure.DataAccess.EfCore;
using Eurocentric.Infrastructure.Messaging;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using SlimMessageBus;

namespace Eurocentric.Apis.Admin.V0.Features;

internal static class GetCountryV0Point2
{
    internal static IEndpointRouteBuilder MapGetCountryV0Point2(this IEndpointRouteBuilder builder)
    {
        builder
            .MapGet("v0.2/countries/{countryId:guid}", ExecuteAsync)
            .WithName("AdminApi.V0.Countries.GetCountryV0Point2")
            .WithTags(V0Group.Countries.Tag)
            .Produces<GetCountryResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound);

        return builder;
    }

    private static async Task<IResult> ExecuteAsync(
        [FromRoute(Name = "countryId")] Guid countryId,
        [FromServices] IRequestResponseBus bus,
        CancellationToken cancellationToken = default
    )
    {
        ErrorOr<Result> errorsOrResult = await bus.Send(
            new Query(countryId),
            cancellationToken: cancellationToken
        );

        return MapToOk(errorsOrResult.Value);
    }

    private static Ok<GetCountryResponse> MapToOk(in Result result)
    {
        GetCountryResponse response = new(result.Country.ToCountryDto());

        return TypedResults.Ok(response);
    }

    internal readonly record struct Result(Country Country);

    internal sealed record Query(Guid CountryId) : IQuery<Result>;

    [UsedImplicitly]
    internal sealed class QueryHandler(AppDbContext dbContext) : IQueryHandler<Query, Result>
    {
        public async Task<ErrorOr<Result>> OnHandle(
            Query query,
            CancellationToken cancellationToken
        )
        {
            Guid countryId = query.CountryId;

            Country? country = await dbContext
                .Countries.AsNoTracking()
                .SingleOrDefaultAsync(country => country.Id == countryId, cancellationToken);

            return country is null ? CountryNotFound(countryId) : new Result(country);
        }

        private static Error CountryNotFound(Guid countryId) =>
            Error.NotFound(
                "Country not found",
                "No country exists with the provided ID.",
                new Dictionary<string, object> { { nameof(countryId), countryId } }
            );
    }
}
