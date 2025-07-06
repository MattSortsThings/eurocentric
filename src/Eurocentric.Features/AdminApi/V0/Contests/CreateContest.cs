using System.ComponentModel;
using ErrorOr;
using Eurocentric.Features.AdminApi.V0.Common.Constants;
using Eurocentric.Features.AdminApi.V0.Common.Contracts;
using Eurocentric.Features.AdminApi.V0.Common.Extensions;
using Eurocentric.Features.Shared.Messaging;
using Eurocentric.Infrastructure.DataAccess.EfCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SlimMessageBus;

namespace Eurocentric.Features.AdminApi.V0.Contests;

public sealed record CreateContestRequest
{
    public required int ContestYear { get; init; }

    public required string CityName { get; init; }

    public required ContestFormat ContestFormat { get; init; }
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
        CancellationToken cancellationToken = default)
    {
        ErrorOr<CreateContestResponse> errorsOrResponse =
            await bus.Send(requestBody.ToCommand(), cancellationToken: cancellationToken);

        return MapToCreatedAtRoute(errorsOrResponse.Value);
    }

    private static Command ToCommand(this CreateContestRequest request) =>
        new(request.ContestYear, request.CityName, request.ContestFormat);

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

            await dbContext.PlaceholderContests.AddAsync(createdContest, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            return new CreateContestResponse(createdContest.ToContestDto());
        }
    }
}
