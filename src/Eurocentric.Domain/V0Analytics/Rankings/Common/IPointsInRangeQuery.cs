namespace Eurocentric.Domain.V0Analytics.Rankings.Common;

public interface IPointsInRangeQuery
{
    int MinPoints { get; }

    int MaxPoints { get; }
}
