# A07 Create contest child broadcast

This document is part of the *Eurocentric* [launch specification](../../../README.md).

- [A07 Create contest child broadcast](#a07-create-contest-child-broadcast)
  - [User story](#user-story)
  - [API contract](#api-contract)
    - [HTTP request](#http-request)
    - [HTTP response](#http-response)
  - [Acceptance criteria](#acceptance-criteria)

## User story

- **As the Admin**
- **I want** to create a new child broadcast for a specific contest
- **So that** I can start awarding the points in the created broadcast

## API contract

### HTTP request

```http request
POST /admin/api/{apiVersion}/contests/{contestId}/broadcasts
```

```json
{
  "broadcastDate": "2025-05-17",
  "contestStage": "GrandFinal",
  "runningOrderCompetingCountryIds": [
    "00000000-0000-0000-0000-000000000000",
    null,
    "00000000-0000-0000-0000-000000000000"
  ]
}
```

**Notes:**

- `broadcastDate` uses the `"yyyy-MM-dd"` date format
- `contestStage` is an [enum value](../../../domain_model.md#conteststage-enum)

### HTTP response

```http request
201 Created
Location: /admin/api/{apiVersion}/broadcasts/{broadcastId}
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
        "pointsAwards": [ ]
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

**CreateContestChildBroadcast endpoint...**

- should succeed with 201 and return created broadcast with televotes and juries
- should succeed with 201 and return created broadcast with televotes only
- should succeed with 201 and return created broadcast with empty running order position
- should fail with 404 and ProblemDetails on ContestNotFound
- should fail with 409 and ProblemDetails on BroadcastDateConflict
- should fail with 409 and ProblemDetails on ChildBroadcastContestStageConflict
- should fail with 409 and ProblemDetails on ChildBroadcastDateOutOfRange
- should fail with 409 and ProblemDetails on OrphanCompetitorCountry
- should fail with 409 and ProblemDetails on IneligibleCompetitorCountry
- should fail with 422 and ProblemDetails on IllegalBroadcastDateValue
- should fail with 422 and ProblemDetails on DuplicateCompetitorCountries
- should fail with 422 and ProblemDetails on IllegalCompetitorCount
