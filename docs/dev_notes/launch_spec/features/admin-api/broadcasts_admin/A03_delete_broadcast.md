# A03 Delete broadcast

This document is part of the *Eurocentric* [launch specification](../../../README.md).

- [A03 Delete broadcast](#a03-delete-broadcast)
  - [User story](#user-story)
  - [API contract](#api-contract)
    - [HTTP request](#http-request)
    - [HTTP response](#http-response)
  - [Acceptance criteria](#acceptance-criteria)

## User story

- **As the Admin**
- **I want** to delete a specific broadcast
- **So that** all trace of the deleted broadcast is removed, and I am free to create a new broadcast with the same broadcast date and/or contest stage if I wish

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

**DeleteBroadcast endpoint...**

- should succeed with 204 and delete requested broadcast
- should fail with 404 and ProblemDetails on BroadcastNotFound
