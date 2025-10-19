namespace Eurocentric.Domain.V0.Queries.Rankings.Common;

public interface IOptionalPaginationSettings
{
    int? PageIndex { get; }

    int? PageSize { get; }

    bool? Descending { get; }
}
