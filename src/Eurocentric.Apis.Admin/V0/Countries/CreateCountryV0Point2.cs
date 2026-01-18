using System.ComponentModel;
using CSharpFunctionalExtensions;
using Eurocentric.Apis.Admin.V0.Common.Configuration;
using Eurocentric.Apis.Admin.V0.Common.Dtos.Countries;
using Eurocentric.Apis.Admin.V0.Common.Enums;
using Eurocentric.Components.EndpointMapping;
using Eurocentric.Components.Messaging;
using Eurocentric.Domain.Aggregates.Placeholders;
using Eurocentric.Domain.Errors;
using Eurocentric.Domain.Persistence;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SlimMessageBus;
using CountryAggregate = Eurocentric.Domain.Aggregates.Placeholders.Country;
using CountryDto = Eurocentric.Apis.Admin.V0.Common.Dtos.Countries.Country;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace Eurocentric.Apis.Admin.V0.Countries;

internal static class CreateCountryV0Point2
{
    private static async Task<IResult> ExecuteAsync(
        [FromBody] CreateCountryRequestBody requestBody,
        [FromServices] IRequestResponseBus bus,
        CancellationToken cancellationToken = default
    )
    {
        Result<CommandResult, DomainError> result = await bus.Send(
            requestBody.MapToCommand(),
            cancellationToken: cancellationToken
        );

        return result.IsSuccess
            ? MapToCreatedAtRoute(result.Value)
            : TypedResults.InternalServerError("Request failed");
    }

    private static Command MapToCommand(this CreateCountryRequestBody requestBody) =>
        new(requestBody.CountryCode, requestBody.CountryName, requestBody.CountryType);

    private static CreatedAtRoute<CreateCountryResponseBody> MapToCreatedAtRoute(CommandResult result)
    {
        CountryDto country = result.Country;

        CreateCountryResponseBody responseBody = new(result.Country);
        RouteValueDictionary routeValues = new() { { "countryId", country.Id } };

        return TypedResults.CreatedAtRoute(responseBody, "AdminApi.V0.GetCountryV0Point2", routeValues);
    }

    internal sealed class Endpoint : IEndpointMapper
    {
        public void Map(IEndpointRouteBuilder builder)
        {
            builder
                .MapPost("v0.2/countries", ExecuteAsync)
                .WithName("AdminApi.V0.CreateCountryV0Point2")
                .WithSummary("Create a country")
                .WithTags(EndpointTags.CountriesAdmin)
                .Produces<CreateCountryResponseBody>(StatusCodes.Status201Created)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status409Conflict)
                .ProducesProblem(StatusCodes.Status422UnprocessableEntity);
        }
    }

    internal readonly record struct CommandResult(CountryDto Country);

    internal sealed record Command(string CountryCode, string CountryName, CountryType CountryType)
        : ICommand<CommandResult>;

    [UsedImplicitly(Reason = "CommandHandler")]
    internal sealed class CommandHandler(ICountryRepository repository, IUnitOfWork unitOfWork)
        : ICommandHandler<Command, CommandResult>
    {
        public async Task<Result<CommandResult, DomainError>> OnHandle(
            Command command,
            CancellationToken cancellationToken
        )
        {
            return await TryBuild(command)
                .Check(CountryInvariants.CountryCodeIsUnique(repository.GetUntrackedQueryable()))
                .Tap(repository.Add)
                .Tap(_ => unitOfWork.CommitAsync(cancellationToken))
                .Map(CountryMapper.MapToDto)
                .Map(country => new CommandResult(country));
        }

        private static Result<CountryAggregate, DomainError> TryBuild(Command command)
        {
            (string countryCode, string countryName, CountryType countryType) = command;

            return countryType switch
            {
                CountryType.Real => CountryAggregate.TryCreateReal(countryCode, countryName),
                CountryType.Pseudo => CountryAggregate.TryCreatePseudo(countryCode, countryName),
                _ => throw new InvalidEnumArgumentException(nameof(countryType), (int)countryType, typeof(CountryType)),
            };
        }
    }
}
