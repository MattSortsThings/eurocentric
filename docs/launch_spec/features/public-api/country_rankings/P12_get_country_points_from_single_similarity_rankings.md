# P12. Get country points from single similarity rankings

This document is part of the [launch specification](../../../README.md).

## User story

- **As a EuroFan**
- **I want** to rank all countries by their *points from single similarity* metric
  - **i.e.** the cosine similarity between the normalized jury and televote points award values each country received from a specified voting country across broadcasts
- **and** to be able to filter the queryable voting data
  - **by** contest year range
  - **by** contest stage
- **and** to be able to override the sorting and pagination settings
- **So that** I can learn from the rankings
  - **and** represent them in a chart or table
  - **and** import them into a database
  - **and** request additional pages of rankings

## Query logic

1. Start with the complete set of all queryable voting data
2. Filter the queryable voting data by:
   - contest year range
   - contest stage
   - voting country
   - voting format (`JuryAndTelevote` only)
3. Normalize each filtered points award value from an integer in the range \[0, 12\] to a decimal (6dp) in the range \[1.0, 15.0\]
4. Group the filtered data by competing country
5. For each group, calculate:
   - the jury and televote vectors, using each broadcast as a dimension
   - the count of unique contests
   - the count of vector dimensions
   - the vector dot product (rounding half-up to 6dp)
   - the jury vector magnitude (rounding half-up to 6dp)
   - the televote vector magnitude (rounding half-up to 6dp)
   - the points from single similarity (rounding half-up to 6dp)
6. Rank all competing countries by descending points from single similarity (using non-dense ranking, assigning equal rank to equal values)
7. Sort the rankings by *either* ascending *or* descending (rank, country code) 2-tuple
8. Apply pagination

## API contract

### HTTP request

```http request
GET /public/api/{apiVersion}/country-rankings/points-from-single-similarity
```

**Required query parameters:**

| Name                |  Type  | Details                                                                            |
|:--------------------|:------:|:-----------------------------------------------------------------------------------|
| `votingCountryCode` | string | Specifies the voting country code. Must be a string of 2 upper-case ASCII letters. |

**Optional query parameters:**

| Name               |     Type      | Default | Details                                                                                                                                                          |
|:-------------------|:-------------:|:-------:|:-----------------------------------------------------------------------------------------------------------------------------------------------------------------|
| `startContestYear` |      int      | `2016`  | Filters the queryable voting data by inclusive start contest year. Must be an integer between 2016 and 2030. Must not be greater than resolved `endContestYear`. |
| `endContestYear`   |      int      | `2030`  | Filters the queryable voting data by inclusive end contest year. Must be an integer between 2016 and 2030. Must not be less than resolved `startContestYear`.    |
| `contestStage`     | string (enum) |  `Any`  | Filters the queryable voting data by contest stage. Enum values are `{ Any, GrandFinal, SemiFinals, SemiFinal1, SemiFinal2 }`.                                   |
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
    "votingCountryCode": "ZZ",
    "startContestYear": 2016,
    "endContestYear": 2030,
    "contestStage": "Any",
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
      "vectorDimensions": 10,
      "vectorDotProduct": 100.0,
      "juryVectorMagnitude": 10.0,
      "televoteVectorMagnitude": 20.0,
      "pointsFromSingleSimilarity": 0.5
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

- [ ] Succeeds_when_specifying_votingCountryCode_FI
- [ ] Succeeds_when_specifying_votingCountryCode_GB
- [ ] Succeeds_when_specifying_votingCountryCode_MK
- [ ] Succeeds_when_specifying_votingCountryCode_SM

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
- [ ] Succeeds_with_0_rankings_when_filtering_to_TelevoteOnly_broadcasts
- [ ] Succeeds_with_0_rankings_when_filtering_to_RestOfTheWorld_voting_country
- [ ] Succeeds_with_0_rankings_when_votingCountryCode_matches_no_voters

### Sad Path : Bad HTTP Request

**Endpoint...**

- [ ] Fails_when_votingCountryCode_is_not_provided
- [ ] Fails_when_contestStage_is_invalid_enum_name

### Sad Path : Invalid Enum Argument

**Endpoint...**

- [ ] Fails_when_contestStage_is_invalid_enum_int_value

### Sad Path : Illegal Country Code

**Endpoint...**

- [ ] Fails_when_votingCountryCode_is_empty_string
- [ ] Fails_when_votingCountryCode_is_shorter_than_2_chars
- [ ] Fails_when_votingCountryCode_is_longer_than_2_chars
- [ ] Fails_when_votingCountryCode_contains_non_upper_case_ASCII_letter_char

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
