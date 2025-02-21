# Initial release: *shared* features, and features out of scope

This document contains the shared feature specification for the initial release of *Eurocentric*.

- [Initial release: *shared* features, and features out of scope](#initial-release-shared-features-and-features-out-of-scope)
  - [API Modules](#api-modules)
    - [S01: Versioned API Releases](#s01-versioned-api-releases)
  - [Documentation](#documentation)
    - [S02: Documentation Web Pages](#s02-documentation-web-pages)
    - [S03: OpenAPI Documents](#s03-openapi-documents)
  - [Error Handling](#error-handling)
    - [S04: Global Exception Handling](#s04-global-exception-handling)
    - [S05: Problem Details Responses](#s05-problem-details-responses)
  - [Security](#security)
    - [S06: *Admin API* Key Security](#s06-admin-api-key-security)
    - [S07: *Public API* Key Security](#s07-public-api-key-security)
  - [Features out of scope](#features-out-of-scope)

## API Modules

### S01: Versioned API Releases

**User Story**

- **As** the Dev
- **I want** each minor version of each API to be released as a whole unit
  - using the version number as a URL segment
  - with each endpoint specifying the major and minor version number when it was introduced
- **So that** endpoints are mapped automatically on application startup
  - and API releases are generated automatically on application startup
  - and each API release automatically includes all endpoints with an equal major version number and an equal or smaller minor version number.

## Documentation

### S02: Documentation Web Pages

**User Story**

- **As** the Dev
- **I want** every API release to serve a documentation web page
  - in development and production
  - at a standardized address
- **So that** users can understand how to work with the API.

### S03: OpenAPI Documents

**User Story**

- **As** the Dev
- **I want** every API release to serve an OpenAPI JSON document
  - in development and production
  - at a standardized address
- **So that** the API is described in a platform-agnostic manner.

## Error Handling

### S04: Global Exception Handling

**User Story**

- **As** the Dev
- **I want** an exception thrown while handling an API HTTP request to be returned as an HTTP response
  - with an unsuccessful status code
  - and a serialized problem details response object
- **So that** the user can understand why their request was unsuccessful.

### S05: Problem Details Responses

**User Story**

- **As** the Dev
- **I want** every unsuccessful API HTTP request to be returned as an HTTP response
  - with an unsuccessful status code
  - and a serialized problem details response object
- **So that** the user can understand why their request was unsuccessful.

## Security

### S06: *Admin API* Key Security

**User Story**

- **As** the Dev
- **I want** every *Admin API* endpoint to require a secret Admin API key as an `"X-Api-Key"` HTTP request header
- **So that** I can ensure that only the Admin can modify the system database.

### S07: *Public API* Key Security

**User Story**

- **As** the Dev
- **I want** every *Public API* endpoint to require either a Public API key or a secret Admin API key as an `"X-Api-Key"` HTTP request header
- **So that** I can exert some control over who can run queries using the *Public API*.

## Features out of scope

The following features are not included in the initial release:

- Request logging
- Request rate limiting
- Response caching

The system is built so that these features could be incorporated in the future as cross-cutting functionality.
