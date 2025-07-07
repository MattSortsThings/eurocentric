using System.ComponentModel;
using Eurocentric.Features.PublicApi.V0.Common.Constants;
using Microsoft.AspNetCore.Mvc;

namespace Eurocentric.Features.PublicApi.V0.Common.Contracts;

public abstract record PaginatedQueryParametersBase
{
    [FromQuery(Name = "pageIndex")]
    [DefaultValue(QueryParameterDefaults.PageIndex)]
    [Description("Zero-indexed pagination page index")]
    public int? PageIndex { get; init; }

    [FromQuery(Name = "pageSize")]
    [DefaultValue(QueryParameterDefaults.PageSize)]
    [Description("Pagination page size")]
    public int? PageSize { get; init; }

    [FromQuery(Name = "descending")]
    [DefaultValue(QueryParameterDefaults.Descending)]
    [Description("Sorts items in descending rank order before pagination.")]
    public bool? Descending { get; init; }
}
