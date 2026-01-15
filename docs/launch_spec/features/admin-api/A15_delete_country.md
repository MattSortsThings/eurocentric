# A15 Delete country

This document is part of the [*Eurocentric* launch specification](../../README.md).

**Feature scope:** *admin-api*

## User story

- **As the Admin**
- **I want** to delete a specific country
- **So that** no trace of the deleted country remains, and I am free to create a new country with the same country code if I wish

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

### Happy path

**DeleteCountry endpoint...**

- Should_succeed_with_204_NoContent_when_request_is_valid
- Should_delete_requested_country

### Sad path

**DeleteCountry endpoint...**

- Should_fail_when_country_does_not_exist
- Should_fail_when_country_has_one_or_more_contest_roles
