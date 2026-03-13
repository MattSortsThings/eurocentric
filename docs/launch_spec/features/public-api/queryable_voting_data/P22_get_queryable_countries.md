# P22. Get queryable countries

This document is part of the [launch specification](../../../README.md).

## User story

- **As a EuroFan**
- **I want** to retrieve all queryable countries, ordered by country code
- **So that** I can see the scope of the Public API's queryable voting data

## API contract

### HTTP request

```http request
GET /public/api/{apiVersion}/queryable-voting-data/countries
```

### HTTP response

```http request
200 OK
```

```json
{
  "queryableCountries": [
    {
      "countryCode": "AA",
      "countryName": "CountryName",
      "queryableContestYears": [
        2016,
        2017,
        2018
      ]
    }
  ]
}
```

## Acceptance criteria

### Happy Path : Baseline

**Endpoint...**

- [ ] Succeeds_and_retrieves_all_queryableCountries_in_countryCode_order

### Happy Path : Zero Queryable Countries

**Endpoint...**

- [ ] Succeeds_with_0_queryableCountries_when_system_is_empty
- [ ] Succeeds_with_0_queryableCountries_when_no_queryable_voting_data_exists
