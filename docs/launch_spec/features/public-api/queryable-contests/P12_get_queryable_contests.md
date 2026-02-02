# P12. Get queryable contests

This document is part of the [*Eurocentric* launch specification](../../../README.md).

- [P12. Get queryable contests](#p12-get-queryable-contests)
  - [User story](#user-story)
  - [API contract](#api-contract)
    - [HTTP request](#http-request)
    - [HTTP response](#http-response)
  - [Acceptance criteria](#acceptance-criteria)
    - [Happy path](#happy-path)

## User story

- **As a EuroFan**
- **I want** to retrieve all queryable contests in contest year order
- **So that** I can understand the scope of the queryable voting data.

## API contract

### HTTP request

```http request
GET /public/api/{apiVersion}/queryable-contests
```

**Notes:**

- `apiVersion` is a major-minor API version URL segment, e.g. `"v1.0"`.

### HTTP response

```http request
200 OK
```

```json
{
  "queryableContests": [
    {
      "contestYear": 2025,
      "cityName": "City Name",
      "semiFinalVotingFormat": "TelevoteOnly",
      "grandFinalVotingFormat": "TelevoteAndJury",
      "globalTelevoteCountryCode": "XX",
      "participatingCountryCodes": [
        "AA",
        "BB"
      ]
    }
  ]
}
```

**Notes:**

- `queryableContests` is ordered by `queryableContest.contestYear`.
- `queryableContest.globalTelevoteCountryCode` may be null.
- `queryableContest.participatingCountryCodes` is ordered alphabetically.

## Acceptance criteria

### Happy path

**GetQueryableContests endpoint...**

- [ ] Should_succeed_with_200_OK_and_all_queryable_contests_in_contest_year_order
- [ ] Should_succeed_with_empty_queryable_contests_list_when_no_queryable_voting_data_exists
- [ ] Should_succeed_with_empty_queryable_contests_list_when_system_contains_no_data
