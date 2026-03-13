# P19. Get broadcast voter points awards

This document is part of the [launch specification](../../../README.md).

## User story

- **As a EuroFan**
- **I want** to retrieve all points awards given by a specified broadcast voter, ordered by competing country code then by voting method
- **So that** I can see how the voting country's jury and/or televote voted for every competitor

## API contract

### HTTP request

```http request
GET /public/api/{apiVersion}/points-awards/broadcast-voter
```

**Required query parameters:**

| Name                |     Type      | Details                                                                                                  |
|:--------------------|:-------------:|:---------------------------------------------------------------------------------------------------------|
| `contestYear`       |      int      | Specifies the broadcast voter's contest year. Must be an integer between 2016 and 2030.                  |
| `contestStage`      | string (enum) | Specifies the broadcast voter's contest stage. Enum values are `{ SemiFinal1, SemiFinal2, GrandFinal }`. |
| `votingCountryCode` |    string     | Specifies the broadcast voter's country code. Must be a string of 2 upper-case ASCII letters.            |

### HTTP response

```http request
200 OK
```

```json
{
  "metadata": {
    "contestYear": 2025,
    "contestStage": "GrandFinal",
    "votingCountryCode": "ZZ",
    "totalPointsAwards": 100
  },
  "pointsAwards": [
    {
      "competingCountryCode": "AA",
      "votingMethod": "Jury",
      "pointsValue": 12
    }
  ]
}
```

## Acceptance criteria

### Happy Path : Baseline

**Endpoint...**

- [ ] Succeeds_with_metadata_and_pointsAwards_for_specified_broadcast_voter

### Happy Path : Variants : Required Params

**Endpoint...**

- [ ] Succeeds_when_specifying_contestYear_2022_contestStage_SemiFinal1_votingCountryCode_LV
- [ ] Succeeds_when_specifying_contestYear_2022_contestStage_SemiFinal1_votingCountryCode_FR
- [ ] Succeeds_when_specifying_contestYear_2022_contestStage_SemiFinal2_votingCountryCode_SM
- [ ] Succeeds_when_specifying_contestYear_2022_contestStage_SemiFinal2_votingCountryCode_GB
- [ ] Succeeds_when_specifying_contestYear_2022_contestStage_GrandFinal_votingCountryCode_LV
- [ ] Succeeds_when_specifying_contestYear_2022_contestStage_GrandFinal_votingCountryCode_SM
- [ ] Succeeds_when_specifying_contestYear_2022_contestStage_GrandFinal_votingCountryCode_FR
- [ ] Succeeds_when_specifying_contestYear_2022_contestStage_GrandFinal_votingCountryCode_GB

- [ ] Succeeds_when_specifying_contestYear_2023_contestStage_SemiFinal1_votingCountryCode_LV
- [ ] Succeeds_when_specifying_contestYear_2023_contestStage_SemiFinal1_votingCountryCode_FR
- [ ] Succeeds_when_specifying_contestYear_2023_contestStage_SemiFinal1_votingCountryCode_XX
- [ ] Succeeds_when_specifying_contestYear_2023_contestStage_SemiFinal2_votingCountryCode_SM
- [ ] Succeeds_when_specifying_contestYear_2023_contestStage_SemiFinal2_votingCountryCode_GB
- [ ] Succeeds_when_specifying_contestYear_2023_contestStage_SemiFinal2_votingCountryCode_XX
- [ ] Succeeds_when_specifying_contestYear_2023_contestStage_GrandFinal_votingCountryCode_LV
- [ ] Succeeds_when_specifying_contestYear_2023_contestStage_GrandFinal_votingCountryCode_SM
- [ ] Succeeds_when_specifying_contestYear_2023_contestStage_GrandFinal_votingCountryCode_FR
- [ ] Succeeds_when_specifying_contestYear_2023_contestStage_GrandFinal_votingCountryCode_GB
- [ ] Succeeds_when_specifying_contestYear_2023_contestStage_GrandFinal_votingCountryCode_XX

### Happy Path : Zero Points Awards

**Endpoint...**

- [ ] Succeeds_with_0_pointsAwards_when_broadcast_does_not_exist
- [ ] Succeeds_with_0_pointsAwards_when_broadcast_is_not_queryable
- [ ] Succeeds_with_0_pointsAwards_when_votingCountryCode_matches_no_broadcast_voter

### Sad Path : Bad HTTP Request

**Endpoint...**

- [ ] Fails_when_contestYear_is_not_provided
- [ ] Fails_when_contestStage_is_not_provided
- [ ] Fails_when_votingCountryCode_is_not_provided
- [ ] Fails_when_contestStage_is_invalid_enum_name

### Sad Path : Invalid Enum Argument

**Endpoint...**

- [ ] Fails_when_contestStage_is_invalid_enum_int_value

### Sad Path : Illegal Country Code

**Endpoint...**

- [ ] Fails_when_votingCountryCode_is_empty_string
- [ ] Fails_when_votingCountryCode_is_shorter_than_2_chars
- [ ] Fails_when_votingCountryCode_is_longer_than_2_chars
- [ ] Fails_when_votingCountryCode_contains_non_upper_case_ASCII_letter_char
