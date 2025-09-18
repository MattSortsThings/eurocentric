namespace Eurocentric.Domain.V0Analytics.Rankings.Common;

public interface IPaginatedQuery
{
    int PageIndex { get; }


    int PageSize { get; }


    bool Descending { get; }
}
