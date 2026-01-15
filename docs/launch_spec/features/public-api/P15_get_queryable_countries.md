# P15 Get queryable countries

This document is part of the [*Eurocentric* launch specification](../../README.md).

**Feature scope:** *public-api*

## User story

- **As a EuroFan**
- **I want** to retrieve all queryable countries, receiving the query metadata and a list of countries ordered by country code
- **So that** I can get an overview of the Public API's queryable data

## API contract

### HTTP request

```http request
GET /public/api/{apiVersion}/countries
```

### HTTP response

```http request
200 OK
```

```json
{
  "metadata": {
    "totalItems": 40
  },
  "broadcasts": [
    {
      "countryCode": "AL",
      "countryName": "Albania"
    }
  ]
}
```

## Acceptance criteria

### Happy path

**GetQueryableCountries endpoint...**

- Should_succeed_with_200_OK_and_metadata_and_ordered_countries_when_request_is_valid
- Should_return_empty_countries_list_when_no_contests_are_queryable
- Should_return_empty_countries_list_when_no_data_exists
