using ErrorOr;
using Eurocentric.Domain.Contests;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.Identifiers;

namespace Eurocentric.Domain.Broadcasts;

internal abstract class BroadcastBuilder
{
    private const int MinLegalCompetitorsCount = 2;
    private readonly ContestId _contestId;
    private ErrorOr<List<Competitor>> _errorsOrCompetitors = Error.Unexpected("Competitors not set");

    private IEnumerable<Jury> _juries = [];
    private IEnumerable<Televote> _televotes = [];

    protected BroadcastBuilder(ContestId contestId)
    {
        _contestId = contestId;
    }

    private protected abstract ContestStage ContestStage { get; }

    public BroadcastBuilder SetCompetitors(IEnumerable<CountryId> competingCountryIds, IEnumerable<Participant> participants)
    {
        _errorsOrCompetitors = competingCountryIds.GroupJoin(participants.Where(MayCompete),
                countryId => countryId,
                participant => participant.ParticipatingCountryId,
                (countryId, matchedParticipants) => new Item(countryId, matchedParticipants.DefaultIfEmpty(null).First()))
            .Aggregate(new Aggregate(ContestStage), AccumulatorFunction)
            .ToErrorsOrCompetitors();

        return this;
    }

    public BroadcastBuilder SetJuries(IEnumerable<Participant> participants)
    {
        _juries = participants.Where(HasJury).Select(participant => participant.CreateJury());

        return this;
    }

    public BroadcastBuilder SetTelevotes(IEnumerable<Participant> participants)
    {
        _televotes = participants.Where(HasTelevote).Select(participant => participant.CreateTelevote());

        return this;
    }

    public ErrorOr<Broadcast> Build(Func<BroadcastId> idProvider)
    {
        ErrorOr<Broadcast> result = _errorsOrCompetitors.Then(competitors => new Broadcast(idProvider(),
            _contestId,
            ContestStage,
            competitors,
            _juries.ToList(),
            _televotes.ToList()));

        return result;
    }

    private protected abstract bool MayCompete(Participant participant);

    private protected abstract bool HasJury(Participant participant);

    private protected abstract bool HasTelevote(Participant participant);

    private static Aggregate AccumulatorFunction(Aggregate aggregate, Item item)
    {
        if (item.MatchedParticipant is null)
        {
            aggregate.IllegalCompetingCountryIds.Add(item.CountryId);
        }
        else
        {
            aggregate.MatchedParticipants.Add(item.MatchedParticipant);
        }

        return aggregate;
    }

    private sealed record Item(CountryId CountryId, Participant? MatchedParticipant);

    private sealed class Aggregate(ContestStage contestStage)
    {
        public List<Participant> MatchedParticipants { get; } = [];

        public List<CountryId> IllegalCompetingCountryIds { get; } = [];

        public ErrorOr<List<Competitor>> ToErrorsOrCompetitors()
        {
            if (IllegalCompetingCountryIds.Count > 0)
            {
                return BroadcastErrors.IllegalCompetingCountryIds(IllegalCompetingCountryIds, contestStage);
            }

            return MatchedParticipants
                .Select((participant, index) => participant.CreateCompetitor(index + 1))
                .ToList()
                .ToErrorOr()
                .FailIf(IllegalBroadcastSize, BroadcastErrors.IllegalBroadcastSize())
                .FailIf(DuplicateCompetingCountryIds, BroadcastErrors.DuplicateCompetingCountryIds());
        }

        private static bool IllegalBroadcastSize(IList<Competitor> competitors) => competitors.Count < MinLegalCompetitorsCount;

        private static bool DuplicateCompetingCountryIds(IList<Competitor> competitors) =>
            competitors.GroupBy(competitor => competitor.CompetingCountryId)
                .Any(grouping => grouping.Count() > 1);
    }
}
