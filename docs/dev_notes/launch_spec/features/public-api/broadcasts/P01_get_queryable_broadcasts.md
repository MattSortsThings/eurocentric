# P01 Get queryable broadcasts

This document is part of the *Eurocentric* [launch specification](../../../README.md).

- [P01 Get queryable broadcasts](#p01-get-queryable-broadcasts)
  - [User story](#user-story)
  - [API contract](#api-contract)
    - [HTTP request](#http-request)
    - [HTTP response](#http-response)
  - [Acceptance criteria](#acceptance-criteria)

## User story

- **As a EuroFan**
- **I want** to list all the queryable broadcasts in broadcast date order
- **So that** I can understand the scope of the Public API's queryable voting data

## API contract

### HTTP request

```http request
GET /public/api/{apiVersion}/broadcasts
```

### HTTP response

```http request
200 Ok
```

```json
{
  "metadata": {
    "totalItems": 27
  },
  "broadcasts": [
    {
      "broadcastDate": "2025-05-17",
      "contestYear": 2025,
      "cityName": "Basel",
      "contestStage": "GrandFinal",
      "votingRules": "TelevoteAndJury",
      "competitors": 26
    }
  ]
}
```

**Notes:**

- `broadcastDate` uses the `"yyyy-MM-dd"` date format
- `contestStage` is an [enum value](../../../domain_model.md#conteststage-enum)
- `votingRules` is an [enum value](../../../domain_model.md#votingrules-enum)

## Acceptance criteria

**GetQueryableBroadcasts endpoint...**

- should succeed with 200 and return all queryable broadcasts in broadcast date order
- should succeed with 200 and return empty list when no broadcasts are queryable
- should succeed with 200 and return empty list when no data exists
