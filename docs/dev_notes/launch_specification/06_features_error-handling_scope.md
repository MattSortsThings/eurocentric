# 06: Features: *error-handling* scope

This document is part of the [launch specification](../README.md#launch-specification).

- [06: Features: *error-handling* scope](#06-features-error-handling-scope)
  - [Errors](#errors)
    - [ce01: Problem details](#ce01-problem-details)
  - [Exceptions](#exceptions)
    - [ce02: Exception handling](#ce02-exception-handling)
    - [ce03: DB timeout Handling](#ce03-db-timeout-handling)

## Errors

### ce01: Problem details

**Details:**

- Every API HTTP request *either* succeeds and returns a successful HTTP response *or* fails and returns an unsuccessful HTTP response containing a `ProblemDetails` object in the body, which describes the error that arose.

## Exceptions

### ce02: Exception handling

**Details:**

- When an uncaught exception is thrown on the server while handling an API HTTP request, this is caught by a global exception handler.
- The exception is mapped to a `ProblemDetails` object, which is returned as an unsuccessful HTTP response with an appropriate status code.

### ce03: DB timeout handling

**Details:**

- A custom `DbTimeoutException` is defined.
- When an `SqlException` is thrown on the server while handling an API HTTP request because the database is temporarily unavailable, this is caught and a `DbTimeoutException` is thrown.
- The exception is mapped to a `ProblemDetails` object, which is returned as an unsuccessful HTTP response with status code 503 and a `"Retry-After"` header with a value of `120`.
