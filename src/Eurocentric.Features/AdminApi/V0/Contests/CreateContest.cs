using System.ComponentModel;
using ErrorOr;
using Eurocentric.Features.AdminApi.V0.Common.Constants;
using Eurocentric.Features.AdminApi.V0.Common.Dtos;
using Eurocentric.Features.AdminApi.V0.Common.Enums;
using Eurocentric.Features.AdminApi.V0.Common.Mapping;
using Eurocentric.Features.Shared.Messaging;
using Eurocentric.Infrastructure.InMemoryRepositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SlimMessageBus;
using PlaceholderContest = Eurocentric.Domain.Placeholders.Contest;

namespace Eurocentric.Features.AdminApi.V0.Contests;

public sealed record CreateContestResponse(Contest Contest);

public sealed record CreateContestRequest
{
    public required int ContestYear { get; init; }

    public required string CityName { get; init; }

    public required ContestFormat ContestFormat { get; init; }
}

internal static class CreateContest
{
    internal static IEndpointRouteBuilder MapCreateContest(this IEndpointRouteBuilder apiGroup)
    {
        apiGroup.MapPost("v0.2/contests", HandleAsync)
            .WithName("AdminApi.V0.2.CreateContest")
            .WithSummary("Create a contest")
            .WithDescription("Creates a new contest.")
            .Produces<CreateContestResponse>(StatusCodes.Status201Created)
            .WithTags(EndpointTags.Contests);

        return apiGroup;
    }

    private static async Task<IResult> HandleAsync([FromBody] CreateContestRequest request,
        IRequestResponseBus bus,
        CancellationToken cancellationToken = default)
    {
        ErrorOr<CreateContestResponse> errorsOrResponse =
            await bus.Send(request.ToCommand(), cancellationToken: cancellationToken);

        return TypedResults.CreatedAtRoute(errorsOrResponse.Value,
            "AdminApi.V0.2.Contests.GetContest",
            new RouteValueDictionary { ["contestId"] = errorsOrResponse.Value.Contest.Id });
    }

    private static Command ToCommand(this CreateContestRequest request) =>
        new(request.ContestYear, request.CityName, request.ContestFormat);

    internal sealed record Command(int ContestYear, string CityName, ContestFormat ContestFormat)
        : ICommand<CreateContestResponse>;

    internal sealed class Handler(InMemoryContestRepository repository) : ICommandHandler<Command, CreateContestResponse>
    {
        public async Task<ErrorOr<CreateContestResponse>> OnHandle(Command command, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            (int contestYear, string cityName, ContestFormat contestFormat) = command;

            PlaceholderContest contest = contestFormat switch
            {
                ContestFormat.Stockholm => PlaceholderContest.CreateStockholmFormat(contestYear, cityName),
                ContestFormat.Liverpool => PlaceholderContest.CreateLiverpoolFormat(contestYear, cityName),
                _ => throw new InvalidEnumArgumentException(nameof(contestFormat), (int)contestFormat, typeof(ContestFormat))
            };

            repository.Contests.Add(contest);

            return ErrorOrFactory.From(new CreateContestResponse(contest.ToContestDto()));
        }
    }
}
