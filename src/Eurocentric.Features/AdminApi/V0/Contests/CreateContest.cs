using System.ComponentModel;
using ErrorOr;
using Eurocentric.Features.AdminApi.V0.Common;
using Eurocentric.Features.AdminApi.V0.Contests.Common;
using Eurocentric.Features.Shared.Documentation;
using Eurocentric.Features.Shared.ErrorHandling;
using Eurocentric.Features.Shared.Messaging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
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
        apiGroup.MapPost("contests", Endpoint.HandleAsync)
            .WithName("AdminApi.V0.CreateContest")
            .HasApiVersion(0, 2)
            .WithSummary("Create a contest")
            .WithDescription("Creates a new contest from the request body.")
            .WithTags(EndpointTags.Contests)
            .Produces<CreateContestResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status422UnprocessableEntity);

        return apiGroup;
    }

    internal sealed record Command(int ContestYear, string CityName, ContestFormat ContestFormat)
        : ICommand<CreateContestResponse>;

    internal sealed class Handler : ICommandHandler<Command, CreateContestResponse>
    {
        public async Task<ErrorOr<CreateContestResponse>> OnHandle(Command command, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            var (contestYear, cityName, contestFormat) = command;

            if (contestYear is < 2016 or > 2050)
            {
                return Error.Failure("Illegal contest year value",
                    "Contest year value must be an integer in the range [2016, 2050].",
                    new Dictionary<string, object> { ["contestYear"] = contestYear });
            }

            Contest contest = contestFormat switch
            {
                ContestFormat.Liverpool => new Contest(Guid.NewGuid(), contestYear, cityName, contestFormat),
                ContestFormat.Stockholm => new Contest(Guid.NewGuid(), contestYear, cityName, contestFormat),
                _ => throw new InvalidEnumArgumentException(nameof(contestFormat), (int)contestFormat, typeof(ContestFormat))
            };

            return ErrorOrFactory.From(new CreateContestResponse(contest));
        }
    }

    private static class Endpoint
    {
        internal static async Task<Results<CreatedAtRoute<CreateContestResponse>, ProblemHttpResult>> HandleAsync(
            [FromBody] CreateContestRequest request,
            IRequestResponseBus bus,
            CancellationToken cancellationToken = default) => await InitializeCommand(request)
            .ThenAsync(command => bus.Send(command, cancellationToken: cancellationToken))
            .ToResultOrProblemAsync(MapToCreatedAtRoute);

        private static CreatedAtRoute<CreateContestResponse> MapToCreatedAtRoute(CreateContestResponse response) =>
            TypedResults.CreatedAtRoute(response,
                "AdminApi.V0.GetContest",
                new RouteValueDictionary { { "contestId", response.Contest.Id } });

        private static ErrorOr<Command> InitializeCommand(CreateContestRequest request) =>
            ErrorOrFactory.From(new Command(request.ContestYear, request.CityName, request.ContestFormat));
    }
}
