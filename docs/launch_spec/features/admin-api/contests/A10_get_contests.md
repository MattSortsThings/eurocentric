# A10. Get contests

This document is part of the [launch specification](../../../README.md).

## User story

- **As the Admin**
- **I want** to retrieve all existing contests, ordered by contest year
- **So that** I can verify the behaviour of features that create, update, or delete one or more contests

## API contract

### HTTP request

```http request
GET /admin/api/{apiVersion}/contests
```

### HTTP response

```http request
200 OK
```

```json
{
  "contests": [
    {
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
  ]
}
```

**Notes:**

- `contest`.`globalTelevote` may be `null`

## Acceptance criteria

### Happy Path : Baseline

**Endpoint...**

- [ ] Succeeds_and_retrieves_all_contests_in_contestYear_order

### Happy Path : Zero Contests

**Endpoint...**

- [ ] Succeeds_with_0_contests_when_no_contests_exist
