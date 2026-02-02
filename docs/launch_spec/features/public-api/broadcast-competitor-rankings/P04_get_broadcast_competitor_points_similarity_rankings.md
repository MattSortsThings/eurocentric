# P04. Get broadcast competitor points similarity rankings

This document is part of the [*Eurocentric* launch specification](../../../README.md).

- [P04. Get broadcast competitor points similarity rankings](#p04-get-broadcast-competitor-points-similarity-rankings)
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
- **I want** to get a page of broadcast competitors ranked by their *points similarity* metric, i.e. the cosine similarity between the televote and jury points awards each competitor received in their broadcast
- **So that** I can learn from the rankings, and represent them in a chart or table, and import them into a spreadsheet, and get additional rankings pages.

## Query behaviour

1. Start with the list of all points awards in all broadcasts in all queryable contests (the *queryable voting data*).
2. Filter the queryable voting data to pairs of points awards given by a televote and a jury from the same voting country in the same broadcast.
3. Normalize each points award value from an integer in the range \[0, 12\] to a decimal (6 decimal places) in the range \[1.0, 10.0\].
4. Filter the normalized voting data by contest year range, and contest stage(s).
5. Group the filtered data by broadcast competitor.
6. For each group, generate a televote vector and a jury vector, using each voting country in each broadcast as a vector dimension.
7. For each group, calculate:
   1. the vector dot product, rounded half-up to 6 decimal places.
   2. the televote vector length, rounded half-up to 6 decimal places.
   3. the jury vector length, rounded half-up to 6 decimal places.
   4. the number of vector dimensions.
8. For each group, calculate the *points similarity* metric as (vector dot product)/((televote vector length) * jury vector length), rounded half-up to 6 decimal places.
9. Rank all groups by descending or ascending *points similarity* metric, using non-dense ranking, equal metrics assigned equal rank.
10. Sort rankings by rank (ascending) then by broadcast date (ascending) then by performing spot (ascending).
11. Apply pagination.

**Notes:**

- Competitors with zero points award pairs in the filtered normalized queryable voting data are not ranked.
- Every ranking has a unique (rank, broadcast date, performing spot) tuple.
- If the query parameters exclude all queryable voting data, zero rankings are generated and an empty page is returned.

## API contract

### HTTP request

```http request
GET /public/api/{apiVersion}/broadcast-competitor-rankings/points-similarity
```

**Notes:**

- `apiVersion` is a major-minor API version URL segment, e.g. `"v1.0"`.

**Optional query parameters:**

| Name                |      Type       | Details                                                                                                                                                                                                        |
|:--------------------|:---------------:|:---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| `startContestYear`  |       int       | Filters the queryable voting data by inclusive start contest year. Must be integer between 2016 and 2050. Must be less than or equal to `endContestYear`. Defaults to 2016.                                    |
| `endContestYear`    |       int       | Filters the queryable voting data by inclusive end contest year. Must be integer between 2016 and 2050. Must be greater than or equal to `startContestYear`. Defaults to 2050.                                 |
| `contestStages`     | string[] (enum) | Filters the queryable contest data by contest stage(s). Enum values are `{ SemiFinal1, SemiFinal2, GrandFinal }`. Values must be passed separately. Duplicate values are ignored. Defaults to all enum values. |
| `rankOrdering`      |  string (enum)  | Sets the rank ordering behaviour. Enum values are `{ DescendingMetric, AscendingMetric }`. Defaults to `DescendingMetric`.                                                                                     |
| `pageSize`          |       int       | Sets the pagination page size. Must be integer between 1 and 100. Defaults to 10.                                                                                                                              |
| `pageIndex`         |       int       | Sets the pagination page index. Must be non-negative integer. Defaults to 0.                                                                                                                                   |

### HTTP response

```http request
200 OK
```

```json
{
  "metadata": {
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
      "broadcastDate": "2025-05-17",
      "contestStage": "GrandFinal",
      "performingSpot": 1,
      "countryCode": "AA",
      "countryName": "Country Name",
      "actName": "Act Name",
      "songTitle": "Song Title",
      "finishingSpot": 1,
      "pointsSimilarity": 0.75,
      "vectorDotProduct": 1500.0,
      "televoteVectorLength": 100.0,
      "juryVectorLength": 100.0,
      "vectorDimensions": 50
    }
  ]
}
```

**Notes:**

- `rankings` is ordered by `rank` then by `broadcastDate` then by `performingSpot`.
- `ranking.pointsSimilarity` is a decimal (6 decimal places), calculated as `vectorDotProduct` / (`televoteVectorLength` * `juryVectorLength`).
- `ranking.vectorDotProduct` is a decimal (6 decimal places).
- `ranking.televoteVectorLength` is a decimal (6 decimal places).
- `ranking.juryVectorLength` is a decimal (6 decimal places).

## Acceptance criteria

### Happy path

**GetBroadcastCompetitorPointsSimilarityRankings endpoint...**

- [ ] Should_succeed_with_200_OK_and_metadata_and_rankings_when_minimal_query_is_valid
- [ ] Should_succeed_with_200_OK_and_metadata_and_rankings_when_complex_query_is_valid
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

**GetBroadcastCompetitorPointsSimilarityRankings endpoint...**

- [ ] Should_fail_when_startContestYear_is_less_than_2016_or_greater_than_2050
- [ ] Should_fail_when_endContestYear_is_less_than_2016_or_greater_than_2050
- [ ] Should_fail_when_endContestYear_is_less_than_startContestYear
- [ ] Should_fail_when_contestStages_is_invalid_enum_string_value
- [ ] Should_fail_when_contestStages_is_invalid_enum_int_value
- [ ] Should_fail_when_rankOrdering_is_invalid_enum_string_value
- [ ] Should_fail_when_rankOrdering_is_invalid_enum_int_value
- [ ] Should_fail_when_pageSize_is_less_than_1_or_greater_than_100
- [ ] Should_fail_when_pageIndex_is_negative
