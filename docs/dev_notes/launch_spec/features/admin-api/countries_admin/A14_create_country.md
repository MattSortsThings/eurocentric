# A14 Create country

This document is part of the *Eurocentric* [launch specification](../../../README.md).

- [A14 Create country](#a14-create-country)
  - [User story](#user-story)
  - [API contract](#api-contract)
    - [HTTP request](#http-request)
    - [HTTP response](#http-response)
  - [Acceptance criteria](#acceptance-criteria)

## User story

- **As the Admin**
- **I want** to create a new country
- **So that** the country can have roles in contests I will go on to create

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

**Notes:**

- `countryType` is an [enum value](../../../domain_model.md#countrytype-enum)
- All properties are required

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

**Notes:**

- `countryType` is an [enum value](../../../domain_model.md#countrytype-enum)
- `contestRoles` is always empty for a new country

## Acceptance criteria

**CreateCountry endpoint...**

- should succeed with 201 and return created real country
- should succeed with 201 and return created pseudo country
- should succeed with 201 and return created country with punctuated name
- should succeed with 201 and return created country with accented name
- should fail with 409 and ProblemDetails on CountryCodeConflict
- should fail with 422 and ProblemDetails on IllegalCountryCodeValue. Test cases include:
  - empty string
  - string of 1 character
  - string longer than 2 characters
  - string with non-letter character
  - string with lower-case letter character
- should fail with 422 and ProblemDetails on IllegalCountryNameValue. Test cases include:
  - empty string
  - all whitespace string
  - string longer than 100 characters
  - string with line-break character
  - string with leading whitespace
  - string with trailing whitespace
