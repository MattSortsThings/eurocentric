using ErrorOr;
using Eurocentric.Domain.Countries;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Features.AdminApi.V1.Common.Constants;
using Eurocentric.Features.AdminApi.V1.Common.DomainMapping;
using Eurocentric.Features.Shared.ErrorHandling;
using Eurocentric.Features.Shared.Messaging;
using Eurocentric.Infrastructure.EFCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using SlimMessageBus;
using CountryDto = Eurocentric.Features.AdminApi.V1.Common.Dtos.Country;

namespace Eurocentric.Features.AdminApi.V1.Countries;

public sealed record GetCountryResponse(CountryDto Country);

internal static class GetCountry
{
    internal static IEndpointRouteBuilder MapGetCountry(this IEndpointRouteBuilder apiGroup)
    {
        apiGroup.MapGet("countries/{countryId:guid}", HandleAsync)
            .WithName(EndpointIds.Countries.GetCountry)
            .WithSummary("Get a country")
            .WithDescription("Retrieves a single country.")
            .HasApiVersion(1, 0)
            .Produces<GetCountryResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithTags(EndpointTags.Countries);

        return apiGroup;
    }

    private static async Task<Results<ProblemHttpResult, Ok<GetCountryResponse>>> HandleAsync(
        [FromRoute(Name = "countryId")] Guid countryId,
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
            CountryId targetId = CountryId.FromValue(query.CountryId);

            CountryDto? country = await dbContext.Countries.AsNoTracking()
                .AsSplitQuery()
                .Where(country => country.Id == targetId)
                .Select(country => country.ToCountryDto())
                .FirstOrDefaultAsync(cancellationToken);

            return country is not null
                ? ErrorOrFactory.From(new GetCountryResponse(country))
                : CountryErrors.CountryNotFound(targetId);
        }
    }
}
