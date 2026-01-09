# A15 Delete country

This document is part of the *Eurocentric* [launch specification](../../../README.md).

- [A15 Delete country](#a15-delete-country)
  - [User story](#user-story)
  - [API contract](#api-contract)
    - [HTTP request](#http-request)
    - [HTTP response](#http-response)
  - [Acceptance criteria](#acceptance-criteria)

## User story

- **As the Admin**
- **I want** to delete a specific country
- **So that** all trace of the deleted country is removed, and I am free to create a new country with the same country code if I wish.

## API contract

### HTTP request

```http request
DELETE /admin/api/{apiVersion}/countries/{countryId}
```

### HTTP response

```http request
204 No Content
```

## Acceptance criteria

**DeleteCountry endpoint...**

- should succeed with 204 and delete requested country
- should fail with 404 and ProblemDetails on CountryNotFound
- should fail with 409 and ProblemDetails on CountryDeletionDisallowed
