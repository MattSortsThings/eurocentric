# A16. Get countries

This document is part of the [*Eurocentric* launch specification](../../../README.md).

- [A16. Get countries](#a16-get-countries)
  - [User story](#user-story)
  - [API contract](#api-contract)
    - [HTTP request](#http-request)
    - [HTTP response](#http-response)
  - [Acceptance criteria](#acceptance-criteria)
    - [Happy path](#happy-path)

## User story

- **As the Admin**
- **I want** to retrieve all existing countries in country code order
- **So that** I can test the behaviour of features that create, update, or delete one or more countries.

## API contract

### HTTP request

```http request
GET /admin/api/{apiVersion}/countries
```

**Notes:**

- `apiVersion` is a major-minor API version URL segment, e.g. `"v1.0"`.

### HTTP response

```http request
200 OK
```

```json
{
  "countries": [
    {
      "id": "00000000-0000-0000-0000-000000000000",
      "countryCode": "AA",
      "countryName": "Country Name",
      "countryType": "Real",
      "contestIds": [
        "00000000-0000-0000-0000-000000000000",
        "00000000-0000-0000-0000-000000000000"
      ]
    }
  ]
}
```

**Notes:**

- `countries` is ordered by `country.countryCode`.

## Acceptance criteria

### Happy path

**GetCountries endpoint...**

- [ ] Should_succeed_with_200_OK_and_all_existing_countries_in_country_code_order
- [ ] Should_succeed_with_empty_countries_list_when_system_contains_no_countries
