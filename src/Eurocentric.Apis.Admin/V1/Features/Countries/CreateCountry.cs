using CSharpFunctionalExtensions;
using Eurocentric.Apis.Admin.V1.Config;
using Eurocentric.Apis.Admin.V1.Dtos.Countries;
using Eurocentric.Components.EndpointMapping;
using Eurocentric.Components.Messaging;
using Eurocentric.Domain.Aggregates.Countries;
using Eurocentric.Domain.Core;
using Eurocentric.Domain.ValueObjects;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SlimMessageBus;
using Country = Eurocentric.Apis.Admin.V1.Dtos.Countries.Country;
using CountryAggregate = Eurocentric.Domain.Aggregates.Countries.Country;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace Eurocentric.Apis.Admin.V1.Features.Countries;

internal static class CreateCountry
{
    private static async Task<IResult> ExecuteAsync(
        [FromBody] CreateCountryRequest request,
        [FromServices] IRequestResponseBus bus,
        CancellationToken ct = default
    ) => await bus.DispatchAsync(request.ToCommand(), MapToCreatedAtRoute, ct);

    private static Command ToCommand(this CreateCountryRequest request)
    {
        return new Command
        {
            ErrorOrCountryCode = CountryCode.FromValue(request.CountryCode),
            ErrorOrCountryName = CountryName.FromValue(request.CountryName),
        };
    }

    private static CreatedAtRoute<CreateCountryResponse> MapToCreatedAtRoute(CountryAggregate aggregate)
    {
        Country countryDto = aggregate.ToDto();

        Guid countryId = countryDto.Id;

        return TypedResults.CreatedAtRoute(
            new CreateCountryResponse(countryDto),
            V1Endpoints.Countries.GetCountry,
            new RouteValueDictionary { { nameof(countryId), countryId } }
        );
    }

    internal sealed class EndpointMapper : IEndpointMapper
    {
        public void MapEndpoint(RouteGroupBuilder routeBuilder)
        {
            routeBuilder
                .MapPost("countries", ExecuteAsync)
                .WithName(V1Endpoints.Countries.CreateCountry)
                .AddedInVersion1Point0()
                .WithSummary("Create a country")
                .WithDescription("Creates a new country from the request body.")
                .WithTags(V1Tags.Countries)
                .Produces<CreateCountryResponse>(StatusCodes.Status201Created)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status409Conflict)
                .ProducesProblem(StatusCodes.Status422UnprocessableEntity);
        }
    }

    internal sealed record Command : ICommand<CountryAggregate>
    {
        public required Result<CountryCode, IDomainError> ErrorOrCountryCode { get; init; }

        public required Result<CountryName, IDomainError> ErrorOrCountryName { get; init; }
    }

    [UsedImplicitly]
    internal sealed class CommandHandler(
        ICountryIdFactory idFactory,
        ICountryReadRepository readRepository,
        ICountryWriteRepository writeRepository
    ) : ICommandHandler<Command, CountryAggregate>
    {
        public async Task<Result<CountryAggregate, IDomainError>> OnHandle(Command command, CancellationToken ct)
        {
            return await CountryAggregate
                .Create()
                .WithCountryCode(command.ErrorOrCountryCode)
                .WithCountryName(command.ErrorOrCountryName)
                .Build(idFactory.Create)
                .Ensure(CountryInvariants.HasUniqueCountryCode(readRepository.GetAsQueryable()))
                .Tap(writeRepository.Add)
                .Tap(_ => writeRepository.SaveChangesAsync(ct))
                .Map(country => country);
        }
    }
}
