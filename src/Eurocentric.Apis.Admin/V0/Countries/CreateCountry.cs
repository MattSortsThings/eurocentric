using System.ComponentModel;
using CSharpFunctionalExtensions;
using Eurocentric.Apis.Admin.V0.Common.Config;
using Eurocentric.Apis.Admin.V0.Common.Contracts.Countries;
using Eurocentric.Apis.Admin.V0.Common.Enums;
using Eurocentric.Components.Endpoints;
using Eurocentric.Domain.Abstractions.Errors;
using Eurocentric.Domain.Abstractions.Messaging;
using Eurocentric.Domain.Abstractions.Repositories;
using Eurocentric.Domain.Aggregates.V0;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SlimMessageBus;
using Country = Eurocentric.Domain.Aggregates.V0.Country;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace Eurocentric.Apis.Admin.V0.Countries;

internal static class CreateCountry
{
    private static async Task<IResult> ExecuteAsync(
        [FromBody] CreateCountryRequest request,
        [FromServices] IRequestResponseBus bus,
        CancellationToken cancellationToken = default
    )
    {
        Result<Country, IDomainError> result = await bus.Send(
            request.ToCommand(),
            cancellationToken: cancellationToken
        );

        return result.IsSuccess
            ? MapToCreatedAtRoute(result.Value)
            : throw new InvalidOperationException("Request failed.");
    }

    private static Command ToCommand(this CreateCountryRequest request)
    {
        return request.CountryType switch
        {
            CountryType.Real or CountryType.Pseudo => new Command(request.CountryCode, request.CountryName),
            _ => throw new InvalidEnumArgumentException($"Invalid CountryType enum value: {request.CountryType}."),
        };
    }

    private static CreatedAtRoute<CreateCountryResponse> MapToCreatedAtRoute(Country country)
    {
        Guid countryId = country.Id;

        return TypedResults.CreatedAtRoute(
            country.ToCreateCountryResponse(),
            EndpointIds.Countries.GetCountry,
            new RouteValueDictionary { { nameof(countryId), countryId } }
        );
    }

    internal sealed class Endpoint : IEndpointMapper
    {
        public void Map(IEndpointRouteBuilder routeBuilder)
        {
            routeBuilder
                .MapPost("v0.1/countries", ExecuteAsync)
                .WithName(EndpointIds.Countries.CreateCountry)
                .WithSummary("Create a country")
                .WithDescription("Creates a new country.")
                .WithTags(EndpointTags.Countries)
                .Produces<CreateCountryResponse>(StatusCodes.Status201Created);
        }
    }

    internal sealed record Command(string CountryCode, string CountryName) : ICommand<Country>;

    internal sealed class CommandHandler(ICountryRepository repository, IUnitOfWork unitOfWork)
        : ICommandHandler<Command, Country>
    {
        public async Task<Result<Country, IDomainError>> OnHandle(Command command, CancellationToken cancellationToken)
        {
            return await Country
                .Create(command.CountryCode, command.CountryName)
                .Ensure(
                    country =>
                        repository
                            .GetUntrackedQueryable()
                            .All(existingCountry => existingCountry.CountryCode != country.CountryCode),
                    country => CountryErrors.IllegalCountryCodeValue(country.CountryCode)
                )
                .Tap(repository.Add)
                .Tap(_ => unitOfWork.CommitAsync(cancellationToken))
                .Map(country => country);
        }
    }
}
