# A14. Create country

This document is part of the [*Eurocentric* launch specification](../../../README.md).

- [A14. Create country](#a14-create-country)
  - [User story](#user-story)
  - [API contract](#api-contract)
    - [HTTP request](#http-request)
    - [HTTP response](#http-response)
  - [Acceptance criteria](#acceptance-criteria)
    - [Happy path](#happy-path)
    - [Sad path](#sad-path)

## User story

- **As the Admin**
- **I want** to create a new country
- **So that** the country can be a global televote or participant in contests I will go on to create.

## API contract

### HTTP request

```http request
POST /admin/api/{apiVersion}/countries
```

**Notes:**

- `apiVersion` is a major-minor API version URL segment, e.g. `"v1.0"`.

### HTTP response

```http request
201 Created
Location: /admin/api/{apiVersion}/countries/{countryId}
```

**Notes:**

- `apiVersion` is a major-minor API version URL segment, e.g. `"v1.0"`.
- `countryId` is the Guid ID of the created country aggregate.

```json
{
  "country": {
    "id": "00000000-0000-0000-0000-000000000000",
    "countryCode": "AA",
    "countryName": "Country Name",
    "countryType": "Real",
    "contestIds": []
  }
}
```

**Notes:**

- `country.contestIds` is always empty for a newly created country aggregate.

## Acceptance criteria

### Happy path

**CreateCountry endpoint...**

- [ ] Should_succeed_with_201_Created_and_Location_and_created_country_when_request_is_valid
- [ ] Should_succeed_when_countryName_contains_punctuation
- [ ] Should_succeed_when_countryName_contains_accented_letter
- [ ] Should_succeed_when_countryType_is_Real
- [ ] Should_succeed_when_countryType_is_Pseudo

### Sad path

**CreateCountry endpoint...**

- [ ] Should_fail_when_countryCode_is_not_unique
- [ ] Should_fail_when_countryCode_is_empty_or_whitespace
- [ ] Should_fail_when_countryCode_length_is_not_2_chars
- [ ] Should_fail_when_countryCode_contains_non_ASCII_letter_upper_char
- [ ] Should_fail_when_countryName_is_empty_or_whitespace
- [ ] Should_fail_when_countryName_is_longer_than_100_chars
- [ ] Should_fail_when_countryName_contains_line_break
- [ ] Should_fail_when_countryName_starts_or_ends_with_whitespace
