# Initial release: *shared* features, and features out of scope

This document contains the shared feature specification for the initial release of *Eurocentric*.

- [Initial release: *shared* features, and features out of scope](#initial-release-shared-features-and-features-out-of-scope)
  - [API Registration](#api-registration)
    - [S01: Endpoint Discovery](#s01-endpoint-discovery)
    - [S02: Versioned API Endpoint Mapping](#s02-versioned-api-endpoint-mapping)
  - [Documentation](#documentation)
    - [S03: Documentation Web Pages](#s03-documentation-web-pages)
    - [S04: OpenAPI Documents](#s04-openapi-documents)
  - [Error Handling](#error-handling)
    - [S05: Global Exception Handling](#s05-global-exception-handling)
    - [S06: Problem Details Responses](#s06-problem-details-responses)
  - [Security](#security)
    - [S07: API Key Authentication](#s07-api-key-authentication)
    - [S08: *Admin API* Authorization](#s08-admin-api-authorization)
    - [S09: *Public API* Authorization](#s09-public-api-authorization)
  - [Features out of scope](#features-out-of-scope)

## API Registration

### S01: Endpoint Discovery

**User Story**

- **As** the Dev
- **I want** each endpoint for an API to be discovered automatically on application startup
- **So that** I don't have to wire up the individual endpoints manually.

### S02: Versioned API Endpoint Mapping

**User Story**

- **As** the Dev
- **I want** each minor version of each API to be released as a whole unit
  - using the version number as a URL segment
  - with each endpoint specifying the major and minor version number when it was introduced
- **So that** each API's versioned endpoints are automatically mapped on application startup
  - and each API release automatically includes all endpoints
    - with an equal major version number
    - and an equal or smaller minor version number.

## Documentation

### S03: Documentation Web Pages

**User Story**

- **As** the Dev
- **I want** the web app to serve a documentation web page for every API release
  - in development and production
  - at a standardized address
  - accessible to anonymous clients
- **So that** users can understand how to work with the API.

### S04: OpenAPI Documents

**User Story**

- **As** the Dev
- **I want** the web app to serve an an OpenAPI JSON document for every API release
  - in development and production
  - at a standardized address
  - accessible to anonymous clients
- **So that** the API is described in a platform-agnostic manner.

## Error Handling

### S05: Global Exception Handling

**User Story**

- **As** the Dev
- **I want** an exception thrown while handling an API HTTP request to be returned as an HTTP response
  - with an unsuccessful status code
  - and a serialized problem details response object
- **So that** the user can understand why their request was unsuccessful.

### S06: Problem Details Responses

**User Story**

- **As** the Dev
- **I want** every unsuccessful API HTTP request to be returned as an HTTP response
  - with an unsuccessful status code
  - and a serialized problem details response object
- **So that** the user can understand why their request was unsuccessful.

## Security

### S07: API Key Authentication

**User Story**

- **As** the Dev
- **I want** requests to API endpoints to use API key authentication
  - with an API key passed as an `"X-Api-Key"` HTTP request header
  - defining a secret API key
  - and a demo API key
- **So that** I can exert some control over who can access the API endpoints.

### S08: *Admin API* Authorization

**User Story**

- **As** the Dev
- **I want** every *Admin API* endpoint to be restricted to clients with the `"Administrator"` role
  - assigned during API key authentication
    - when the request contains the secret API key
- **So that** I can ensure that only the Admin can modify the system database.

### S09: *Public API* Authorization

**User Story**

- **As** the Dev
- **I want** every *Public API* endpoint to be restricted to clients with the `"User"` role
  - assigned during API key authentication
    - when the request contains the secret API key
    - or the demo API key
- **So that** I can exert some control over who can run queries using the *Public API*.

## Features out of scope

The following features are not included in the initial release:

- Request logging
- Request rate limiting
- Response caching

The system is built so that these features could be incorporated in the future as cross-cutting functionality.
