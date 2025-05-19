using ErrorOr;
using Eurocentric.Domain.Contests;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.ValueObjects;
using Eurocentric.Features.AdminApi.V1.Common.Constants;
using Eurocentric.Features.AdminApi.V1.Common.Dtos;
using Eurocentric.Features.Shared.ErrorHandling;
using Eurocentric.Features.Shared.Messaging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SlimMessageBus;
using Contest = Eurocentric.Features.AdminApi.V1.Common.Dtos.Contest;
using Participant = Eurocentric.Domain.Contests.Participant;

namespace Eurocentric.Features.AdminApi.V1.Contests;

public sealed record GetContestResponse(Contest Contest);

internal static class GetContest
{
    internal static IEndpointRouteBuilder MapGetContest(this IEndpointRouteBuilder apiVersionGroup)
    {
        apiVersionGroup.MapGet("contests/{contestId:guid}", Endpoint.HandleAsync)
            .WithName(RouteIds.Contests.GetContest)
            .HasApiVersion(1, 0)
            .WithSummary("Get a contest")
            .WithDescription("Retrieves a single contest.")
            .WithTags(EndpointTags.Contests)
            .Produces<GetContestResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound);

        return apiVersionGroup;
    }

    internal sealed record Query(Guid ContestId) : IQuery<GetContestResponse>;

    internal sealed class Handler : IQueryHandler<Query, GetContestResponse>
    {
        public async Task<ErrorOr<GetContestResponse>> OnHandle(Query query, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            Contest contest = char.IsDigit(query.ContestId.ToString()[0])
                ? CreateLiverpoolFormatContest()
                : CreateStockholmFormatContest();

            contest = contest with { Id = query.ContestId };

            return ErrorOrFactory.From(new GetContestResponse(contest));
        }

        private static Contest CreateStockholmFormatContest()
        {
            List<Participant> participants =
            [
                CreateGroupOneParticipant(),
                CreateGroupOneParticipant(),
                CreateGroupOneParticipant(),
                CreateGroupTwoParticipant(),
                CreateGroupTwoParticipant(),
                CreateGroupTwoParticipant()
            ];

            return new StockholmFormatContest(ContestId.Create(DateTimeOffset.UtcNow),
                ContestYear.FromValue(2022).Value,
                CityName.FromValue("Turin").Value,
                participants).ToContestDto();
        }

        private static Contest CreateLiverpoolFormatContest()
        {
            List<Participant> participants =
            [
                CreateGroupZeroParticipant(),
                CreateGroupOneParticipant(),
                CreateGroupOneParticipant(),
                CreateGroupOneParticipant(),
                CreateGroupTwoParticipant(),
                CreateGroupTwoParticipant(),
                CreateGroupTwoParticipant()
            ];

            return new LiverpoolFormatContest(ContestId.Create(DateTimeOffset.UtcNow),
                ContestYear.FromValue(2023).Value,
                CityName.FromValue("Liverpool").Value,
                participants).ToContestDto();
        }

        private static Participant CreateGroupZeroParticipant() =>
            Participant.CreateInGroupZero(CountryId.Create(DateTimeOffset.UtcNow));

        private static Participant CreateGroupOneParticipant() =>
            Participant.CreateInGroupOne(CountryId.Create(DateTimeOffset.UtcNow),
                ActName.FromValue("ActName"),
                SongTitle.FromValue("SongTitle")).Value;

        private static Participant CreateGroupTwoParticipant() =>
            Participant.CreateInGroupTwo(CountryId.Create(DateTimeOffset.UtcNow),
                ActName.FromValue("ActName"),
                SongTitle.FromValue("SongTitle")).Value;
    }

    private static class Endpoint
    {
        internal static async Task<Results<Ok<GetContestResponse>, ProblemHttpResult>> HandleAsync(
            [FromRoute(Name = "contestId")] Guid contestId,
            IRequestResponseBus bus,
            CancellationToken cancellationToken = default) => await InitializeQuery(contestId)
            .ThenAsync(query => bus.Send(query, cancellationToken: cancellationToken))
            .ToResultOrProblemAsync(TypedResults.Ok);

        private static ErrorOr<Query> InitializeQuery(Guid contestId) => ErrorOrFactory.From(new Query(contestId));
    }
}
