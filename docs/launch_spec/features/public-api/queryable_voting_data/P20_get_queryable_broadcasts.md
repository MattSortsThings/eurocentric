# P20. Get queryable broadcasts

This document is part of the [launch specification](../../../README.md).

## User story

- **As a EuroFan**
- **I want** to retrieve all queryable broadcasts, ordered by broadcast date
- **So that** I can see the scope of the Public API's queryable voting data

## API contract

### HTTP request

```http request
GET /public/api/{apiVersion}/queryable-voting-data/broadcasts
```

### HTTP response

```http request
200 OK
```

```json
{
  "queryableBroadcasts": [
    {
      "broadcastDate": "2025-01-01",
      "cityName": "CityName",
      "contestStage": "GrandFinal",
      "votingFormat": "JuryAndTelevote",
      "competingCountryCodes": [
        "AA",
        "BB",
        "CC"
      ],
      "votingCountryCodes": [
        "AA",
        "BB",
        "CC",
        "XX"
      ]
    }
  ]
}
```

## Acceptance criteria

### Happy Path : Baseline

**Endpoint...**

- [ ] Succeeds_and_retrieves_all_queryableBroadcasts_in_broadcastDate_order

### Happy Path : Zero Queryable Broadcasts

**Endpoint...**

- [ ] Succeeds_with_0_queryableBroadcasts_when_system_is_empty
- [ ] Succeeds_with_0_queryableBroadcasts_when_no_queryable_voting_data_exists
