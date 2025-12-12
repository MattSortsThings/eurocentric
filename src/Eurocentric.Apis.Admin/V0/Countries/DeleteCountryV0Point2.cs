using CSharpFunctionalExtensions;
using Eurocentric.Apis.Admin.V0.Common.Config;
using Eurocentric.Components.Endpoints;
using Eurocentric.Domain.Abstractions.Errors;
using Eurocentric.Domain.Abstractions.Messaging;
using Eurocentric.Domain.Abstractions.Repositories;
using Eurocentric.Domain.Aggregates.V0;
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
        UnitResult<IDomainError> result = await bus.Send(
            new UnitCommand(countryId),
            cancellationToken: cancellationToken
        );

        return result.IsSuccess ? TypedResults.NoContent() : throw new InvalidOperationException("Request failed.");
    }

    internal sealed class Endpoint : IEndpointMapper
    {
        public void Map(IEndpointRouteBuilder routeBuilder)
        {
            routeBuilder
                .MapDelete("v0.2/countries/{countryId:guid}", ExecuteAsync)
                .WithName(EndpointIds.Countries.DeleteCountryV0Point2)
                .WithSummary("Delete a country")
                .WithDescription("Deletes the requested country.")
                .WithTags(EndpointTags.Countries)
                .Produces(StatusCodes.Status204NoContent);
        }
    }

    internal sealed record UnitCommand(Guid CountryId) : IUnitCommand;

    [UsedImplicitly]
    internal sealed class UnitCommandHandler(ICountryRepository repository, IUnitOfWork unitOfWork)
        : IUnitCommandHandler<UnitCommand>
    {
        public async Task<UnitResult<IDomainError>> OnHandle(UnitCommand command, CancellationToken cancellationToken)
        {
            return await repository
                .GetTrackedAsync(command.CountryId, cancellationToken)
                .Ensure(
                    country => country.ContestRoles.Count == 0,
                    country => CountryErrors.CountryDeletionConflict(country.Id)
                )
                .Tap(repository.Remove)
                .Tap(_ => unitOfWork.CommitAsync(cancellationToken))
                .Bind(_ => UnitResult.Success<IDomainError>());
        }
    }
}
