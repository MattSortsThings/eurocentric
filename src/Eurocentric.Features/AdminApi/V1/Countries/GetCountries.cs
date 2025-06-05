using ErrorOr;
using Eurocentric.Features.AdminApi.V1.Common.Constants;
using Eurocentric.Features.AdminApi.V1.Common.DomainMapping;
using Eurocentric.Features.Shared.ErrorHandling;
using Eurocentric.Features.Shared.Messaging;
using Eurocentric.Infrastructure.EFCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using SlimMessageBus;
using CountryDto = Eurocentric.Features.AdminApi.V1.Common.Dtos.Country;

namespace Eurocentric.Features.AdminApi.V1.Countries;

public sealed record GetCountriesResponse(CountryDto[] Countries);

internal static class GetCountries
{
    internal static IEndpointRouteBuilder MapGetCountries(this IEndpointRouteBuilder apiGroup)
    {
        apiGroup.MapGet("countries", HandleAsync)
            .WithName(EndpointIds.Countries.GetCountries)
            .WithSummary("Get all countries")
            .WithDescription("Retrieves a list of all existing countries, ordered by country code.")
            .HasApiVersion(1, 0)
            .Produces<GetCountriesResponse>()
            .WithTags(EndpointTags.Countries);

        return apiGroup;
    }

    private static async Task<Results<ProblemHttpResult, Ok<GetCountriesResponse>>> HandleAsync(IRequestResponseBus bus,
        CancellationToken cancellationToken = default) => await InitializeQuery()
        .ThenAsync(query => bus.Send(query, cancellationToken: cancellationToken))
        .ToProblemOrResponseAsync(TypedResults.Ok);

    private static ErrorOr<Query> InitializeQuery() => ErrorOrFactory.From(new Query());

    internal sealed record Query : IQuery<GetCountriesResponse>;

    internal sealed class Handler(AppDbContext dbContext) : IQueryHandler<Query, GetCountriesResponse>
    {
        public async Task<ErrorOr<GetCountriesResponse>> OnHandle(Query _, CancellationToken cancellationToken)
        {
            CountryDto[] countries = await dbContext.Countries
                .AsNoTracking()
                .AsSplitQuery()
                .OrderBy(country => country.CountryCode)
                .Select(country => country.ToCountryDto())
                .ToArrayAsync(cancellationToken);

            return ErrorOrFactory.From(new GetCountriesResponse(countries));
        }
    }
}
