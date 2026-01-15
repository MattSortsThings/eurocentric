# A10 Get contests

This document is part of the [*Eurocentric* launch specification](../../README.md).

**Feature scope:** *admin-api*

## User story

- **As the Admin**
- **I want** to retrieve all existing contests ordered by contest year
- **So that** I can test the behaviour of features that create, update or delete one or more contests

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
      "cityName": "Basel",
      "semiFinalVotingRules": "TelevoteOnly",
      "grandFinalVotingRules": "TelevoteAndJury",
      "queryable": false,
      "childBroadcasts": [
        {
          "childBroadcastId": "00000000-0000-0000-0000-000000000000",
          "contestStage": "GrandFinal",
          "completed": false
        }
      ],
      "globalTelevote": {
        "votingCountryId": "00000000-0000-0000-0000-000000000000"
      },
      "participants": [
        {
          "participatingCountryId": "00000000-0000-0000-0000-000000000000",
          "semiFinalDraw": "SemiFinal2",
          "actName": "JJ",
          "songTitle": "Wasted Love"
        }
      ]
    }
  ]
}
```

## Acceptance criteria

### Happy path

**GetContests endpoint...**

- Should_succeed_with_200_OK_and_all_existing_contests_in_order
- Should_return_empty_contests_list_when_no_contests_exist
