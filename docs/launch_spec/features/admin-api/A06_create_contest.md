# A06 Create contest

This document is part of the [*Eurocentric* launch specification](../../README.md).

**Feature scope:** *admin-api*

## User story

- **As the Admin**
- **I want** to create a new contest
- **So that** I can go on to create its broadcasts and start awarding points

## API contract

### HTTP request

```http request
POST /admin/api/{apiVersion}/contests
```

```json
{
  "contestYear": 2025,
  "cityName": "Basel",
  "semiFinalVotingRules": "TelevoteOnly",
  "globalTelevoteVotingCountryId": "00000000-0000-0000-0000-000000000000",
  "semiFinal1participants": [
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

- `globalTelevoteVotingCountryId` may be null

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
    "queryable": false,
    "childBroadcasts": [],
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

- `globalTelevote` may be null

## Acceptance criteria

### Happy path

**CreateContest endpoint...**

- Should_succeed_with_201_Created_and_created_contest_when_request_is_valid
- Should_return_Location_header_with_apiVersion_and_contestId_when_request_is_valid
- Should_create_contest_with_globalTelevote_and_semiFinalVotingRules_TelevoteAndJury
- Should_create_contest_with_globalTelevote_and_semiFinalVotingRules_TelevoteOnly
- Should_create_contest_with_no_globalTelevote_and_semiFinalVotingRules_TelevoteAndJury
- Should_create_contest_with_no_globalTelevote_and_semiFinalVotingRules_TelevoteOnly

### Sad path

**CreateContest endpoint...**

- Should_fail_when_contestYear_is_not_globally_unique
- Should_fail_when_contestYear_is_less_than_2016
- Should_fail_when_contestYear_is_greater_than_2050
- Should_fail_when_cityName_is_empty
- Should_fail_when_cityName_is_whitespace
- Should_fail_when_cityName_is_longer_than_100_characters
- Should_fail_when_cityName_starts_with_whitespace
- Should_fail_when_cityName_ends_with_whitespace
- Should_fail_when_cityName_contains_line_break
- Should_fail_when_participant_actName_is_empty
- Should_fail_when_participant_actName_is_whitespace
- Should_fail_when_participant_actName_is_longer_than_100_characters
- Should_fail_when_participant_actName_starts_with_whitespace
- Should_fail_when_participant_actName_ends_with_whitespace
- Should_fail_when_participant_actName_contains_line_break
- Should_fail_when_participant_songTitle_is_empty
- Should_fail_when_participant_songTitle_is_whitespace
- Should_fail_when_participant_songTitle_is_longer_than_100_characters
- Should_fail_when_participant_songTitle_starts_with_whitespace
- Should_fail_when_participant_songTitle_ends_with_whitespace
- Should_fail_when_participant_songTitle_contains_line_break
- Should_fail_when_globalTelevoteVotingCountryId_matches_no_country
- Should_fail_when_globalTelevoteVotingCountryId_matches_Real_country
- Should_fail_when_participant_participatingCountryId_matches_no_country
- Should_fail_when_participant_participatingCountryId_matches_Pseudo_country
- Should_fail_when_participants_have_same_participatingCountryId
- Should_fail_when_fewer_than_3_participants_have_SemiFinalDraw_SemiFinal1
- Should_fail_when_fewer_than_3_participants_have_SemiFinalDraw_SemiFinal2
