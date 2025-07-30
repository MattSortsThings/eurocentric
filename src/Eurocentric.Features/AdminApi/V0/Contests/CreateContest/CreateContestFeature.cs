using System.ComponentModel;
using ErrorOr;
using Eurocentric.Domain.V0Entities;
using Eurocentric.Features.AdminApi.V0.Common.Constants;
using Eurocentric.Features.AdminApi.V0.Common.Enums;
using Eurocentric.Features.AdminApi.V0.Common.Mappings;
using Eurocentric.Features.Shared.Messaging;
using Eurocentric.Infrastructure.DataAccess.EfCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using SlimMessageBus;

namespace Eurocentric.Features.AdminApi.V0.Contests.CreateContest;

internal static class CreateContestFeature
{
    internal static async Task<IResult> ExecuteAsync([FromBody] CreateContestRequest requestBody,
        IRequestResponseBus bus,
        CancellationToken cancellationToken = default)
    {
        ErrorOr<CreateContestResponse> errorsOrResponse =
            await bus.Send(requestBody.ToCommand(), cancellationToken: cancellationToken);

        return MapToCreatedAtRoute(errorsOrResponse.Value);
    }

    private static CreatedAtRoute<CreateContestResponse> MapToCreatedAtRoute(CreateContestResponse responseBody) =>
        TypedResults.CreatedAtRoute(responseBody,
            EndpointNames.Routes.Contests.GetContest,
            new RouteValueDictionary { { "contestId", responseBody.Contest.Id } });

    private static Command ToCommand(this CreateContestRequest requestBody) => new(requestBody.ContestYear,
        requestBody.CityName,
        requestBody.ContestFormat,
        requestBody.ParticipatingCountryIds);

    internal sealed record Command(
        int ContestYear,
        string CityName,
        ContestFormat ContestFormat,
        Guid[] ParticipatingCountryIds) : ICommand<CreateContestResponse>;

    internal sealed class CommandHandler(AppDbContext dbContext) : ICommandHandler<Command, CreateContestResponse>
    {
        public async Task<ErrorOr<CreateContestResponse>> OnHandle(Command command, CancellationToken cancellationToken)
        {
            var (contestYear, cityName, contestFormat, participatingCountryIds) = command;

            Contest contest = GetFactoryFunction(contestFormat)(contestYear, cityName, participatingCountryIds);

            if (contest.ContestYear is < 2016 or > 2050)
            {
                return Error.Failure("Illegal contest year value",
                    "Contest year value must be an integer between 2016 and 2050.",
                    new Dictionary<string, object> { { "contestYear", contestYear } });
            }

            if (dbContext.V0Contests.AsNoTracking()
                .AsSplitQuery()
                .Any(existing => existing.ContestYear == contest.ContestYear))
            {
                return Error.Conflict("Contest year conflict",
                    "A contest already exists with the specified contest year.",
                    new Dictionary<string, object> { { "contestYear", contestYear } });
            }

            dbContext.V0Contests.Add(contest);
            await dbContext.SaveChangesAsync(cancellationToken);

            return new CreateContestResponse(contest.ToContestDto());
        }

        private static Func<int, string, Guid[], Contest> GetFactoryFunction(ContestFormat contestFormat) => contestFormat switch
        {
            ContestFormat.Liverpool => Contest.CreateLiverpoolFormat,
            ContestFormat.Stockholm => Contest.CreateStockholmFormat,
            _ => throw new InvalidEnumArgumentException(nameof(contestFormat), (int)contestFormat, typeof(ContestFormat))
        };
    }
}
