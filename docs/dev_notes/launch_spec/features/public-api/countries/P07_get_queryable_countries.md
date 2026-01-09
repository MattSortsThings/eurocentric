# P07 Get queryable countries

This document is part of the *Eurocentric* [launch specification](../../../README.md).

- [P07 Get queryable countries](#p07-get-queryable-countries)
  - [User story](#user-story)
  - [API contract](#api-contract)
    - [HTTP request](#http-request)
    - [HTTP response](#http-response)
  - [Acceptance criteria](#acceptance-criteria)

## User story

- **As a EuroFan**
- **I want** to list all the queryable countries in country code order
- **So that** I can understand the scope of the Public API's queryable voting data

## API contract

### HTTP request

```http request
GET /public/api/{apiVersion}/countries
```

### HTTP response

```http request
200 Ok
```

```json
{
  "metadata": {
    "totalItems": 40
  },
  "countries": [
    {
      "countryCode": "AT",
      "countryName": "Austria"
    }
  ]
}
```

## Acceptance criteria

**GetQueryableCountries endpoint...**

- should succeed with 200 and return all queryable countries in country code order
- should succeed with 200 and return empty list when no countries are queryable
- should succeed with 200 and return empty list when no data exists
