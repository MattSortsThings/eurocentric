using CSharpFunctionalExtensions;
using Eurocentric.Apis.Admin.V0.Common.Configuration;
using Eurocentric.Components.EndpointMapping;
using Eurocentric.Components.Messaging;
using Eurocentric.Domain.Aggregates.Placeholders;
using Eurocentric.Domain.Errors;
using Eurocentric.Domain.Persistence;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SlimMessageBus;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace Eurocentric.Apis.Admin.V0.Countries;

internal static class DeleteCountryV0Point2
{
    private static async Task<IResult> ExecuteAsync(
        [FromRoute(Name = "countryId")] Guid countryId,
        [FromServices] IRequestResponseBus bus,
        CancellationToken cancellationToken = default
    )
    {
        UnitResult<DomainError> result = await bus.Send(
            new UnitCommand(countryId),
            cancellationToken: cancellationToken
        );

        return result.IsSuccess ? TypedResults.NoContent() : TypedResults.InternalServerError("Request failed");
    }

    internal sealed class Endpoint : IEndpointMapper
    {
        public void Map(IEndpointRouteBuilder builder)
        {
            builder
                .MapDelete("v0.2/countries/{countryId:guid}", ExecuteAsync)
                .WithName("AdminApi.V0.DeleteCountryV0Point2")
                .WithSummary("Delete a country")
                .WithTags(EndpointTags.CountriesAdmin)
                .Produces(StatusCodes.Status204NoContent)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .ProducesProblem(StatusCodes.Status409Conflict);
        }
    }

    internal sealed record UnitCommand(Guid CountryId) : IUnitCommand;

    [UsedImplicitly(Reason = "UnitCommandHandler")]
    internal sealed class UnitCommandHandler(ICountryWriteRepository repository, IUnitOfWork unitOfWork)
        : IUnitCommandHandler<UnitCommand>
    {
        public async Task<UnitResult<DomainError>> OnHandle(
            UnitCommand unitCommand,
            CancellationToken cancellationToken
        )
        {
            Guid countryId = unitCommand.CountryId;

            return await repository
                .GetTrackedAsync(countryId, cancellationToken)
                .ToResult(() => CountryErrors.CountryNotFound(countryId))
                .Check(CountryInvariants.DeletionAllowed)
                .Tap(repository.Remove)
                .Tap(_ => unitOfWork.CommitAsync(cancellationToken))
                .Bind(_ => UnitResult.Success<DomainError>());
        }
    }
}
