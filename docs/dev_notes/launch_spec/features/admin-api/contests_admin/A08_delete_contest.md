# A08 Delete contest

This document is part of the *Eurocentric* [launch specification](../../../README.md).

- [A08 Delete contest](#a08-delete-contest)
  - [User story](#user-story)
  - [API contract](#api-contract)
    - [HTTP request](#http-request)
    - [HTTP response](#http-response)
  - [Acceptance criteria](#acceptance-criteria)

## User story

- **As the Admin**
- **I want** to delete a specific contest
- **So that** all trace of the deleted contest is removed, and I am free to create a new contest with the same contest year if I wish

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

**DeleteCountry endpoint...**

- should succeed with 204 and delete requested contest
- should fail with 404 and ProblemDetails on ContestNotFound
- should fail with 409 and ProblemDetails on ContestDeletionDisallowed
