# P23. Get broadcast scoreboard rows

This document is part of the [launch specification](../../../README.md).

## User story

- **As a EuroFan**
- **I want** to retrieve all scoreboard rows for a specified broadcast, ordered by performing spot
- **So that** I can see all the competitors and the results they achieved

## API contract

### HTTP request

```http request
GET /public/api/{apiVersion}/scoreboard-rows/broadcast
```

**Required query parameters:**

| Name           |     Type      | Details                                                                                            |
|:---------------|:-------------:|:---------------------------------------------------------------------------------------------------|
| `contestYear`  |      int      | Specifies the broadcast's contest year. Must be an integer between 2016 and 2030.                  |
| `contestStage` | string (enum) | Specifies the broadcast's contest stage. Enum values are `{ SemiFinal1, SemiFinal2, GrandFinal }`. |

### HTTP response

```http request
200 OK
```

```json
{
  "metadata": {
    "contestYear": 2025,
    "contestStage": "GrandFinal",
    "totalScoreboardRows": 26
  },
  "scoreboardRows": [
    {
      "performingSpot": 1,
      "broadcastHalf": "First",
      "countryCode": "AA",
      "countryName": "CountryName",
      "actName": "ActName",
      "songTitle": "SongTitle",
      "juryPoints": 100,
      "juryRank": 2,
      "televotePoints": 150,
      "televoteRank": 1,
      "overallPoints": 250,
      "finishingSpot": 1
    }
  ]
}
```

**Notes:**

- `scoreboardRows`.`juryPoints` may be `null`
- `scoreboardRows`.`juryRank` may be `null`

## Acceptance criteria

### Happy Path : Baseline

**Endpoint...**

- [ ] Succeeds_with_metadata_and_scoreboardRows_for_specified_broadcast

### Happy Path : Variants : Required Params

**Endpoint...**

- [ ] Succeeds_when_specifying_contestYear_2022_contestStage_SemiFinal1
- [ ] Succeeds_when_specifying_contestYear_2022_contestStage_SemiFinal2
- [ ] Succeeds_when_specifying_contestYear_2022_contestStage_GrandFinal
- [ ] Succeeds_when_specifying_contestYear_2023_contestStage_SemiFinal1
- [ ] Succeeds_when_specifying_contestYear_2023_contestStage_SemiFinal2
- [ ] Succeeds_when_specifying_contestYear_2023_contestStage_GrandFinal

### Happy Path : Zero Scoreboard Rows

**Endpoint...**

- [ ] Succeeds_with_0_scoreboardRows_when_broadcast_does_not_exist
- [ ] Succeeds_with_0_scoreboardRows_when_broadcast_is_not_queryable

### Sad Path : Bad HTTP Request

**Endpoint...**

- [ ] Fails_when_contestYear_is_not_provided
- [ ] Fails_when_contestStage_is_not_provided
- [ ] Fails_when_contestStage_is_invalid_enum_name

### Sad Path : Invalid Enum Argument

**Endpoint...**

- [ ] Fails_when_contestStage_is_invalid_enum_int_value

### Sad Path : Illegal Contest Year

**Endpoint...**

- [ ] Fails_when_contestYear_is_less_than_2016
- [ ] Fails_when_contestYear_is_greater_than_2030
