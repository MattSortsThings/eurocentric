# Initial release: *shared* features

This document contains the shared feature specification for the initial release of *Eurocentric*.

- [Initial release: *shared* features](#initial-release-shared-features)
  - [API Modules](#api-modules)
    - [S01: Versioned API Releases](#s01-versioned-api-releases)
  - [Error Handling](#error-handling)
    - [S02: Global Exception Handling](#s02-global-exception-handling)
    - [S03: Problem Details Responses](#s03-problem-details-responses)
  - [OpenAPI](#openapi)
    - [S04: Documentation Web Pages](#s04-documentation-web-pages)
    - [S05: OpenAPI Documents](#s05-openapi-documents)
  - [Security](#security)
    - [S06: *Admin API* Key Security](#s06-admin-api-key-security)
    - [S07: *Public API* Key Security](#s07-public-api-key-security)

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

## Error Handling

### S02: Global Exception Handling

**User Story**

- **As** the Dev
- **I want** an exception thrown while handling an API HTTP request to be returned as an HTTP response
  - with an unsuccessful status code
  - and a serialized problem details response object
- **So that** the user can understand why their request was unsuccessful.

### S03: Problem Details Responses

**User Story**

- **As** the Dev
- **I want** every unsuccessful API HTTP request to be returned as an HTTP response
  - with an unsuccessful status code
  - and a serialized problem details response object
- **So that** the user can understand why their request was unsuccessful.

## OpenAPI

### S04: Documentation Web Pages

**User Story**

- **As** the Dev
- **I want** every API release to serve a documentation web page
  - in development and production
  - at a standardized address
- **So that** users can understand how to work with the API.

### S05: OpenAPI Documents

**User Story**

- **As** the Dev
- **I want** every API release to serve an OpenAPI JSON document
  - in development and production
  - at a standardized address
- **So that** the API is described in a platform-agnostic manner.

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
