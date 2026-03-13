# A15. Delete country

This document is part of the [launch specification](../../../README.md).

## User story

- **As the Admin**
- **I want** to delete a specified country
- **So that** all trace of the country is removed from the system
- **and** I am free to create a new country with the same country code if I wish

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

### Happy Path : Baseline

**Endpoint...**

- [ ] Succeeds_and_deletes_country

### Sad Path : Country Not Found

**Endpoint...**

- [ ] Fails_when_country_does_not_exist

### Sad Path : Country Deletion Disallowed

**Endpoint...**

- [ ] Fails_when_country_activeContestIds_is_not_empty
