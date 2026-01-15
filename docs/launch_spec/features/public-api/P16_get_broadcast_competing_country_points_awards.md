# P16 Get broadcast competing country points awards

This document is part of the [*Eurocentric* launch specification](../../README.md).

**Feature scope:** *public-api*

## User story

- **As a EuroFan**
- **I want** to list all the points awards received by a specific competing country in a specific broadcast, receiving the query metadata and a list of points awards ordered by voting country code, then by voting method (`Televote` &lt; `Jury`)
- **So that** I can see exactly where the competing country's points came from

## API contract

### HTTP request

```http request
GET /public/api/{apiVersion}/points-award/broadcast-competing-countries
```

**Query parameters:**

| Name                   |     Type      | Required | Details                                                                           |
|:-----------------------|:-------------:|:--------:|:----------------------------------------------------------------------------------|
| `contestYear`          |      int      |   yes    | Sets the contest year. Must be &geq; 2016.                                        |
| `contestStage`         | string (enum) |   yes    | Sets the contest stage. Enum values are `{ GrandFinal, SemiFinal1, SemiFinal2 }`. |
| `competingCountryCode` |    string     |   yes    | Sets the competing country code. Must be a string of 2 upper-case letters.        |

### HTTP response

```http request
200 OK
```

```json
{
  "metadata": {
    "contestYear": 2025,
    "contestStage": "GrandFinal",
    "competingCountryCode": "NO",
    "totalItems": 50
  },
  "pointsAwards": [
    {
      "votingCountryCode": "AL",
      "votingMethod": "Televote",
      "pointsValue": 0
    }
  ]
}
```

## Acceptance criteria

### Happy path

**GetCompetingCountryBroadcastPointsAwards endpoint...**

- Should_succeed_with_200_OK_and_metadata_and_ordered_points_awards_when_request_is_valid
- Should_query_with_contestYear_2022_and_contestStage_SemiFinal1_and_competingCountryCode_AT
- Should_query_with_contestYear_2022_and_contestStage_SemiFinal1_and_competingCountryCode_NO
- Should_query_with_contestYear_2022_and_contestStage_SemiFinal1_and_competingCountryCode_CH
- Should_query_with_contestYear_2022_and_contestStage_SemiFinal2_and_competingCountryCode_FI
- Should_query_with_contestYear_2022_and_contestStage_SemiFinal2_and_competingCountryCode_RS
- Should_query_with_contestYear_2022_and_contestStage_SemiFinal2_and_competingCountryCode_SM
- Should_query_with_contestYear_2022_and_contestStage_GrandFinal_and_competingCountryCode_FI
- Should_query_with_contestYear_2022_and_contestStage_GrandFinal_and_competingCountryCode_GB
- Should_query_with_contestYear_2022_and_contestStage_GrandFinal_and_competingCountryCode_RS
- Should_query_with_contestYear_2023_and_contestStage_SemiFinal1_and_competingCountryCode_CH
- Should_query_with_contestYear_2023_and_contestStage_SemiFinal1_and_competingCountryCode_FI
- Should_query_with_contestYear_2023_and_contestStage_SemiFinal1_and_competingCountryCode_RS
- Should_query_with_contestYear_2023_and_contestStage_SemiFinal2_and_competingCountryCode_AT
- Should_query_with_contestYear_2023_and_contestStage_SemiFinal2_and_competingCountryCode_BE
- Should_query_with_contestYear_2023_and_contestStage_SemiFinal2_and_competingCountryCode_SM
- Should_query_with_contestYear_2023_and_contestStage_GrandFinal_and_competingCountryCode_AT
- Should_query_with_contestYear_2023_and_contestStage_GrandFinal_and_competingCountryCode_FI
- Should_query_with_contestYear_2023_and_contestStage_GrandFinal_and_competingCountryCode_GB
- Should_return_empty_results_list_when_no_queryable_data_fits_query_params
- Should_return_empty_results_list_when_no_contests_are_queryable
- Should_return_empty_results_list_when_no_data_exists

### Sad path

**GetCompetingCountryBroadcastPointsAwards endpoint...**

- Should_fail_when_contestYear_is_less_than_2016
- Should_fail_when_contestYear_is_not_provided
- Should_fail_when_contestStage_is_not_provided
- Should_fail_when_competingCountryCode_is_empty
- Should_fail_when_competingCountryCode_is_whitespace
- Should_fail_when_competingCountryCode_is_shorter_than_2_characters
- Should_fail_when_competingCountryCode_is_longer_than_2_characters
- Should_fail_when_competingCountryCode_contains_non_letter_character
- Should_fail_when_competingCountryCode_contains_lowerCase_letter
- Should_fail_when_competingCountryCode_is_not_provided
