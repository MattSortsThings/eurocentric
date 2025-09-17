# 5. Non-functional requirements

This document is part of the [launch specification](../README.md#launch-specification).

- [5. Non-functional requirements](#5-non-functional-requirements)
  - [*errors* feature scope](#errors-feature-scope)
    - [ne01: Problem details responses](#ne01-problem-details-responses)
    - [ne02: Exception handling](#ne02-exception-handling)
  - [*open-api* feature scope](#open-api-feature-scope)
    - [no01: OpenAPI documents](#no01-openapi-documents)
    - [no02: Documentation web pages](#no02-documentation-web-pages)
  - [*security* feature scope](#security-feature-scope)
    - [ns01: API key authentication](#ns01-api-key-authentication)
    - [ns02: Admin API authorization](#ns02-admin-api-authorization)
    - [ns03: Public API authorization](#ns03-public-api-authorization)
  - [*versioning* feature scope](#versioning-feature-scope)
    - [nv01: API versioning](#nv01-api-versioning)

## *errors* feature scope

### ne01: Problem details responses

- An HTTP request either succeeds and returns a successful HTTP response or fails and returns an unsuccessful HTTP response.
- An unsuccessful HTTP response has an unsuccessful status code and a serialized `ProblemDetails` object in the body.

### ne02: Exception handling

- Any exception thrown while handling a request is caught by a global exception handler.
- The exception handler sends an unsuccessful HTTP response with an unsuccessful status code and a serialized `ProblemDetails` object.
- The `ProblemDetails` names the exception type that was thrown but does not expose implementation details.

## *open-api* feature scope

### no01: OpenAPI documents

- The system serves an OpenAPI v3.0.1 JSON document for each version of each API, from an endpoint that does not authenticate requests.

### no02: Documentation web pages

- The system serves a documentation web page for each OpenAPI document, from an endpoint that does not authenticate requests.

## *security* feature scope

### ns01: API key authentication

- Two API keys are defined: a PUBLIC_API_KEY and a SECRET_API_KEY.
- The SECRET_API_KEY is kept out of source code, version control, and documentation.
- Any HTTP request to the Admin API or the Public API must include an `"X-Api-Key"` header.

### ns02: Admin API authorization

- Any HTTP request to the Admin API must include the SECRET_API_KEY.

### ns03: Public API authorization

- Any HTTP request to the Public API must include the PUBLIC_API_KEY or the SECRET_API_KEY.

## *versioning* feature scope

### nv01: API versioning

- The system uses major/minor API versioning.
- Having defined version 1.0 of an API, version 1.1 can only add endpoints to version 1.0. It cannot remove or modify endpoints from version 1.0.
- An HTTP response includes all supported API versions as a response header.
