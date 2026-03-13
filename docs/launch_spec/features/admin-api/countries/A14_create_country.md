# A14. Create country

This document is part of the [launch specification](../../../README.md).

## User story

- **As the Admin**
- **I want** to create a new country
- **So that** I can reference the country in one or more contests I will go on to create

## API contract

### HTTP request

```http request
POST /admin/api/{apiVersion}/countries
```

```json
{
  "countryCode": "AA",
  "countryName": "CountryName",
  "countryType": "Real"
}
```

### HTTP response

```http request
201 Created
Location: {host}/admin/api/{apiVersion}/countries/{countryId}
```

```json
{
  "country": {
    "id": "00000000-0000-0000-0000-000000000000",
    "countryCode": "AA",
    "countryName": "CountryName",
    "countryType": "Real",
    "activeContestIds": []
  }
}
```

## Acceptance criteria

### Happy Path : Baseline

**Endpoint...**

- [ ] Succeeds_and_creates_country

### Happy Path : Variants

**Endpoint...**

- [ ] Succeeds_when_creating_Real_country
- [ ] Succeeds_when_creating_Pseudo_country

### Sad Path : Bad HTTP Request

**Endpoint...**

- [ ] Fails_when_countryCode_is_not_provided
- [ ] Fails_when_countryName_is_not_provided
- [ ] Fails_when_countryType_is_not_provided
- [ ] Fails_when_countryType_is_invalid_enum_name

### Sad Path : Invalid Enum Argument

**Endpoint...**

- [ ] Fails_when_countryType_is_invalid_enum_int_value

### Sad Path : Country Code Duplicated

**Endpoint...**

- [ ] Fails_when_existing_country_has_same_countryCode

### Sad Path : Illegal Country Code

**Endpoint...**

- [ ] Fails_when_countryCode_is_empty_string
- [ ] Fails_when_countryCode_is_shorter_than_2_chars
- [ ] Fails_when_countryCode_is_longer_than_2_chars
- [ ] Fails_when_countryCode_contains_non_upper_case_ASCII_letter_char

### Sad Path : Illegal Country Name

**Endpoint...**

- [ ] Fails_when_countryName_is_empty_string
- [ ] Fails_when_countryName_is_longer_than_100_chars
- [ ] Fails_when_countryName_starts_with_white_space_char
- [ ] Fails_when_countryName_ends_with_white_space_char
- [ ] Fails_when_countryName_is_multiline_string
