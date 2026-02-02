# P09. Get broadcast competing country points awards

This document is part of the [*Eurocentric* launch specification](../../../README.md).

- [P09. Get broadcast competing country points awards](#p09-get-broadcast-competing-country-points-awards)
  - [User story](#user-story)
  - [API contract](#api-contract)
    - [HTTP request](#http-request)
    - [HTTP response](#http-response)
  - [Acceptance criteria](#acceptance-criteria)
    - [Happy path](#happy-path)
    - [Sad path](#sad-path)

## User story

- **As a EuroFan**
- **I want** to retrieve all the points awards received by a specific competing country in a specific broadcast
- **So that** I can see exactly how every voting country voted for the competing country.

## API contract

### HTTP request

```http request
GET /public/api/{apiVersion}/points-awards/broadcast-competing-countries
```

**Notes:**

- `apiVersion` is a major-minor API version URL segment, e.g. `"v1.0"`.

**Required query parameters:**

| Name                   |     Type      | Details                                                                                |
|:-----------------------|:-------------:|:---------------------------------------------------------------------------------------|
| `contestYear`          |      int      | Specifies the contest year. Must be integer between 2016 and 2050.                     |
| `contestStage`         | string (enum) | Specifies the contest stage. Enum values are `{ SemiFinal1, SemiFinal2, GrandFinal }`. |
| `competingCountryCode` |    string     | Specifies the competing country code. Must be string of 2 upper-case ASCII letters.    |

### HTTP response

```http request
200 OK
```

```json
{
  "metadata": {
    "contestYear": 2025,
    "contestStage": "GrandFinal",
    "competingCountryCode": "ZZ"
  },
  "televotePointsAwards": [
    {
      "votingCountryCode": "AA",
      "pointsValue": 12
    }
  ],
  "juryPointsAwards": [
    {
      "votingCountryCode": "AA",
      "pointsValue": 12
    }
  ]
}
```

**Notes:**

- `televotePointsAwards` is sorted by `receivedTelevotePointsAward.votingCountryCode`.
- `juryPointsAwards` is sorted by `receivedJuryPointsAward.votingCountryCode`.

## Acceptance criteria

### Happy path

**GetBroadcastCompetingCountryPointsAwards endpoint...**

- [ ] Should_succeed_with_200_OK_and_metadata_and_ordered_points_awards_when_query_is_valid
- [ ] Should_succeed_when_querying_contestYear_2022_contestStage_SemiFinal1_competingCountryCode_CH
- [ ] Should_succeed_when_querying_contestYear_2022_contestStage_SemiFinal1_competingCountryCode_SI
- [ ] Should_succeed_when_querying_contestYear_2022_contestStage_SemiFinal2_competingCountryCode_FI
- [ ] Should_succeed_when_querying_contestYear_2022_contestStage_SemiFinal2_competingCountryCode_MK
- [ ] Should_succeed_when_querying_contestYear_2022_contestStage_GrandFinal_competingCountryCode_FI
- [ ] Should_succeed_when_querying_contestYear_2022_contestStage_GrandFinal_competingCountryCode_GB
- [ ] Should_succeed_when_querying_contestYear_2023_contestStage_SemiFinal1_competingCountryCode_CH
- [ ] Should_succeed_when_querying_contestYear_2023_contestStage_SemiFinal1_competingCountryCode_FI
- [ ] Should_succeed_when_querying_contestYear_2023_contestStage_SemiFinal2_competingCountryCode_SI
- [ ] Should_succeed_when_querying_contestYear_2023_contestStage_SemiFinal2_competingCountryCode_SM
- [ ] Should_succeed_when_querying_contestYear_2023_contestStage_GrandFinal_competingCountryCode_FI
- [ ] Should_succeed_when_querying_contestYear_2023_contestStage_GrandFinal_competingCountryCode_GB
- [ ] Should_succeed_with_empty_points_awards_lists_when_query_excludes_all_queryable_voting_data
- [ ] Should_succeed_with_empty_points_awards_lists_when_no_queryable_voting_data_exists
- [ ] Should_succeed_with_empty_points_awards_lists_when_system_contains_no_data

### Sad path

**GetBroadcastCompetingCountryPointsAwards endpoint...**

- [ ] Should_fail_when_competingCountryCode_is_not_provided
- [ ] Should_fail_when_competingCountryCode_is_empty_or_whitespace
- [ ] Should_fail_when_competingCountryCode_length_is_not_2_chars
- [ ] Should_fail_when_competingCountryCode_contains_non_ASCII_letter_upper_char
