using System.ComponentModel;
using ErrorOr;
using Eurocentric.Features.AdminApi.V0.Common.Constants;
using Eurocentric.Features.AdminApi.V0.Common.Contracts;
using Eurocentric.Features.AdminApi.V0.Common.Extensions;
using Eurocentric.Features.Shared.Documentation;
using Eurocentric.Features.Shared.ErrorHandling;
using Eurocentric.Features.Shared.Messaging;
using Eurocentric.Infrastructure.DataAccess.EfCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using SlimMessageBus;

namespace Eurocentric.Features.AdminApi.V0.Contests;

public sealed record CreateContestRequest : IExampleProvider<CreateContestRequest>
{
    public required int ContestYear { get; init; }

    public required string CityName { get; init; }

    public required ContestFormat ContestFormat { get; init; }

    public static CreateContestRequest CreateExample() => new()
    {
        ContestYear = 2025, CityName = "Basel", ContestFormat = ContestFormat.Liverpool
    };
}

public sealed record CreateContestResponse(Contest Contest);

internal static class CreateContest
{
    internal static IEndpointRouteBuilder MapCreateContest(this IEndpointRouteBuilder apiGroup)
    {
        apiGroup.MapPost("contests", ExecuteAsync)
            .WithName(EndpointNames.Contests.CreateContest)
            .WithSummary("Create a contest")
            .WithDescription("Creates a new contest.")
            .WithTags(EndpointTags.Contests)
            .HasApiVersion(0, 1)
            .HasApiVersion(0, 2)
            .Produces<CreateContestResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status422UnprocessableEntity);

        return apiGroup;
    }

    private static async Task<Results<ProblemHttpResult, CreatedAtRoute<CreateContestResponse>>> ExecuteAsync(
        [FromBody] CreateContestRequest requestBody,
        IRequestResponseBus bus,
        CancellationToken cancellationToken = default) => await InitializeCommand(requestBody)
        .ThenAsync(command => bus.Send(command, cancellationToken: cancellationToken))
        .ToProblemOrResponseAsync(MapToCreatedAtRoute);

    private static ErrorOr<Command> InitializeCommand(CreateContestRequest requestBody) =>
        ErrorOrFactory.From(new Command(requestBody.ContestYear, requestBody.CityName, requestBody.ContestFormat));

    private static CreatedAtRoute<CreateContestResponse> MapToCreatedAtRoute(CreateContestResponse response) =>
        TypedResults.CreatedAtRoute(response,
            EndpointNames.Contests.GetContest,
            new RouteValueDictionary { { "contestId", response.Contest.Id } });

    internal sealed record Command(int ContestYear, string CityName, ContestFormat ContestFormat)
        : ICommand<CreateContestResponse>;

    internal sealed class Handler(AppDbContext dbContext) : ICommandHandler<Command, CreateContestResponse>
    {
        public async Task<ErrorOr<CreateContestResponse>> OnHandle(Command command, CancellationToken cancellationToken)
        {
            var (contestYear, cityName, contestFormat) = command;

            Domain.PlaceholderEntities.Contest createdContest = contestFormat switch
            {
                ContestFormat.Stockholm => Domain.PlaceholderEntities.Contest.CreateStockholmFormat(contestYear, cityName),
                ContestFormat.Liverpool => Domain.PlaceholderEntities.Contest.CreateLiverpoolFormat(contestYear, cityName),
                _ => throw new InvalidEnumArgumentException(nameof(contestFormat), (int)contestFormat, typeof(ContestFormat))
            };

            if (IllegalContestYearValue(createdContest))
            {
                return Error.Failure("Illegal contest year value",
                    "Contest year value must be an integer between 2016 and 2050.",
                    new Dictionary<string, object> { { "contestYear", createdContest.ContestYear } });
            }

            if (ContestYearConflict(createdContest))
            {
                return Error.Conflict("Contest year conflict",
                    "A contest already exists with the provided contest year.",
                    new Dictionary<string, object> { { "contestYear", createdContest.ContestYear } });
            }

            await dbContext.PlaceholderContests.AddAsync(createdContest, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            return new CreateContestResponse(createdContest.ToContestDto());
        }

        private bool ContestYearConflict(Domain.PlaceholderEntities.Contest createdContest) => dbContext.PlaceholderContests
            .AsNoTracking()
            .Any(contest => contest.ContestYear == createdContest.ContestYear);

        private static bool IllegalContestYearValue(Domain.PlaceholderEntities.Contest createdContest) =>
            createdContest.ContestYear is < 2016 or > 2050;
    }
}
