using ErrorOr;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.ValueObjects;
using Eurocentric.Features.AdminApi.V1.Common.Constants;
using Eurocentric.Features.AdminApi.V1.Common.Contracts;
using Eurocentric.Features.Shared.ErrorHandling;
using Eurocentric.Features.Shared.Messaging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;
using SlimMessageBus;
using DomainCountry = Eurocentric.Domain.Aggregates.Countries.Country;

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

    internal sealed class Handler : IQueryHandler<Query, GetCountryResponse>
    {
        public async Task<ErrorOr<GetCountryResponse>> OnHandle(Query request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            DomainCountry dummyCountry = new(CountryId.FromValue(request.CountryId),
                CountryCode.FromValue("AT").Value,
                CountryName.FromValue("Austria").Value);

            Func<DomainCountry, Country> mapper = Projections.ProjectToCountryDto.Compile();

            return ErrorOrFactory.From(new GetCountryResponse(mapper(dummyCountry)));
        }
    }
}
