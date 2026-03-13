# A08. Delete contest

This document is part of the [launch specification](../../../README.md).

## User story

- **As the Admin**
- **I want** to delete a specified contest
- **So that** all trace of the contest is removed from the system
- **and** I am free to create a new contest with the same contest year if I wish

## API contract

### HTTP request

```http request
DELETE /admin/api/{apiVersion}/contests/{contestId}
```

### HTTP response

```http request
204 No Content
```

## Acceptance criteria

### Happy Path : Baseline

**Endpoint...**

- [ ] Succeeds_and_deletes_contest

### Sad Path : Contest Not Found

**Endpoint...**

- [ ] Fails_when_contest_does_not_exist

### Sad Path : Contest Deletion Disallowed

**Endpoint...**

- [ ] Fails_when_contest_childBroadcasts_is_not_empty
