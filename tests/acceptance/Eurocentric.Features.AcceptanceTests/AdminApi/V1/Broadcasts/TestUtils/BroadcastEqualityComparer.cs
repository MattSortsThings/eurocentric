using Eurocentric.Features.AdminApi.V1.Common.Dtos;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Broadcasts.TestUtils;

internal sealed class BroadcastEqualityComparer : IEqualityComparer<Broadcast>
{
    public bool Equals(Broadcast? x, Broadcast? y)
    {
        if (ReferenceEquals(x, y))
        {
            return true;
        }

        if (x is null)
        {
            return false;
        }

        if (y is null)
        {
            return false;
        }

        if (x.GetType() != y.GetType())
        {
            return false;
        }

        return x.Id.Equals(y.Id)
               && x.ContestId.Equals(y.ContestId)
               && x.ContestStage == y.ContestStage
               && x.Status == y.Status &&
               x.Competitors.SequenceEqual(y.Competitors, new CompetitorEqualityComparer())
               && x.Televotes.SequenceEqual(y.Televotes)
               && x.Juries.SequenceEqual(y.Juries);
    }

    public int GetHashCode(Broadcast obj) => HashCode.Combine(obj.Id, obj.ContestId, (int)obj.ContestStage, (int)obj.Status,
        obj.Competitors, obj.Televotes, obj.Juries);
}
