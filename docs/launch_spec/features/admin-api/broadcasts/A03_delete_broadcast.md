# A03. Delete broadcast

This document is part of the [launch specification](../../../README.md).

## User story

- **As the Admin**
- **I want** to delete a specified broadcast
- **So that** all trace of the broadcast is removed from the system
- **and** I am free to create a new broadcast with the same broadcast date if I wish
- **and** I am free to create a new broadcast with the same parent contest and contest stage if I wish

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

### Happy Path : Baseline

**Endpoint...**

- [ ] Succeeds_and_deletes_broadcast

### Sad Path : Broadcast Not Found

**Endpoint...**

- [ ] Fails_when_broadcast_does_not_exist
