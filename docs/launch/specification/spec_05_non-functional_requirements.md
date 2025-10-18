# 5. Non-functional requirements

This document is part of the [launch specification](../README.md#launch-specification).

- [5. Non-functional requirements](#5-non-functional-requirements)
  - [*error-handling* feature scope](#error-handling-feature-scope)
    - [ne01: Problem details responses](#ne01-problem-details-responses)
    - [ne02: Exception handling](#ne02-exception-handling)
    - [ne03: Database timeout handling](#ne03-database-timeout-handling)
  - [*open-api* feature scope](#open-api-feature-scope)
    - [no01: OpenAPI documents](#no01-openapi-documents)
    - [no02: Documentation web pages](#no02-documentation-web-pages)
  - [*security* feature scope](#security-feature-scope)
    - [ns01: API key authentication](#ns01-api-key-authentication)
    - [ns02: Admin API authorization](#ns02-admin-api-authorization)
    - [ns03: Public API authorization](#ns03-public-api-authorization)
  - [*versioning* feature scope](#versioning-feature-scope)
    - [nv01: API versioning](#nv01-api-versioning)

## *error-handling* feature scope

### ne01: Problem details responses

- An HTTP request either succeeds and returns a successful HTTP response or fails and returns an unsuccessful HTTP response.
- An unsuccessful HTTP response has an unsuccessful status code and a serialized `ProblemDetails` object in the body.

### ne02: Exception handling

- Any exception thrown while handling a request is caught by a global exception handler.
- The exception handler sends an unsuccessful HTTP response with an unsuccessful status code and a serialized `ProblemDetails` object.
- The `ProblemDetails` names the exception type that was thrown but does not expose implementation details.

### ne03: Database timeout handling

- If an HTTP request is unsuccessful due to a db connection/command timeout, a custom `DbTimeoutException` is thrown.
- This is caught by the exception handler, and returned as an unsuccessful HTTP response with status code `503` and a serialized `ProblemDetails` object.
- The `ProblemDetails` includes a `"Retry-After"` header with a value of `120`.

## *open-api* feature scope

### no01: OpenAPI documents

- The system serves an OpenAPI v3.1 JSON document for each version of each API, from an endpoint that does not authenticate requests.

### no02: Documentation web pages

- The system serves a documentation web page for each OpenAPI document, from an endpoint that does not authenticate requests.

## *security* feature scope

### ns01: API key authentication

- The application defines two API keys: a SECRET_API_KEY and a DEMO_API_KEY.
- An HTTP request must include an `"X-Api-Key"` header with either the SECRET_API_KEY or the DEMO_API_KEY.

### ns02: Admin API authorization

- An HTTP request to the Admin API using the SECRET_API_KEY as a request header is authenticated and authorized.
- An HTTP request using the DEMO_API_KEY is authenticated but not authorized.
- An HTTP request using an unrecognized API key or no API key is not authenticated.

### ns03: Public API authorization

- An HTTP request to the Public API using the SECRET_API_KEY as a request header is authenticated and authorized.
- An HTTP request using the DEMO_API_KEY is authenticated and authorized.
- An HTTP request using an unrecognized API key or no API key is not authenticated.

## *versioning* feature scope

### nv01: API versioning

- The system uses major/minor API versioning.
- Having defined version 1.0 of an API, version 1.1 can only add endpoints to version 1.0. It cannot remove or modify any endpoint from version 1.0.
- An HTTP response includes all supported versions of the API as a response header.
