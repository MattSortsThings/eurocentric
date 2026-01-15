# S02 API key authorization

This document is part of the [*Eurocentric* launch specification](../../README.md).

**Feature scope:** *security*

## User story

- **As the Dev**, having associated specific user roles with the demo API key and the secret API key
- **I want** an authenticated Admin API or Public API HTTP request to be authorized based on the granted user roles
- **So that** only the Admin can use the Admin API.

## Acceptance criteria

**Admin API V0 endpoints...**

- Should_not_authorize_request_using_demo_API_key
- Should_authorize_request_using_secret_API_key

**Admin API V1 endpoints...**

- Should_not_authorize_request_using_demo_API_key
- Should_authorize_request_using_secret_API_key

**Public API V0 endpoints...**

- Should_authorize_request_using_demo_API_key
- Should_authorize_request_using_secret_API_key

**Public API V0 endpoints...**

- Should_authorize_request_using_demo_API_key
- Should_authorize_request_using_secret_API_key
