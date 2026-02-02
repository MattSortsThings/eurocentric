# A07. Create contest child broadcast

This document is part of the [*Eurocentric* launch specification](../../../README.md).

- [A07. Create contest child broadcast](#a07-create-contest-child-broadcast)
  - [User story](#user-story)
  - [API contract](#api-contract)
    - [HTTP request](#http-request)
    - [HTTP response](#http-response)
  - [Acceptance criteria](#acceptance-criteria)
    - [Happy path](#happy-path)
    - [Sad path](#sad-path)

## User story

- **As the Admin**
- **I want** to create a new child broadcast for contest
- **So that** I can start awarding points in the broadcast.

## API contract

### HTTP request

```http request
POST /admin/api/{apiVersion}/contests/{contestId}/broadcasts
```

**Notes:**

- `apiVersion` is a major-minor API version URL segment, e.g. `"v1.0"`.
- `contestId` is the Guid ID of the requested contest aggregate.

```json
{
  "broadcastDate": "2025-05-17",
  "contestStage": "GrandFinal",
  "firstHalfCompetingCountryIds": [
    "00000000-0000-0000-0000-000000000000",
    "00000000-0000-0000-0000-000000000000",
    null
  ],
  "secondHalfCompetingCountryIds": [
    "00000000-0000-0000-0000-000000000000",
    null,
    "00000000-0000-0000-0000-000000000000"
  ]
}
```

**Notes:**

- `firstHalfCompetingCountryIds` and `secondHalfCompetingCountryIds` contain a null value for a broadcast performing spot that must be kept vacant.

### HTTP response

```http request
201 Created
Location: /admin/api/{apiVersion}/broadcasts/{broadcastId}
```

**Notes:**

- `apiVersion` is a major-minor API version URL segment, e.g. `"v1.0"`.
- `broadcastId` is the Guid ID of the created broadcast aggregate.

```json
{
  "contest": {
    "id": "00000000-0000-0000-0000-000000000000",
    "broadcastDate": "2025-05-17",
    "parentContestId": "00000000-0000-0000-0000-000000000000",
    "contestStage": "GrandFinal",
    "votingFormat": "TelevoteAndJury",
    "completed": false,
    "competitors": [
      {
        "competingCountryId": "00000000-0000-0000-0000-000000000000",
        "performingSpot": 1,
        "broadcastHalf": "First",
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

- `broadcast.completed` is always `false` for a newly created broadcast aggregate.
- every item in `broadcast.competitors` has an empty `competitor.pointsAwards` collection for a newly created broadcast aggregate.
- every item in `broadcast.televotes` is `pointsAwarded=false` for a newly created broadcast aggregate.
- every item in `broadcast.juries` is `pointsAwarded=false` for a newly created broadcast aggregate.
- Any secondary effects are scoped to event handler features.

## Acceptance criteria

### Happy path

**CreateContestChildBroadcast endpoint...**

- [ ] Should_succeed_with_201_Created_and_Location_and_created_broadcast_when_request_is_valid
- [ ] Should_succeed_when_creating_TelevoteOnly_SemiFinal1_broadcast
- [ ] Should_succeed_when_creating_TelevoteOnly_SemiFinal2_broadcast
- [ ] Should_succeed_when_creating_TelevoteAndJury_SemiFinal1_broadcast
- [ ] Should_succeed_when_creating_TelevoteAndJury_SemiFinal2_broadcast
- [ ] Should_succeed_when_creating_TelevoteAndJury_GrandFinal_broadcast
- [ ] Should_succeed_when_creating_broadcast_with_vacant_running_order_spot_in_first_half
- [ ] Should_succeed_when_creating_broadcast_with_vacant_running_order_spot_in_second_half

### Sad path

**CreateContestChildBroadcast endpoint...**

- [ ] Should_fail_when_contest_does_not_exist
- [ ] Should_fail_when_broadcast_contestStage_is_not_unique_for_parent_contest
- [ ] Should_fail_when_broadcast_date_is_not_unique
- [ ] Should_fail_when_broadcast_date_has_year_less_than_2016_or_greater_than_2050
- [ ] Should_fail_when_broadcast_date_year_does_not_match_contest_year
- [ ] Should_fail_when_broadcast_has_fewer_than_3_competitors
- [ ] Should_fail_when_first_half_broadcast_competitors_have_same_competingCountryId
- [ ] Should_fail_when_second_half_broadcast_competitors_have_same_competingCountryId
- [ ] Should_fail_when_first_and_second_half_broadcast_competitors_have_same_competingCountryId
- [ ] Should_fail_when_broadcast_first_half_is_longer_than_second_half_by_more_than_2_spots
- [ ] Should_fail_when_broadcast_second_half_is_longer_than_first_half_by_more_than_2_spots
- [ ] Should_fail_when_SemiFinal1_broadcast_competingCountryId_matches_no_participant
- [ ] Should_fail_when_SemiFinal1_broadcast_competingCountryId_matches_SemiFinal2_participant
- [ ] Should_fail_when_SemiFinal2_broadcast_competingCountryId_matches_no_participant
- [ ] Should_fail_when_SemiFinal2_broadcast_competingCountryId_matches_SemiFinal1_participant
- [ ] Should_fail_when_GrandFinal_broadcast_competingCountryId_matches_no_participant
