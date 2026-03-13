# A20. Get correlated log entries

This document is part of the [launch specification](../../../README.md).

## User story

- **As the Admin**
- **I want** to retrieve all the log entries for a specific HTTP request using its correlation ID
- **So that** I can trace the request's path through the system

## API contract

### HTTP request

```http request
GET /admin/api/{apiVersion}/observability/log-entries/correlated
```

**Required query parameters:**

| Name            |     Type      | Details                                 |
|:----------------|:-------------:|:----------------------------------------|
| `correlationId` | string (guid) | Specifies the log entry correlation ID. |

### HTTP response

```http request
200 OK
```

```json
{
  "metadata": {
    "correlationId": "00000000-0000-0000-0000-000000000000",
    "totalLogEntries": 5
  },
  "logEntries": [
    {
      "timestampUtc": "2026-01-01T00:00:00.000000",
      "correlationId": "00000000-0000-0000-0000-000000000000",
      "logLevel": "Information",
      "logEntryType": "HttpRequest",
      "httpRequestPath": "/public/api/v1.0/country-rankings/points-from-all-average",
      "httpRequestMethod": "GET",
      "httpRequestQueryString": "?pageSize=101",
      "httpRequestEndpointDisplayName": "PublicApi.V1.GetCountryPointsFromAllAverageRankings"
    },
    {
      "timestampUtc": "2026-01-01T00:00:00.000000",
      "correlationId": "00000000-0000-0000-0000-000000000000",
      "logLevel": "Information",
      "logEntryType": "HttpResponse",
      "httpResponseStatusCode": 422
    },
    {
      "timestampUtc": "2026-01-01T00:00:00.000000",
      "correlationId": "00000000-0000-0000-0000-000000000000",
      "logLevel": "Information",
      "logEntryType": "InternalRequest",
      "internalRequestPath": "Eurocentric.Apis.Public.V1.Features.CountryRankings.GetCountryPointsFromAllAverageRankings+Query"
    },
    {
      "timestampUtc": "2026-01-01T00:00:00.000000",
      "correlationId": "00000000-0000-0000-0000-000000000000",
      "logLevel": "Information",
      "logEntryType": "InternalResponse",
      "internalResponseSuccessful": false,
      "internalResponseDomainError": {
        "title": "Illegal Page Size",
        "type": "Intrinsic",
        "description": "Page size value must be an integer in the range [5, 100].",
        "additionalData": {
          "pageSize": 101
        }
      }
    },
    {
      "timestampUtc": "2026-01-01T00:00:00.000000",
      "correlationId": "00000000-0000-0000-0000-000000000000",
      "logLevel": "Error",
      "logEntryType": "Exception",
      "exceptionType": "System.DivideByZeroException",
      "exceptionMessage": "Attempted to divide by zero.",
      "exceptionStackTrace": "[STACK_TRACE]"
    }
  ]
}
```

**Notes:**

- each `logEntry` has 4 required non-`null` properties:
  - `timestampUtc`
  - `correlationId`
  - `logLevel`
  - `logEntryType`

## Acceptance criteria

### Happy Path : Baseline

**Endpoint...**

- [ ] Succeeds_and_retrieves_all_logEntries_with_correlationId

### Happy Path : Zero Log Entries

**Endpoint...**

- [ ] Succeeds_with_0_logEntries_when_correlationId_matches_no_logEntries

### Sad Path : Bad HTTP Request

**Endpoint...**

- [ ] Fails_when_correlationId_is_not_provided
