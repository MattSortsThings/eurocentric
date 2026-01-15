# P11 Get competing country points in range rankings

This document is part of the [*Eurocentric* launch specification](../../README.md).

**Feature scope:** *public-api*

## User story

- **As a EuroFan**
- **I want** to rank competing countries by their *points in range* metric, i.e. the relative frequency of points awards the country received across broadcasts having a value within a specific range, receiving the query metadata and a page of rankings ordered by rank, then by country code
- **So that** I can learn from the rankings, and represent them in a chart, and import them into a spreadsheet or database table

## API contract

### HTTP request

```http request
GET /public/api/{apiVersion}/rankings/competing-countries/points-in-range
```

**Query parameters:**

| Name                |     Type      | Required | Details                                                                                                                                                                                                 |
|:--------------------|:-------------:|:--------:|:--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| `minPointsValue`    |      int      |    no    | Sets the inclusive minimum points value. Must be &geq; 0. Must not be null or greater than `maxPointsValue` when `maxPointsValue` is not null. Defaults to 1.                                           |
| `maxPointsValue`    |      int      |    no    | Sets the inclusive maximum points value. Must be &geq; 0. Must not be null or smaller than `minPointsValue` when `minPointsValue` is not null. Defaults to 12.                                          |
| `startContestYear`  |      int      |    no    | Filters the queryable voting data by inclusive start contest year. Must be &geq; 2016. Must not be null or greater than `endContestYear` when `endContestYear` is not null. Defaults to 2016.           |
| `endContestYear`    |      int      |    no    | Filters the queryable voting data by inclusive end contest year. Must be &geq; 2016. Must not be null or earlier than `startContestYear` when `startContestYear` is not null. Defaults to current year. |
| `contestStage`      | string (enum) |    no    | Filters the queryable voting data by contest stage(s). Enum values are `{ GrandFinal, SemiFinal1, SemiFinal2 }`. Duplicate values ignored. Defaults to `[ GrandFinal, SemiFinal1, SemiFinal2 ]`.        |
| `votingMethod`      | string (enum) |    no    | Filters the queryable voting data by voting method(s). Enum values are `{ Televote, Jury }`. Duplicate values ignored. Defaults to `[ Televote, Jury ]`.                                                |
| `votingCountryCode` |    string     |    no    | Filters the queryable voting data by competing country code when not null. Must be a string of 2 upper-case letters when not null.                                                                      |
| `rankDirection`     | string (enum) |    no    | Sets the rank direction. Enum values are `{ DescendingMetric, AscendingMetric }`. Defaults to `DescendingMetric`.                                                                                       |
| `pageSize`          |      int      |    no    | Sets the pagination page size. Must be in range \[1, 100\]. Defaults to 10.                                                                                                                             |
| `pageIndex`         |      int      |    no    | Sets the pagination page index. Must not be negative. Defaults to 0.                                                                                                                                    |

### HTTP response

```http request
200 OK
```

```json
{
  "metadata": {
    "minPointsValue": 1,
    "maxPointsValue": 12,
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
    "votingCountryCode": null,
    "rankDirection": "DescendingMetric",
    "pageSize": 10,
    "pageIndex": 0,
    "totalItems": 40
  },
  "rankings": [
    {
      "rank": 1,
      "countryCode": "AT",
      "countryName": "Austria",
      "contests": 3,
      "broadcasts": 4,
      "votingCountries": 25,
      "pointsAwards": 150,
      "pointsAwardsInRange": 140,
      "pointsInRange": 0.5
    }
  ]
}
```

**Notes:**

- `pointsInRange` is a decimal value, calculated as `pointsAwardsInRange` / `pointsAwards`, rounded to 6 decimal places

## Acceptance criteria

### Happy path

**GetCompetingCountryPointsInRangeRankings endpoint...**

- Should_succeed_with_200_OK_and_metadata_and_ordered_rankings_when_request_is_valid
- Should_query_with_minPointsValue_1_and_maxPointsValue_12
- Should_query_with_minPointsValue_0_and_maxPointsValue_0
- Should_query_with_minPointsValue_8_and_maxPointsValue_12
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
- Should_query_with_votingCountryCode_AT
- Should_query_with_votingCountryCode_CH
- Should_query_with_votingCountryCode_GB
- Should_query_with_votingCountryCode_FI
- Should_query_with_votingCountryCode_IT
- Should_query_with_votingCountryCode_ME
- Should_query_with_votingCountryCode_PT
- Should_query_with_votingCountryCode_RS
- Should_query_with_votingCountryCode_SM
- Should_query_with_votingCountryCode_UA
- Should_query_with_votingCountryCode_XX
- Should_apply_rankDirection_DescendingMetric
- Should_apply_rankDirection_AscendingMetric
- Should_apply_custom_pageSize
- Should_apply_custom_pageIndex
- Should_return_empty_rankings_page_when_no_queryable_data_fits_query_params
- Should_return_empty_rankings_page_when_no_contests_are_queryable
- Should_return_empty_rankings_page_when_no_data_exists

### Sad path

**GetCompetingCountryPointsInRangeRankings endpoint...**

- Should_fail_when_minPointsValue_is_less_than_0
- Should_fail_when_maxPointsValue_is_less_than_0
- Should_fail_when_minPointsValue_is_provided_without_maxPointsValue
- Should_fail_when_maxPointsValue_is_provided_without_minPointsValue
- Should_fail_when_minPointsValue_is_greater_than_maxPointsValue
- Should_fail_when_startContestYear_is_less_than_2016
- Should_fail_when_endContestYear_is_less_than_2016
- Should_fail_when_startContestYear_is_provided_without_endContestYear
- Should_fail_when_endContestYear_is_provided_without_startContestYear
- Should_fail_when_startContestYear_is_greater_than_endContestYear
- Should_fail_when_votingCountryCode_is_empty
- Should_fail_when_votingCountryCode_is_whitespace
- Should_fail_when_votingCountryCode_is_shorter_than_2_characters
- Should_fail_when_votingCountryCode_is_longer_than_2_characters
- Should_fail_when_votingCountryCode_contains_non_letter_character
- Should_fail_when_votingCountryCode_contains_lowerCase_letter
- Should_fail_when_pageSize_is_less_than_1
- Should_fail_when_pageSize_is_greater_than_100
- Should_fail_when_pageIndex_is_less_than_0
