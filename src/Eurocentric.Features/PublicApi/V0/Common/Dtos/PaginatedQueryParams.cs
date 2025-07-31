using System.ComponentModel;
using Eurocentric.Features.PublicApi.V0.Common.Constants;
using Microsoft.AspNetCore.Mvc;

namespace Eurocentric.Features.PublicApi.V0.Common.Dtos;

public abstract record PaginatedQueryParams
{
    [FromQuery(Name = "pageIndex")]
    [DefaultValue(QueryParamDefaults.PageIndex)]
    public int? PageIndex { get; init; }

    [FromQuery(Name = "pageSize")]
    [DefaultValue(QueryParamDefaults.PageSize)]
    public int? PageSize { get; init; }

    [FromQuery(Name = "descending")]
    [DefaultValue(QueryParamDefaults.Descending)]
    public bool? Descending { get; init; }
}
