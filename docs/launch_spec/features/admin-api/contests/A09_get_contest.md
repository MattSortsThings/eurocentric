# A09. Get contest

This document is part of the [*Eurocentric* launch specification](../../../README.md).

- [A09. Get contest](#a09-get-contest)
  - [User story](#user-story)
  - [API contract](#api-contract)
    - [HTTP request](#http-request)
    - [HTTP response](#http-response)
  - [Acceptance criteria](#acceptance-criteria)
    - [Happy path](#happy-path)
    - [Sad path](#sad-path)

## User story

- **As the Admin**
- **I want** to retrieve a single contest
- **So that** I can review its current status.

## API contract

### HTTP request

```http request
GET /admin/api/{apiVersion}/contests/{contestId}
```

**Notes:**

- `apiVersion` is a major-minor API version URL segment, e.g. `"v1.0"`.
- `contestId` is the Guid ID of the requested contest aggregate.

### HTTP response

```http request
200 OK
```

```json
{
  "contest": {
    "id": "00000000-0000-0000-0000-000000000000",
    "contestYear": 2025,
    "cityName": "City Name",
    "semiFinalVotingFormat": "TelevoteOnly",
    "grandFinalVotingFormat": "TelevoteAndJury",
    "queryable": false,
    "broadcastMemos": [
      {
        "broadcastId": "00000000-0000-0000-0000-000000000000",
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
        "semiFinalDraw": "SemiFinal1",
        "actName": "Act Name",
        "songTitle": "Song Title"
      }
    ]
  }
}
```

**Notes:**

- `contest.globalTelevote` may be null.

## Acceptance criteria

### Happy path

**GetContest endpoint...**

- [ ] Should_succeed_with_200_OK_and_requested_contest_when_request_is_valid

### Sad path

**GetContest endpoint...**

- [ ] Should_fail_when_contest_does_not_exist
