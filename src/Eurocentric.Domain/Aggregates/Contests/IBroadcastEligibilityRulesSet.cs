namespace Eurocentric.Domain.Aggregates.Contests;

internal interface IBroadcastEligibilityRulesSet
{
    public bool MayCompete(Participant participant);

    public bool HasJury(Participant participant);

    public bool HasTelevote(Participant participant);
}
