using ErrorOr;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.ErrorHandling;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.ValueObjects;
using JetBrains.Annotations;

namespace Eurocentric.Domain.Aggregates.Contests;

/// <summary>
///     Represents a contest aggregate using the <see cref="Enums.ContestFormat.Stockholm" /> contest format.
/// </summary>
public sealed class StockholmFormatContest : Contest
{
    [UsedImplicitly(Reason = "EF Core")]
    private StockholmFormatContest()
    {
    }

    public StockholmFormatContest(ContestId id,
        ContestYear contestYear,
        CityName cityName,
        List<Participant> participants) : base(id, contestYear, cityName, participants)
    {
    }

    /// <inheritdoc />
    public override ContestFormat ContestFormat { get; private protected init; } = ContestFormat.Stockholm;

    /// <summary>
    ///     Starts the process of creating a new <see cref="StockholmFormatContest" /> instance using the fluent builder.
    /// </summary>
    /// <returns>A new <see cref="ContestBuilder" /> instance.</returns>
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
                        .FailIf(IllegalParticipantGroupSizes, ContestErrors.IllegalStockholmFormatParticipantGroups()))
                .Combine()
                .Then(Contest (tuple) => new StockholmFormatContest(idProvider(), tuple.Item1, tuple.Item2, tuple.Item3));
        }

        private static bool IllegalParticipantGroupSizes(IList<Participant> participants)
        {
            Dictionary<ParticipantGroup, int> groupSizes = participants.GroupBy(participant => participant.ParticipantGroup)
                .ToDictionary(grouping => grouping.Key, grouping => grouping.Count());

            return groupSizes.ContainsKey(ParticipantGroup.Zero)
                   || groupSizes.TryGetValue(ParticipantGroup.One, out int group1Size) is false || group1Size < 3
                   || groupSizes.TryGetValue(ParticipantGroup.Two, out int group2Size) is false || group2Size < 3;
        }
    }
}
