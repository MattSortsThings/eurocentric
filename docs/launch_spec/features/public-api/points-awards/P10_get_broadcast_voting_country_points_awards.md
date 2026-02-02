# P10. Get broadcast voting country points awards

This document is part of the [*Eurocentric* launch specification](../../../README.md).

- [P10. Get broadcast voting country points awards](#p10-get-broadcast-voting-country-points-awards)
  - [User story](#user-story)
  - [API contract](#api-contract)
    - [HTTP request](#http-request)
    - [HTTP response](#http-response)
  - [Acceptance criteria](#acceptance-criteria)
    - [Happy path](#happy-path)
    - [Sad path](#sad-path)

## User story

- **As a EuroFan**
- **I want** to retrieve all the points awards given by a specific voting country in a specific broadcast
- **So that** I can see exactly how the voting country voted for every competing country.

## API contract

### HTTP request

```http request
GET /public/api/{apiVersion}/points-awards/broadcast-voting-countries
```

**Notes:**

- `apiVersion` is a major-minor API version URL segment, e.g. `"v1.0"`.

**Required query parameters:**

| Name                |     Type      | Details                                                                                |
|:--------------------|:-------------:|:---------------------------------------------------------------------------------------|
| `contestYear`       |      int      | Specifies the contest year. Must be integer between 2016 and 2050.                     |
| `contestStage`      | string (enum) | Specifies the contest stage. Enum values are `{ SemiFinal1, SemiFinal2, GrandFinal }`. |
| `votingCountryCode` |    string     | Specifies the voting country code. Must be string of 2 upper-case ASCII letters.       |

### HTTP response

```http request
200 OK
```

```json
{
  "metadata": {
    "contestYear": 2025,
    "contestStage": "GrandFinal",
    "votingCountryCode": "ZZ"
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

- `televotePointsAwards` is sorted by `givenTelevotePointsAward.votingCountryCode`.
- `juryPointsAwards` is sorted by `givenJuryPointsAward.votingCountryCode`.

## Acceptance criteria

### Happy path

**GetBroadcastVotingCountryPointsAwards endpoint...**

- [ ] Should_succeed_with_200_OK_and_metadata_and_ordered_points_awards_when_query_is_valid
- [ ] Should_succeed_when_querying_contestYear_2022_contestStage_SemiFinal1_votingCountryCode_CH
- [ ] Should_succeed_when_querying_contestYear_2022_contestStage_SemiFinal1_votingCountryCode_FR
- [ ] Should_succeed_when_querying_contestYear_2022_contestStage_SemiFinal2_votingCountryCode_FI
- [ ] Should_succeed_when_querying_contestYear_2022_contestStage_SemiFinal2_votingCountryCode_MK
- [ ] Should_succeed_when_querying_contestYear_2022_contestStage_GrandFinal_votingCountryCode_CH
- [ ] Should_succeed_when_querying_contestYear_2022_contestStage_GrandFinal_votingCountryCode_GB
- [ ] Should_succeed_when_querying_contestYear_2023_contestStage_SemiFinal1_votingCountryCode_DE
- [ ] Should_succeed_when_querying_contestYear_2023_contestStage_SemiFinal1_votingCountryCode_FI
- [ ] Should_succeed_when_querying_contestYear_2023_contestStage_SemiFinal1_votingCountryCode_XX
- [ ] Should_succeed_when_querying_contestYear_2023_contestStage_SemiFinal2_votingCountryCode_GB
- [ ] Should_succeed_when_querying_contestYear_2023_contestStage_SemiFinal2_votingCountryCode_SM
- [ ] Should_succeed_when_querying_contestYear_2023_contestStage_SemiFinal2_votingCountryCode_XX
- [ ] Should_succeed_when_querying_contestYear_2023_contestStage_GrandFinal_votingCountryCode_FI
- [ ] Should_succeed_when_querying_contestYear_2023_contestStage_GrandFinal_votingCountryCode_GB
- [ ] Should_succeed_when_querying_contestYear_2023_contestStage_GrandFinal_votingCountryCode_SM
- [ ] Should_succeed_when_querying_contestYear_2023_contestStage_GrandFinal_votingCountryCode_XX
- [ ] Should_succeed_with_empty_points_awards_lists_when_query_excludes_all_queryable_voting_data
- [ ] Should_succeed_with_empty_points_awards_lists_when_no_queryable_voting_data_exists
- [ ] Should_succeed_with_empty_points_awards_lists_when_system_contains_no_data

### Sad path

**GetBroadcastVotingCountryPointsAwards endpoint...**

- [ ] Should_fail_when_votingCountryCode_is_not_provided
- [ ] Should_fail_when_votingCountryCode_is_empty_or_whitespace
- [ ] Should_fail_when_votingCountryCode_length_is_not_2_chars
- [ ] Should_fail_when_votingCountryCode_contains_non_ASCII_letter_upper_char
