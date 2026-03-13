# A07. Create contest broadcast

This document is part of the [launch specification](../../../README.md).

## User story

- **As the Admin**
- **I want** to create a new broadcast for a specified contest
- **So that** I can start awarding points in the broadcast

## API contract

### HTTP request

```http request
POST /admin/api/{apiVersion}/contests/{contestId}/broadcasts
```

```json
{
  "broadcastDate": "2025-01-01",
  "contestStage": "GrandFinal",
  "firstPerformingSpotAfterInterval": 3,
  "orderedCompetingCountryIds": [
    "00000000-0000-0000-0000-000000000000",
    null,
    "00000000-0000-0000-0000-000000000000",
    "00000000-0000-0000-0000-000000000000",
    "00000000-0000-0000-0000-000000000000"
  ]
}
```

**Notes:**

- `null` value in `orderedCompetingCountryIds` denotes a vacant performing spot

### HTTP response

```http request
201 Created
Location: {host}/admin/api/{apiVersion}/broadcasts/{broadcastId}
```

```json
{
  "broadcast": {
    "id": "00000000-0000-0000-0000-000000000000",
    "broadcastDate": "2025-01-01",
    "parentContestId": "00000000-0000-0000-0000-000000000000",
    "contestStage": "GrandFinal",
    "broadcastFormat": "JuryAndTelevote",
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
    "juries": [
      {
        "votingCountryId": "00000000-0000-0000-0000-000000000000",
        "pointsAwarded": false
      }
    ],
    "televotes": [
      {
        "votingCountryId": "00000000-0000-0000-0000-000000000000",
        "pointsAwarded": false
      }
    ]
  }
}
```

## Acceptance criteria

### Happy Path : Baseline

**Endpoint...**

- [ ] Succeeds_and_creates_broadcast

### Happy Path : Variants

**Endpoint...**

- [ ] Succeeds_and_creates_JuryAndTelevote_SemiFinal1_broadcast_with_globalTelevote
- [ ] Succeeds_and_creates_JuryAndTelevote_SemiFinal1_broadcast_with_no_globalTelevote
- [ ] Succeeds_and_creates_JuryAndTelevote_SemiFinal2_broadcast_with_globalTelevote
- [ ] Succeeds_and_creates_JuryAndTelevote_SemiFinal2_broadcast_with_no_globalTelevote
- [ ] Succeeds_and_creates_JuryAndTelevote_GrandFinal_broadcast_with_globalTelevote
- [ ] Succeeds_and_creates_JuryAndTelevote_GrandFinal_broadcast_with_no_globalTelevote
- [ ] Succeeds_and_creates_TelevoteOnly_SemiFinal1_broadcast_with_globalTelevote
- [ ] Succeeds_and_creates_TelevoteOnly_SemiFinal1_broadcast_with_no_globalTelevote
- [ ] Succeeds_and_creates_TelevoteOnly_SemiFinal2_broadcast_with_globalTelevote
- [ ] Succeeds_and_creates_TelevoteOnly_SemiFinal2_broadcast_with_no_globalTelevote

### Sad Path : Bad HTTP Request

**Endpoint...**

- [ ] Fails_when_broadcastDate_is_not_provided
- [ ] Fails_when_contestStage_is_not_provided
- [ ] Fails_when_firstPerformingSpotAfterInterval_is_not_provided
- [ ] Fails_when_orderedCompetingCountryIds_is_not_provided
- [ ] Fails_when_contestStage_is_invalid_enum_name

### Sad Path : Invalid Enum Argument

**Endpoint...**

- [ ] Fails_when_contestStage_is_invalid_enum_int_value

### Sad Path : Contest Not Found

**Endpoint...**

- [ ] Fails_when_contest_does_not_exist

### Sad Path : Broadcast Date Duplicated

**Endpoint...**

- [ ] Fails_when_existing_broadcast_has_same_broadcastDate

### Sad Path : Contest Child Broadcast Competitor Ineligible

**Endpoint...**

- [ ] Fails_when_SemiFinal1_competitor_competingCountryId_matches_SemiFinal2_participant
- [ ] Fails_when_SemiFinal2_competitor_competingCountryId_matches_participant

### Sad Path : Contest Child Broadcast Competitor Not Found

**Endpoint...**

- [ ] Fails_when_competitor_competingCountryId_matches_globalTelevote
- [ ] Fails_when_competitor_competingCountryId_matches_no_participant

### Sad Path : Contest Child Broadcast Date Out Of Range

**Endpoint...**

- [ ] Fails_when_broadcastDate_predates_contestYear
- [ ] Fails_when_broadcastDate_postdates_contestYear

### Sad Path : Contest Child Broadcast Stage Duplicated

**Endpoint...**

- [ ] Fails_when_childBroadcast_has_same_contestStage

### Sad Path : Illegal Broadcast Date

**Endpoint...**

- [ ] Fails_when_broadcastDate_year_is_less_than_2016
- [ ] Fails_when_broadcastDate_year_is_greater_than_2030

### Sad Path : Illegal Performing Spot

**Endpoint...**

- [ ] Fails_when_firstPerformingSpotAfterInterval_is_less_than_1

### Sad Path : Illegal Broadcast Competitor Count

**Endpoint...**

- [ ] Fails_when_broadcast_has_fewer_than_3_competitors

### Sad Path : Illegal Broadcast Half Boundaries

**Endpoint...**

- [ ] Fails_when_firstPerformingSpotAfterInterval_is_1
- [ ] Fails_when_firstPerformingSpotAfterInterval_is_greater_than_final_broadcast_performingSpot
