# A01. Award broadcast jury points

This document is part of the [launch specification](../../../README.md).

## User story

- **As the Admin**
- **I want** to award the points from a single jury to all the competitors in a specified broadcast
- **So that** I can add to the voting data that will eventually be queryable using the Public API

## API contract

### HTTP request

```http request
PATCH /admin/api/{apiVersion}/broadcasts/{broadcastId}/juries/{votingCountryId}
```

```json
{
  "rankedCompetingCountryIds": [
    "00000000-0000-0000-0000-000000000000",
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

### Happy Path : Baseline

**Endpoint...**

- [ ] Succeeds_and_awards_points_from_jury_to_competitors_in_broadcast

### Happy Path : Variants

**Endpoint...**

- [ ] Succeeds_when_all_jury_and_some_televote_points_awarded_in_JuryAndTelevote_broadcast
- [ ] Succeeds_and_completes_JuryAndTelevote_broadcast_when_all_jury_and_all_televote_points_awarded

### Sad Path : Bad HTTP Request

**Endpoint...**

- [ ] Fails_when_rankedCompetingCountryIds_is_not_provided

### Sad Path : Broadcast Not Found

**Endpoint...**

- [ ] Fails_when_broadcast_does_not_exist

### Sad Path : Broadcast Jury Not Found

**Endpoint...**

- [ ] Fails_when_votingCountryId_matches_no_jury_in_broadcast

### Sad Path : Broadcast Jury Points Already Awarded

**Endpoint...**

- [ ] Fails_when_votingCountryId_matches_jury_in_broadcast_that_has_already_awarded_points

### Sad Path : Broadcast Ranked Competitor Not Found

**Endpoint...**

- [ ] Fails_when_rankedCompetingCountryIds_item_matches_no_competitor_in_broadcast

### Sad Path : Broadcast Competitor Not Ranked

**Endpoint...**

- [ ] Fails_when_rankedCompetingCountryIds_omits_competitor_in_broadcast

### Sad Path : Duplicated Broadcast Ranked Competitors

**Endpoint...**

- [ ] Fails_when_rankedCompetingCountryIds_contains_duplicate_items

### Sad Path : Broadcast Voter As Ranked Competitor

**Endpoint...**

- [ ] Fails_when_rankedCompetingCountryIds_contains_votingCountryId
