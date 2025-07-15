using ErrorOr;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.ErrorHandling;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.Aggregates.Contests;

/// <summary>
///     Represents a contest aggregate using the <see cref="Enums.ContestFormat.Liverpool" /> contest format.
/// </summary>
public sealed class LiverpoolFormatContest : Contest
{
    private LiverpoolFormatContest()
    {
    }

    private LiverpoolFormatContest(ContestId id, List<Participant> participants, ContestYear contestYear, CityName cityName) :
        base(id, participants, contestYear, cityName)
    {
    }

    /// <inheritdoc />
    public override ContestFormat ContestFormat { get; private protected init; } = ContestFormat.Liverpool;

    /// <summary>
    ///     Starts the process of creating a new <see cref="LiverpoolFormatContest" /> instance using the fluent builder.
    /// </summary>
    /// <returns>
    ///     A new <see cref="ContestBuilder" /> instance configured to build an instance of the
    ///     <see cref="LiverpoolFormatContest" /> derivative.
    /// </returns>
    public static ContestBuilder Create() => new Builder();

    private sealed class Builder : ContestBuilder
    {
        public override ErrorOr<Contest> Build(Func<ContestId> idProvider)
        {
            ArgumentNullException.ThrowIfNull(idProvider);

            return Tuple.Create(ErrorsOrContestYear,
                    ErrorsOrCityName,
                    ErrorsOrParticipants.Collect()
                        .FailIf(DuplicateParticipatingCountryIds, ContestErrors.DuplicateParticipatingCountryIds())
                        .FailIf(IllegalParticipantGroupSizes, ContestErrors.IllegalLiverpoolFormatParticipantGroups()))
                .Combine()
                .Then(Contest (tuple) => new LiverpoolFormatContest(idProvider(), tuple.Item3, tuple.Item1, tuple.Item2));
        }

        private static bool IllegalParticipantGroupSizes(IList<Participant> participants)
        {
            Dictionary<ParticipantGroup, int> groupSizes = participants.GroupBy(participant => participant.ParticipantGroup)
                .ToDictionary(grouping => grouping.Key, grouping => grouping.Count());

            return groupSizes.TryGetValue(ParticipantGroup.Zero, out int group0Size) is false || group0Size != 1
                || groupSizes.TryGetValue(ParticipantGroup.One, out int group1Size) is false || group1Size < 3
                || groupSizes.TryGetValue(ParticipantGroup.Two, out int group2Size) is false || group2Size < 3;
        }
    }
}
