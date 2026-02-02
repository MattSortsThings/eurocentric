# A04. Get broadcast

This document is part of the [*Eurocentric* launch specification](../../../README.md).

- [A04. Get broadcast](#a04-get-broadcast)
  - [User story](#user-story)
  - [API contract](#api-contract)
    - [HTTP request](#http-request)
    - [HTTP response](#http-response)
  - [Acceptance criteria](#acceptance-criteria)
    - [Happy path](#happy-path)
    - [Sad path](#sad-path)

## User story

- **As the Admin**
- **I want** to retrieve a single broadcast
- **So that** I can review its current status.

## API contract

### HTTP request

```http request
GET /admin/api/{apiVersion}/broadcasts/{broadcastId}
```

**Notes:**

- `apiVersion` is a major-minor API version URL segment, e.g. `"v1.0"`.
- `broadcastId` is the Guid ID of the requested broadcast aggregate.

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
    "votingFormat": "TelevoteAndJury",
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
        "pointsAwarded": true
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

- [ ] Should_succeed_with_200_OK_and_requested_broadcast_when_request_is_valid

### Sad path

**GetBroadcast endpoint...**

- [ ] Should_fail_when_broadcast_does_not_exist
