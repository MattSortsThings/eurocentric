# A16 Get countries

This document is part of the *Eurocentric* [launch specification](../../../README.md).

- [A16 Get countries](#a16-get-countries)
  - [User story](#user-story)
  - [API contract](#api-contract)
    - [HTTP request](#http-request)
    - [HTTP response](#http-response)
  - [Acceptance criteria](#acceptance-criteria)

## User story

- **As the Admin**
- **I want** to retrieve all existing countries in country code order
- **So that** I can test the behaviour of features that create, update or delete one or more countries

## API contract

### HTTP request

```http request
GET /admin/api/{apiVersion}/countries
```

### HTTP response

```http request
200 Ok
```

```json
{
  "countries": [
    {
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
  ]
}
```

**Notes:**

- `countryType` is an [enum value](../../../domain_model.md#countrytype-enum)
- `contestRoleType` is an [enum value](../../../domain_model.md#contestroletype-enum)

## Acceptance criteria

**GetCountries endpoint...**

- should succeed with 200 and return all existing countries in country code order
- should succeed with 200 and return empty list when no countries exist
