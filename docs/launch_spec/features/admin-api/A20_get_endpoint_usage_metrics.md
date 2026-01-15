# A20 Get endpoint usage metrics

This document is part of the [*Eurocentric* launch specification](../../README.md).

**Feature scope:** *admin-api*

## User story

- **As the Admin**
- **I want** to query the non-zero endpoint usage metrics for up to the past 60 days, receiving the query metadata and a list of metrics ordered by endpoint display name
- **So that** I can see how frequently each endpoint is used, if at all

## API contract

### HTTP request

```http request
GET /admin/api/{apiVersion}/observability/endpoint-usage-metrics
```

**Query parameters:**

| Name        |     Type      | Required | Details                                                                                                                                                                                         |
|:------------|:-------------:|:--------:|:------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| `startDate` | string (date) |    no    | Filters the log entries by inclusive start HTTP request date. Must use `"yyyy-MM-dd"` format. Must not be null or later than `endDate` when `endDate` is not null. Defaults to today - 59 days. |
| `endDate`   | string (date) |    no    | Filters the log entries by inclusive end HTTP request date. Must use `"yyyy-MM-dd"` format. Must not be null or earlier than `startDate` when `startDate` is not null. Defaults to today.       |

### HTTP response

```http request
200 OK
```

```json
{
  "metadata": {
    "startDate": "2026-01-01",
    "endDate": "2026-01-07",
    "totalItems": 10
  },
  "metrics": [
    {
      "endpointDisplayName": "PublicApi:V1:GetQueryableBroadcasts",
      "httpRequests": 50,
      "successfulHttpResponses": 40,
      "unsuccessfulHttpResponses": 10
    }
  ]
}
```

## Acceptance criteria

### Happy path

**GetEndpointUsageMetrics endpoint...**

- Should_succeed_with_200_OK_and_metadata_and_ordered_metrics_when_request_is_valid
- Should_query_with_startDate_2026_01_01_and_endDate_2026_01_07
- Should_return_empty_metrics_list_when_no_log_entries_fit_query_params

### Sad path

**GetEndpointUsageMetrics endpoint...**

- Should_fail_when_startDate_is_malformed
- Should_fail_when_endDate_is_malformed
- Should_fail_when_startDate_is_provided_without_endDate
- Should_fail_when_endDate_is_provided_without_startDate
- Should_fail_when_startDate_is_later_than_endDate
