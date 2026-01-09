# A04 Get broadcast

This document is part of the *Eurocentric* [launch specification](../../../README.md).

- [A04 Get broadcast](#a04-get-broadcast)
  - [User story](#user-story)
  - [API contract](#api-contract)
    - [HTTP request](#http-request)
    - [HTTP response](#http-response)
  - [Acceptance criteria](#acceptance-criteria)

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
200 Ok
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
        "runningOrderPosition": 1,
        "finishingPosition": 1,
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

**Notes:**

- `broadcastDate` uses the `"yyyy-MM-dd"` date format
- `contestStage` is an [enum value](../../../domain_model.md#conteststage-enum)
- `votingMethod` is an [enum value](../../../domain_model.md#votingmethod-enum)
- `votingRules` is an [enum value](../../../domain_model.md#votingrules-enum)

## Acceptance criteria

**GetBroadcast endpoint...**

- should succeed with 200 and return requested broadcast
- should fail with 404 and ProblemDetails on BroadcastNotFound
