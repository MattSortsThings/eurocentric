# P13 Get competing country points similarity rankings

This document is part of the [*Eurocentric* launch specification](../../README.md).

**Feature scope:** *public-api*

## User story

- **As a EuroFan**
- **I want** to rank competing countries by their *points share* metric, i.e. the cosine similarity between the normalized televote and jury points awards the country received across broadcasts, receiving the query metadata and a page of rankings ordered by rank, then by country code
- **So that** I can learn from the rankings, and represent them in a chart, and import them into a spreadsheet or database table

## API contract

### HTTP request

```http request
GET /public/api/{apiVersion}/rankings/competing-countries/points-similarity
```

**Query parameters:**

| Name                |     Type      | Required | Details                                                                                                                                                                                                 |
|:--------------------|:-------------:|:--------:|:--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| `startContestYear`  |      int      |    no    | Filters the queryable voting data by inclusive start contest year. Must be &geq; 2016. Must not be null or greater than `endContestYear` when `endContestYear` is not null. Defaults to 2016.           |
| `endContestYear`    |      int      |    no    | Filters the queryable voting data by inclusive end contest year. Must be &geq; 2016. Must not be null or earlier than `startContestYear` when `startContestYear` is not null. Defaults to current year. |
| `contestStage`      | string (enum) |    no    | Filters the queryable voting data by contest stage(s). Enum values are `{ GrandFinal, SemiFinal1, SemiFinal2 }`. Duplicate values ignored. Defaults to `[ GrandFinal, SemiFinal1, SemiFinal2 ]`.        |
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
    "startContestYear": 2016,
    "endContestYear": 2026,
    "contestStages": [
      "GrandFinal",
      "SemiFinal1",
      "SemiFinal2"
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
      "vectorDimensions": 100,
      "vectorDotProduct": 886.0,
      "televoteVectorLength": 29.899833,
      "juryVectorLength": 32.557641,
      "pointsSimilarity": 0.910148
    }
  ]
}
```

**Notes:**

- `vectorDotProduct`, `televoteVectorLength` and `juryVectorLength` are all decimal values, rounded to 6 decimal places
- `pointsSimilarity` is a decimal value, calculated as `vectorDotProduct` / (`televoteVectorLength` * `juryVectorLength`), rounded to 6 decimal places

## Acceptance criteria

### Happy path

**GetCompetingCountryPointsSimilarityRankings endpoint...**

- Should_succeed_with_200_OK_and_metadata_and_ordered_rankings_when_request_is_valid
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
- Should_apply_rankDirection_DescendingMetric
- Should_apply_rankDirection_AscendingMetric
- Should_apply_custom_pageSize
- Should_apply_custom_pageIndex
- Should_return_empty_rankings_page_when_no_queryable_data_fits_query_params
- Should_return_empty_rankings_page_when_no_contests_are_queryable
- Should_return_empty_rankings_page_when_no_data_exists

### Sad path

**GetCompetingCountryPointsSimilarityRankings endpoint...**

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
