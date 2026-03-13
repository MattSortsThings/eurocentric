# P18. Get broadcast competitor points awards

This document is part of the [launch specification](../../../README.md).

## User story

- **As a EuroFan**
- **I want** to retrieve all points awards received by a specified broadcast competitor, ordered by voting country code then by voting method
- **So that** I can see how every jury and televote voted for the competitor

## API contract

### HTTP request

```http request
GET /public/api/{apiVersion}/points-awards/broadcast-competitor
```

**Required query parameters:**

| Name                   |     Type      | Details                                                                                                       |
|:-----------------------|:-------------:|:--------------------------------------------------------------------------------------------------------------|
| `contestYear`          |      int      | Specifies the broadcast competitor's contest year. Must be an integer between 2016 and 2030.                  |
| `contestStage`         | string (enum) | Specifies the broadcast competitor's contest stage. Enum values are `{ SemiFinal1, SemiFinal2, GrandFinal }`. |
| `competingCountryCode` |    string     | Specifies the broadcast competitor's country code. Must be a string of 2 upper-case ASCII letters.            |

### HTTP response

```http request
200 OK
```

```json
{
  "metadata": {
    "contestYear": 2025,
    "contestStage": "GrandFinal",
    "competingCountryCode": "ZZ",
    "totalPointsAwards": 100
  },
  "pointsAwards": [
    {
      "votingCountryCode": "AA",
      "votingMethod": "Jury",
      "pointsValue": 12
    }
  ]
}
```

## Acceptance criteria

### Happy Path : Baseline

**Endpoint...**

- [ ] Succeeds_with_metadata_and_pointsAwards_for_specified_broadcast_competitor

### Happy Path : Variants : Required Params

**Endpoint...**

- [ ] Succeeds_when_specifying_contestYear_2022_contestStage_SemiFinal1_competingCountryCode_LV
- [ ] Succeeds_when_specifying_contestYear_2022_contestStage_SemiFinal1_competingCountryCode_NO
- [ ] Succeeds_when_specifying_contestYear_2022_contestStage_SemiFinal2_competingCountryCode_SM
- [ ] Succeeds_when_specifying_contestYear_2022_contestStage_SemiFinal2_competingCountryCode_BE
- [ ] Succeeds_when_specifying_contestYear_2022_contestStage_GrandFinal_competingCountryCode_NO
- [ ] Succeeds_when_specifying_contestYear_2022_contestStage_GrandFinal_competingCountryCode_BE
- [ ] Succeeds_when_specifying_contestYear_2022_contestStage_GrandFinal_competingCountryCode_GB
- [ ] Succeeds_when_specifying_contestYear_2023_contestStage_SemiFinal1_competingCountryCode_LV
- [ ] Succeeds_when_specifying_contestYear_2023_contestStage_SemiFinal1_competingCountryCode_NO
- [ ] Succeeds_when_specifying_contestYear_2023_contestStage_SemiFinal2_competingCountryCode_SM
- [ ] Succeeds_when_specifying_contestYear_2023_contestStage_SemiFinal2_competingCountryCode_BE
- [ ] Succeeds_when_specifying_contestYear_2023_contestStage_GrandFinal_competingCountryCode_NO
- [ ] Succeeds_when_specifying_contestYear_2023_contestStage_GrandFinal_competingCountryCode_BE
- [ ] Succeeds_when_specifying_contestYear_2023_contestStage_GrandFinal_competingCountryCode_GB

### Happy Path : Zero Points Awards

**Endpoint...**

- [ ] Succeeds_with_0_pointsAwards_when_broadcast_does_not_exist
- [ ] Succeeds_with_0_pointsAwards_when_broadcast_is_not_queryable
- [ ] Succeeds_with_0_pointsAwards_when_competingCountryCode_matches_no_broadcast_competitor

### Sad Path : Bad HTTP Request

**Endpoint...**

- [ ] Fails_when_contestYear_is_not_provided
- [ ] Fails_when_contestStage_is_not_provided
- [ ] Fails_when_competingCountryCode_is_not_provided
- [ ] Fails_when_contestStage_is_invalid_enum_name

### Sad Path : Invalid Enum Argument

**Endpoint...**

- [ ] Fails_when_contestStage_is_invalid_enum_int_value

### Sad Path : Illegal Country Code

**Endpoint...**

- [ ] Fails_when_competingCountryCode_is_empty_string
- [ ] Fails_when_competingCountryCode_is_shorter_than_2_chars
- [ ] Fails_when_competingCountryCode_is_longer_than_2_chars
- [ ] Fails_when_competingCountryCode_contains_non_upper_case_ASCII_letter_char
