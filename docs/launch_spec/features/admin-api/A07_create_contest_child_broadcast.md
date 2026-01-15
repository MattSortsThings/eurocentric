# A07 Create contest child broadcast

This document is part of the [*Eurocentric* launch specification](../../README.md).

**Feature scope:** *admin-api*

## User story

- **As the Admin**
- **I want** to create a new broadcast from a specific contest
- **So that** I can start awarding points

## API contract

### HTTP request

```http request
POST /admin/api/{apiVersion}/contests/{contestId}/broadcasts
```

```json
{
  "contestStage": "GrandFinal",
  "broadcastDate": "2025-05-17",
  "competingCountryIds": [
    "00000000-0000-0000-0000-000000000000",
    "00000000-0000-0000-0000-000000000000",
    null,
    "00000000-0000-0000-0000-000000000000",
    "00000000-0000-0000-0000-000000000000"
  ],
  "firstHalfLength": 3
}
```

**Notes:**

- A null value in `competingCountryIds` indicates an empty performing spot
- `firstHalfLength` must be greater than 0

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
        "performingSpot": 1,
        "performingHalf": "First",
        "finishingSpot": 1,
        "pointsAwards": []
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

- `votingRules` is set from the parent contest, which means that a Grand Final broadcast can only ever be `VotingRules=TelevoteAndJury`

## Acceptance criteria

### Happy path

**CreateContestBroadcast endpoint...**

- Should_succeed_with_201_Created_and_created_broadcast_when_request_is_valid
- Should_return_Location_header_with_apiVersion_and_broadcastId_when_request_is_valid
- Should_create_broadcast_with_contestStage_GrandFinal_and_votingRules_TelevoteAndJury
- Should_create_broadcast_with_contestStage_SemiFinal1_and_votingRules_TelevoteAndJury
- Should_create_broadcast_with_contestStage_SemiFinal1_and_votingRules_TelevoteOnly
- Should_create_broadcast_with_contestStage_SemiFinal2_and_votingRules_TelevoteAndJury
- Should_create_broadcast_with_contestStage_SemiFinal2_and_votingRules_TelevoteOnly
- Should_create_broadcast_with_single_empty_performing_spot
- Should_create_broadcast_with_multiple_empty_performing_spots

### Sad path

**CreateContestBroadcast endpoint...**

- Should_fail_when_broadcastDate_is_not_globally_unique
- Should_fail_when_contest_already_has_childBroadcast_with_same_contestStage
- Should_fail_when_broadcastDate_year_is_earlier_than_contest_contestYear
- Should_fail_when_broadcastDate_is_later_than_contest_contestYear
- Should_fail_when_broadcastDate_year_is_earlier_than_2016
- Should_fail_when_broadcastDate_year_is_later_than_2050
- Should_fail_when_competingCountryIds_contains_duplicate_non_null_IDs
- Should_fail_when_competingCountryIds_contains_fewer_than_3_non_null_IDs
- Should_fail_when_competingCountryId_matches_no_participant
- Should_fail_when_contestStage_is_SemiFinal1_and_competing_country_has_drawn_SemiFinal2
- Should_fail_when_contestStage_is_SemiFinal2_and_competing_country_has_drawn_SemiFinal1
- Should_fail_when_firstHalfLength_is_0
- Should_fail_when_performing_half_lengths_differ_by_more_than_2
