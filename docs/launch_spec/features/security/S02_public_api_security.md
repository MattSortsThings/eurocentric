# S02. Public API security

This document is part of the [launch specification](../../README.md).

## User story

- **As the Dev**
  - **given** I have defined a demo API key that grants the "Reader" role
  - **given** I have defined a secret API key that grants the "Administrator" and "Reader" roles
- **I want** requests to the Public API to require either the demo API key or the secret API key as an `"X-Api-Key"` header
- **So that** I can exert a degree of control over who can use the Public API endpoints

## Acceptance criteria

**Any Public API endpoint...**

- [ ] Authenticates_and_authorizes_request_with_secret_API_key_as_XApiKey_header
- [ ] Authenticates_and_authorizes_request_with_demo_API_key_as_XApiKey_header
- [ ] Does_not_authenticate_request_with_unrecognized_API_key_as_XApiKey_header
- [ ] Does_not_authenticate_request_with_no_XApiKey_header
