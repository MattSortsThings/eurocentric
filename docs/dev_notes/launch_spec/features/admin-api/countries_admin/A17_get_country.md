# A17 Get country

This document is part of the *Eurocentric* [launch specification](../../../README.md).

- [A17 Get country](#a17-get-country)
  - [User story](#user-story)
  - [API contract](#api-contract)
    - [HTTP request](#http-request)
    - [HTTP response](#http-response)
  - [Acceptance criteria](#acceptance-criteria)

## User story

- **As the Admin**
- **I want** to retrieve a specific country
- **So that** I can review its current status

## API contract

### HTTP request

```http request
GET /admin/api/{apiVersion}/countries/{countryId}
```

### HTTP response

```http request
200 Ok
```

```json
{
  "country": {
    "id": "00000000-0000-0000-0000-000000000000",
    "countryCode": "AT",
    "countryName": "Austria",
    "countryType": "Real",
    "contestRoles": [
      {
        "contestId": "00000000-0000-0000-0000-000000000000",
        "contestRoleType": "Participant"
      }
    ]
  }
}
```

**Notes:**

- `countryType` is an [enum value](../../../domain_model.md#countrytype-enum)
- `contestRoleType` is an [enum value](../../../domain_model.md#contestroletype-enum)

## Acceptance criteria

**GetCountry endpoint...**

- should succeed with 200 and return requested country
- should fail with 404 and ProblemDetails on CountryNotFound
