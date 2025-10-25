using Eurocentric.Apis.Admin.V1.Dtos.Contests;

namespace Eurocentric.AcceptanceTests.Functional.AdminApi.V1.Contests.TestUtils;

public sealed class ChildBroadcastEqualityComparer : IEqualityComparer<ChildBroadcast>
{
    public bool Equals(ChildBroadcast? x, ChildBroadcast? y)
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

        return x.ChildBroadcastId.Equals(y.ChildBroadcastId)
            && x.ContestStage == y.ContestStage
            && x.Completed == y.Completed;
    }

    public int GetHashCode(ChildBroadcast obj) =>
        HashCode.Combine(obj.ChildBroadcastId, (int)obj.ContestStage, obj.Completed);
}
