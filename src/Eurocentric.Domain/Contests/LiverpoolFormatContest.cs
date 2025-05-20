using ErrorOr;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.ErrorHandling;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.Contests;

/// <summary>
///     Represents a contest aggregate using the Liverpool contest format.
/// </summary>
public sealed class LiverpoolFormatContest : Contest
{
    private LiverpoolFormatContest()
    {
    }

    private LiverpoolFormatContest(ContestId id, ContestYear year, CityName cityName, List<Participant> participants) :
        base(id, year, cityName, participants)
    {
    }

    /// <inheritdoc />
    public override ContestFormat Format { get; private protected init; } = ContestFormat.Liverpool;

    public static ContestBuilder Create() => new Builder();

    private sealed class Builder : ContestBuilder
    {
        private const int RequiredGroup0Size = 1;
        private const int MinLegalGroup1Size = 3;
        private const int MinLegalGroup2Size = 3;

        public override ErrorOr<Contest> Build(Func<ContestId> idProvider)
        {
            ArgumentNullException.ThrowIfNull(idProvider);

            ErrorOr<List<Participant>> errorsOrParticipants = ErrorsOrParticipants.Collect()
                .FailIf(DuplicateParticipatingCountryIds, ContestErrors.DuplicateParticipatingCountryIds())
                .FailIf(IllegalGroupSizes, ContestErrors.IllegalLiverpoolFormatGroupSizes());

            return (ErrorsOrYear, ErrorsOrCityName, errorsOrParticipants)
                .Combine()
                .Then(Contest (tuple) => new LiverpoolFormatContest(idProvider(), tuple.First, tuple.Second, tuple.Third));
        }

        private static bool DuplicateParticipatingCountryIds(IList<Participant> participants) =>
            participants.GroupBy(p => p.ParticipatingCountryId)
                .Any(grouping => grouping.Count() > 1);

        private static bool IllegalGroupSizes(IList<Participant> participants)
        {
            Dictionary<ParticipantGroup, int> groupSizes = participants.Select(p => p.Group)
                .Concat(Enum.GetValues<ParticipantGroup>())
                .GroupBy(group => group)
                .ToDictionary(grouping => grouping.Key, grouping => grouping.Count() - 1);

            return groupSizes[ParticipantGroup.Zero] != RequiredGroup0Size
                   || groupSizes[ParticipantGroup.One] < MinLegalGroup1Size
                   || groupSizes[ParticipantGroup.Two] < MinLegalGroup2Size;
        }
    }
}
