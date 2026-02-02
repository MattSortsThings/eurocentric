# A20. Get correlated log entries

This document is part of the [*Eurocentric* launch specification](../../../README.md).

- [A20. Get correlated log entries](#a20-get-correlated-log-entries)
  - [User story](#user-story)
  - [API contract](#api-contract)
    - [HTTP request](#http-request)
    - [HTTP response](#http-response)
  - [Acceptance criteria](#acceptance-criteria)
    - [Happy path](#happy-path)
    - [Sad path](#sad-path)

## User story

- **As the Admin**
- **I want** to get a list of all the log entries for a specific correlation ID returned as an `"X-Correlation-Id"` HTTP response header
- **So that** I can trace the request's path through the system.

## API contract

### HTTP request

```http request
GET /admin/api/{apiVersion}/observability/log-entries/correlated
```

**Required query parameters:**

| Name                |     Type      | Details                                 |
|:--------------------|:-------------:|:----------------------------------------|
| `correlationId`     | string (guid) | Specifies the log entry correlation ID. |

### HTTP response

```http request
200 OK
```

```json
{
  "metadata": {
    "correlationId": "00000000-0000-0000-0000-000000000000"
  },
  "logEntries": [
    {
      "timestampUtc": "2026-01-01T12:30:45.000Z",
      "correlationId": "00000000-0000-0000-0000-000000000000",
      "logEntryType": "HttpRequest",
      "httpRequestDetails": {
        "path": "/public/api/v1.0/results/competing-countries",
        "queryString": "?competingCountryCode=GB",
        "method": "GET",
        "endpointDisplayName": "PublicApi:V1:GetCompetingCountryResults"
      },
      "httpResponseDetails": null,
      "internalRequestDetails": null,
      "internalResponseDetails": null,
      "exceptionDetails": null
    }
  ]
}
```

**Notes:**

- `logEntries` is ordered by `logEntry.timestampUtc`.
- `logEntry.logEntryType` is an enum, values = `{ HttpRequest, HttpResponse, InternalRequest, InternalResponse, Exception }`.
- `logEntry.httpRequestDetails` is an optional object, not null when `logEntry.logEntryType` is `HttpRequest`.
- `logEntry.httpResponseDetails` is an optional object, not null when `logEntry.logEntryType` is `HttpResponse`.
- `logEntry.internalRequestDetails` is an optional object, not null when `logEntry.logEntryType` is `InternalRequest`.
- `logEntry.internalResponseDetails` is an optional object, not null when `logEntry.logEntryType` is `InternalResponse`.
- `logEntry.exceptionDetails` is an optional object, not null when `logEntry.logEntryType` is `Exception`.

**HttpRequestDetails object example:**

```json
{
  "path": "/public/api/v1.0/results/competing-countries",
  "queryString": "?competingCountryCode=GB",
  "method": "GET",
  "endpointDisplayName": "PublicApi:V1:GetCompetingCountryResults"
}
```

**HttpResponseDetails object example:**

```json
{
   "statusCode": 200
}
```

**InternalRequestDetails object example:**

```json
{
  "path" : "Eurocentric.Apis.Admin.V0.Countries.GetCountry+Query"
}
```

**InternalResponseDetails object example:**

```json
{
  "successful" : false,
  "domainErrorTitle": "Country not found",
  "domainErrorType": "NotFound",
  "domainErrorDescription": "The requested country does not exist.",
  "domainErrorAdditionalData": {
    "countryId": "00000000-0000-0000-0000-000000000000"
  }
}
```

## Acceptance criteria

### Happy path

**GetCorrelatedLogEntries endpoint...**

- [ ] Should_succeed_with_200_OK_and_metadata_and_log_entries_when_request_is_valid
- [ ] Should_return_empty_log_entries_list_when_no_log_entries_match_correlationId

### Sad path

**GetCorrelatedLogEntries endpoint...**

- [ ] Should_fail_when_correlationId_is_not_provided
- [ ] Should_fail_when_correlationId_is_not_parseable
