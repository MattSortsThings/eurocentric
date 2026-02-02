# A10. Get contests

This document is part of the [*Eurocentric* launch specification](../../../README.md).

- [A10. Get contests](#a10-get-contests)
  - [User story](#user-story)
  - [API contract](#api-contract)
    - [HTTP request](#http-request)
    - [HTTP response](#http-response)
  - [Acceptance criteria](#acceptance-criteria)
    - [Happy path](#happy-path)

## User story

- **As the Admin**
- **I want** to retrieve all existing contests in contest year order
- **So that** I can test the behaviour of features that create, update or delete one or more contests.

## API contract

### HTTP request

```http request
GET /admin/api/{apiVersion}/contests
```

**Notes:**

- `apiVersion` is a major-minor API version URL segment, e.g. `"v1.0"`.

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
  ]
}
```

**Notes:**

- `contests` is ordered by `contest.contestYear`.
- `contest.globalTelevote` may be null.

## Acceptance criteria

### Happy path

**GetContests endpoint...**

- [ ] Should_succeed_with_200_OK_and_all_existing_contests_in_contest_year_order
- [ ] Should_succeed_with_empty_contests_list_when_system_contains_no_contests
