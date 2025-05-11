using System.ComponentModel;
using ErrorOr;
using Eurocentric.Features.AdminApi.V0.Common;
using Eurocentric.Features.AdminApi.V0.Contests.Common;
using Eurocentric.Features.Shared.ErrorHandling;
using Eurocentric.Features.Shared.Messaging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SlimMessageBus;

namespace Eurocentric.Features.AdminApi.V0.Contests;

public static class CreateContest
{
    internal static IEndpointRouteBuilder MapCreateContest(this IEndpointRouteBuilder apiGroup)
    {
        apiGroup.MapPost("contests", Endpoint.HandleAsync)
            .WithName("AdminApi.V0.CreateContest")
            .HasApiVersion(0, 2)
            .WithSummary("Create a contest")
            .WithDescription("Creates a new contest from the request body.")
            .WithTags(EndpointTags.Contests)
            .Produces<Response>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status422UnprocessableEntity);

        return apiGroup;
    }

    public sealed record Response(Contest Contest);

    public sealed record Request
    {
        public required int ContestYear { get; init; }

        public required string CityName { get; init; }

        public required ContestFormat ContestFormat { get; init; }
    }

    internal sealed record Command(int ContestYear, string CityName, ContestFormat ContestFormat) : ICommand<Response>;

    internal sealed class Handler : ICommandHandler<Command, Response>
    {
        public async Task<ErrorOr<Response>> OnHandle(Command command, CancellationToken cancellationToken)
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

            return ErrorOrFactory.From(new Response(contest));
        }
    }

    internal static class Endpoint
    {
        internal static async Task<Results<CreatedAtRoute<Response>, ProblemHttpResult>> HandleAsync(
            [FromBody] Request request,
            IRequestResponseBus bus,
            CancellationToken cancellationToken = default) => await InitializeCommand(request)
            .ThenAsync(command => bus.Send(command, cancellationToken: cancellationToken))
            .ToResultOrProblemAsync(MapToCreatedAtRoute);

        private static CreatedAtRoute<Response> MapToCreatedAtRoute(Response response) => TypedResults.CreatedAtRoute(response,
            "AdminApi.V0.GetContest",
            new RouteValueDictionary { { "contestId", response.Contest.Id } });

        private static ErrorOr<Command> InitializeCommand(Request request) =>
            ErrorOrFactory.From(new Command(request.ContestYear, request.CityName, request.ContestFormat));
    }
}
