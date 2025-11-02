using System.ComponentModel;
using CSharpFunctionalExtensions;
using Eurocentric.Apis.Admin.V1.Config;
using Eurocentric.Apis.Admin.V1.Dtos.Broadcasts;
using Eurocentric.Apis.Admin.V1.Enums;
using Eurocentric.Components.EndpointMapping;
using Eurocentric.Components.Messaging;
using Eurocentric.Domain.Aggregates.Broadcasts;
using Eurocentric.Domain.Aggregates.Contests;
using Eurocentric.Domain.Core;
using Eurocentric.Domain.ValueObjects;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SlimMessageBus;
using BroadcastAggregate = Eurocentric.Domain.Aggregates.Broadcasts.Broadcast;
using BroadcastDto = Eurocentric.Apis.Admin.V1.Dtos.Broadcasts.Broadcast;
using ContestAggregate = Eurocentric.Domain.Aggregates.Contests.Contest;
using DomainContestStage = Eurocentric.Domain.Enums.ContestStage;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace Eurocentric.Apis.Admin.V1.Features.Contests;

internal static class CreateContestBroadcast
{
    private static async Task<IResult> ExecuteAsync(
        [FromRoute(Name = "contestId")] Guid contestId,
        [FromBody] CreateContestBroadcastRequest request,
        [FromServices] IRequestResponseBus bus,
        CancellationToken ct = default
    ) => await bus.DispatchAsync(MapToCommand(contestId, request), MapToCreatedAtRoute, ct);

    private static Command MapToCommand(Guid contestId, CreateContestBroadcastRequest request)
    {
        return new Command(
            ContestId.FromValue(contestId),
            BroadcastDate.FromValue(request.BroadcastDate),
            request.ContestStage.ToDomainContestStage(),
            request.CompetingCountryIds.ToNullableCountryIds()
        );
    }

    private static CreatedAtRoute<CreateContestBroadcastResponse> MapToCreatedAtRoute(BroadcastAggregate aggregate)
    {
        BroadcastDto broadcastDto = aggregate.ToDto();
        Guid broadcastId = broadcastDto.Id;

        return TypedResults.CreatedAtRoute(
            new CreateContestBroadcastResponse(broadcastDto),
            V1Endpoints.Broadcasts.GetBroadcast,
            new RouteValueDictionary { { nameof(broadcastId), broadcastId } }
        );
    }

    private static CountryId?[] ToNullableCountryIds(this IEnumerable<Guid?> nullableGuids) =>
        nullableGuids.Select(guid => guid.HasValue ? CountryId.FromValue(guid.Value) : null).ToArray();

    internal sealed class EndpointMapper : IEndpointMapper
    {
        public void MapEndpoint(RouteGroupBuilder routeBuilder)
        {
            routeBuilder
                .MapPost("contests/{contestId:guid}/broadcasts", ExecuteAsync)
                .WithName(V1Endpoints.Contests.CreateContestBroadcast)
                .AddedInVersion1Point0()
                .WithSummary("Create a broadcast for contest")
                .WithDescription("Creates a new broadcast for the requested contest.")
                .WithTags(V1Tags.Contests)
                .Produces<CreateContestBroadcastResponse>(StatusCodes.Status201Created)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .ProducesProblem(StatusCodes.Status409Conflict)
                .ProducesProblem(StatusCodes.Status422UnprocessableEntity);
        }
    }

    internal sealed record Command(
        ContestId ContestId,
        Result<BroadcastDate, IDomainError> ErrorOrBroadcastDate,
        DomainContestStage ContestStage,
        CountryId?[] CompetingCountryIds
    ) : ICommand<BroadcastAggregate>;

    [UsedImplicitly]
    internal sealed class CommandHandler(
        IBroadcastIdFactory idFactory,
        IBroadcastRepository broadcastRepository,
        IContestWriteRepository contestWriteRepository,
        IUnitOfWork unitOfWork
    ) : ICommandHandler<Command, BroadcastAggregate>
    {
        public async Task<Result<BroadcastAggregate, IDomainError>> OnHandle(Command command, CancellationToken ct)
        {
            return await contestWriteRepository
                .GetTrackedAsync(command.ContestId, ct)
                .Map(InitializeBuilder(command.ContestStage))
                .Tap(Apply(command))
                .Bind(builder => builder.Build(idFactory.Create))
                .Ensure(BroadcastInvariants.HasUniqueBroadcastDate(broadcastRepository.GetUntrackedQueryable()))
                .Tap(broadcastRepository.Add)
                .Tap(() => unitOfWork.SaveChangesAsync(ct))
                .Map(broadcast => broadcast);
        }

        private static Func<ContestAggregate, IBroadcastBuilder> InitializeBuilder(DomainContestStage contestStage)
        {
            return contestStage switch
            {
                DomainContestStage.SemiFinal1 => contest => contest.CreateSemiFinal1Broadcast(),
                DomainContestStage.SemiFinal2 => contest => contest.CreateSemiFinal2Broadcast(),
                DomainContestStage.GrandFinal => contest => contest.CreateGrandFinalBroadcast(),
                _ => throw new InvalidEnumArgumentException(
                    nameof(contestStage),
                    (int)contestStage,
                    typeof(DomainContestStage)
                ),
            };
        }

        private static Action<IBroadcastBuilder> Apply(Command command)
        {
            return builder =>
                builder
                    .WithBroadcastDate(command.ErrorOrBroadcastDate)
                    .WithCompetingCountries(command.CompetingCountryIds);
        }
    }
}
