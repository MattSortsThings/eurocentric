# A16. Get countries

This document is part of the [launch specification](../../../README.md).

## User story

- **As the Admin**
- **I want** to retrieve all existing countries, ordered by country code
- **So that** I can verify the behaviour of features that create, update, or delete one or more countries

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
      "countryCode": "AA",
      "countryName": "CountryName",
      "countryType": "Real",
      "activeContestIds": [
        "00000000-0000-0000-0000-000000000000",
        "00000000-0000-0000-0000-000000000000"
      ]
    }
  ]
}
```

## Acceptance criteria

### Happy Path : Baseline

**Endpoint...**

- [ ] Succeeds_and_retrieves_all_countries_in_countryCode_order

### Happy Path : Zero Countries

**Endpoint...**

- [ ] Succeeds_with_0_countries_when_no_countries_exist
