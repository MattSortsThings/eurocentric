# A05. Get broadcasts

This document is part of the [*Eurocentric* launch specification](../../../README.md).

- [A05. Get broadcasts](#a05-get-broadcasts)
  - [User story](#user-story)
  - [API contract](#api-contract)
    - [HTTP request](#http-request)
    - [HTTP response](#http-response)
  - [Acceptance criteria](#acceptance-criteria)
    - [Happy path](#happy-path)

## User story

- **As the Admin**
- **I want** to retrieve all existing broadcasts in broadcast date order
- **So that** I can test the behaviour of features that create, update or delete one or more broadcasts.

## API contract

### HTTP request

```http request
GET /admin/api/{apiVersion}/broadcasts
```

**Notes:**

- `apiVersion` is a major-minor API version URL segment, e.g. `"v1.0"`.

### HTTP response

```http request
200 OK
```

```json
{
  "broadcasts": [
    {
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
  ]
}
```

**Notes:**

- `broadcasts` is ordered by `broadcast.broadcastDate`.

## Acceptance criteria

### Happy path

**GetBroadcasts endpoint...**

- [ ] Should_succeed_with_200_OK_and_all_existing_broadcasts_in_broadcast_date_order
- [ ] Should_succeed_with_empty_broadcasts_list_when_system_contains_no_broadcasts
