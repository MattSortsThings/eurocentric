# A09 Get contest

This document is part of the *Eurocentric* [launch specification](../../../README.md).

- [A09 Get contest](#a09-get-contest)
  - [User story](#user-story)
  - [API contract](#api-contract)
    - [HTTP request](#http-request)
    - [HTTP response](#http-response)
  - [Acceptance criteria](#acceptance-criteria)

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
200 Ok
```

```json
{
  "contest": {
    "id": "00000000-0000-0000-0000-000000000000",
    "contestYear": 2025,
    "cityName": "Basel",
    "semiFinalVotingRules": "TelevoteOnly",
    "grandFinalVotingRules": "TelevoteAndJury",
    "complete": false,
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
        "semiFinalDraw": "SemiFinal2",
        "actName": "JJ",
        "songTitle": "Wasted Love"
      }
    ]
  }
}
```

**Notes:**

- `semiFinalVotingRules` is an [enum value](../../../domain_model.md#votingrules-enum)
- `grandFinalVotingRules` is an [enum value](../../../domain_model.md#votingrules-enum)
- `contestStage` is an [enum value](../../../domain_model.md#conteststage-enum)
- `semiFinalDraw` is an [enum value](../../../domain_model.md#semifinaldraw-enum)
- `globalTelevote` is optional

## Acceptance criteria

**GetContest endpoint...**

- should succeed with 200 and return requested contest
- should fail with 404 and ProblemDetails on ContestNotFound
