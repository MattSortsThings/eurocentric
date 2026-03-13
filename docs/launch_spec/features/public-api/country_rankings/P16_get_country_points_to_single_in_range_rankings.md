# P16. Get country points to single in range rankings

This document is part of the [launch specification](../../../README.md).

## User story

- **As a EuroFan**
- **I want** to rank all countries by their *points to single in range* metric
  - **i.e.** the relative frequency of points awards within a specified value range each country gave to a specified competing country across broadcasts
- **and** to be able to filter the queryable voting data
  - **by** contest year range
  - **by** contest stage
  - **by** voting method
- **and** to be able to override the sorting and pagination settings
- **So that** I can learn from the rankings
  - **and** represent them in a chart or table
  - **and** import them into a database
  - **and** request additional pages of rankings

## Query logic

1. Start with the complete set of all queryable voting data
2. Filter the queryable voting data by:
   - competing country
   - contest year range
   - contest stage
   - voting method
3. Group the filtered data by voting country
4. For each group, calculate:
   - the count of unique contests
   - the count of unique broadcasts
   - the count of points awards
   - the count of points awards in range
   - the points to single in range (rounding half-up to 6dp)
5. Rank all voting countries by descending points to single in range (using non-dense ranking, assigning equal rank to equal values)
6. Sort the rankings by *either* ascending *or* descending (rank, country code) 2-tuple
7. Apply pagination

## API contract

### HTTP request

```http request
GET /public/api/{apiVersion}/country-rankings/points-to-single-in-range
```

**Required query parameters:**

| Name                   |  Type  | Details                                                                                                                       |
|:-----------------------|:------:|:------------------------------------------------------------------------------------------------------------------------------|
| `competingCountryCode` | string | Specifies the competing country code. Must be a string of 2 upper-case ASCII letters.                                         |
| `minPointsValue`       |  int   | Specifies the inclusive minimum points value. Must be an integer between 0 and 12. Must not be greater than `maxPointsValue`. |
| `maxPointsValue`       |  int   | Specifies the inclusive maximum points value. Must be an integer between 0 and 12. Must not be less than `minPointsValue`.    |

**Optional query parameters:**

| Name               |     Type      | Default | Details                                                                                                                                                          |
|:-------------------|:-------------:|:-------:|:-----------------------------------------------------------------------------------------------------------------------------------------------------------------|
| `startContestYear` |      int      | `2016`  | Filters the queryable voting data by inclusive start contest year. Must be an integer between 2016 and 2030. Must not be greater than resolved `endContestYear`. |
| `endContestYear`   |      int      | `2030`  | Filters the queryable voting data by inclusive end contest year. Must be an integer between 2016 and 2030. Must not be less than resolved `startContestYear`.    |
| `contestStage`     | string (enum) |  `Any`  | Filters the queryable voting data by contest stage. Enum values are `{ Any, GrandFinal, SemiFinals, SemiFinal1, SemiFinal2 }`.                                   |
| `votingMethod`     | string (enum) |  `Any`  | Filters the queryable voting data by voting method. Enum values are `{ Any, Jury, Televote }`.                                                                   |
| `sortDescending`   |     bool      | `false` | Sets the pre-pagination rankings sort to descending order (`false`) or ascending order (`true`).                                                                 |
| `pageSize`         |      int      |  `10`   | Sets the pagination maximum page size. Must be an integer between 5 and 100.                                                                                     |
| `pageIndex`        |      int      |   `0`   | Sets the pagination page-index. Must be a non-negative integer.                                                                                                  |

### HTTP response

```http request
200 OK
```

```json
{
  "metadata": {
    "competingCountryCode": "ZZ",
    "minPointsValue": 1,
    "maxPointsValue": 12,
    "startContestYear": 2016,
    "endContestYear": 2030,
    "contestStage": "Any",
    "votingMethod": "Any",
    "sortDescending": false,
    "pageSize": 10,
    "pageIndex": 0,
    "totalRankings": 50
  },
  "rankings": [
    {
      "rank": 1,
      "countryCode": "AA",
      "countryName": "CountryName",
      "contests": 5,
      "broadcasts": 10,
      "pointsAwards": 20,
      "pointsAwardsInRange": 15,
      "pointsToSingleInRange": 0.75
    }
  ]
}
```

## Acceptance criteria

### Happy Path : Baseline

**Endpoint...**

- [ ] Succeeds_and_returns_metadata_and_rankings_given_minimal_query
- [ ] Succeeds_and_returns_metadata_and_rankings_given_explicit_default_values_for_all_optional_params

### Happy Path : Variants : Specification

**Endpoint...**

- [ ] Succeeds_when_specifying_competingCountryCode_CH_minPointsValue_1_maxPointsValue_8
- [ ] Succeeds_when_specifying_competingCountryCode_CH_minPointsValue_1_maxPointsValue_12
- [ ] Succeeds_when_specifying_competingCountryCode_CH_minPointsValue_8_maxPointsValue_12
- [ ] Succeeds_when_specifying_competingCountryCode_AT_minPointsValue_0_maxPointsValue_0
- [ ] Succeeds_when_specifying_competingCountryCode_AT_minPointsValue_1_maxPointsValue_12
- [ ] Succeeds_when_specifying_competingCountryCode_FI_minPointsValue_0_maxPointsValue_0
- [ ] Succeeds_when_specifying_competingCountryCode_FI_minPointsValue_1_maxPointsValue_12
- [ ] Succeeds_when_specifying_competingCountryCode_GB_minPointsValue_0_maxPointsValue_0
- [ ] Succeeds_when_specifying_competingCountryCode_GB_minPointsValue_1_maxPointsValue_12
- [ ] Succeeds_when_specifying_competingCountryCode_SM_minPointsValue_0_maxPointsValue_0
- [ ] Succeeds_when_specifying_competingCountryCode_SM_minPointsValue_1_maxPointsValue_12

### Happy Path : Variants : Filtering : Contest Year Range

**Endpoint...**

- [ ] Succeeds_when_filtering_by_startContestYear_2023
- [ ] Succeeds_when_filtering_by_endContestYear_2022
- [ ] Succeeds_when_filtering_by_startContestYear_2022_endContestYear_2023
- [ ] Succeeds_when_filtering_by_startContestYear_2023_endContestYear_2023

### Happy Path : Variants : Filtering : Contest Stage

**Endpoint...**

- [ ] Succeeds_when_filtering_by_contestStage_GrandFinal
- [ ] Succeeds_when_filtering_by_contestStage_SemiFinals
- [ ] Succeeds_when_filtering_by_contestStage_SemiFinal1
- [ ] Succeeds_when_filtering_by_contestStage_SemiFinal2

### Happy Path : Variants : Filtering : Voting Method

**Endpoint...**

- [ ] Succeeds_when_filtering_by_votingMethod_Jury
- [ ] Succeeds_when_filtering_by_votingMethod_Televote

### Happy Path : Variants : Sorting

**Endpoint...**

- [ ] Succeeds_when_setting_sortDescending_true

### Happy Path : Variants : Pagination

**Endpoint...**

- [ ] Succeeds_when_setting_pageSize_5
- [ ] Succeeds_when_setting_pageIndex_1
- [ ] Succeeds_when_setting_pageSize_5_pageIndex_1
- [ ] Succeeds_when_setting_pageSize_5_pageIndex_2

### Happy Path : Zero Rankings

**Endpoint...**

- [ ] Succeeds_with_0_rankings_when_system_is_empty
- [ ] Succeeds_with_0_rankings_when_no_queryable_voting_data_exists
- [ ] Succeeds_with_0_rankings_when_pagination_exceeds_totalRankings
- [ ] Succeeds_with_0_rankings_when_startContestYear_postdates_latest_queryable_contest
- [ ] Succeeds_with_0_rankings_when_endContestYear_predates_earliest_queryable_contest
- [ ] Succeeds_with_0_rankings_when_filtering_to_Jury_points_in_TelevoteOnly_broadcasts
- [ ] Succeeds_with_0_rankings_when_competingCountryCode_matches_no_competitors

### Sad Path : Bad HTTP Request

**Endpoint...**

- [ ] Fails_when_competingCountryCode_is_not_provided
- [ ] Fails_when_minPointsValue_is_not_provided
- [ ] Fails_when_maxPointsValue_is_not_provided
- [ ] Fails_when_contestStage_is_invalid_enum_name
- [ ] Fails_when_votingMethod_is_invalid_enum_name

### Sad Path : Invalid Enum Argument

**Endpoint...**

- [ ] Fails_when_contestStage_is_invalid_enum_int_value
- [ ] Fails_when_votingMethod_is_invalid_enum_int_value

### Sad Path : Illegal Country Code

**Endpoint...**

- [ ] Fails_when_competingCountryCode_is_empty_string
- [ ] Fails_when_competingCountryCode_is_shorter_than_2_chars
- [ ] Fails_when_competingCountryCode_is_longer_than_2_chars
- [ ] Fails_when_competingCountryCode_contains_non_upper_case_ASCII_letter_char

### Sad Path : Illegal Points Value

**Endpoint...**

- [ ] Fails_when_minPointsValue_is_less_than_0
- [ ] Fails_when_minPointsValue_is_greater_than_12
- [ ] Fails_when_maxPointsValue_is_less_than_0
- [ ] Fails_when_maxPointsValue_is_greater_than_12

### Sad Path : Illegal Points Value Range

**Endpoint...**

- [ ] Fails_when_minPointsValue_is_greater_than_maxPointsValue

### Sad Path : Illegal Contest Year

**Endpoint...**

- [ ] Fails_when_startContestYear_is_less_than_2016
- [ ] Fails_when_startContestYear_is_greater_than_2030
- [ ] Fails_when_endContestYear_is_less_than_2016
- [ ] Fails_when_endContestYear_is_greater_than_2030

### Sad Path : Illegal Contest Year Range

**Endpoint...**

- [ ] Fails_when_startContestYear_is_greater_than_endContestYear

### Sad Path : Illegal Page Index

**Endpoint...**

- [ ] Fails_when_pageIndex_is_negative

### Sad Path : Illegal Page Size

**Endpoint...**

- [ ] Fails_when_pageSize_is_less_than_5
- [ ] Fails_when_pageSize_is_greater_than_100
