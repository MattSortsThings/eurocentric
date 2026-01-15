# P09 Get queryable broadcasts

This document is part of the [*Eurocentric* launch specification](../../README.md).

**Feature scope:** *public-api*

## User story

- **As a EuroFan**
- **I want** to retrieve all queryable broadcasts, receiving the query metadata and a list of broadcasts ordered by broadcast date
- **So that** I can get an overview of the Public API's queryable data

## API contract

### HTTP request

```http request
GET /public/api/{apiVersion}/broadcasts
```

### HTTP response

```http request
200 OK
```

```json
{
  "metadata": {
    "totalItems": 27
  },
  "broadcasts": [
    {
      "broadcastDate": "2025-05-17",
      "contestYear": 2025,
      "contestStage": "GrandFinal",
      "competitors": 26,
      "votingRules": "TelevoteAndJury"
    }
  ]
}
```

## Acceptance criteria

### Happy path

**GetQueryableBroadcasts endpoint...**

- Should_succeed_with_200_OK_and_metadata_and_ordered_broadcasts_when_request_is_valid
- Should_return_empty_broadcasts_list_when_no_contests_are_queryable
- Should_return_empty_broadcasts_list_when_no_data_exists
