# P14 Get queryable contests

This document is part of the [*Eurocentric* launch specification](../../README.md).

**Feature scope:** *public-api*

## User story

- **As a EuroFan**
- **I want** to list all queryable contests, receiving the query metadata and a list of contests ordered by contest year
- **So that** I can get an overview of the Public API's queryable data

## API contract

### HTTP request

```http request
GET /public/api/{apiVersion}/contests
```

### HTTP response

```http request
200 OK
```

```json
{
  "metadata": {
    "totalItems": 9
  },
  "broadcasts": [
    {
      "contestYear": 2025,
      "cityName": "Basel",
      "participants": 37,
      "semiFinalVotingRules": "TelevoteOnly",
      "grandFinalVotingRules": "TelevoteAndJury",
      "usesGlobalTelevote": true
    }
  ]
}
```

## Acceptance criteria

### Happy path

**GetQueryableContests endpoint...**

- Should_succeed_with_200_OK_and_metadata_ordered_contests_when_request_is_valid
- Should_return_empty_contests_list_when_no_contests_are_queryable
- Should_return_empty_contests_list_when_no_data_exists
