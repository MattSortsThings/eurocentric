# A09 Get contest

This document is part of the [*Eurocentric* launch specification](../../README.md).

**Feature scope:** *admin-api*

## User story

- **As the Admin**
- **I want** to retrieve a specific contest
- **So that** I can review its current status

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
}
```

## Acceptance criteria

### Happy path

**GetContest endpoint...**

- Should_succeed_with_200_OK_and_requested_contest_when_request_is_valid

### Sad path

**GetContest endpoint...**

- Should_fail_when_contest_does_not_exist
