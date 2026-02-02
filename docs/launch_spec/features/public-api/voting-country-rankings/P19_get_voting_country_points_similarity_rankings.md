# P19. Get voting country points similarity rankings

This document is part of the [*Eurocentric* launch specification](../../../README.md).

- [P19. Get voting country points similarity rankings](#p19-get-voting-country-points-similarity-rankings)
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
- **I want** to get a page of voting countries ranked by their *points similarity* metric, i.e. the cosine similarity between the televote and jury points awards each country gave across broadcasts
- **So that** I can learn from the rankings, and represent them in a chart or table, and import them into a spreadsheet, and get additional rankings pages.

## Query behaviour

1. Start with the list of all points awards in all broadcasts in all queryable contests (the *queryable voting data*).
2. Filter the queryable voting data to pairs of points awards given by a televote and a jury from the same voting country in the same broadcast.
3. Normalize each points award value from an integer in the range \[0, 12\] to a decimal (6 decimal places) in the range \[1.0, 10.0\].
4. Filter the normalized voting data by contest year range, contest stage(s), and optionally by competing country.
5. Group the filtered data by voting country.
6. For each group, generate a televote vector and a jury vector, using each competing country in each broadcast as a vector dimension.
7. For each group, calculate:
   1. the vector dot product, rounded half-up to 6 decimal places.
   2. the televote vector length, rounded half-up to 6 decimal places.
   3. the jury vector length, rounded half-up to 6 decimal places.
   4. the number of vector dimensions.
   5. the number of unique competing countries.
   6. the number of unique broadcasts.
   7. the number of unique contests.
8. For each group, calculate the *points similarity* metric as (vector dot product)/((televote vector length) * jury vector length), rounded half-up to 6 decimal places.
9. Rank all groups by descending or ascending *points similarity* metric, using non-dense ranking, equal metrics assigned equal rank.
10. Sort rankings by rank (ascending) then by country code (ascending).
11. Apply pagination.

**Notes:**

- Voting countries with zero points award pairs in the filtered normalized queryable voting data are not ranked.
- Every ranking has a unique (rank, country code) tuple.
- If the query parameters exclude all queryable voting data, zero rankings are generated and an empty page is returned.

## API contract

### HTTP request

```http request
GET /public/api/{apiVersion}/voting-country-rankings/points-similarity
```

**Notes:**

- `apiVersion` is a major-minor API version URL segment, e.g. `"v1.0"`.

**Optional query parameters:**

| Name                   |      Type       | Details                                                                                                                                                                                                        |
|:-----------------------|:---------------:|:---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| `competingCountryCode` |     string      | Filters the queryable voting data by competing country code when provided. Must be string of 2 upper-case ASCII letters.                                                                                       |
| `startContestYear`     |       int       | Filters the queryable voting data by inclusive start contest year. Must be integer between 2016 and 2050. Must be less than or equal to `endContestYear`. Defaults to 2016.                                    |
| `endContestYear`       |       int       | Filters the queryable voting data by inclusive end contest year. Must be integer between 2016 and 2050. Must be greater than or equal to `startContestYear`. Defaults to 2050.                                 |
| `contestStages`        | string[] (enum) | Filters the queryable contest data by contest stage(s). Enum values are `{ SemiFinal1, SemiFinal2, GrandFinal }`. Values must be passed separately. Duplicate values are ignored. Defaults to all enum values. |
| `rankOrdering`         |  string (enum)  | Sets the rank ordering behaviour. Enum values are `{ DescendingMetric, AscendingMetric }`. Defaults to `DescendingMetric`.                                                                                     |
| `pageSize`             |       int       | Sets the pagination page size. Must be integer between 1 and 100. Defaults to 10.                                                                                                                              |
| `pageIndex`            |       int       | Sets the pagination page index. Must be non-negative integer. Defaults to 0.                                                                                                                                   |

### HTTP response

```http request
200 OK
```

```json
{
  "metadata": {
    "competingCountryCode": "CH",
    "contestYearRange": {
      "startContestYear": 2016,
      "endContestYear": 2050
    },
    "contestStages": [
      "SemiFinal1",
      "SemiFinal2",
      "GrandFinal"
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
      "pointsSimilarity": 0.75,
      "vectorDotProduct": 150.0,
      "televoteVectorLength": 10.0,
      "juryVectorLength": 20.0,
      "vectorDimensions": 10,
      "competingCountries": 1,
      "broadcasts": 10,
      "contests": 5
    }
  ]
}
```

**Notes:**

- `metadata.competingCountryCode` may be null.
- `rankings` is ordered by `rank` then by `countryCode`.
- `ranking.pointsSimilarity` is a decimal (6 decimal places), calculated as `vectorDotProduct` / (`televoteVectorLength` * `juryVectorLength`).
- `ranking.vectorDotProduct` is a decimal (6 decimal places).
- `ranking.televoteVectorLength` is a decimal (6 decimal places).
- `ranking.juryVectorLength` is a decimal (6 decimal places).

## Acceptance criteria

### Happy path

**GetVotingCountryPointsSimilarityRankings endpoint...**

- [ ] Should_succeed_with_200_OK_and_metadata_and_rankings_when_minimal_query_is_valid
- [ ] Should_succeed_with_200_OK_and_metadata_and_rankings_when_complex_query_is_valid
- [ ] Should_succeed_when_filtering_by_competingCountryCode_CH
- [ ] Should_succeed_when_filtering_by_competingCountryCode_FI
- [ ] Should_succeed_when_filtering_by_competingCountryCode_GB
- [ ] Should_succeed_when_filtering_by_competingCountryCode_SI
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

**GetVotingCountryPointsSimilarityRankings endpoint...**

- [ ] Should_fail_when_competingCountryCode_is_empty_or_whitespace
- [ ] Should_fail_when_competingCountryCode_length_is_not_2_chars
- [ ] Should_fail_when_competingCountryCode_contains_non_ASCII_letter_upper_char
- [ ] Should_fail_when_startContestYear_is_less_than_2016_or_greater_than_2050
- [ ] Should_fail_when_endContestYear_is_less_than_2016_or_greater_than_2050
- [ ] Should_fail_when_endContestYear_is_less_than_startContestYear
- [ ] Should_fail_when_contestStages_is_invalid_enum_string_value
- [ ] Should_fail_when_contestStages_is_invalid_enum_int_value
- [ ] Should_fail_when_rankOrdering_is_invalid_enum_string_value
- [ ] Should_fail_when_rankOrdering_is_invalid_enum_int_value
- [ ] Should_fail_when_pageSize_is_less_than_1_or_greater_than_100
- [ ] Should_fail_when_pageIndex_is_negative
