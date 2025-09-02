using Microsoft.AspNetCore.Mvc;

namespace Eurocentric.Features.PublicApi.V0.Common.Dtos;

public abstract record PaginatedRequest
{
    [FromQuery(Name = "pageIndex")]
    public int? PageIndex { get; init; }

    [FromQuery(Name = "pageSize")]
    public int? PageSize { get; init; }

    [FromQuery(Name = "descending")]
    public bool? Descending { get; init; }
}
