namespace Eurocentric.Domain.Analytics.Rankings.Common;

public interface IOptionalPaginationSettings
{
    int? PageIndex { get; }

    int? PageSize { get; }

    bool? Descending { get; }
}
