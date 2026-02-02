# P16. Get voting country points average rankings

This document is part of the [*Eurocentric* launch specification](../../../README.md).

- [P16. Get voting country points average rankings](#p16-get-voting-country-points-average-rankings)
  - [User story](#user-story)
  - [Query behaviour](#query-behaviour)
  - [API contract](#api-contract)
    - [HTTP request](#http-request)
    - [HTTP response](#http-response)
  - [Acceptance criteria](#acceptance-criteria)
    - [Happy path](#happy-path)
    - [Sad path](#sad-path)

## User story

- **As a EuroFan**
- **I want** to get a page of voting countries ranked by their *points average* metric, i.e. the average individual points award value each country gave to a specific competing country across broadcasts
- **So that** I can learn from the rankings, and represent them in a chart or table, and import them into a spreadsheet, and get additional rankings pages.

## Query behaviour

1. Start with the list of all points awards in all broadcasts in all queryable contests (the *queryable voting data*).
2. Filter the queryable voting data by competing country, contest year range, contest stage(s), and voting method(s).
3. Group the filtered data by voting country.
4. For each group, calculate:
   1. The sum total points given.
   2. The number of points awards.
   3. The number of unique broadcasts.
   4. The number of unique contests.
5. For each group, calculate the *points average* metric as (sum total points)/(points awards), rounded half-up to 6 decimal places.
6. Rank all groups by descending or ascending *points average* metric, using non-dense ranking, equal metrics assigned equal rank.
7. Sort rankings by rank (ascending) then by country code (ascending).
8. Apply pagination.

**Notes:**

- Voting countries with zero points awards in the filtered queryable voting data are not ranked.
- Every ranking has a unique (rank, country code) tuple.
- If the query parameters exclude all queryable voting data, zero rankings are generated and an empty page is returned.

## API contract

### HTTP request

```http request
GET /public/api/{apiVersion}/voting-country-rankings/points-average
```

**Notes:**

- `apiVersion` is a major-minor API version URL segment, e.g. `"v1.0"`.

**Required query parameters:**

| Name                   |  Type  | Details                                                                             |
|:-----------------------|:------:|:------------------------------------------------------------------------------------|
| `competingCountryCode` | string | Specifies the competing country code. Must be string of 2 upper-case ASCII letters. |

**Optional query parameters:**

| Name               |      Type       | Details                                                                                                                                                                                                        |
|:-------------------|:---------------:|:---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| `startContestYear` |       int       | Filters the queryable voting data by inclusive start contest year. Must be integer between 2016 and 2050. Must be less than or equal to `endContestYear`. Defaults to 2016.                                    |
| `endContestYear`   |       int       | Filters the queryable voting data by inclusive end contest year. Must be integer between 2016 and 2050. Must be greater than or equal to `startContestYear`. Defaults to 2050.                                 |
| `contestStages`    | string[] (enum) | Filters the queryable contest data by contest stage(s). Enum values are `{ SemiFinal1, SemiFinal2, GrandFinal }`. Values must be passed separately. Duplicate values are ignored. Defaults to all enum values. |
| `votingMethods`    | string[] (enum) | Filters the queryable contest data by voting method(s). Enum values are `{ Televote, Jury }`. Values must be passed separately. Duplicate values are ignored. Defaults to all enum values.                     |
| `rankOrdering`     |  string (enum)  | Sets the rank ordering behaviour. Enum values are `{ DescendingMetric, AscendingMetric }`. Defaults to `DescendingMetric`.                                                                                     |
| `pageSize`         |       int       | Sets the pagination page size. Must be integer between 1 and 100. Defaults to 10.                                                                                                                              |
| `pageIndex`        |       int       | Sets the pagination page index. Must be non-negative integer. Defaults to 0.                                                                                                                                   |

### HTTP response

```http request
200 OK
```

```json
{
  "metadata": {
    "competingCountryCode": "ZZ",
    "contestYearRange": {
      "startContestYear": 2016,
      "endContestYear": 2050
    },
    "contestStages": [
      "SemiFinal1",
      "SemiFinal2",
      "GrandFinal"
    ],
    "votingMethods": [
      "Televote",
      "Jury"
    ],
    "rankOrdering": "DescendingMetric",
    "pagination": {
      "pageSize": 10,
      "pageIndex": 0,
      "totalItems": 50
    }
  },
  "rankings": [
    {
      "rank": 1,
      "countryCode": "AA",
      "countryName": "Country Name",
      "pointsAverage": 2.5,
      "totalPoints": 50,
      "pointsAwards": 20,
      "broadcasts": 10,
      "contests": 5
    }
  ]
}
```

**Notes:**

- `rankings` is ordered by `rank` then by `countryCode`.
- `ranking.pointsAverage` is a decimal (6 decimal places), calculated as `totalPoints` / `pointsAwards`.

## Acceptance criteria

### Happy path

**GetVotingCountryPointsAverageRankings endpoint...**

- [ ] Should_succeed_with_200_OK_and_metadata_and_rankings_when_minimal_query_is_valid
- [ ] Should_succeed_with_200_OK_and_metadata_and_rankings_when_complex_query_is_valid
- [ ] Should_succeed_when_specifying_competingCountryCode_CH
- [ ] Should_succeed_when_specifying_competingCountryCode_FI
- [ ] Should_succeed_when_specifying_competingCountryCode_GB
- [ ] Should_succeed_when_specifying_competingCountryCode_MK
- [ ] Should_succeed_when_specifying_competingCountryCode_SI
- [ ] Should_succeed_when_filtering_by_startContestYear_2023
- [ ] Should_succeed_when_filtering_by_endContestYear_2022
- [ ] Should_succeed_when_filtering_by_startContestYear_2016_endContestYear_2050
- [ ] Should_succeed_when_filtering_by_startContestYear_2023_endContestYear_2023
- [ ] Should_succeed_when_filtering_by_contestStages_SemiFinal1
- [ ] Should_succeed_when_filtering_by_contestStages_SemiFinal2
- [ ] Should_succeed_when_filtering_by_contestStages_GrandFinal
- [ ] Should_succeed_when_filtering_by_contestStages_SemiFinal1_SemiFinal2
- [ ] Should_succeed_when_filtering_by_contestStages_SemiFinal1_GrandFinal
- [ ] Should_succeed_when_filtering_by_contestStages_SemiFinal2_GrandFinal
- [ ] Should_succeed_when_filtering_by_contestStages_SemiFinal1_SemiFinal2_GrandFinal
- [ ] Should_succeed_when_filtering_by_contestStages_omitting_duplicate_values
- [ ] Should_succeed_when_filtering_by_votingMethods_Televote
- [ ] Should_succeed_when_filtering_by_votingMethods_Jury
- [ ] Should_succeed_when_filtering_by_votingMethods_Televote_Jury
- [ ] Should_succeed_when_filtering_by_votingMethods_omitting_duplicate_values
- [ ] Should_succeed_when_applying_rankOrdering_DescendingMetric
- [ ] Should_succeed_when_applying_rankOrdering_AscendingMetric
- [ ] Should_succeed_when_applying_pageSize_10
- [ ] Should_succeed_when_applying_pageSize_7
- [ ] Should_succeed_when_applying_pageIndex_0
- [ ] Should_succeed_when_applying_pageIndex_1
- [ ] Should_succeed_when_applying_pageSize_7_pageIndex_1
- [ ] Should_succeed_with_empty_rankings_when_pagination_exceeds_totalItems
- [ ] Should_succeed_with_empty_rankings_when_query_excludes_all_queryable_voting_data
- [ ] Should_succeed_with_empty_rankings_when_no_queryable_voting_data_exists
- [ ] Should_succeed_with_empty_rankings_when_system_contains_no_data

### Sad path

**GetVotingCountryPointsAverageRankings endpoint...**

- [ ] Should_fail_when_competingCountryCode_is_not_provided
- [ ] Should_fail_when_competingCountryCode_is_empty_or_whitespace
- [ ] Should_fail_when_competingCountryCode_length_is_not_2_chars
- [ ] Should_fail_when_competingCountryCode_contains_non_ASCII_letter_upper_char
- [ ] Should_fail_when_startContestYear_is_less_than_2016_or_greater_than_2050
- [ ] Should_fail_when_endContestYear_is_less_than_2016_or_greater_than_2050
- [ ] Should_fail_when_endContestYear_is_less_than_startContestYear
- [ ] Should_fail_when_contestStages_is_invalid_enum_string_value
- [ ] Should_fail_when_contestStages_is_invalid_enum_int_value
- [ ] Should_fail_when_votingMethods_is_invalid_enum_string_value
- [ ] Should_fail_when_votingMethods_is_invalid_enum_int_value
- [ ] Should_fail_when_rankOrdering_is_invalid_enum_string_value
- [ ] Should_fail_when_rankOrdering_is_invalid_enum_int_value
- [ ] Should_fail_when_pageSize_is_less_than_1_or_greater_than_100
- [ ] Should_fail_when_pageIndex_is_negative
