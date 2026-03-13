# A17. Get country

This document is part of the [launch specification](../../../README.md).

## User story

- **As the Admin**
- **I want** to retrieve a specified country
- **So that** I can verify the country's current status

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
    "countryCode": "AA",
    "countryName": "CountryName",
    "countryType": "Real",
    "activeContestIds": [
      "00000000-0000-0000-0000-000000000000",
      "00000000-0000-0000-0000-000000000000"
    ]
  }
}
```

## Acceptance criteria

### Happy Path : Baseline

**Endpoint...**

- [ ] Succeeds_and_retrieves_country

### Sad Path : Country Not Found

**Endpoint...**

- [ ] Fails_when_country_does_not_exist
