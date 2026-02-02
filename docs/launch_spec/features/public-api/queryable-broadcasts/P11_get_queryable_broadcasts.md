# P11. Get queryable broadcasts

This document is part of the [*Eurocentric* launch specification](../../../README.md).

- [P11. Get queryable broadcasts](#p11-get-queryable-broadcasts)
  - [User story](#user-story)
  - [API contract](#api-contract)
    - [HTTP request](#http-request)
    - [HTTP response](#http-response)
  - [Acceptance criteria](#acceptance-criteria)
    - [Happy path](#happy-path)

## User story

- **As a EuroFan**
- **I want** to retrieve all queryable broadcasts in broadcast date order
- **So that** I can understand the scope of the queryable voting data.

## API contract

### HTTP request

```http request
GET /public/api/{apiVersion}/queryable-broadcasts
```

**Notes:**

- `apiVersion` is a major-minor API version URL segment, e.g. `"v1.0"`.

### HTTP response

```http request
200 OK
```

```json
{
  "queryableBroadcasts": [
    {
      "broadcastDate": "2025-05-17",
      "contestStage": "GrandFinal",
      "votingFormat": "TelevoteAndJury",
      "competingCountryCodes": [
        "AA",
        "BB"
      ],
      "votingCountryCodes": [
        "AA",
        "BB"
      ]
    }
  ]
}
```

**Notes:**

- `queryableBroadcasts` is ordered by `queryableBroadcasts.broadcastDate`.
- `queryableBroadcast.competingCountryCodes` is ordered alphabetically.
- `queryableBroadcast.votingCountryCodes` is ordered alphabetically.

## Acceptance criteria

### Happy path

**GetQueryableBroadcasts endpoint...**

- [ ] Should_succeed_with_200_OK_and_all_queryable_broadcasts_in_broadcast_date_order
- [ ] Should_succeed_with_empty_queryable_broadcasts_list_when_no_queryable_voting_data_exists
- [ ] Should_succeed_with_empty_queryable_broadcasts_list_when_system_contains_no_data
