# P17 Get broadcast voting country points awards

This document is part of the [*Eurocentric* launch specification](../../README.md).

**Feature scope:** *public-api*

## User story

- **As a EuroFan**
- **I want** to list all the points awards given by a specific voting country in a specific broadcast, receiving the query metadata and a list of points awards ordered by competing country code, then by voting method (`Televote` &lt; `Jury`)
- **So that** I can see exactly where the voting country's points went

## API contract

### HTTP request

```http request
GET /public/api/{apiVersion}/points-award/broadcast-voting-countries
```

**Query parameters:**

| Name                |     Type      | Required | Details                                                                           |
|:--------------------|:-------------:|:--------:|:----------------------------------------------------------------------------------|
| `contestYear`       |      int      |   yes    | Sets the contest year. Must be &geq; 2016.                                        |
| `contestStage`      | string (enum) |   yes    | Sets the contest stage. Enum values are `{ GrandFinal, SemiFinal1, SemiFinal2 }`. |
| `votingCountryCode` |    string     |   yes    | Sets the voting country code. Must be a string of 2 upper-case letters.           |

### HTTP response

```http request
200 OK
```

```json
{
  "metadata": {
    "contestYear": 2025,
    "contestStage": "GrandFinal",
    "votingCountryCode": "NO",
    "totalItems": 50
  },
  "pointsAwards": [
    {
      "competingCountryCode": "AL",
      "votingMethod": "Televote",
      "pointsValue": 0
    }
  ]
}
```

## Acceptance criteria

### Happy path

**GetVotingCountryBroadcastPointsAwards endpoint...**

- Should_succeed_with_200_OK_and_metadata_and_ordered_points_awards_when_request_is_valid
- Should_query_with_contestYear_2022_and_contestStage_SemiFinal1_and_votingCountryCode_AT
- Should_query_with_contestYear_2022_and_contestStage_SemiFinal1_and_votingCountryCode_NO
- Should_query_with_contestYear_2022_and_contestStage_SemiFinal1_and_votingCountryCode_IT
- Should_query_with_contestYear_2022_and_contestStage_SemiFinal2_and_votingCountryCode_FI
- Should_query_with_contestYear_2022_and_contestStage_SemiFinal2_and_votingCountryCode_GB
- Should_query_with_contestYear_2022_and_contestStage_SemiFinal2_and_votingCountryCode_RS
- Should_query_with_contestYear_2022_and_contestStage_GrandFinal_and_votingCountryCode_FI
- Should_query_with_contestYear_2022_and_contestStage_GrandFinal_and_votingCountryCode_GB
- Should_query_with_contestYear_2022_and_contestStage_GrandFinal_and_votingCountryCode_RS
- Should_query_with_contestYear_2023_and_contestStage_SemiFinal1_and_votingCountryCode_FI
- Should_query_with_contestYear_2023_and_contestStage_SemiFinal1_and_votingCountryCode_IT
- Should_query_with_contestYear_2023_and_contestStage_SemiFinal1_and_votingCountryCode_XX
- Should_query_with_contestYear_2023_and_contestStage_SemiFinal2_and_votingCountryCode_AT
- Should_query_with_contestYear_2023_and_contestStage_SemiFinal2_and_votingCountryCode_GB
- Should_query_with_contestYear_2023_and_contestStage_SemiFinal2_and_votingCountryCode_XX
- Should_query_with_contestYear_2023_and_contestStage_GrandFinal_and_votingCountryCode_FI
- Should_query_with_contestYear_2023_and_contestStage_GrandFinal_and_votingCountryCode_GB
- Should_query_with_contestYear_2023_and_contestStage_GrandFinal_and_votingCountryCode_XX
- Should_return_empty_results_list_when_no_queryable_data_fits_query_params
- Should_return_empty_results_list_when_no_contests_are_queryable
- Should_return_empty_results_list_when_no_data_exists

### Sad path

**GetVotingCountryBroadcastPointsAwards endpoint...**

- Should_fail_when_contestYear_is_less_than_2016
- Should_fail_when_contestYear_is_not_provided
- Should_fail_when_contestStage_is_not_provided
- Should_fail_when_votingCountryCode_is_empty
- Should_fail_when_votingCountryCode_is_whitespace
- Should_fail_when_votingCountryCode_is_shorter_than_2_characters
- Should_fail_when_votingCountryCode_is_longer_than_2_characters
- Should_fail_when_votingCountryCode_contains_non_letter_character
- Should_fail_when_votingCountryCode_contains_lowerCase_letter
- Should_fail_when_votingCountryCode_is_not_provided
