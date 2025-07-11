using ErrorOr;
using Eurocentric.Domain.Aggregates.Contests;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.ValueObjects;
using Eurocentric.Features.AdminApi.V1.Common.Constants;
using Eurocentric.Features.Shared.ErrorHandling;
using Eurocentric.Features.Shared.Messaging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SlimMessageBus;
using Contest = Eurocentric.Features.AdminApi.V1.Common.Contracts.Contest;
using DomainContest = Eurocentric.Domain.Aggregates.Contests.Contest;

namespace Eurocentric.Features.AdminApi.V1.Contests;

public sealed record GetContestResponse(Contest Contest);

internal static class GetContest
{
    internal static IEndpointRouteBuilder MapGetContest(this IEndpointRouteBuilder v1Group)
    {
        v1Group.MapGet("contests/{contestId:guid}", ExecuteAsync)
            .WithName(EndpointConstants.Names.Contests.GetContest)
            .WithSummary("Get a contest")
            .WithDescription("Retrieves a single contest.")
            .WithTags(EndpointConstants.Tags.Contests)
            .HasApiVersion(1, 0)
            .Produces<GetContestResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound);

        return v1Group;
    }

    private static async Task<Results<ProblemHttpResult, Ok<GetContestResponse>>> ExecuteAsync(
        [FromRoute(Name = "contestId")] Guid contestId,
        IRequestResponseBus bus,
        CancellationToken cancellationToken = default) => await InitializeQuery(contestId)
        .ThenAsync(query => bus.Send(query, cancellationToken: cancellationToken))
        .ToProblemOrResponseAsync(TypedResults.Ok);

    private static ErrorOr<Query> InitializeQuery(Guid contestId) => ErrorOrFactory.From(new Query(contestId));

    internal sealed record Query(Guid ContestId) : IQuery<GetContestResponse>;

    internal sealed class Handler : IQueryHandler<Query, GetContestResponse>
    {
        public async Task<ErrorOr<GetContestResponse>> OnHandle(Query query, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            Func<DomainContest, Contest> mapper = Projections.ContestToContestDto.Compile();

            DomainContest dummyContest = new LiverpoolFormatContest(ContestId.FromValue(query.ContestId),
                [Participant.CreateInGroup0(CountryId.FromValue(ExampleIds.Country))],
                ContestYear.FromValue(2025).Value,
                CityName.FromValue("Basel").Value);

            return ErrorOrFactory.From(new GetContestResponse(mapper(dummyContest)));
        }
    }
}
