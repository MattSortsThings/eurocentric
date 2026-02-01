# A20. Get correlated log entries

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
      "httpRequestPath": "/public/api/v1.0/results/competing-countries",
      "httpRequestQueryString": "?competingCountryCode=GB",
      "httpRequestMethod": "GET",
      "httpRequestEndpointDisplayName": "PublicApi:V1:GetCompetingCountryResults",
      "httpResponseStatusCode": null,
      "internalRequestPath": null,
      "internalRequestType": null,
      "internalResultSuccessful": null,
      "internalResultDomainErrorTitle": null,
      "internalResultDomainErrorType": null,
      "internalResultDomainErrorDetail": null,
      "internalResultDomainErrorMetadata": null,
      "exceptionType": null,
      "exceptionMessage": null,
      "exceptionStackTrace": null
    }
  ]
}
```

**Notes:**

- `logEntry.logEntryType` is an enum, values = `{ HttpRequest, HttpResponse, AppRequest, AppResult, Exception }`.
- `logEntry.httpRequestPath` is an optional string
- `logEntry.httpRequestQueryString` is an optional string
- `logEntry.httpRequestMethod` is an optional string
- `logEntry.httpRequestEndpointDisplayName` is an optional string
- `logEntry.httpResponseStatusCode` is an optional int
- `logEntry.internalRequestPath` is an optional string
- `logEntry.internalRequestType` is an optional string
- `logEntry.internalResultSuccessful` is an optional boolean
- `logEntry.internalResultDomainErrorTitle` is an optional string
- `logEntry.internalResultDomainErrorType` is an optional enum, values = `{ Unexpected, NotFound, Extrinsic, Intrinsic }`.
- `logEntry.internalResultDomainErrorDetail` is an optional string
- `logEntry.internalResultDomainErrorMetadata` is an optional dictionary
- `logEntry.exceptionType` is an optional string
- `logEntry.exceptionMessage` is an optional string
- `logEntry.exceptionStackTrace` is an optional string

## Acceptance criteria

### Happy path

**GetCorrelatedLogEntries endpoint...**

- [ ] Should_succeed_with_200_OK_and_metadata_and_log_entries_when_request_is_valid
- [ ] Should_return_empty_metrics_list_when_no_log_entries_match_correlationId

### Sad path

**GetCorrelatedLogEntries endpoint...**

- [ ] Should_fail_when_correlationId_is_not_provided
- [ ] Should_fail_when_correlationId_is_not_parseable
