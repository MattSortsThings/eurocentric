# A06. Create contest

This document is part of the [*Eurocentric* launch specification](../../../README.md).

- [A06. Create contest](#a06-create-contest)
  - [User story](#user-story)
  - [API contract](#api-contract)
    - [HTTP request](#http-request)
    - [HTTP response](#http-response)
  - [Acceptance criteria](#acceptance-criteria)
    - [Happy path](#happy-path)
    - [Sad path](#sad-path)

## User story

- **As the Admin**
- **I want** to create a new contest
- **So that** I can create child broadcasts for the contest and start awarding points.

## API contract

### HTTP request

```http request
POST /admin/api/{apiVersion}/contests
```

**Notes:**

- `apiVersion` is a major-minor API version URL segment, e.g. `"v1.0"`.

```json
{
  "contestYear": 2025,
  "cityName": "City Name",
  "semiFinalVotingFormat": "TelevoteOnly",
  "globalTelevoteVotingCountryId": "00000000-0000-0000-0000-000000000000",
  "participants": [
    {
      "participatingCountryId": "00000000-0000-0000-0000-000000000000",
      "semiFinalDraw": "SemiFinal1",
      "actName": "Act Name",
      "songTitle": "Song Title"
    }
  ]
}
```

**Notes:**

- `globalTelevoteVotingCountryId` may be null.

### HTTP response

```http request
201 Created
Location: /admin/api/{apiVersion}/contests/{contestId}
```

**Notes:**

- `apiVersion` is a major-minor API version URL segment, e.g. `"v1.0"`.
- `contestId` is the Guid ID of the created contest aggregate.

```json
{
  "contest": {
    "id": "00000000-0000-0000-0000-000000000000",
    "contestYear": 2025,
    "cityName": "City Name",
    "semiFinalVotingFormat": "TelevoteOnly",
    "grandFinalVotingFormat": "TelevoteAndJury",
    "queryable": false,
    "broadcastMemos": [],
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

- `contest.grandFinalVotingFormat` is always `"TelevoteAndJury"`.
- `contest.queryable` is always `false` for a newly created contest aggregate.
- `contest.broadcastMemos` is always empty for a newly created contest aggregate.
- `contest.globalTelevote` may be null.
- Any secondary effects are scoped to event handler features.

## Acceptance criteria

### Happy path

**CreateContest endpoint...**

- [ ] Should_succeed_with_201_Created_and_Location_and_created_contest_when_request_is_valid
- [ ] Should_succeed_when_creating_contest_with_TelevoteAndJury_semiFinals_and_globalTelevote
- [ ] Should_succeed_when_creating_contest_with_TelevoteAndJury_semiFinals_and_no_globalTelevote
- [ ] Should_succeed_when_creating_contest_with_TelevoteOnly_semiFinals_and_globalTelevote
- [ ] Should_succeed_when_creating_contest_with_TelevoteOnly_semiFinals_and_no_globalTelevote

### Sad path

**CreateContest endpoint...**

- [ ] Should_fail_when_contestYear_is_not_unique
- [ ] Should_fail_when_contestYear_is_less_than_2016_or_greater_than_2050
- [ ] Should_fail_when_cityName_is_empty_or_whitespace
- [ ] Should_fail_when_cityName_is_longer_than_100_chars
- [ ] Should_fail_when_cityName_contains_line_break
- [ ] Should_fail_when_cityName_starts_or_ends_with_whitespace
- [ ] Should_fail_when_participant_actName_is_empty_or_whitespace
- [ ] Should_fail_when_participant_actName_is_longer_than_100_chars
- [ ] Should_fail_when_participant_actName_contains_line_break
- [ ] Should_fail_when_participant_actName_starts_or_ends_with_whitespace
- [ ] Should_fail_when_participant_songTitle_is_empty_or_whitespace
- [ ] Should_fail_when_participant_songTitle_is_longer_than_100_chars
- [ ] Should_fail_when_participant_songTitle_contains_line_break
- [ ] Should_fail_when_participant_songTitle_starts_or_ends_with_whitespace
- [ ] Should_fail_when_globalTelevoteVotingCountryId_matches_no_country
- [ ] Should_fail_when_globalTelevoteVotingCountryId_matches_Real_country
- [ ] Should_fail_when_participant_participatingCountryId_matches_no_country
- [ ] Should_fail_when_participant_participatingCountryId_matches_Pseudo_country
- [ ] Should_fail_when_participants_have_duplicate_participatingCountryIds
- [ ] Should_fail_when_fewer_than_3_participants_have_SemiFinalDraw_SemiFinal1
- [ ] Should_fail_when_fewer_than_3_participants_have_SemiFinalDraw_SemiFinal2
