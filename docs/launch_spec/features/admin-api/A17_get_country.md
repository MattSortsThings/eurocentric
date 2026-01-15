# A17 Get country

This document is part of the [*Eurocentric* launch specification](../../README.md).

**Feature scope:** *admin-api*

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
200 OK
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

## Acceptance criteria

### Happy path

**GetCountry endpoint...**

- Should_succeed_with_200_OK_and_requested_country_when_request_is_valid

### Sad path

**GetCountry endpoint...**

- Should_fail_when_country_does_not_exist
