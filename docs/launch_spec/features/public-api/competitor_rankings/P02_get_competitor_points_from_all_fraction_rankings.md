# P02. Get competitor points from all fraction rankings

This document is part of the [launch specification](../../../README.md).

## User story

- **As a EuroFan**
- **I want** to rank all broadcast competitors by their *points from all fraction* metric
  - **i.e.** the fraction of the maximum possible points each competitor received from all voting countries in their broadcast
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
   - contest year range
   - contest stage
   - voting method
3. Group the filtered data by broadcast competitor
4. For each group, calculate:
   - the count of points awards
   - the sum total points
   - the maximum possible points
   - the points from all fraction (rounding half-up to 6dp)
5. Rank all competitors by descending points from all fraction (using non-dense ranking, assigning equal rank to equal values)
6. Sort the rankings by *either* ascending *or* descending (rank, broadcast date, performing spot) 3-tuple
7. Apply pagination

## API contract

### HTTP request

```http request
GET /public/api/{apiVersion}/competitor-rankings/points-from-all-fraction
```

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
    "startContestYear": 2016,
    "endContestYear": 2030,
    "contestStage": "Any",
    "votingMethod": "Any",
    "sortDescending": false,
    "pageSize": 10,
    "pageIndex": 0,
    "totalRankings": 500
  },
  "rankings": [
    {
      "rank": 1,
      "broadcastDate": "2025-01-01",
      "contestStage": "GrandFinal",
      "broadcastHalf": "First",
      "performingSpot": 1,
      "countryCode": "AA",
      "countryName": "CountryName",
      "actName": "ActName",
      "songTitle": "SongTitle",
      "finishingSpot": 1,
      "pointsAwards": 100,
      "sumTotalPoints": 300,
      "maxPossiblePoints": 1200,
      "pointsFromAllFraction": 0.25
    }
  ]
}
```

## Acceptance criteria

### Happy Path : Baseline

**Endpoint...**

- [ ] Succeeds_and_returns_metadata_and_rankings_given_minimal_query
- [ ] Succeeds_and_returns_metadata_and_rankings_given_explicit_default_values_for_all_optional_params

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

### Sad Path : Bad HTTP Request

**Endpoint...**

- [ ] Fails_when_contestStage_is_invalid_enum_name
- [ ] Fails_when_votingMethod_is_invalid_enum_name

### Sad Path : Invalid Enum Argument

**Endpoint...**

- [ ] Fails_when_contestStage_is_invalid_enum_int_value
- [ ] Fails_when_votingMethod_is_invalid_enum_int_value

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
