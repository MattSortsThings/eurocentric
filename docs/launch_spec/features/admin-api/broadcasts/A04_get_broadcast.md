# A04. Get broadcast

This document is part of the [launch specification](../../../README.md).

## User story

- **As the Admin**
- **I want** to retrieve a specified broadcast
- **So that** I can verify the broadcast's current status

## API contract

### HTTP request

```http request
GET /admin/api/{apiVersion}/broadcasts/{broadcastId}
```

### HTTP response

```http request
200 OK
```

```json
{
  "broadcast": {
    "id": "00000000-0000-0000-0000-000000000000",
    "broadcastDate": "2025-01-01",
    "parentContestId": "00000000-0000-0000-0000-000000000000",
    "contestStage": "GrandFinal",
    "broadcastFormat": "JuryAndTelevote",
    "completed": false,
    "competitors": [
      {
        "competingCountryId": "00000000-0000-0000-0000-000000000000",
        "performingSpot": 1,
        "broadcastHalf": "First",
        "finishingSpot": 1,
        "pointsAwards": [
          {
            "votingCountryId": "00000000-0000-0000-0000-000000000000",
            "votingMethod": "Jury",
            "pointsValue": 12
          }
        ]
      }
    ],
    "juries": [
      {
        "votingCountryId": "00000000-0000-0000-0000-000000000000",
        "pointsAwarded": false
      }
    ],
    "televotes": [
      {
        "votingCountryId": "00000000-0000-0000-0000-000000000000",
        "pointsAwarded": false
      }
    ]
  }
}
```

## Acceptance criteria

### Happy Path : Baseline

**Endpoint...**

- [ ] Succeeds_and_retrieves_broadcast

### Sad Path : Broadcast Not Found

**Endpoint...**

- [ ] Fails_when_broadcast_does_not_exist
