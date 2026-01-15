# P18 Get broadcast results

This document is part of the [*Eurocentric* launch specification](../../README.md).

**Feature scope:** *public-api*

## User story

- **As a EuroFan**
- **I want** to list all the results for a specific broadcast, receiving the query metadata and a list of results ordered by performing spot
- **So that** I can see how all the competitors fared in the broadcast

## API contract

### HTTP request

```http request
GET /public/api/{apiVersion}/results/broadcasts
```

**Query parameters:**

| Name           |     Type      | Required | Details                                                                           |
|:---------------|:-------------:|:--------:|:----------------------------------------------------------------------------------|
| `contestYear`  |      int      |   yes    | Sets the contest year. Must be &geq; 2016.                                        |
| `contestStage` | string (enum) |   yes    | Sets the contest stage. Enum values are `{ GrandFinal, SemiFinal1, SemiFinal2 }`. |

### HTTP response

```http request
200 OK
```

```json
{
  "metadata": {
    "contestYear": 2025,
    "contestStage": "GrandFinal",
    "totalItems": 26
  },
  "results": [
    {
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

**GetBroadcastResults endpoint...**

- Should_succeed_with_200_OK_and_metadata_and_ordered_results_when_request_is_valid
- Should_query_with_contestYear_2022_and_contestStage_SemiFinal1
- Should_query_with_contestYear_2022_and_contestStage_SemiFinal2
- Should_query_with_contestYear_2022_and_contestStage_GrandFinal
- Should_query_with_contestYear_2023_and_contestStage_SemiFinal1
- Should_query_with_contestYear_2023_and_contestStage_SemiFinal2
- Should_query_with_contestYear_2023_and_contestStage_GrandFinal
- Should_return_empty_results_list_when_no_queryable_data_fits_query_params
- Should_return_empty_results_list_when_no_contests_are_queryable
- Should_return_empty_results_list_when_no_data_exists

### Sad path

**GetBroadcastResults endpoint...**

- Should_fail_when_contestYear_is_less_than_2016
- Should_fail_when_contestYear_is_not_provided
- Should_fail_when_contestStage_is_not_provided
