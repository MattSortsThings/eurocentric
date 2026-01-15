# A14 Create country

This document is part of the [*Eurocentric* launch specification](../../README.md).

**Feature scope:** *admin-api*

## User story

- **As the Admin**
- **I want** to create a new country
- **So that** the created country can have roles in one or more contests I will go on to create

## API contract

### HTTP request

```http request
POST /admin/api/{apiVersion}/countries
```

```json
{
  "countryCode": "AT",
  "countryName": "Austria",
  "countryType": "Real"
}
```

### HTTP response

```http request
201 Created
Location: /admin/api/{apiVersion}/countries/{countryId}
```

```json
{
  "country": {
    "id": "00000000-0000-0000-0000-000000000000",
    "countryCode": "AT",
    "countryName": "Austria",
    "countryType": "Real",
    "contestRoles": []
  }
}
```

## Acceptance criteria

### Happy path

**CreateCountry endpoint...**

- Should_succeed_with_201_Created_and_created_country_when_request_is_valid
- Should_return_Location_header_with_apiVersion_and_countryId_when_request_is_valid
- Should_create_country_with_countryType_Real
- Should_create_country_with_countryType_Pseudo
- Should_create_country_when_countryName_contains_punctuation
- Should_create_country_when_countryName_contains_accented_letter

### Sad path

**CreateCountry endpoint...**

- Should_fail_when_countryCode_is_not_globally_unique
- Should_fail_when_countryCode_is_empty
- Should_fail_when_countryCode_is_whitespace
- Should_fail_when_countryCode_is_shorter_than_2_characters
- Should_fail_when_countryCode_is_longer_than_2_characters
- Should_fail_when_countryCode_contains_non_letter_character
- Should_fail_when_countryCode_contains_lowerCase_letter
- Should_fail_when_countryName_is_empty
- Should_fail_when_countryName_is_whitespace
- Should_fail_when_countryName_is_longer_than_100_characters
- Should_fail_when_countryName_starts_with_whitespace
- Should_fail_when_countryName_ends_with_whitespace
- Should_fail_when_countryName_contains_line_break
