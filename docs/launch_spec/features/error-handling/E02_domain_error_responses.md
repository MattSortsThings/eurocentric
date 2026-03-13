# E02. Domain error responses

This document is part of the [launch specification](../../README.md).

## User story

- **As the Dev**
- **I want** any HTTP request to an API endpoint that fails due to a `DomainError` to be returned to the client as a failure HTTP response
- **with** an appropriate status code
- **with** a `ProblemDetails` response body mapped from the `DomainError`
- **So that** the user can understand why their request failed
- **and** the user knows how to fix their request

## Acceptance criteria

### Domain Error Type : Undefined

**Any API endpoint...**

- [ ] Returns_500_InternalServiceError_when_request_fails_with_Undefined_DomainError
- [ ] Returns_ProblemDetails_when_request_fails_with_Undefined_DomainError

### Domain Error Type : NotFound

**Any API endpoint...**

- [ ] Returns_404_NotFound_when_request_fails_with_NotFound_DomainError
- [ ] Returns_ProblemDetails_when_request_fails_with_NotFound_DomainError

### Domain Error Type : Extrinsic

**Any API endpoint...**

- [ ] Returns_409_Conflict_when_request_fails_with_Extrinsic_DomainError
- [ ] Returns_ProblemDetails_when_request_fails_with_Extrinsic_DomainError

### Domain Error Type : Intrinsic

**Any API endpoint...**

- [ ] Returns_422_UnprocessableEntity_when_request_fails_with_Intrinsic_DomainError
- [ ] Returns_ProblemDetails_when_request_fails_with_Intrinsic_DomainError
