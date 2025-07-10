using ErrorOr;
using Eurocentric.Domain.Aggregates.Countries;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Features.AdminApi.V1.Common.Constants;
using Eurocentric.Features.Shared.ErrorHandling;
using Eurocentric.Features.Shared.Messaging;
using Eurocentric.Infrastructure.DataAccess.EfCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using SlimMessageBus;
using Country = Eurocentric.Features.AdminApi.V1.Common.Contracts.Country;

namespace Eurocentric.Features.AdminApi.V1.Countries;

public sealed record GetCountryResponse(Country Country);

internal static class GetCountry
{
    internal static IEndpointRouteBuilder MapGetCountry(this IEndpointRouteBuilder apiGroup)
    {
        apiGroup.MapGet("countries/{countryId:guid}", ExecuteAsync)
            .WithName(EndpointNames.Countries.GetCountry)
            .WithSummary("Get a country")
            .WithDescription("Retrieves a single country")
            .WithTags(EndpointTags.Countries)
            .HasApiVersion(1, 0)
            .Produces<GetCountryResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound);

        return apiGroup;
    }

    private static async Task<Results<ProblemHttpResult, Ok<GetCountryResponse>>> ExecuteAsync(Guid countryId,
        IRequestResponseBus bus,
        CancellationToken cancellationToken = default) => await InitializeQuery(countryId)
        .ThenAsync(query => bus.Send(query, cancellationToken: cancellationToken))
        .ToProblemOrResponseAsync(TypedResults.Ok);

    private static ErrorOr<Query> InitializeQuery(Guid countryId) => ErrorOrFactory.From(new Query(countryId));

    internal sealed record Query(Guid CountryId) : IQuery<GetCountryResponse>;

    internal sealed class Handler(AppDbContext dbContext) : IQueryHandler<Query, GetCountryResponse>
    {
        public async Task<ErrorOr<GetCountryResponse>> OnHandle(Query query, CancellationToken cancellationToken)
        {
            CountryId countryId = CountryId.FromValue(query.CountryId);

            Country? country = await dbContext.Countries.AsNoTracking()
                .Where(country => country.Id == countryId)
                .Select(Projections.ProjectToCountryDto)
                .FirstOrDefaultAsync(cancellationToken);

            return country is null
                ? CountryErrors.CountryNotFound(countryId)
                : new GetCountryResponse(country);
        }
    }
}
