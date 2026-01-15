# A21 Get log entries

This document is part of the [*Eurocentric* launch specification](../../README.md).

**Feature scope:** *admin-api*

## User story

- **As the Admin**
- **I want** to list the log entries for up to the past 60 days, receiving the query metadata and a page of log entries
- **So that** I can monitor the requests handled by the system

## API contract

### HTTP request

```http request
GET /admin/api/{apiVersion}/observability/log-entries
```

**Query parameters:**

| Name                |     Type      | Required | Details                                                                                                                                                                                         |
|:--------------------|:-------------:|:--------:|:------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| `startDate`         | string (date) |    no    | Filters the log entries by inclusive start HTTP request date. Must use `"yyyy-MM-dd"` format. Must not be null or later than `endDate` when `endDate` is not null. Defaults to today - 59 days. |
| `endDate`           | string (date) |    no    | Filters the log entries by inclusive end HTTP request date. Must use `"yyyy-MM-dd"` format. Must not be null or earlier than `startDate` when `startDate` is not null. Defaults to today.       |
| `correlationId`     | string (guid) |    no    | Filters the log entries by correlation ID when not null.                                                                                                                                        |
| `exceptionsOnly`    |     bool      |    no    | Filters the log entries to exceptions only when true. Defaults to false.                                                                                                                        |
| `logEntrySortField` | string (enum) |    no    | Sets the field by which log entries are sorted before pagination. Enum values are `{ TimestampUtc, CorrelationId }`. Defaults to `TimestampUtc`.                                                |
| `pageSize`          |      int      |    no    | Sets the pagination page size. Must be in range \[1, 100\]. Defaults to 10.                                                                                                                     |
| `pageIndex`         |      int      |    no    | Sets the pagination page index. Must not be negative. Defaults to 0.                                                                                                                            |

### HTTP response

```http request
200 OK
```

```json
{
  "metadata": {
    "startDate": "2026-01-01",
    "endDate": "2026-01-07",
    "correlationId": "00000000-0000-0000-0000-000000000000",
    "exceptionsOnly": false,
    "logEntrySortField": "TimestampUtc",
    "pageSize": 100,
    "pageIndex": 0,
    "totalItems": 1000
  },
  "logEntries": [
    {
      "timestampUtc": "2026-01-01T12:30:45.000Z",
      "correlationId": "00000000-0000-0000-0000-000000000000",
      "logEntryType": "HttpRequest",
      "httpRequestPath": "/public/api/v1.0/results/competing-countries",
      "httpRequestQueryString": "?competingCountryCode=GB",
      "httpRequestMethod": "GET",
      "httpRequestEndpointDisplayName": "PublicApi:V1:GetCompetingCountryResults",
      "httpResponseStatusCode": null,
      "internalRequestPath": null,
      "internalRequestType": null,
      "internalResultSuccessful": null,
      "internalResultDomainError": null,
      "exceptionType": null,
      "exceptionMessage": null,
      "exceptionStackTrace": null
    }
  ]
}
```

**Notes:**

- `logEntryType` is an enum, values = `{ HttpRequest, HttpResponse, AppRequest, AppResult, Exception }`.
- `httpRequestPath` is an optional string
- `httpRequestQueryString` is an optional string
- `httpRequestMethod` is an optional string
- `httpRequestEndpointDisplayName` is an optional string
- `httpResponseStatusCode` is an optional int
- `internalRequestPath` is an optional string
- `internalRequestType` is an optional string
- `internalResultSuccessful` is an optional boolean
- `internalResultDomainError` is an optional object
- `exceptionType` is an optional string
- `exceptionMessage` is an optional string
- `exceptionStackTrace` is an optional string

## Acceptance criteria

### Happy path

**GetLogEntries endpoint...**

- Should_succeed_with_200_OK_and_metadata_and_ordered_metrics_when_request_is_valid
- Should_query_with_startDate_2026_01_01_and_endDate_2026_01_07
- Should_query_with_correlationId
- Should_query_with_exceptionsOnly_false
- Should_query_with_exceptionsOnly_true
- Should_apply_logEntrySortField_TimestampUtc
- Should_apply_logEntrySortField_CorrelationId
- Should_apply_custom_page_size
- Should_apply_custom_page_index
- Should_return_empty_metrics_list_when_no_log_entries_fit_query_params

### Sad path

**GetLogEntries endpoint...**

- Should_fail_when_startDate_is_malformed
- Should_fail_when_endDate_is_malformed
- Should_fail_when_startDate_is_provided_without_endDate
- Should_fail_when_endDate_is_provided_without_startDate
- Should_fail_when_startDate_is_later_than_endDate
- Should_fail_when_pageSize_is_less_than_1
- Should_fail_when_pageSize_is_greater_than_1000
- Should_fail_when_pageIndex_is_less_than_0
