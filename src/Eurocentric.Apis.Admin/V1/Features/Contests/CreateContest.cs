using System.ComponentModel;
using CSharpFunctionalExtensions;
using Eurocentric.Apis.Admin.V1.Config;
using Eurocentric.Apis.Admin.V1.Dtos.Contests;
using Eurocentric.Apis.Admin.V1.Enums;
using Eurocentric.Components.EndpointMapping;
using Eurocentric.Components.Messaging;
using Eurocentric.Domain.Aggregates.Contests;
using Eurocentric.Domain.Aggregates.Countries;
using Eurocentric.Domain.Core;
using Eurocentric.Domain.ValueObjects;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SlimMessageBus;
using ApiSemiFinalDraw = Eurocentric.Apis.Admin.V1.Enums.SemiFinalDraw;
using ContestAggregate = Eurocentric.Domain.Aggregates.Contests.Contest;
using ContestDto = Eurocentric.Apis.Admin.V1.Dtos.Contests.Contest;
using DomainContestRules = Eurocentric.Domain.Enums.ContestRules;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace Eurocentric.Apis.Admin.V1.Features.Contests;

internal static class CreateContest
{
    private static async Task<IResult> ExecuteAsync(
        [FromBody] CreateContestRequest request,
        [FromServices] IRequestResponseBus bus,
        CancellationToken ct = default
    ) => await bus.DispatchAsync(request.ToCommand(), MapToCreatedAtRoute, ct);

    private static Command ToCommand(this CreateContestRequest request)
    {
        return new Command
        {
            ErrorOrContestYear = ContestYear.FromValue(request.ContestYear),
            ErrorOrCityName = CityName.FromValue(request.CityName),
            ContestRules = request.ContestRules.ToDomainContestRules(),
            GlobalTelevoteVotingCountryId = request.GlobalTelevoteVotingCountryId is { } v
                ? CountryId.FromValue(v)
                : null,
            AddParticipantActions = request.Participants.Select(MapToAddParticipantAction).ToArray(),
        };
    }

    private static Action<IContestParticipantAdder> MapToAddParticipantAction(CreateParticipantRequest request)
    {
        (Guid countryId, ApiSemiFinalDraw semiFinalDraw, string actName, string songTitle) = request;

        return semiFinalDraw switch
        {
            ApiSemiFinalDraw.SemiFinal1 => adder =>
                adder.AddSemiFinal1Participant(
                    CountryId.FromValue(countryId),
                    ActName.FromValue(actName),
                    SongTitle.FromValue(songTitle)
                ),
            ApiSemiFinalDraw.SemiFinal2 => adder =>
                adder.AddSemiFinal2Participant(
                    CountryId.FromValue(countryId),
                    ActName.FromValue(actName),
                    SongTitle.FromValue(songTitle)
                ),
            _ => throw new InvalidEnumArgumentException($"Invalid SemiFinalDraw enum value: {semiFinalDraw}."),
        };
    }

    private static CreatedAtRoute<CreateContestResponse> MapToCreatedAtRoute(ContestAggregate aggregate)
    {
        ContestDto contestDto = aggregate.ToDto();
        Guid contestId = contestDto.Id;

        return TypedResults.CreatedAtRoute(
            new CreateContestResponse(contestDto),
            V1Endpoints.Contests.GetContest,
            new RouteValueDictionary { { nameof(contestId), contestId } }
        );
    }

    internal sealed class EndpointMapper : IEndpointMapper
    {
        public void MapEndpoint(RouteGroupBuilder routeBuilder)
        {
            routeBuilder
                .MapPost("contests", ExecuteAsync)
                .WithName(V1Endpoints.Contests.CreateContest)
                .AddedInVersion1Point0()
                .WithSummary("Create a contest")
                .WithDescription("Creates a new contest from the request body.")
                .WithTags(V1Tags.Contests)
                .Produces<CreateContestResponse>(StatusCodes.Status201Created)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status409Conflict)
                .ProducesProblem(StatusCodes.Status422UnprocessableEntity);
        }
    }

    internal sealed record Command : ICommand<ContestAggregate>
    {
        public Result<ContestYear, IDomainError> ErrorOrContestYear { get; init; }

        public Result<CityName, IDomainError> ErrorOrCityName { get; init; }

        public DomainContestRules ContestRules { get; init; }

        public CountryId? GlobalTelevoteVotingCountryId { get; init; }

        public Action<IContestParticipantAdder>[] AddParticipantActions { get; init; } = [];
    }

    [UsedImplicitly]
    internal sealed class CommandHandler(
        IContestIdFactory idFactory,
        IContestRepository contestRepository,
        ICountryReadRepository countryReadRepository,
        IUnitOfWork unitOfWork
    ) : ICommandHandler<Command, ContestAggregate>
    {
        public async Task<Result<ContestAggregate, IDomainError>> OnHandle(Command command, CancellationToken ct)
        {
            return await InitializeBuilder(command.ContestRules)
                .Tap(Apply(command))
                .Bind(builder => builder.Build(idFactory.Create))
                .Ensure(ContestInvariants.HasUniqueContestYear(contestRepository.GetUntrackedQueryable()))
                .Ensure(ContestInvariants.HasNoOrphanContestCountries(countryReadRepository.GetUntrackedQueryable()))
                .Tap(contestRepository.Add)
                .Tap(() => unitOfWork.SaveChangesAsync(ct))
                .Map(contest => contest);
        }

        private static Result<IContestBuilder, IDomainError> InitializeBuilder(DomainContestRules contestRules)
        {
            IContestBuilder builder = contestRules switch
            {
                DomainContestRules.Liverpool => LiverpoolRulesContest.Create(),
                DomainContestRules.Stockholm => StockholmRulesContest.Create(),
                _ => throw new InvalidEnumArgumentException($"Invalid ContestRules enum value: {contestRules}."),
            };

            return Result.Success<IContestBuilder, IDomainError>(builder);
        }

        private static Action<IContestBuilder> Apply(Command command)
        {
            return builder =>
            {
                builder = builder.WithContestYear(command.ErrorOrContestYear).WithCityName(command.ErrorOrCityName);

                if (command.GlobalTelevoteVotingCountryId is { } countryId)
                {
                    builder = builder.WithGlobalTelevote(countryId);
                }

                foreach (Action<IContestParticipantAdder> action in command.AddParticipantActions)
                {
                    action(builder);
                }
            };
        }
    }
}
