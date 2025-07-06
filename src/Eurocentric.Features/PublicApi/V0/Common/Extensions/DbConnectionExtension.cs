using System.Data;
using Dapper;
using Eurocentric.Features.PublicApi.V0.Common.Contracts;

namespace Eurocentric.Features.PublicApi.V0.Common.Extensions;

internal static class DbConnectionExtension
{
    internal static async Task<(PaginationInfo Pagination, TItem[] Items)> QueryPagedAsync<TItem>(
        this IDbConnection dbConnection,
        string storedProcedureName,
        DynamicParameters parameters) where TItem : class
    {
        await using SqlMapper.GridReader multi = await dbConnection.QueryMultipleAsync(
            storedProcedureName,
            commandType: CommandType.StoredProcedure,
            param: parameters);

        PaginationInfo pagination = await multi.ReadSingleAsync<PaginationInfo>();
        TItem[] items = (await multi.ReadAsync<TItem>()).ToArray();

        return (pagination, items);
    }
}
