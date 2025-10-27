using CSharpFunctionalExtensions;
using Eurocentric.Domain.Core;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.ValueObjects;
using JetBrains.Annotations;

namespace Eurocentric.Domain.Aggregates.Contests;

/// <summary>
///     Represents a contest using the "Liverpool" contest rules.
/// </summary>
public sealed class LiverpoolRulesContest : Contest
{
    [UsedImplicitly(Reason = "EF Core")]
    private LiverpoolRulesContest() { }

    private LiverpoolRulesContest(
        ContestId id,
        ContestYear contestYear,
        CityName cityName,
        List<Participant> participants,
        GlobalTelevote? globalTelevote
    )
        : base(id, contestYear, cityName, participants, globalTelevote) { }

    public override ContestRules ContestRules { get; private protected init; } = ContestRules.Liverpool;

    public static IContestBuilder Create() => new Builder();

    private sealed class Builder : ContestBuilder
    {
        public override Result<Contest, IDomainError> Build(Func<ContestId> idProvider)
        {
            ArgumentNullException.ThrowIfNull(idProvider);

            return ValueTuple
                .Create(ErrorOrContestYear, ErrorOrCityName, ErrorsOrParticipants.Collect())
                .Combine()
                .Map(InitializeWithDummyId)
                .Ensure(ContestInvariants.HasLegalGlobalTelevote)
                .Ensure(ContestInvariants.HasLegalContestCountries)
                .Ensure(ContestInvariants.HasLegalParticipantCounts)
                .Tap(contest => contest.Id = idProvider())
                .Map(Contest (contest) => contest);
        }

        private LiverpoolRulesContest InitializeWithDummyId(
            ValueTuple<ContestYear, CityName, List<Participant>> tuple
        ) => new(ContestId.FromValue(Guid.Empty), tuple.Item1, tuple.Item2, tuple.Item3, GlobalTelevote);
    }
}
