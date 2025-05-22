using ErrorOr;
using Eurocentric.Domain.Countries;
using Eurocentric.Domain.ValueObjects;
using Eurocentric.Features.AdminApi.V1.Common.Constants;
using Eurocentric.Features.AdminApi.V1.Common.Dtos;
using Eurocentric.Features.Shared.Documentation;
using Eurocentric.Features.Shared.ErrorHandling;
using Eurocentric.Features.Shared.Messaging;
using Eurocentric.Infrastructure.EfCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using SlimMessageBus;
using Country = Eurocentric.Features.AdminApi.V1.Common.Dtos.Country;
using DomainCountry = Eurocentric.Domain.Countries.Country;

namespace Eurocentric.Features.AdminApi.V1.Countries;

public sealed record CreateCountryRequest : IExampleProvider<CreateCountryRequest>
{
    public required string CountryCode { get; init; }

    public required string CountryName { get; init; }

    public static CreateCountryRequest CreateExample() => new() { CountryCode = "AL", CountryName = "Albania" };
}

public sealed record CreateCountryResponse(Country Country) : IExampleProvider<CreateCountryResponse>
{
    public static CreateCountryResponse CreateExample() => new(Country.CreateExample() with { ContestMemos = [] });
}

internal static class CreateCountry
{
    internal static IEndpointRouteBuilder MapCreateCountry(this IEndpointRouteBuilder apiVersionGroup)
    {
        apiVersionGroup.MapPost("countries", Endpoint.HandleAsync)
            .WithName(RouteIds.Countries.CreateCountry)
            .HasApiVersion(1, 0)
            .WithSummary("Create a country")
            .WithDescription("Creates a new country.")
            .WithTags(EndpointTags.Countries)
            .Produces<CreateCountryResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status422UnprocessableEntity);

        return apiVersionGroup;
    }

    internal sealed record Command(string CountryCode, string Name) : ICommand<CreateCountryResponse>;

    internal sealed class Handler(AppDbContext dbContext, ICountryIdProvider idProvider) :
        ICommandHandler<Command, CreateCountryResponse>
    {
        public async Task<ErrorOr<CreateCountryResponse>> OnHandle(Command command, CancellationToken cancellationToken) =>
            await CreateCountryFrom(command)
                .FailIfCountryCodeIsNotUnique(dbContext.Countries.AsNoTracking())
                .ThenDo(country => dbContext.Countries.Add(country))
                .ThenDoAsync(_ => dbContext.SaveChangesAsync(cancellationToken))
                .Then(MapToResponse);

        private ErrorOr<DomainCountry> CreateCountryFrom(Command command) => DomainCountry.Create()
            .WithCountryCode(CountryCode.FromValue(command.CountryCode))
            .WithName(CountryName.FromValue(command.Name))
            .Build(idProvider.Create);

        private static CreateCountryResponse MapToResponse(DomainCountry domainCountry) => new(domainCountry.ToCountryDto());
    }

    private static class Endpoint
    {
        internal static async Task<Results<CreatedAtRoute<CreateCountryResponse>, ProblemHttpResult>> HandleAsync(
            [FromBody] CreateCountryRequest request,
            IRequestResponseBus bus,
            CancellationToken cancellationToken = default) => await InitializeCommand(request)
            .ThenAsync(command => bus.Send(command, cancellationToken: cancellationToken))
            .ToResultOrProblemAsync(MapToCreatedAtRoute);

        private static ErrorOr<Command> InitializeCommand(CreateCountryRequest request) =>
            ErrorOrFactory.From(new Command(request.CountryCode, request.CountryName));

        private static CreatedAtRoute<CreateCountryResponse> MapToCreatedAtRoute(CreateCountryResponse response) =>
            TypedResults.CreatedAtRoute(response,
                RouteIds.Countries.GetCountry,
                new RouteValueDictionary { { "countryId", response.Country.Id } });
    }
}
