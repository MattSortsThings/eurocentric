# A06 Create contest

This document is part of the *Eurocentric* [launch specification](../../../README.md).

- [A06 Create contest](#a06-create-contest)
  - [User story](#user-story)
  - [API contract](#api-contract)
    - [HTTP request](#http-request)
    - [HTTP response](#http-response)
  - [Acceptance criteria](#acceptance-criteria)

## User story

- **As the Admin**
- **I want** to create a new contest
- **So that** I can go on to create its child broadcasts and start awarding points

## API contract

### HTTP request

```http request
POST /admin/api/{apiVersion}/contests
```

```json
{
  "contestYear": 2025,
  "cityName": "Basel",
  "semiFinalVotingRules": "TelevoteAndJury",
  "globalTelevoteVotingCountryId": "00000000-0000-0000-0000-000000000000",
  "participants": [
    {
      "participatingCountryId": "00000000-0000-0000-0000-000000000000",
      "semiFinalDraw": "SemiFinal2",
      "actName": "JJ",
      "songTitle": "Wasted Love"
    }
  ]
}
```

**Notes:**

- `globalTelevoteVotingCountryId` is optional
- `semiFinalDraw` is an [enum value](../../../domain_model.md#semifinaldraw-enum)
- `semiFinalVotingRules` is an [enum value](../../../domain_model.md#votingrules-enum)

### HTTP response

```http request
201 Created
Location: /admin/api/{apiVersion}/contests/{contestId}
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
    "broadcastMemos": [],
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

**Notes:**

## Acceptance criteria

**CreateContest endpoint...**

- should succeed with 201 and return created contest with televote and jury semi-finals and global televote
- should succeed with 201 and return created contest with televote and jury semi-finals and no global televote
- should succeed with 201 and return created contest with televote only semi-finals and global televote
- should succeed with 201 and return created contest with televote only semi-finals and no global televote
- should fail with 409 and ProblemDetails on ContestYearConflict
- should fail with 409 and ProblemDetails on OrphanGlobalTelevoteCountry
- should fail with 409 and ProblemDetails on IneligibleGlobalTelevoteCountry
- should fail with 409 and ProblemDetails on OrphanParticipantCountry
- should fail with 409 and ProblemDetails on IneligibleParticipantCountry
- should fail with 422 and ProblemDetails on IllegalContestYearValue. Test cases include:
  - integer less than 2016
  - integer greater than 2050
- should fail with 422 and ProblemDetails on IllegalCityNameValue. Test cases include:
  - empty string
  - all whitespace string
  - string longer than 100 characters
  - string with line-break character
  - string with leading whitespace
  - string with trailing whitespace
- should fail with 422 and ProblemDetails on IllegalActNameValue. Test cases include:
  - empty string
  - all whitespace string
  - string longer than 100 characters
  - string with line-break character
  - string with leading whitespace
  - string with trailing whitespace
- should fail with 422 and ProblemDetails on IllegalSongTitleValue. Test cases include:
  - empty string
  - all whitespace string
  - string longer than 100 characters
  - string with line-break character
  - string with leading whitespace
  - string with trailing whitespace
- should fail with 422 and ProblemDetails on DuplicateParticipantCountries
- should fail with 422 and ProblemDetails on IllegalSemiFinal1DrawCount
- should fail with 422 and ProblemDetails on IllegalSemiFinal2DrawCount
