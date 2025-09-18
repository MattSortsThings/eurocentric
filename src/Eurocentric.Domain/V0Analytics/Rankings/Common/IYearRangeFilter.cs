namespace Eurocentric.Domain.V0Analytics.Rankings.Common;

public interface IYearRangeFilter
{
    int? MinYear { get; }

    int? MaxYear { get; }
}
