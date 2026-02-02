# A01. Award broadcast jury points

This document is part of the [*Eurocentric* launch specification](../../../README.md).

- [A01. Award broadcast jury points](#a01-award-broadcast-jury-points)
  - [User story](#user-story)
  - [HTTP contract](#http-contract)
    - [HTTP request](#http-request)
    - [HTTP response](#http-response)
  - [Acceptance criteria](#acceptance-criteria)
    - [Happy path](#happy-path)
    - [Sad path](#sad-path)

## User story

- **As the Admin**
- **I want** to award a set of points from a jury to the competitors in a broadcast
- **So that** I can add to the voting data that will eventually become queryable.

## HTTP contract

### HTTP request

```http request
PATCH /admin/api/{apiVersion}/broadcasts/{broadcastId}/award-jury-points
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
  - the jury with the voting country ID is now `PointsAwarded=true`.
  - every ranked competitor now has an additional jury points award from the voting country.
  - every competitor has an updated finishing position.
  - if all televotes and juries are now `PointsAwarded=true`, the broadcast is now `Completed=true`.
- Any secondary effects are scoped to event handler features.

## Acceptance criteria

### Happy path

**AwardBroadcastJuryPoints endpoint...**

- [ ] Should_succeed_with_204_NoContent_and_update_broadcast_when_request_is_valid
- [ ] Should_succeed_when_awarding_first_jury_points_in_broadcast_no_televote_points_awarded
- [ ] Should_succeed_when_awarding_mid_jury_points_in_broadcast_no_televote_points_awarded
- [ ] Should_succeed_when_awarding_final_jury_points_in_broadcast_no_televote_points_awarded
- [ ] Should_succeed_when_awarding_first_jury_points_in_broadcast_some_televote_points_awarded
- [ ] Should_succeed_when_awarding_mid_jury_points_in_broadcast_some_televote_points_awarded
- [ ] Should_succeed_when_awarding_final_jury_points_in_broadcast_some_televote_points_awarded
- [ ] Should_succeed_when_awarding_first_jury_points_in_broadcast_all_televote_points_awarded
- [ ] Should_succeed_when_awarding_mid_jury_points_in_broadcast_all_televote_points_awarded
- [ ] Should_succeed_when_awarding_final_jury_points_in_broadcast_all_televote_points_awarded

### Sad path

**AwardBroadcastJuryPoints endpoint...**

- [ ] Should_fail_when_broadcast_does_not_exist
- [ ] Should_fail_when_votingCountryId_matches_no_jury
- [ ] Should_fail_when_votingCountryId_matches_jury_already_points_awarded
- [ ] Should_fail_when_rankedCompetingCountryIds_is_empty_list
- [ ] Should_fail_when_rankedCompetingCountryIds_contains_votingCountryId
- [ ] Should_fail_when_rankedCompetingCountryIds_contains_duplicates
- [ ] Should_fail_when_rankedCompetingCountryIds_item_matches_no_competitor
- [ ] Should_fail_when_rankedCompetingCountryIds_omits_competitor
