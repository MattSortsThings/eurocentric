# S01 API key authentication

This document is part of the [*Eurocentric* launch specification](../../README.md).

**Feature scope:** *security*

## User story

- **As the Dev**, having defined a demo API key and a secret API key
- **I want** an Admin API or Public API HTTP request to be authenticated based on the presence of a recognized API key
- **So that** I can exert a degree of control over who uses the APIs.

## Acceptance criteria

**Admin API V0 endpoints...**

- Should_authenticate_request_using_demo_API_key
- Should_authenticate_request_using_secret_API_key
- Should_not_authenticate_request_using_unrecognized_API_key
- Should_not_authenticate_request_using_no_API_key

**Admin API V1 endpoints...**

- Should_authenticate_request_using_demo_API_key
- Should_authenticate_request_using_secret_API_key
- Should_not_authenticate_request_using_unrecognized_API_key
- Should_not_authenticate_request_using_no_API_key

**Public API V0 endpoints...**

- Should_authenticate_request_using_demo_API_key
- Should_authenticate_request_using_secret_API_key
- Should_not_authenticate_request_using_unrecognized_API_key
- Should_not_authenticate_request_using_no_API_key

**Public API V0 endpoints...**

- Should_authenticate_request_using_demo_API_key
- Should_authenticate_request_using_secret_API_key
- Should_not_authenticate_request_using_unrecognized_API_key
- Should_not_authenticate_request_using_no_API_key
