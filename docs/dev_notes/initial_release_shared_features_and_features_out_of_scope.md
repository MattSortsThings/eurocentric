# Initial release: *shared* features, and features out of scope

This document lists the features with *shared* scope for the initial release of *Eurocentric*. It also lists the features that are out of scope for the project.

- [Initial release: *shared* features, and features out of scope](#initial-release-shared-features-and-features-out-of-scope)
  - [Documentation](#documentation)
    - [S01: Documentation Web Pages](#s01-documentation-web-pages)
    - [S02: OpenAPI Documents](#s02-openapi-documents)
  - [Error Handling](#error-handling)
    - [S03: Global Exception Handling](#s03-global-exception-handling)
    - [S04: Problem Details Responses](#s04-problem-details-responses)
  - [Security](#security)
    - [S05: API Key Authentication](#s05-api-key-authentication)
    - [S06: *Admin API* Authorization](#s06-admin-api-authorization)
    - [S07: *Public API* Authorization](#s07-public-api-authorization)
  - [Versioning](#versioning)
    - [S08: API Major/Minor Versioning](#s08-api-majorminor-versioning)
  - [Features out of scope](#features-out-of-scope)

## Documentation

### S01: Documentation Web Pages

**User Story**

- **AS** the Dev
- **I WANT** the web app to serve a documentation web page for every API release
  - in development and production
  - at a standardized address
  - accessible to anonymous clients
- **SO THAT** users can understand how to work with the API.

### S02: OpenAPI Documents

**User Story**

- **AS** the Dev
- **I WANT** the web app to serve an OpenAPI JSON document for every API release
  - in development and production
  - at a standardized address
  - accessible to anonymous clients
- **SO THAT** the API is described in a platform-agnostic manner.

## Error Handling

### S03: Global Exception Handling

**User Story**

- **AS** the Dev
- **I WANT** an exception thrown while handling an API HTTP request to be returned as an HTTP response
  - with an unsuccessful status code
  - and a serialized problem details response object
- **SO THAT** the user can understand why their request was unsuccessful.

### S04: Problem Details Responses

**User Story**

- **AS** the Dev
- **I WANT** every unsuccessful API HTTP request to be returned as an HTTP response
  - with an unsuccessful status code
  - and a serialized problem details response object
- **SO THAT** the user can understand why their request was unsuccessful.

## Security

### S05: API Key Authentication

**User Story**

- **AS** the Dev
- **I WANT** requests to API endpoints to use API key authentication
  - with an API key passed as an `"X-Api-Key"` HTTP request header
  - defining a secret API key
  - and a demo API key
- **SO THAT** I can exert some control over who can access the API endpoints.

### S06: *Admin API* Authorization

**User Story**

- **AS** the Dev
- **I WANT** every *Admin API* endpoint to be restricted to clients with the `"Administrator"` role
  - assigned during API key authentication
    - when the request contains the secret API key
- **SO THAT** I can ensure that only the Admin can modify the system database.

### S07: *Public API* Authorization

**User Story**

- **AS** the Dev
- **I WANT** every *Public API* endpoint to be restricted to clients with the `"User"` role
  - assigned during API key authentication
    - when the request contains the secret API key
    - or the demo API key
- **SO THAT** I can exert some control over who can run queries using the *Public API*.

## Versioning

### S08: API Major/Minor Versioning

**User Story**

- **AS** the Dev
- **I WANT** to use major/minor versioning in the *Admin API* and the *Public API*
  - so that each major version of an API is independent of any previous major versions
  - and each minor version of an API major version includes all previous minor versions without modification
  - and the user must specify the API version as a URL segment
- **SO THAT** the APIs can be updated in the future in a predictable, backwards-compatible manner.

## Features out of scope

The following features are not included in the initial release:

- Request logging
- Request rate limiting
- Response caching

The system is built so that these features could be incorporated in the future as cross-cutting functionality.
