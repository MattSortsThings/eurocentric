using ErrorOr;
using Eurocentric.Domain.Broadcasts;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.ErrorHandling;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.Contests;

public abstract class BroadcastBuilder
{
    private protected const int MinLegalCompetitors = 2;

    protected BroadcastBuilder(ContestStage contestStage)
    {
        ContestStage = contestStage;
    }

    private protected ContestStage ContestStage { get; }

    private protected abstract ContestId ParentContestId { get; }

    private ErrorOr<BroadcastDate> ErrorsOrBroadcastDate { get; set; } = Error.Unexpected("Broadcast date not set.");

    private ErrorOr<List<Competitor>> ErrorsOrCompetitors { get; set; } = Error.Unexpected("Competitors not set.");

    public BroadcastBuilder WithBroadcastDate(ErrorOr<BroadcastDate> errorsOrBroadcastDate)
    {
        ErrorsOrBroadcastDate = ConfirmInRange(errorsOrBroadcastDate);

        return this;
    }

    public BroadcastBuilder WithCompetingCountryIds(params IEnumerable<CountryId> competingCountryIds)
    {
        ArgumentNullException.ThrowIfNull(competingCountryIds);

        ErrorsOrCompetitors = CreateCompetitors(competingCountryIds);

        return this;
    }

    public ErrorOr<Broadcast> Build(IBroadcastIdGenerator idGenerator)
    {
        ArgumentNullException.ThrowIfNull(idGenerator);

        return Tuple.Create(ErrorsOrBroadcastDate, ErrorsOrCompetitors)
            .Combine()
            .FailIf(_ => ChildBroadcastContestStageConflict(), ContestErrors.ChildBroadcastContestStageConflict(ContestStage))
            .Then(tuple => new Broadcast(idGenerator.Generate(),
                tuple.Item1,
                ParentContestId,
                ContestStage,
                tuple.Item2,
                CreateJuries(),
                CreateTelevotes()));
    }

    private protected abstract bool ChildBroadcastContestStageConflict();

    private protected abstract ErrorOr<List<Competitor>> CreateCompetitors(IEnumerable<CountryId> competingCountryIds);

    private protected abstract List<Jury> CreateJuries();

    private protected abstract List<Televote> CreateTelevotes();

    private protected abstract ErrorOr<BroadcastDate> ConfirmInRange(ErrorOr<BroadcastDate> errorsOrBroadcastDate);
}
