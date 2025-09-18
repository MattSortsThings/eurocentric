using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;

namespace Eurocentric.Apis.Public.V0.Dtos.Rankings.Common;

public abstract record PaginatedRequest
{
    [FromQuery(Name = "pageIndex")]
    [DefaultValue(0)]
    public int? PageIndex { get; init; }

    [FromQuery(Name = "pageSize")]
    [DefaultValue(10)]
    public int? PageSize { get; init; }

    [FromQuery(Name = "descending")]
    [DefaultValue(false)]
    public bool? Descending { get; init; }
}
