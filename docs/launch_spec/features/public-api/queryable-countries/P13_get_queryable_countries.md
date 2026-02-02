# P13. Get queryable countries

This document is part of the [*Eurocentric* launch specification](../../../README.md).

- [P13. Get queryable countries](#p13-get-queryable-countries)
  - [User story](#user-story)
  - [API contract](#api-contract)
    - [HTTP request](#http-request)
    - [HTTP response](#http-response)
  - [Acceptance criteria](#acceptance-criteria)
    - [Happy path](#happy-path)

## User story

- **As a EuroFan**
- **I want** to retrieve all queryable countries in country code order
- **So that** I can understand the scope of the queryable voting data.

## API contract

### HTTP request

```http request
GET /public/api/{apiVersion}/queryable-countries
```

**Notes:**

- `apiVersion` is a major-minor API version URL segment, e.g. `"v1.0"`.

### HTTP response

```http request
200 OK
```

```json
{
  "queryableCountries": [
    {
      "countryCode": "AA",
      "countryName": "Country Name",
      "activeContestYears": [
        2016,
        2017
      ]
    }
  ]
}
```

**Notes:**

- `queryableCountries` is ordered by `queryableCountry.countryCode`.
- `queryableCountry.activeContestYears` is ordered numerically.

## Acceptance criteria

### Happy path

**GetQueryableCountries endpoint...**

- [ ] Should_succeed_with_200_OK_and_all_queryable_countries_in_country_code_order
- [ ] Should_succeed_with_empty_queryable_countries_list_when_no_queryable_voting_data_exists
- [ ] Should_succeed_with_empty_queryable_countries_list_when_system_contains_no_data
