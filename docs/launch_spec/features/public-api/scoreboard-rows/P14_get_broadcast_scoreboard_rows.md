# P14. Get broadcast scoreboard rows

This document is part of the [*Eurocentric* launch specification](../../../README.md).

- [P14. Get broadcast scoreboard rows](#p14-get-broadcast-scoreboard-rows)
  - [User story](#user-story)
  - [API contract](#api-contract)
    - [HTTP request](#http-request)
    - [HTTP response](#http-response)
  - [Acceptance criteria](#acceptance-criteria)
    - [Happy path](#happy-path)
    - [Sad path](#sad-path)

## User story

- **As a EuroFan**
- **I want** to retrieve all the scoreboard rows for a specific broadcast
- **So that** I can see who the competitors in the broadcast were and how they fared.

## API contract

### HTTP request

```http request
GET /public/api/{apiVersion}/scoreboard-rows/broadcasts
```

**Notes:**

- `apiVersion` is a major-minor API version URL segment, e.g. `"v1.0"`.

**Required query parameters:**

| Name           |     Type      | Details                                                                                |
|:---------------|:-------------:|:---------------------------------------------------------------------------------------|
| `contestYear`  |      int      | Specifies the contest year. Must be integer between 2016 and 2050.                     |
| `contestStage` | string (enum) | Specifies the contest stage. Enum values are `{ SemiFinal1, SemiFinal2, GrandFinal }`. |

### HTTP response

```http request
200 OK
```

```json
{
  "metadata": {
    "contestYear": 2025,
    "contestStage": "GrandFinal"
  },
  "scoreboardRows": [
    {
      "performingSpot": 1,
      "countryCode": "AA",
      "countryName": "Country Name",
      "televotePoints": 100,
      "televoteRank": 1,
      "juryPoints": 100,
      "juryRank": 1,
      "overallPoints": 200,
      "finishingSpot": 1
    }
  ]
}
```

**Notes:**

- `scoreboardRows` is sorted by `scoreboardRow.performingSpot`.
- `scoreboardRow.juryPoints` may be null.
- `scoreboardRow.juryRank` may be null.

## Acceptance criteria

### Happy path

**GetBroadcastScoreboardRows endpoint...**

- [ ] Should_succeed_with_200_OK_and_metadata_and_ordered_scoreboard_rows_when_query_is_valid
- [ ] Should_succeed_when_querying_contestYear_2022_contestStage_SemiFinal1
- [ ] Should_succeed_when_querying_contestYear_2022_contestStage_SemiFinal2
- [ ] Should_succeed_when_querying_contestYear_2022_contestStage_GrandFinal
- [ ] Should_succeed_when_querying_contestYear_2023_contestStage_SemiFinal1
- [ ] Should_succeed_when_querying_contestYear_2023_contestStage_SemiFinal2
- [ ] Should_succeed_when_querying_contestYear_2023_contestStage_GrandFinal
- [ ] Should_succeed_with_empty_scoreboard_rows_when_query_excludes_all_queryable_voting_data
- [ ] Should_succeed_with_empty_scoreboard_rows_when_no_queryable_voting_data_exists
- [ ] Should_succeed_with_empty_scoreboard_rows_when_system_contains_no_data

### Sad path

**GetBroadcastScoreboardRows endpoint...**

- [ ] Should_fail_when_contestYear_is_not_provided
- [ ] Should_fail_when_contestYear_is_less_than_2016_or_greater_than_2050
- [ ] Should_fail_when_contestStage_is_not_provided
- [ ] Should_fail_when_contestStage_is_invalid_enum_string_value
- [ ] Should_fail_when_contestStage_is_invalid_enum_int_value
