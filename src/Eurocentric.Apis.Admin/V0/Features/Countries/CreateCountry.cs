using System.ComponentModel;
using CSharpFunctionalExtensions;
using Eurocentric.Apis.Admin.V0.Config;
using Eurocentric.Apis.Admin.V0.Dtos.Countries;
using Eurocentric.Apis.Admin.V0.Enums;
using Eurocentric.Components.EndpointMapping;
using Eurocentric.Components.Messaging;
using Eurocentric.Domain.Functional;
using Eurocentric.Domain.V0.Aggregates.Countries;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SlimMessageBus;
using CountryAggregate = Eurocentric.Domain.V0.Aggregates.Countries.Country;
using CountryDto = Eurocentric.Apis.Admin.V0.Dtos.Countries.Country;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace Eurocentric.Apis.Admin.V0.Features.Countries;

internal static class CreateCountry
{
    private static async Task<IResult> ExecuteAsync(
        [FromBody] CreateCountryRequest request,
        [FromServices] IRequestResponseBus bus,
        CancellationToken ct = default
    ) => await bus.DispatchAsync(request.ToCommand(), MapToCreatedAtRoute, ct);

    private static Command ToCommand(this CreateCountryRequest request)
    {
        return request.CountryType switch
        {
            CountryType.Real or CountryType.Pseudo => new Command(request.CountryCode, request.CountryName),
            _ => throw new InvalidEnumArgumentException($"Invalid CountryType enum value: {request.CountryType}."),
        };
    }

    private static CreatedAtRoute<CreateCountryResponse> MapToCreatedAtRoute(CountryAggregate country)
    {
        CountryDto countryDto = country.ToDto();

        Guid countryId = countryDto.Id;

        return TypedResults.CreatedAtRoute(
            new CreateCountryResponse(countryDto),
            V0EndpointNames.Countries.GetCountry,
            new RouteValueDictionary { { nameof(countryId), countryId } }
        );
    }

    internal sealed class EndpointMapper : IEndpointMapper
    {
        public void MapEndpoint(RouteGroupBuilder routeBuilder)
        {
            routeBuilder
                .MapPost("countries", ExecuteAsync)
                .WithName(V0EndpointNames.Countries.CreateCountry)
                .AddedInVersion0Point1()
                .WithSummary("Create a country")
                .WithDescription("Creates a new country in the system.")
                .WithTags(V0Tags.Countries)
                .Produces<CreateCountryResponse>(StatusCodes.Status201Created)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status409Conflict)
                .ProducesProblem(StatusCodes.Status422UnprocessableEntity);
        }
    }

    internal sealed record Command(string CountryCode, string CountryName) : ICommand<CountryAggregate>;

    [UsedImplicitly]
    internal sealed class CommandHandler(ICountryReadRepository readRepository, ICountryWriteRepository writeRepository)
        : ICommandHandler<Command, CountryAggregate>
    {
        public async Task<Result<CountryAggregate, IDomainError>> OnHandle(Command command, CancellationToken ct)
        {
            return await CountryAggregate
                .Create(command.CountryCode, command.CountryName)
                .Ensure(CountryRules.HasUniqueCountryCode(readRepository.GetQueryable()))
                .Tap(writeRepository.Add)
                .Tap(_ => writeRepository.SaveChangesAsync(ct))
                .Map(country => country);
        }
    }
}
