# P19 Get competing country results

This document is part of the [*Eurocentric* launch specification](../../README.md).

**Feature scope:** *public-api*

## User story

- **As a EuroFan**
- **I want** to list all the results for a specific competing country, receiving the query metadata and a list of results ordered by contest year, then by contest stage (`SemiFinal1` &lt; `SemiFinal2` &lt; `GrandFinal`)
- **So that** I can see how all the competitors fared in the broadcast

## API contract

### HTTP request

```http request
GET /public/api/{apiVersion}/results/competing-countries
```

**Query parameters:**

| Name                   |  Type  | Required | Details                                                                    |
|:-----------------------|:------:|:--------:|:---------------------------------------------------------------------------|
| `competingCountryCode` | string |   yes    | Sets the competing country code. Must be a string of 2 upper-case letters. |

### HTTP response

```http request
200 OK
```

```json
{
  "metadata": {
    "competingCountryCode": "NO",
    "totalItems": 26
  },
  "results": [
    {
      "contestYear": 2025,
      "contestStage": "GrandFinal",
      "performingSpot": 1,
      "countryCode": "NO",
      "countryName": "Norway",
      "actName": "Kyle Alessandro",
      "songTitle": "Lighter",
      "televotePoints": 67,
      "televoteRank": 12,
      "juryPoints": 22,
      "juryRank": 23,
      "overallPoints": 89,
      "finishingSpot": 18
    }
  ]
}
```

**Notes:**

- `juryPoints` is null when the broadcast is televote-only
- `juryRank` is null when `juryPoints` is null

## Acceptance criteria

### Happy path

**GetCompetingCountryResults endpoint...**

- Should_succeed_with_200_OK_and_metadata_and_ordered_results_when_request_is_valid
- Should_query_with_competingCountryCode_AT
- Should_query_with_competingCountryCode_CH
- Should_query_with_competingCountryCode_FI
- Should_query_with_competingCountryCode_GB
- Should_query_with_competingCountryCode_IT
- Should_query_with_competingCountryCode_ME
- Should_query_with_competingCountryCode_PT
- Should_query_with_competingCountryCode_RS
- Should_query_with_competingCountryCode_SM
- Should_query_with_competingCountryCode_UA
- Should_return_empty_results_list_when_no_queryable_data_fits_query_params
- Should_return_empty_results_list_when_no_contests_are_queryable
- Should_return_empty_results_list_when_no_data_exists

### Sad path

**GetCompetingCountryResults endpoint...**

- Should_fail_when_competingCountryCode_is_empty
- Should_fail_when_competingCountryCode_is_whitespace
- Should_fail_when_competingCountryCode_is_shorter_than_2_characters
- Should_fail_when_competingCountryCode_is_longer_than_2_characters
- Should_fail_when_competingCountryCode_contains_non_letter_character
- Should_fail_when_competingCountryCode_contains_lowerCase_letter
- Should_fail_when_competingCountryCode_is_not_provided
