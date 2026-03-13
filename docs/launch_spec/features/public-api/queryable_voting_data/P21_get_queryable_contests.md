# P21. Get queryable contests

This document is part of the [launch specification](../../../README.md).

## User story

- **As a EuroFan**
- **I want** to retrieve all queryable contests, ordered by contest year
- **So that** I can see the scope of the Public API's queryable voting data

## API contract

### HTTP request

```http request
GET /public/api/{apiVersion}/queryable-voting-data/contests
```

### HTTP response

```http request
200 OK
```

```json
{
  "queryableContests": [
    {
      "contestYear": 2025,
      "cityName": "CityName",
      "semiFinalVotingFormat": "JuryAndTelevote",
      "grandFinalVotingFormat": "JuryAndTelevote",
      "globalTelevoteVotingCountryCode": "XX",
      "participatingCountryCodes": [
        "AA",
        "BB",
        "CC"
      ]
    }
  ]
}
```

**Notes:**

- `queryableContest`.`globalTelevoteCountryCode` may be `null`

## Acceptance criteria

### Happy Path : Baseline

**Endpoint...**

- [ ] Succeeds_and_retrieves_all_queryableContests_in_contestYear_order

### Happy Path : Zero Queryable Contests

**Endpoint...**

- [ ] Succeeds_with_0_queryableContests_when_system_is_empty
- [ ] Succeeds_with_0_queryableContests_when_no_queryable_voting_data_exists
