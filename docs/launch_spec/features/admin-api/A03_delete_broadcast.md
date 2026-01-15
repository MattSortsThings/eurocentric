# A03 Delete broadcast

This document is part of the [*Eurocentric* launch specification](../../README.md).

**Feature scope:** *admin-api*

## User story

- **As the Admin**
- **I want** to delete a specific broadcast
- **So that** no trace of the deleted broadcast remains, and the Public API's queryable voting data is always up to date, and I am free to create a new broadcast with the same broadcast date or contest stage if I wish

## API contract

### HTTP request

```http request
DELETE /admin/api/{apiVersion}/broadcasts/{broadcastId}
```

### HTTP response

```http request
204 No Content
```

## Acceptance criteria

### Happy path

**DeleteBroadcast endpoint...**

- Should_succeed_with_204_NoContent_when_request_is_valid
- Should_delete_requested_broadcast

### Sad path

**DeleteBroadcast endpoint...**

- Should_fail_when_broadcast_does_not_exist
