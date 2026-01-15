# A05 Get broadcasts

This document is part of the [*Eurocentric* launch specification](../../README.md).

**Feature scope:** *admin-api*

## User story

- **As the Admin**
- **I want** to retrieve all existing broadcasts ordered by broadcast date
- **So that** I can test the behaviour of features that create, update or delete one or more broadcasts

## API contract

### HTTP request

```http request
GET /admin/api/{apiVersion}/broadcasts
```

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
  ]
}
```

## Acceptance criteria

### Happy path

**GetBroadcasts endpoint...**

- Should_succeed_with_200_OK_and_all_existing_broadcasts_in_order
- Should_return_empty_broadcasts_list_when_no_broadcasts_exist
