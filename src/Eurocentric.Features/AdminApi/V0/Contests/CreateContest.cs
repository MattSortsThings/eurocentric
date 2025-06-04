using System.ComponentModel;
using ErrorOr;
using Eurocentric.Features.AdminApi.V0.Common.Constants;
using Eurocentric.Features.AdminApi.V0.Common.Dtos;
using Eurocentric.Features.AdminApi.V0.Common.Enums;
using Eurocentric.Features.AdminApi.V0.Common.Mapping;
using Eurocentric.Features.Shared.ErrorHandling;
using Eurocentric.Features.Shared.Messaging;
using Eurocentric.Infrastructure.InMemoryRepositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
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
        apiGroup.MapPost("contests", HandleAsync)
            .WithName(EndpointIds.Contests.CreateContest)
            .WithSummary("Create a contest")
            .WithDescription("Creates a new contest.")
            .HasApiVersion(0, 2)
            .Produces<CreateContestResponse>(StatusCodes.Status201Created)
            .WithTags(EndpointTags.Contests);

        return apiGroup;
    }

    private static async Task<IResult> HandleAsync([FromBody] CreateContestRequest request,
        IRequestResponseBus bus,
        CancellationToken cancellationToken = default) => await InitializeCommand(request)
        .ThenAsync(command => bus.Send(command, cancellationToken: cancellationToken))
        .ToProblemOrResponseAsync(MapToCreatedAtRoute);

    private static ErrorOr<Command> InitializeCommand(CreateContestRequest request) =>
        ErrorOrFactory.From(new Command(request.ContestYear, request.CityName, request.ContestFormat));

    private static CreatedAtRoute<CreateContestResponse> MapToCreatedAtRoute(CreateContestResponse response) =>
        TypedResults.CreatedAtRoute(response,
            EndpointIds.Contests.GetContest,
            new RouteValueDictionary { ["contestId"] = response.Contest.Id });

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
