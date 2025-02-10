# Shared requirements

This document outlines the *Shared* (non-functional) requirements for version 1.0 of *Eurocentric*.

Refer to the [project summary](project_summary.md) dev note for an overview of the APIs and the Dev user role.

- [Shared requirements](#shared-requirements)
  - [S1: Documentation](#s1-documentation)
    - [S101: OpenAPI Documents](#s101-openapi-documents)
    - [S102: Documentation Web Pages](#s102-documentation-web-pages)
  - [S2: Error Handling](#s2-error-handling)
    - [S201: Global Exception Handling](#s201-global-exception-handling)
    - [S202: Problem Details Responses](#s202-problem-details-responses)
  - [S3: Security](#s3-security)
    - [S301: Admin API Security](#s301-admin-api-security)
    - [S302: Public API Security](#s302-public-api-security)
  - [S4: Versioning](#s4-versioning)
    - [S401: Versioned API Releases](#s401-versioned-api-releases)

## S1: Documentation

### S101: OpenAPI Documents

```
As the Dev,

I want each version of each API to serve an OpenAPI JSON document,
  describing its operations and schemas,
  in development and production.
```

### S102: Documentation Web Pages

```
As the Dev,

I want each version of each API to serve a documentation web page
  describing its endpoints and models,
  allowing users with the right credentials to experiment with them,
  in development and production.
```

## S2: Error Handling

### S201: Global Exception Handling

```
As the Dev,

I want any uncaught exception on the server
  to be returned as an unsuccessful HTTP response,
  with the appropriate status code,
  and a serialized problem details object.
```

### S202: Problem Details Responses

```
As the Dev,

I want any unsuccessful request
  to be returned as an unsuccessful HTTP response,
  with the appropriate status code,
  and a serialized problem details object.
```

## S3: Security

### S301: Admin API Security

```
As the Dev,

I want to block all Admin API requests
  which do not include a secret Admin API key as a request header,

so that I can keep the system data secure.
```

### S302: Public API Security

```
As the Dev,

I want to block all Public API requests
  which do not include either a Public API key or a secret Admin API key as a request header,

so that I can exert a little control over who is making requests.
```

## S4: Versioning

### S401: Versioned API Releases

```
As the Dev,

I want to use major+minor versioning for both APIs,
  and release each minor version
  of each API
  as its own named release.
```
