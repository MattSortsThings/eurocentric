# P07 get broadcast segment points share rankings

This document is part of the [*Eurocentric* launch specification](../../README.md).

**Feature scope:** *public-api*

## User story

- **As a EuroFan**
- **I want** to rank broadcast running order segments by their *points share* metric, i.e. the fraction of the available points received by competitors performing in the segment across broadcasts, receiving the query metadata and the rankings ordered by rank, then by segment
- **So that** I can learn from the rankings, and represent them in a chart, and import them into a spreadsheet or database table

## API contract

### HTTP request

```http request
GET /public/api/{apiVersion}/rankings/broadcast-segments/points-share
```

**Query parameters:**

| Name                |     Type      | Required | Details                                                                                                                                                                                                 |
|:--------------------|:-------------:|:--------:|:--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| `broadcastSegments` |      int      |    no    | Sets the number of broadcast segments. Must be an even number in range \[2, 10\]. Defaults to 4.                                                                                                        |
| `startContestYear`  |      int      |    no    | Filters the queryable voting data by inclusive start contest year. Must be &geq; 2016. Must not be null or greater than `endContestYear` when `endContestYear` is not null. Defaults to 2016.           |
| `endContestYear`    |      int      |    no    | Filters the queryable voting data by inclusive end contest year. Must be &geq; 2016. Must not be null or earlier than `startContestYear` when `startContestYear` is not null. Defaults to current year. |
| `contestStage`      | string (enum) |    no    | Filters the queryable voting data by contest stage(s). Enum values are `{ GrandFinal, SemiFinal1, SemiFinal2 }`. Duplicate values ignored. Defaults to `[ GrandFinal, SemiFinal1, SemiFinal2 ]`.        |
| `votingMethod`      | string (enum) |    no    | Filters the queryable voting data by voting method(s). Enum values are `{ Televote, Jury }`. Duplicate values ignored. Defaults to `[ Televote, Jury ]`.                                                |
| `rankDirection`     | string (enum) |    no    | Sets the rank direction. Enum values are `{ DescendingMetric, AscendingMetric }`. Defaults to `DescendingMetric`.                                                                                       |

### HTTP response

```http request
200 OK
```

```json
{
  "metadata": {
    "broadcastSegments": 4,
    "startContestYear": 2016,
    "endContestYear": 2026,
    "contestStages": [
      "GrandFinal",
      "SemiFinal1",
      "SemiFinal2"
    ],
    "votingMethods": [
      "Televote",
      "Jury"
    ],
    "rankDirection": "DescendingMetric",
    "totalItems": 4
  },
  "rankings": [
    {
      "rank": 1,
      "broadcastSegment": 2,
      "broadcastHalf": 1,
      "contests": 3,
      "broadcasts": 6,
      "votingCountries": 35,
      "pointsAwards": 70,
      "totalPoints": 385,
      "availablePoints": 840,
      "pointsShare": 0.458333
    }
  ]
}
```

**Notes:**

- `pointsShare` is a decimal value, calculated as `totalPoints` / `availablePoints`, rounded to 6 decimal places

## Acceptance criteria

### Happy path

**GetBroadcastSegmentPointsShareRankings endpoint...**

- Should_succeed_with_200_OK_and_metadata_and_ordered_rankings_when_request_is_valid
- Should_query_with_broadcastSegments_2
- Should_query_with_broadcastSegments_3
- Should_query_with_broadcastSegments_6
- Should_query_with_broadcastSegments_10
- Should_filter_queryable_data_by_startContestYear_2023_and_endContestYear_2023
- Should_filter_queryable_data_by_startContestYear_2016_and_endContestYear_2022
- Should_filter_queryable_data_by_contestStage_GrandFinal
- Should_filter_queryable_data_by_contestStage_SemiFinal1
- Should_filter_queryable_data_by_contestStage_SemiFinal2
- Should_filter_queryable_data_by_contestStage_GrandFinal_and_SemiFinal1
- Should_filter_queryable_data_by_contestStage_GrandFinal_and_SemiFinal2
- Should_filter_queryable_data_by_contestStage_GrandFinal_and_SemiFinal1_and_SemiFinal2
- Should_filter_queryable_data_by_contestStage_omitting_duplicates
- Should_filter_queryable_data_by_votingMethod_Televote
- Should_filter_queryable_data_by_votingMethod_Jury
- Should_filter_queryable_data_by_votingMethod_Televote_and_Jury
- Should_filter_queryable_data_by_votingMethod_omitting_duplicates
- Should_apply_rankDirection_DescendingMetric
- Should_apply_rankDirection_AscendingMetric
- Should_return_empty_rankings_page_when_no_queryable_data_fits_query_params
- Should_return_empty_rankings_page_when_no_contests_are_queryable
- Should_return_empty_rankings_page_when_no_data_exists

### Sad path

**GetBroadcastSegmentPointsShareRankings endpoint...**

- Should_fail_when_broadcastSegments_is_less_than_2
- Should_fail_when_broadcastSegments_is_greater_than_10
- Should_fail_when_broadcastSegments_is_odd
- Should_fail_when_startContestYear_is_less_than_2016
- Should_fail_when_endContestYear_is_less_than_2016
- Should_fail_when_startContestYear_is_provided_without_endContestYear
- Should_fail_when_endContestYear_is_provided_without_startContestYear
- Should_fail_when_startContestYear_is_greater_than_endContestYear
