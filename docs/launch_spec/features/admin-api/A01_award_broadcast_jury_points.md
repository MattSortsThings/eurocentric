# A01 Award broadcast jury points

This document is part of the [*Eurocentric* launch specification](../../README.md).

**Feature scope:** *admin-api*

## User story

- **As the Admin**
- **I want** to award the points from a jury to the competitors in a broadcast
- **So that** I can add to the voting data that will eventually be queryable using the Public API.

## API contract

### HTTP request

```http request
PATCH /admin/api/{apiVersion}/broadcasts/{broadcastId}/jury-points
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

**Notes:**

- `votingCountryId` must reference a jury in the broadcast that is `PointsAwarded=false`
- `rankedCompetingCountryIds` must be equivalent to the set of all competing country IDs in the broadcast excluding the voting country ID of the jury

### HTTP response

```http request
204 No Content
```

## Acceptance criteria

### Happy path

**AwardBroadcastJuryPoints endpoint...**

- Should_succeed_with_204_NoContent_when_request_is_valid
- Should_give_points_award_to_every_ranked_competitor_when_request_is_valid
- Should_update_all_competitor_finishing_spots_after_awarding_points
- Should_set_jury_to_PointsAwarded_after_awarding_points
- Should_set_broadcast_to_Completed_when_final_televote_and_jury_is_set_to_PointsAwarded

### Sad path

**AwardBroadcastJuryPoints endpoint...**

- Should_fail_when_broadcast_does_not_exist
- Should_fail_when_votingCountryId_matches_no_jury
- Should_fail_when_jury_is_PointsAwarded
- Should_fail_when_rankedCompetingCountryIds_includes_votingCountryId
- Should_fail_when_rankedCompetingCountryIds_contains_duplicates
- Should_fail_when_rankedCompetingCountryIds_omits_competing_country
- Should_fail_when_rankedCompetingCountryIds_includes_ID_matching_no_competitor
