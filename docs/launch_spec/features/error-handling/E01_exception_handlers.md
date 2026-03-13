# E01. Exception handlers

This document is part of the [launch specification](../../README.md).

## User story

- **As the Dev**
- **I want** any exception thrown on the server to be caught by exception handling middleware and returned to the client as a failure HTTP response
  - **with** an appropriate status code
  - **with** a `ProblemDetails` response body describing the exception
- **So that** the user can understand what went wrong

## Acceptance criteria

### BadHttpRequestException

**Any API endpoint...**

- [ ] Returns_400_BadRequest_on_BadHttpRequestException
- [ ] Returns_ProblemDetails_with_exceptionMessage_on_BadHttpRequestException

### InvalidEnumArgumentException

**Any API endpoint...**

- [ ] Returns_400_BadRequest_on_InvalidEnumArgumentException
- [ ] Returns_ProblemDetails_with_exceptionMessage_on_InvalidEnumArgumentException

### Timeout DbUpdateException

**Any API endpoint...**

- [ ] Returns_503_ServiceUnavailable_with_RetryAfter_header_on_timeout_DbUpdateException
- [ ] Returns_ProblemDetails_without_exceptionMessage_on_timeout_DbUpdateException

### Non Timeout DbUpdateException

**Any API endpoint...**

- [ ] Returns_503_ServiceUnavailable_with_RetryAfter_header_on_non_timeout_DbUpdateException
- [ ] Returns_ProblemDetails_without_exceptionMessage_on_non_timeout_DbUpdateException

### General Exception

**Any API endpoint...**

- [ ] Returns_500_InternalServerError_on_general_Exception
- [ ] Returns_ProblemDetails_without_exceptionMessage_on_general_Exception
