# A17. Get country

This document is part of the [*Eurocentric* launch specification](../../../README.md).

- [A17. Get country](#a17-get-country)
  - [User story](#user-story)
  - [API contract](#api-contract)
    - [HTTP request](#http-request)
    - [HTTP response](#http-response)
  - [Acceptance criteria](#acceptance-criteria)
    - [Happy path](#happy-path)
    - [Sad path](#sad-path)

## User story

- **As the Admin**
- **I want** to retrieve a single country
- **So that** I can review its current status.

## API contract

### HTTP request

```http request
GET /admin/api/{apiVersion}/countries/{countryId}
```

**Notes:**

- `apiVersion` is a major-minor API version URL segment, e.g. `"v1.0"`.
- `countryId` is the Guid ID of the requested country aggregate.

### HTTP response

```http request
200 OK
```

```json
{
  "country": {
    "id": "00000000-0000-0000-0000-000000000000",
    "countryCode": "AA",
    "countryName": "Country Name",
    "countryType": "Real",
    "contestIds": [
      "00000000-0000-0000-0000-000000000000",
      "00000000-0000-0000-0000-000000000000"
    ]
  }
}
```

## Acceptance criteria

### Happy path

**GetCountry endpoint...**

- [ ] Should_succeed_with_200_OK_and_requested_country_when_request_is_valid

### Sad path

**GetCountry endpoint...**

- [ ] Should_fail_when_country_does_not_exist
