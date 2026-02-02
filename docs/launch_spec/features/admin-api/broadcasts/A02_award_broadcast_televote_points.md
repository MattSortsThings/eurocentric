# A02. Award broadcast televote points

This document is part of the [*Eurocentric* launch specification](../../../README.md).

- [A02. Award broadcast televote points](#a02-award-broadcast-televote-points)
  - [User story](#user-story)
  - [HTTP contract](#http-contract)
    - [HTTP request](#http-request)
    - [HTTP response](#http-response)
  - [Acceptance criteria](#acceptance-criteria)
    - [Happy path](#happy-path)
    - [Sad path](#sad-path)

## User story

- **As the Admin**
- **I want** to award a set of points from a televote to the competitors in a broadcast
- **So that** I can add to the voting data that will eventually become queryable.

## HTTP contract

### HTTP request

```http request
PATCH /admin/api/{apiVersion}/broadcasts/{broadcastId}/award-televote-points
```

**Notes:**

- `apiVersion` is a major-minor API version URL segment, e.g. `"v1.0"`.
- `broadcastId` is the Guid ID of the requested broadcast aggregate.

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

**Notes:**

- The requested broadcast aggregate is updated as follows:
  - the televote with the voting country ID is now `PointsAwarded=true`.
  - every ranked competitor now has an additional televote points award from the voting country.
  - every competitor has an updated finishing position.
  - if all televotes and juries are now `PointsAwarded=true`, the broadcast is now `Completed=true`.
- Any secondary effects are scoped to event handler features.

## Acceptance criteria

### Happy path

**AwardBroadcastTelevotePoints endpoint...**

- [ ] Should_succeed_with_204_NoContent_and_update_broadcast_when_request_is_valid
- [ ] Should_succeed_when_awarding_first_televote_points_in_broadcast_with_no_juries
- [ ] Should_succeed_when_awarding_mid_televote_points_in_broadcast_with_no_juries
- [ ] Should_succeed_when_awarding_final_televote_points_in_broadcast_with_no_juries
- [ ] Should_succeed_when_awarding_first_televote_points_in_broadcast_no_jury_points_awarded
- [ ] Should_succeed_when_awarding_mid_televote_points_in_broadcast_no_jury_points_awarded
- [ ] Should_succeed_when_awarding_final_televote_points_in_broadcast_no_jury_points_awarded
- [ ] Should_succeed_when_awarding_first_televote_points_in_broadcast_some_jury_points_awarded
- [ ] Should_succeed_when_awarding_mid_televote_points_in_broadcast_some_jury_points_awarded
- [ ] Should_succeed_when_awarding_final_televote_points_in_broadcast_some_jury_points_awarded
- [ ] Should_succeed_when_awarding_first_televote_points_in_broadcast_all_jury_points_awarded
- [ ] Should_succeed_when_awarding_mid_televote_points_in_broadcast_all_jury_points_awarded
- [ ] Should_succeed_when_awarding_final_televote_points_in_broadcast_all_jury_points_awarded

### Sad path

**AwardBroadcastTelevotePoints endpoint...**

- [ ] Should_fail_when_broadcast_does_not_exist
- [ ] Should_fail_when_votingCountryId_matches_no_televote
- [ ] Should_fail_when_votingCountryId_matches_televote_already_points_awarded
- [ ] Should_fail_when_rankedCompetingCountryIds_is_empty_list
- [ ] Should_fail_when_rankedCompetingCountryIds_contains_votingCountryId
- [ ] Should_fail_when_rankedCompetingCountryIds_contains_duplicates
- [ ] Should_fail_when_rankedCompetingCountryIds_item_matches_no_competitor
- [ ] Should_fail_when_rankedCompetingCountryIds_omits_competitor
