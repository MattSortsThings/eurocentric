using CSharpFunctionalExtensions;
using Eurocentric.Apis.Admin.V0.Config;
using Eurocentric.Components.EndpointMapping;
using Eurocentric.Domain.Functional;
using Eurocentric.Domain.V0.Aggregates.Countries;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SlimMessageBus;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace Eurocentric.Apis.Admin.V0.Features.Countries;

internal static class DeleteCountryV0Point1
{
    private static async Task<IResult> ExecuteAsync(
        [FromRoute(Name = "countryId")] Guid countryId,
        [FromServices] IRequestResponseBus bus,
        CancellationToken ct = default
    )
    {
        UnitResult<IDomainError> result = await bus.Send(new UnitCommand(countryId), cancellationToken: ct);

        return result.IsSuccess
            ? TypedResults.NoContent()
            : throw new InvalidOperationException("Unit command failed.");
    }

    internal sealed class EndpointMapper : IEndpointMapper
    {
        public void MapEndpoint(RouteGroupBuilder routeBuilder)
        {
            routeBuilder
                .MapDelete("v0.1/countries/{countryId:guid}", ExecuteAsync)
                .WithName("AdminApi.V0.1.DeleteCountry")
                .WithSummary("Delete a country")
                .WithDescription("Deletes a single country from the system, specified by its ID.")
                .WithTags(EndpointConstants.Tags.Countries)
                .Produces(StatusCodes.Status204NoContent)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .ProducesProblem(StatusCodes.Status409Conflict);
        }
    }

    internal sealed record UnitCommand(Guid CountryId) : IUnitCommand;

    [UsedImplicitly]
    internal sealed class UnitCommandHandler(ICountryWriteRepository writeRepository) : IUnitCommandHandler<UnitCommand>
    {
        public async Task<UnitResult<IDomainError>> OnHandle(UnitCommand unitCommand, CancellationToken ct)
        {
            return await writeRepository
                .GetByIdAsync(unitCommand.CountryId, ct)
                .Ensure(CountryRules.CanBeDeleted)
                .Tap(writeRepository.Remove)
                .Tap(_ => writeRepository.SaveChangesAsync(ct))
                .Bind(_ => UnitResult.Success<IDomainError>());
        }
    }
}

internal static class DeleteCountryV0Point2
{
    private static async Task<IResult> ExecuteAsync(
        [FromRoute(Name = "countryId")] Guid countryId,
        [FromServices] IRequestResponseBus bus,
        CancellationToken ct = default
    )
    {
        UnitResult<IDomainError> result = await bus.Send(new UnitCommand(countryId), cancellationToken: ct);

        return result.IsSuccess
            ? TypedResults.NoContent()
            : throw new InvalidOperationException("Unit command failed.");
    }

    internal sealed class EndpointMapper : IEndpointMapper
    {
        public void MapEndpoint(RouteGroupBuilder routeBuilder)
        {
            routeBuilder
                .MapDelete("v0.2/countries/{countryId:guid}", ExecuteAsync)
                .WithName("AdminApi.V0.2.DeleteCountry")
                .WithSummary("Delete a country")
                .WithDescription("Deletes a single country from the system, specified by its ID.")
                .WithTags(EndpointConstants.Tags.Countries)
                .Produces(StatusCodes.Status204NoContent)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .ProducesProblem(StatusCodes.Status409Conflict);
        }
    }

    internal sealed record UnitCommand(Guid CountryId) : IUnitCommand;

    [UsedImplicitly]
    internal sealed class UnitCommandHandler(ICountryWriteRepository writeRepository) : IUnitCommandHandler<UnitCommand>
    {
        public async Task<UnitResult<IDomainError>> OnHandle(UnitCommand unitCommand, CancellationToken ct)
        {
            return await writeRepository
                .GetByIdAsync(unitCommand.CountryId, ct)
                .Ensure(CountryRules.CanBeDeleted)
                .Tap(writeRepository.Remove)
                .Tap(_ => writeRepository.SaveChangesAsync(ct))
                .Bind(_ => UnitResult.Success<IDomainError>());
        }
    }
}
