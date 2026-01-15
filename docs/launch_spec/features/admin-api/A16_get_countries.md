# A16 Get countries

This document is part of the [*Eurocentric* launch specification](../../README.md).

**Feature scope:** *admin-api*

## User story

- **As the Admin**
- **I want** to retrieve all existing countries ordered by country code
- **So that** I can test the behaviour of features that create, update or delete one or more countries

## API contract

### HTTP request

```http request
GET /admin/api/{apiVersion}/countries
```

### HTTP response

```http request
200 OK
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

## Acceptance criteria

### Happy path

**GetCountries endpoint...**

- Should_succeed_with_200_OK_and_all_existing_countries_in_order
- Should_return_empty_countries_list_when_no_countries_exist
