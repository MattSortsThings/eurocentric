# P06 Get queryable contests

This document is part of the *Eurocentric* [launch specification](../../../README.md).

- [P06 Get queryable contests](#p06-get-queryable-contests)
  - [User story](#user-story)
  - [API contract](#api-contract)
    - [HTTP request](#http-request)
    - [HTTP response](#http-response)
  - [Acceptance criteria](#acceptance-criteria)

## User story

- **As a EuroFan**
- **I want** to list all the queryable contests in contest year order
- **So that** I can understand the scope of the Public API's queryable voting data

## API contract

### HTTP request

```http request
GET /public/api/{apiVersion}/contests
```

### HTTP response

```http request
200 Ok
```

```json
{
  "metadata": {
    "totalItems": 9
  },
  "contests": [
    {
      "contestYear": 2025,
      "cityName": "Basel",
      "semiFinalVotingRules": "TelevoteOnly",
      "grandFinalVotingRules": "TelevoteAndJury",
      "usesGlobalTelevote": true
    }
  ]
}
```

**Notes:**

- `semiFinalVotingRules` is an [enum value](../../../domain_model.md#votingrules-enum)
- `grandFinalVotingRules` is an [enum value](../../../domain_model.md#votingrules-enum)

## Acceptance criteria

**GetQueryableContests endpoint...**

- should succeed with 200 and return all queryable contests in contest year
- should succeed with 200 and return empty list when no contests are queryable
- should succeed with 200 and return empty list when no data exists
