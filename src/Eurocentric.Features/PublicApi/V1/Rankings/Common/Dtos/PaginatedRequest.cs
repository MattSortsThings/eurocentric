using System.ComponentModel;
using Eurocentric.Features.PublicApi.V1.Common.Constants;
using Microsoft.AspNetCore.Mvc;

namespace Eurocentric.Features.PublicApi.V1.Rankings.Common.Dtos;

public abstract record PaginatedRequest
{
    [FromQuery(Name = "pageIndex")]
    [Description("Sets the zero-based pagination page index.")]
    [DefaultValue(QueryParamDefaults.PageIndex)]
    public int? PageIndex { get; init; }

    [FromQuery(Name = "pageSize")]
    [Description("Sets the zero-based pagination page size.")]
    [DefaultValue(QueryParamDefaults.PageSize)]
    public int? PageSize { get; init; }

    [FromQuery(Name = "descending")]
    [Description("Sorts rankings by descending rank before pagination.")]
    [DefaultValue(QueryParamDefaults.Descending)]
    public bool? Descending { get; init; }
}
