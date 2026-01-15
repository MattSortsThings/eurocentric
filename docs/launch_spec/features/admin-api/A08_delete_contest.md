# A08 Delete contest

This document is part of the [*Eurocentric* launch specification](../../README.md).

**Feature scope:** *admin-api*

## User story

- **As the Admin**
- **I want** to delete a specific contest
- **So that** no trace of the deleted contest remains, and I am free to create a new contest with the same contest year if I wish

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

### Happy path

**DeleteContest endpoint...**

- Should_succeed_with_204_NoContent_when_request_is_valid
- Should_delete_requested_contest

### Sad path

**DeleteContest endpoint...**

- Should_fail_when_contest_does_not_exist
- Should_fail_when_contest_has_one_or_more_child_broadcasts
