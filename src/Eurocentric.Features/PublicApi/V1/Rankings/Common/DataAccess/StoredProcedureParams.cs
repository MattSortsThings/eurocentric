using System.Data;
using Dapper;
using ErrorOr;
using Eurocentric.Features.PublicApi.V1.Rankings.Common.Dtos;
using Eurocentric.Features.PublicApi.V1.Rankings.Common.Errors;
using Eurocentric.Features.PublicApi.V1.Rankings.Common.Queries;

namespace Eurocentric.Features.PublicApi.V1.Rankings.Common.DataAccess;

internal sealed class StoredProcedureParams : DynamicParameters
{
    private StoredProcedureParams()
    {
    }

    public PaginationMetadata GetPaginationMetadata()
    {
        int pageIndex = Get<int>("@page_index");
        int pageSize = Get<int>("@page_size");
        bool descending = Get<bool>("@descending");
        int totalItems = Get<int>("@total_items");

        return new PaginationMetadata
        {
            PageIndex = pageIndex,
            PageSize = pageSize,
            Descending = descending,
            TotalItems = totalItems,
            TotalPages = totalItems == 0 ? 0 : (int)Math.Ceiling(totalItems / (double)pageSize)
        };
    }

    internal static ErrorOr<StoredProcedureParams> CreateWithPaginationParamsFrom(IPaginatedQuery query)
    {
        (int pageIndex, int pageSize, bool descending) = (query.PageIndex, query.PageSize, query.Descending);

        if (pageIndex < 0)
        {
            return QueryParamErrors.PageIndexOutOfRange(pageIndex);
        }

        if (pageSize is < 1 or > 100)
        {
            return QueryParamErrors.PageSizeOutOfRange(pageSize);
        }

        StoredProcedureParams instance = new();

        instance.Add("@page_index", pageIndex, DbType.Int32, ParameterDirection.Input);
        instance.Add("@page_size", pageSize, DbType.Int32, ParameterDirection.Input);
        instance.Add("@descending", descending, DbType.Boolean, ParameterDirection.Input);
        instance.Add("@total_items", null, DbType.Int32, ParameterDirection.Output);

        return instance;
    }
}
