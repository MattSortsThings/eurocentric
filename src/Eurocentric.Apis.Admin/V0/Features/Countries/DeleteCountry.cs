using CSharpFunctionalExtensions;
using Eurocentric.Apis.Admin.V0.Config;
using Eurocentric.Components.EndpointMapping;
using Eurocentric.Components.Messaging;
using Eurocentric.Domain.Aggregates.Countries;
using Eurocentric.Domain.Core;
using Eurocentric.Domain.ValueObjects;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SlimMessageBus;
using ICountryWriteRepository = Eurocentric.Domain.Aggregates.Countries.ICountryWriteRepository;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace Eurocentric.Apis.Admin.V0.Features.Countries;

internal static class DeleteCountry
{
    private static async Task<IResult> ExecuteAsync(
        [FromRoute(Name = "countryId")] Guid countryId,
        [FromServices] IRequestResponseBus bus,
        CancellationToken ct = default
    ) => await bus.DispatchAsync(countryId.ToUnitCommand(), ct);

    private static UnitCommand ToUnitCommand(this Guid countryId) => new(CountryId.FromValue(countryId));

    internal sealed class EndpointMapper : IEndpointMapper
    {
        public void MapEndpoint(RouteGroupBuilder routeBuilder)
        {
            routeBuilder
                .MapDelete("countries/{countryId:guid}", ExecuteAsync)
                .WithName(V0EndpointNames.Countries.DeleteCountry)
                .AddedInVersion0Point1()
                .WithSummary("Delete a country")
                .WithDescription("Deletes a single country from the system, specified by its ID.")
                .WithTags(V0Tags.Countries)
                .Produces(StatusCodes.Status204NoContent)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .ProducesProblem(StatusCodes.Status409Conflict);
        }
    }

    internal sealed record UnitCommand(CountryId CountryId) : IUnitCommand;

    [UsedImplicitly]
    internal sealed class UnitCommandHandler(ICountryWriteRepository writeRepository, IUnitOfWork unitOfWork)
        : IUnitCommandHandler<UnitCommand>
    {
        public async Task<UnitResult<IDomainError>> OnHandle(UnitCommand unitCommand, CancellationToken ct)
        {
            return await writeRepository
                .GetTrackedAsync(unitCommand.CountryId, ct)
                .Ensure(CountryInvariants.CanBeDeleted)
                .Tap(writeRepository.Remove)
                .Tap(_ => unitOfWork.SaveChangesAsync(ct))
                .Bind(_ => UnitResult.Success<IDomainError>());
        }
    }
}
