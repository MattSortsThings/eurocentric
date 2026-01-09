# A01 Award broadcast jury points

This document is part of the *Eurocentric* [launch specification](../../../README.md).

- [A01 Award broadcast jury points](#a01-award-broadcast-jury-points)
  - [User story](#user-story)
  - [API contract](#api-contract)
    - [HTTP request](#http-request)
    - [HTTP response](#http-response)
  - [Acceptance criteria](#acceptance-criteria)

## User story

- **As the Admin**
- **I want** award a set of points from a jury to the competitors in a broadcast
- **So that** I can add to the voting data that will eventually be queryable using the Public API

## API contract

### HTTP request

```http request
PATCH /admin/api/{apiVersion}/broadcasts/{broadcastId}/award-jury-points
```

```json
{
  "votingCountryId": "00000000-0000-0000-0000-000000000000",
  "rankedCompetingCountryIds": [
    "00000000-0000-0000-0000-000000000000",
    "00000000-0000-0000-0000-000000000000"
  ]
}
```

### HTTP response

```http request
204 No Content
```

## Acceptance criteria

**AwardBroadcastJuryPoints endpoint...**

- should succeed with 204 and award points from jury to competitors in requested broadcast
- should succeed with 204 and complete broadcast when awarding final set of points
- should fail with 404 and ProblemDetails on BroadcastNotFound
- should fail with 409 and ProblemDetails on OrphanJuryCountry
- should fail with 409 and ProblemDetails on IneligibleJuryCountry
- should fail with 409 and ProblemDetails on OrphanRankedCompetitor
- should fail with 409 and ProblemDetails on MissingRankedCompetitor
- should fail with 422 and ProblemDetails on DuplicateRankedCompetitor
- should fail with 422 and ProblemDetails on VoterAsRankedCompetitor
