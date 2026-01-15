# A04 Get broadcast

This document is part of the [*Eurocentric* launch specification](../../README.md).

**Feature scope:** *admin-api*

## User story

- **As the Admin**
- **I want** to retrieve a specific broadcast
- **So that** I can review its current status

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
    "broadcastDate": "2025-05-17",
    "parentContestId": "00000000-0000-0000-0000-000000000000",
    "contestStage": "GrandFinal",
    "votingRules": "TelevoteAndJury",
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
            "votingMethod": "Televote",
            "pointsValue": 12
          }
        ]
      }
    ],
    "televotes": [
      {
        "votingCountryId": "00000000-0000-0000-0000-000000000000",
        "pointsAwarded": false
      }
    ],
    "juries": [
      {
        "votingCountryId": "00000000-0000-0000-0000-000000000000",
        "pointsAwarded": false
      }
    ]
  }
}
```

## Acceptance criteria

### Happy path

**GetBroadcast endpoint...**

- Should_succeed_with_200_OK_and_requested_broadcast_when_request_is_valid

### Sad path

**GetBroadcast endpoint...**

- Should_fail_when_broadcast_does_not_exist
