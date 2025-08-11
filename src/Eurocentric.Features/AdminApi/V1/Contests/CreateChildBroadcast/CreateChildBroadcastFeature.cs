using System.ComponentModel;
using ErrorOr;
using Eurocentric.Domain.Aggregates.Broadcasts;
using Eurocentric.Domain.Aggregates.Contests;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.ValueObjects;
using Eurocentric.Features.AdminApi.V1.Common.Constants;
using Eurocentric.Features.AdminApi.V1.Common.Enums;
using Eurocentric.Features.AdminApi.V1.Common.Mapping;
using Eurocentric.Features.Shared.Messaging;
using Eurocentric.Infrastructure.DataAccess.EfCore;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using SlimMessageBus;

namespace Eurocentric.Features.AdminApi.V1.Contests.CreateChildBroadcast;

internal static class CreateChildBroadcastFeature
{
    internal static async Task<IResult> ExecuteAsync([FromRoute(Name = "contestId")] Guid contestId,
        [FromBody] CreateChildBroadcastRequest requestBody,
        IRequestResponseBus bus,
        CancellationToken cancellationToken = default) =>
        await bus.SendWithResponseMapperAsync(MapToCommand(contestId, requestBody), MapToCreatedAtRoute, cancellationToken);

    private static Command MapToCommand(Guid contestId, CreateChildBroadcastRequest requestBody) => new(contestId,
        requestBody.ContestStage,
        requestBody.BroadcastDate,
        requestBody.CompetingCountryIds);

    private static CreatedAtRoute<CreateChildBroadcastResponse> MapToCreatedAtRoute(CreateChildBroadcastResponse responseBody) =>
        TypedResults.CreatedAtRoute(responseBody,
            EndpointNames.Routes.Broadcasts.GetBroadcast,
            new RouteValueDictionary { { "broadcastId", responseBody.Broadcast.Id } });

    private static BroadcastBuilder Apply(this BroadcastBuilder builder, Command command) =>
        builder.WithBroadcastDate(BroadcastDate.FromValue(command.BroadcastDate))
            .WithCompetingCountryIds(command.CompetingCountryIds.Select(CountryId.FromValue));

    private static ErrorOr<BroadcastBuilder> InitializeBroadcastBuilder(this Contest contest, ContestStage contestStage) =>
        contestStage switch
        {
            ContestStage.SemiFinal1 => contest.CreateSemiFinal1(),
            ContestStage.SemiFinal2 => contest.CreateSemiFinal2(),
            ContestStage.GrandFinal => contest.CreateGrandFinal(),
            _ => throw new InvalidEnumArgumentException(nameof(contestStage), (int)contestStage, typeof(ContestStage))
        };

    internal sealed record Command(
        Guid ContestId,
        ContestStage ContestStage,
        DateOnly BroadcastDate,
        Guid[] CompetingCountryIds) : ICommand<CreateChildBroadcastResponse>;

    [UsedImplicitly]
    internal sealed class CommandHandler(AppDbContext dbContext, IBroadcastIdProvider idProvider) :
        ICommandHandler<Command, CreateChildBroadcastResponse>
    {
        public async Task<ErrorOr<CreateChildBroadcastResponse>>
            OnHandle(Command command, CancellationToken cancellationToken) => await GetTrackedContestAsync(command.ContestId)
            .Then(contest => contest.InitializeBroadcastBuilder(command.ContestStage)
                .ThenDo(builder => builder.Apply(command))
                .Then(builder => builder.Build(idProvider.CreateSingle))
                .FailOnBroadcastDateConflict(dbContext.Broadcasts.AsNoTracking().AsSplitQuery())
                .ThenDo(broadcast => contest.AddChildBroadcast(broadcast.Id, broadcast.ContestStage))
                .ThenDo(broadcast => dbContext.Broadcasts.Add(broadcast))
                .Then(broadcast => broadcast.ToBroadcastDto()))
            .ThenDoAsync(_ => dbContext.SaveChangesAsync(cancellationToken))
            .Then(broadcastDto => new CreateChildBroadcastResponse(broadcastDto));

        private async Task<ErrorOr<Contest>> GetTrackedContestAsync(Guid contestId)
        {
            ContestId id = ContestId.FromValue(contestId);

            Contest? contest = await dbContext.Contests.Where(contest => contest.Id == id)
                .FirstOrDefaultAsync();

            return contest is null
                ? ContestErrors.ContestNotFound(id)
                : contest;
        }
    }
}
