# A03. Delete broadcast

This document is part of the [*Eurocentric* launch specification](../../../README.md).

- [A03. Delete broadcast](#a03-delete-broadcast)
  - [User story](#user-story)
  - [API contract](#api-contract)
    - [HTTP request](#http-request)
    - [HTTP response](#http-response)
  - [Acceptance criteria](#acceptance-criteria)
    - [Happy path](#happy-path)
    - [Sad path](#sad-path)

## User story

- **As the Admin**
- **I want** to delete a single broadcast
- **So that** no trace of the deleted broadcast remains, and the queryable voting data is always up-to-date, and I am free to create a new broadcast with the same broadcast date or contest year + stage if I wish.

## API contract

### HTTP request

```http request
DELETE /admin/api/{apiVersion}/broadcasts/{broadcastId}
```

**Notes:**

- `apiVersion` is a major-minor API version URL segment, e.g. `"v1.0"`.
- `broadcastId` is the Guid ID of the requested broadcast aggregate.

### HTTP response

```http request
204 No Content
```

**Notes:**

- The requested broadcast aggregate no longer exists.
- Any secondary effects are scoped to event handler features.

## Acceptance criteria

### Happy path

**DeleteBroadcast endpoint...**

- [ ] Should_succeed_with_204_NoContent_and_delete_broadcast_when_request_is_valid

### Sad path

**DeleteBroadcast endpoint...**

- [ ] Should_fail_when_broadcast_does_not_exist
