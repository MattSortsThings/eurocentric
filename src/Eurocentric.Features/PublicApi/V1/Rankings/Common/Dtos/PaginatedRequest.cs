using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Eurocentric.Features.PublicApi.V1.Common.Constants;
using Microsoft.AspNetCore.Mvc;

namespace Eurocentric.Features.PublicApi.V1.Rankings.Common.Dtos;

public abstract record PaginatedRequest
{
    [FromQuery(Name = "pageIndex")]
    [Description("Sets the zero-based pagination page index.")]
    [Range(0, int.MaxValue, ErrorMessage = "Page index must be greater than or equal to 0.")]
    [DefaultValue(QueryParamDefaults.PageIndex)]
    public int? PageIndex { get; init; }

    [FromQuery(Name = "pageSize")]
    [Description("Sets the zero-based pagination page size.")]
    [Range(1, 100, ErrorMessage = "Page size must be between 1 and 100.")]
    [DefaultValue(QueryParamDefaults.PageSize)]
    public int? PageSize { get; init; }

    [FromQuery(Name = "descending")]
    [Description("Sorts rankings by descending rank before pagination.")]
    [DefaultValue(QueryParamDefaults.Descending)]
    public bool? Descending { get; init; }
}
