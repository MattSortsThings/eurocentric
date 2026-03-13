# A09. Get contest

This document is part of the [launch specification](../../../README.md).

## User story

- **As the Admin**
- **I want** to retrieve a specified contest
- **So that** I can verify the contest's current status

## API contract

### HTTP request

```http request
GET /admin/api/{apiVersion}/contests/{contestId}
```

### HTTP response

```http request
200 OK
```

```json
{
  "contest": {
    "id": "00000000-0000-0000-0000-000000000000",
    "contestYear": 2025,
    "cityName": "CityName",
    "semiFinalBroadcastFormat": "TelevoteOnly",
    "grandFinalBroadcastFormat": "JuryAndTelevote",
    "queryable": false,
    "globalTelevote": {
      "votingCountryId": "00000000-0000-0000-0000-000000000000"
    },
    "childBroadcasts": [
      {
        "childBroadcastId": "00000000-0000-0000-0000-000000000000",
        "contestStage": "GrandFinal",
        "completed": false
      }
    ],
    "participants": [
      {
        "participatingCountryId": "00000000-0000-0000-0000-000000000000",
        "semiFinalDraw": "SemiFinal1",
        "actName": "ActName",
        "songTitle": "SongTitle"
      }
    ]
  }
}
```

**Notes:**

- `contest`.`globalTelevote` may be `null`

## Acceptance criteria

### Happy Path : Baseline

**Endpoint...**

- [ ] Succeeds_and_retrieves_contest

### Sad Path : Contest Not Found

**Endpoint...**

- [ ] Fails_when_contest_does_not_exist
