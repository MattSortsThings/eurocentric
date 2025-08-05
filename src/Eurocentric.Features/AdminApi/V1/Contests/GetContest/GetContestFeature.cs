using ErrorOr;
using Eurocentric.Domain.Aggregates.Contests;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.ValueObjects;
using Eurocentric.Features.AdminApi.V1.Common.Constants;
using Eurocentric.Features.AdminApi.V1.Common.Mapping;
using Eurocentric.Features.Shared.Messaging;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SlimMessageBus;
using ContestAggregate = Eurocentric.Domain.Aggregates.Contests.Contest;
using ContestDto = Eurocentric.Features.AdminApi.V1.Common.Dtos.Contest;
using Participant = Eurocentric.Domain.Aggregates.Contests.Participant;

namespace Eurocentric.Features.AdminApi.V1.Contests.GetContest;

internal static class GetContestFeature
{
    internal static async Task<IResult> ExecuteAsync([FromRoute(Name = "contestId")] Guid contestId,
        IRequestResponseBus bus,
        CancellationToken cancellationToken = default) =>
        await bus.SendWithResponseMapperAsync(new Query(contestId), TypedResults.Ok, cancellationToken);

    private static ContestDto CreateDummyContest(Guid contestId)
    {
        ContestAggregate dummyContest = new StockholmFormatContest(ContestId.FromValue(contestId),
            ContestYear.FromValue(2025).Value,
            CityName.FromValue("Basel").Value,
            [
                new Participant(
                    CountryId.FromValue(ExampleValues.CountryId),
                    ParticipantGroup.Two,
                    ActName.FromValue("JJ").Value,
                    SongTitle.FromValue("Wasted Love").Value)
            ]
        );

        return dummyContest.ToContestDto();
    }

    internal sealed record Query(Guid ContestId) : IQuery<GetContestResponse>;

    [UsedImplicitly]
    internal sealed class QueryHandler : IQueryHandler<Query, GetContestResponse>
    {
        public async Task<ErrorOr<GetContestResponse>> OnHandle(Query query, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            ContestDto dummyContest = CreateDummyContest(query.ContestId);

            return new GetContestResponse(dummyContest);
        }
    }
}
