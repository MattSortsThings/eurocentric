# A06. Create contest

This document is part of the [launch specification](../../../README.md).

## User story

- **As the Admin**
- **I want** to create a new contest
- **So that** I can create child broadcasts for the contest and start awarding points

## API contract

### HTTP request

```http request
POST /admin/api/{apiVersion}/contests
```

```json
{
  "contestYear": 2025,
  "cityName": "CityName",
  "semiFinalBroadcastFormat": "TelevoteOnly",
  "globalTelevoteVotingCountryId": "00000000-0000-0000-0000-000000000000",
  "participants": [
    {
      "participatingCountryId": "00000000-0000-0000-0000-000000000000",
      "semiFinalDraw": "SemiFinal1",
      "actName": "ActName",
      "songTitle": "SongTitle"
    }
  ]
}
```

**Notes:**

- `globalTelevoteVotingCountryId` may be `null`

### HTTP response

```http request
201 Created
Location: {host}/admin/api/{apiVersion}/contests/{contestId}
```

```json
{
  "contest": {
    "id": "00000000-0000-0000-0000-000000000000",
    "contestYear": 2025,
    "cityName": "CityName",
    "semiFinalBroadcastFormat": "TelevoteOnly",
    "grandFinalBroadcastFormat": "JuryAndTelevote",
    "queryable": false,
    "globalTelevote": {
      "votingCountryId": "00000000-0000-0000-0000-000000000000"
    },
    "childBroadcasts": [],
    "participants": [
      {
        "participatingCountryId": "00000000-0000-0000-0000-000000000000",
        "semiFinalDraw": "SemiFinal1",
        "actName": "ActName",
        "songTitle": "SongTitle"
      }
    ]
  }
}
```

**Notes:**

- `contest`.`globalTelevote` may be `null`

## Acceptance criteria

### Happy Path : Baseline

**Endpoint...**

- [ ] Succeeds_and_creates_contest

### Happy Path : Variants

**Endpoint...**

- [ ] Succeeds_when_creating_contest_with_globalTelevote_and_JuryAndTelevote_SemiFinals
- [ ] Succeeds_when_creating_contest_with_globalTelevote_and_TelevoteOnly_SemiFinals
- [ ] Succeeds_when_creating_contest_with_no_globalTelevote_and_JuryAndTelevote_SemiFinals
- [ ] Succeeds_when_creating_contest_with_no_globalTelevote_and_TelevoteOnly_SemiFinals

### Sad Path : Bad HTTP Request

**Endpoint...**

- [ ] Fails_when_contestYear_is_not_provided
- [ ] Fails_when_cityName_is_not_provided
- [ ] Fails_when_semiFinalBroadcastFormat_is_not_provided
- [ ] Fails_when_participants_is_not_provided
- [ ] Fails_when_participant_participatingCountryId_is_not_provided
- [ ] Fails_when_participant_semiFinalDraw_is_not_provided
- [ ] Fails_when_participant_actName_is_not_provided
- [ ] Fails_when_participant_songTitle_is_not_provided
- [ ] Fails_when_semiFinalBroadcastFormat_is_invalid_enum_name
- [ ] Fails_when_participant_semiFinalDraw_is_invalid_enum_name

### Sad Path : Invalid Enum Argument

**Endpoint...**

- [ ] Fails_when_semiFinalBroadcastFormat_is_invalid_enum_int_value
- [ ] Fails_when_participant_semiFinalDraw_is_invalid_enum_int_value

### Sad Path : Country Not Found

**Endpoint...**

- [ ] Fails_when_globalTelevote_votingCountryId_matches_no_country
- [ ] Fails_when_participant_participatingCountryId_matches_no_country

### Sad Path : Contest Participating Country Ineligible

**Endpoint...**

- [ ] Fails_when_participant_participatingCountryId_matches_Pseudo_country

### Sad Path : Contest Voting Country Ineligible

**Endpoint...**

- [ ] Fails_when_globalTelevote_votingCountryId_matches_Real_country

### Sad Path : Contest Year Duplicated

**Endpoint...**

- [ ] Fails_when_existing_contest_has_same_contestYear

### Sad Path : Illegal Act Name

**Endpoint...**

- [ ] Fails_when_participant_actName_is_empty_string
- [ ] Fails_when_participant_actName_is_longer_than_100_chars
- [ ] Fails_when_participant_actName_starts_with_white_space_char
- [ ] Fails_when_participant_actName_ends_with_white_space_char
- [ ] Fails_when_participant_actName_is_multiline_string

### Sad Path : Illegal City Name

**Endpoint...**

- [ ] Fails_when_cityName_is_empty_string
- [ ] Fails_when_cityName_is_longer_than_100_chars
- [ ] Fails_when_cityName_starts_with_white_space_char
- [ ] Fails_when_cityName_ends_with_white_space_char
- [ ] Fails_when_cityName_is_multiline_string

### Sad Path : Illegal Contest Year

**Endpoint...**

- [ ] Fails_when_contestYear_is_less_than_2016
- [ ] Fails_when_contestYear_is_greater_than_2030

### Sad Path : Illegal Song Title

**Endpoint...**

- [ ] Fails_when_participant_songTitle_is_empty_string
- [ ] Fails_when_participant_songTitle_is_longer_than_100_chars
- [ ] Fails_when_participant_songTitle_starts_with_white_space_char
- [ ] Fails_when_participant_songTitle_ends_with_white_space_char
- [ ] Fails_when_participant_songTitle_is_multiline_string

### Sad Path : Duplicated Contest Participating Countries

**Endpoint...**

- [ ] Fails_when_participants_have_same_participatingCountryId

### Sad Path : Illegal Contest Semi-Final Draw Distribution

**Endpoint...**

- [ ] Fails_when_fewer_than_3_participants_have_SemiFinalDraw_SemiFinal1
- [ ] Fails_when_fewer_than_3_participants_have_SemiFinalDraw_SemiFinal2
