# A08. Delete contest

This document is part of the [*Eurocentric* launch specification](../../../README.md).

- [A08. Delete contest](#a08-delete-contest)
  - [User story](#user-story)
  - [API contract](#api-contract)
    - [HTTP request](#http-request)
    - [HTTP response](#http-response)
  - [Acceptance criteria](#acceptance-criteria)
    - [Happy path](#happy-path)
    - [Sad path](#sad-path)

## User story

- **As the Admin**
- **I want** to delete a single contest
- **So that** no trace of the deleted contest remains, and the queryable voting data is always up-to-date, and I am free to create a new contest with the same contest year if I wish.

## API contract

### HTTP request

```http request
DELETE /admin/api/{apiVersion}/contests/{contestId}
```

**Notes:**

- `apiVersion` is a major-minor API version URL segment, e.g. `"v1.0"`.
- `contestId` is the Guid ID of the requested contest aggregate.

### HTTP response

```http request
204 No Content
```

**Notes:**

- The requested contest aggregate no longer exists.
- Any secondary effects are scoped to event handler features.

## Acceptance criteria

### Happy path

**DeleteContest endpoint...**

- [ ] Should_succeed_with_204_NoContent_and_delete_contest_when_request_is_valid

### Sad path

**DeleteContest endpoint...**

- [ ] Should_fail_when_contest_does_not_exist
- [ ] Should_fail_when_contest_owns_one_or_more_broadcastMemos
