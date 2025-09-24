# 5. Non-functional requirements

This document is part of the [launch specification](../README.md#launch-specification).

- [5. Non-functional requirements](#5-non-functional-requirements)
  - [*errors* feature scope](#errors-feature-scope)
    - [ne01: Problem details responses](#ne01-problem-details-responses)
    - [ne02: Exception handling](#ne02-exception-handling)
  - [*limiting* feature scope](#limiting-feature-scope)
    - [nl01: Rate limiting](#nl01-rate-limiting)
  - [*open-api* feature scope](#open-api-feature-scope)
    - [no01: OpenAPI documents](#no01-openapi-documents)
    - [no02: Documentation web pages](#no02-documentation-web-pages)
  - [*security* feature scope](#security-feature-scope)
    - [ns01: Admin API security](#ns01-admin-api-security)
    - [ns02: Public API security](#ns02-public-api-security)
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

## *limiting* feature scope

### nl01: Rate limiting

- Public API HTTP requests using the PUBLIC_API_KEY as a request header are rate limited (ideally by IP address).
- Public API HTTP requests using the SECRET_API_KEY are not rate limited.
- The Admin API does not use rate limiting.

## *open-api* feature scope

### no01: OpenAPI documents

- The system serves an OpenAPI v3.1 JSON document for each version of each API, from an endpoint that does not authenticate requests.

### no02: Documentation web pages

- The system serves a documentation web page for each OpenAPI document, from an endpoint that does not authenticate requests.

## *security* feature scope

### ns01: Admin API security

- An HTTP request to the Admin API using the SECRET_API_KEY as a request header is authenticated and authorized.
- An HTTP request using the DEMO_API_KEY is authenticated but not authorized.
- An HTTP request using an unrecognized API key or no API key is not authenticated.

### ns02: Public API security

- An HTTP request to the Public API using the SECRET_API_KEY as a request header is authenticated and authorized.
- An HTTP request using the DEMO_API_KEY is authenticated and authorized.
- An HTTP request using an unrecognized API key or no API key is not authenticated.

## *versioning* feature scope

### nv01: API versioning

- The system uses major/minor API versioning.
- Having defined version 1.0 of an API, version 1.1 can only add endpoints to version 1.0. It cannot remove or modify any endpoint from version 1.0.
- An HTTP response includes all supported versions of the API as a response header.
