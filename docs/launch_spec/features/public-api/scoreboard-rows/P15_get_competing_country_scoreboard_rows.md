# P15 Get competing country scoreboard rows

This document is part of the [*Eurocentric* launch specification](../../../README.md).

- [P15 Get competing country scoreboard rows](#p15-get-competing-country-scoreboard-rows)
  - [User story](#user-story)
  - [API contract](#api-contract)
    - [HTTP request](#http-request)
    - [HTTP response](#http-response)
  - [Acceptance criteria](#acceptance-criteria)
    - [Happy path](#happy-path)
    - [Sad path](#sad-path)

## User story

- **As a EuroFan**
- **I want** to retrieve all the scoreboard rows for a specific competing country across broadcasts
- **So that** I can see how the country fared in each broadcast.

## API contract

### HTTP request

```http request
GET /public/api/{apiVersion}/scoreboard-rows/competing-countries
```

**Notes:**

- `apiVersion` is a major-minor API version URL segment, e.g. `"v1.0"`.

**Required query parameters:**

| Name                   |  Type  | Details                                                                             |
|:-----------------------|:------:|:------------------------------------------------------------------------------------|
| `competingCountryCode` | string | Specifies the competing country code. Must be string of 2 upper-case ASCII letters. |

### HTTP response

```http request
200 OK
```

```json
{
  "metadata": {
    "competingCountryCode": "ZZ"
  },
  "scoreboardRows": [
    {
      "broadcastDate": "2025-05-17",
      "contestStage": "GrandFinal",
      "performingSpot": 1,
      "actName": "Act Name",
      "songTitle": "Song Title",
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

- `scoreboardRows` is sorted by `scoreboardRow.broadcastDate`.
- `scoreboardRow.juryPoints` may be null.
- `scoreboardRow.juryRank` may be null.

## Acceptance criteria

### Happy path

**GetCompetingCountryScoreboardRows endpoint...**

- [ ] Should_succeed_with_200_OK_and_metadata_and_ordered_scoreboard_rows_when_query_is_valid
- [ ] Should_succeed_when_querying_competingCountryCode_CH
- [ ] Should_succeed_when_querying_competingCountryCode_FI
- [ ] Should_succeed_when_querying_competingCountryCode_GB
- [ ] Should_succeed_when_querying_competingCountryCode_MK
- [ ] Should_succeed_when_querying_competingCountryCode_SI
- [ ] Should_succeed_when_querying_competingCountryCode_SM
- [ ] Should_succeed_with_empty_scoreboard_rows_when_query_excludes_all_queryable_voting_data
- [ ] Should_succeed_with_empty_scoreboard_rows_when_no_queryable_voting_data_exists
- [ ] Should_succeed_with_empty_scoreboard_rows_when_system_contains_no_data

### Sad path

**GetCompetingCountryScoreboardRows endpoint...**

- [ ]Should_fail_when_contestStage_is_not_provided
- [ ] Should_fail_when_contestStage_is_invalid_enum_string_value
- [ ] Should_fail_when_contestStage_is_invalid_enum_int_value
- [ ] Should_fail_when_competingCountryCode_is_not_provided
- [ ] Should_fail_when_competingCountryCode_is_empty_or_whitespace
- [ ] Should_fail_when_competingCountryCode_length_is_not_2_chars
- [ ] Should_fail_when_competingCountryCode_contains_non_ASCII_letter_upper_char
