# 08: Features: *security* scope

This document is part of the [launch specification](../README.md#launch-specification).

- [08: Features: *security* scope](#08-features-security-scope)
  - [Authentication](#authentication)
    - [cs01: API Key Authentication](#cs01-api-key-authentication)
  - [Authorization](#authorization)
    - [cs02: Admin API Authorization](#cs02-admin-api-authorization)
    - [cs03: Public API Authorization](#cs03-public-api-authorization)

## Authentication

### cs01: API Key Authentication

**Details:**

- The application defines two API keys: a SECRET_API_KEY and a DEMO_API_KEY.
- The application internally defines two roles: "administrator" and "reader"
- An HTTP request must include an `"X-Api-Key"` header with either the SECRET_API_KEY or the DEMO_API_KEY.
- A client using the SECRET_API_KEY is assigned the "administrator" and "reader" roles.
- A client using the DEMO_API_KEY is assigned the "reader" role.

## Authorization

### cs02: Admin API Authorization

**Details:**

- Admin API endpoints can only be accessed by an authenticated client assigned the "administrator" role.

### cs03: Public API Authorization

**Details:**

- Admin API endpoints can only be accessed by an authenticated client assigned the "reader" role.
