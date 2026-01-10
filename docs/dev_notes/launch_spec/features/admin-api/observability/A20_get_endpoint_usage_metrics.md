# A20 Get endpoint usage metrics

This document is part of the *Eurocentric* [launch specification](../../../README.md).

- [A20 Get endpoint usage metrics](#a20-get-endpoint-usage-metrics)
  - [User story](#user-story)
  - [API contract](#api-contract)
    - [HTTP request](#http-request)
    - [HTTP response](#http-response)
  - [Acceptance criteria](#acceptance-criteria)

## User story

- **As the Admin**
- **I want** to see how frequently every API endpoint has been accessed over the past 60 days
- **So that** I can understand how often (if ever) the Eurocentric APIs are being used

## API contract

### HTTP request

```http request
GET /admin/api/{apiVersion}/observability/endpoint-usage-metrics
```

**Query params:**

| Name                | Type | Required | Details                                                                                               |
|:--------------------|:----:|:--------:|:------------------------------------------------------------------------------------------------------|
| `startLogEntryDate` | date |    no    | Filters the log entries by inclusive start date. Must be `"yyyy-MM-dd"`. Defaults to today - 60 days. |
| `endLogEntryDate`   | date |    no    | Filters the log entries by inclusive end date. Must be `"yyyy-MM-dd"`. Defaults to today.             |

### HTTP response

```http request
200 Ok
```

```json
{
  "metadata": {
    "startLogEntryDate": "2025-12-01",
    "endLogEntryDate": "2025-01-09",
    "totalItems": 10
  },
  "endpointUsageMetrics" : [
    {
      "endpointDisplayName": "PublicApi:V1:GetContests",
      "totalRequests": 15
    }
  ]
}
```

**Notes:**

- `startLogEntryDate` uses the `"yyyy-MM-dd"` date format
- `endLogEntryDate` uses the `"yyyy-MM-dd"` date format

## Acceptance criteria

**GetEndpointUsageMetrics endpoint...**

- should succeed with 200 and return all non-zero non-observability endpoint usage metrics
- should succeed with 200 and filter log entries by start log entry date
- should succeed with 200 and filter log entries by end log entry date
- should succeed with 200 and filter log entries by log entry date range
- should succeed with 200 and return empty list when no log entries match query
- should succeed with 200 and return empty list when no data exists
- should fail with 400 and ProblemDetails on IllegalLogEntryDateRange
