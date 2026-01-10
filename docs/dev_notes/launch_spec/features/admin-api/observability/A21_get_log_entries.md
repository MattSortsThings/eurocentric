# A21 Get log entries

This document is part of the *Eurocentric* [launch specification](../../../README.md).

- [A21 Get log entries](#a21-get-log-entries)
  - [User story](#user-story)
  - [API contract](#api-contract)
    - [HTTP request](#http-request)
    - [HTTP response](#http-response)
  - [Acceptance criteria](#acceptance-criteria)

## User story

- **As the Admin**
- **I want** to query all the log entries for the past 60 days in reverse timestamp order
- **So that** I can trace successful and unsuccessful requests handled by all API endpoints

## API contract

### HTTP request

```http request
GET /admin/api/{apiVersion}/observability/log-entries
```

**Query params:**

| Name                |     Type      | Required | Details                                                                                                                                                          |
|:--------------------|:-------------:|:--------:|:-----------------------------------------------------------------------------------------------------------------------------------------------------------------|
| `startLogEntryDate` | string (date) |    no    | Filters the log entries by inclusive start date. Must be `"yyyy-MM-dd"`. Defaults to today - 59 days.                                                            |
| `endLogEntryDate`   | string (date) |    no    | Filters the log entries by inclusive end date. Must be `"yyyy-MM-dd"`. Defaults to today.                                                                        |
| `correlationId`     |    string     |    no    | Filters the log entries by correlation ID (cast-insensitive). Must be a string in the format "cid-00000000-0000-0000-0000-000000000000". Resolves to lower-case. |
| `exceptionsOnly`    |     bool      |    no    | Filters the log entries to exceptions only when true. Defaults to false.                                                                                         |
| `pageSize`          |      int      |    no    | Overrides the pagination page size. Must be an integer in the range \[1, 100\]. Defaults to 100.                                                                 |
| `pageIndex`         |      int      |    no    | Overrides the pagination page index. Must be a non-negative integer. Defaults to 0.                                                                              |

**Notes:**

### HTTP response

```http request
200 Ok
```

```json
{
  "metadata": {
    "startLogEntryDate": "2025-12-01",
    "endLogEntryDate": "2026-01-09",
    "correlationId": "cid-68f5e5e1-8c13-40fc-b5bc-9295bdef9a4a",
    "exceptionsOnly": false,
    "pageSize": 10,
    "pageIndex": 0,
    "totalItems": 1000
  },
  "logEntries": [
    {
      "correlationId": "cid-68f5e5e1-8c13-40fc-b5bc-9295bdef9a4a",
      "timestampUtc": "2026-01-01T06:13:57.000Z",
      "logEntryType": "HttpRequest",
      "httpRequestPath": "/public/api/v1.0/competitor-rankings/points-average",
      "httpRequestQueryString": "?votingMethod=Televote",
      "httpRequestMethod": "GET",
      "httpRequestEndpointDisplayName": "PublicApi:V1:GetCompetitorPointsAverageRankings",
      "httpResponseStatusCode": null,
      "appRequestPath": null,
      "appRequestType": null,
      "appResultSuccessful": null,
      "appResultErrorTitle": null,
      "appResultErrorDetail": null,
      "appResultErrorMetadata": null,
      "exceptionType": null,
      "exceptionMessage": null
    }
  ]
}
```

**Notes:**

- `startLogEntryDate` uses the `"yyyy-MM-dd"` date format
- `endLogEntryDate` uses the `"yyyy-MM-dd"` date format
- `logEntryType` is an enum value, one of `{ HttpRequest, HttpResult, AppRequest, AppResult, Exception }`
- `httpRequestPath` is an optional string
- `httpRequestQueryString` is an optional string
- `httpRequestMethod` is an optional string
- `httpRequestEndpointDisplayName` is an optional string
- `httpResponseStatusCode` is an optional integer
- `appRequestPath` is an optional string
- `appRequestType` is an optional string
- `appResultSuccessful` is an optional boolean
- `appResultErrorTitle` is an optional string
- `appResultErrorDetail` is an optional string
- `appResultErrorMetadata` is an optional dictionary
- `exceptionType` is an optional string
- `exceptionMessage` is an optional string

## Acceptance criteria

**GetLogEntries endpoint...**

- should succeed with 200 and return page of log entries in reverse timestamp order
- should succeed with 200 and return page of log entries using start date filter
- should succeed with 200 and return page of log entries using end date filter
- should succeed with 200 and return page of log entries using start and end date filters
- should succeed with 200 and return page of log entries using correlation ID filter
- should succeed with 200 and return page of exception log entries when exceptions only filter applied
- should succeed with 200 and return page of log entries using page size override
- should succeed with 200 and return page of log entries using page index override
- should succeed with 200 and return empty page when correlation ID matches no entries
- should succeed with 200 and return empty page when no log entries exist
- should fail with 400 and ProblemDetails on IllegalLogEntryDateRange
- should fail with 400 and ProblemDetails on IllegalCorrelationIdValue
- should fail with 400 and ProblemDetails on IllegalPageSizeValue. Test cases include:
  - 0
  - 101
- should fail with 400 and ProblemDetails on IllegalPageIndexValue
