using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;

namespace Eurocentric.Features.PublicApi.V0.Common.Dtos;

public abstract record PaginatedQuery
{
    [DefaultValue(0)]
    [FromQuery(Name = "pageIndex")]
    public int? PageIndex { get; init; }

    [DefaultValue(10)]
    [FromQuery(Name = "pageSize")]
    public int? PageSize { get; init; }

    [DefaultValue(false)]
    [FromQuery(Name = "descending")]
    public bool? Descending { get; init; }
}
