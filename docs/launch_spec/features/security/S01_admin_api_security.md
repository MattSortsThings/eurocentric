# S01. Admin API security

This document is part of the [launch specification](../../README.md).

## User story

- **As the Dev**
  - **given** I have defined a demo API key that grants the "Reader" role
  - **given** I have defined a secret API key that grants the "Administrator" and "Reader" roles
- **I want** requests to the Admin API to require the secret API key as an `"X-Api-Key"` header
- **So that** only the Admin can use the Admin API endpoints

## Acceptance criteria

**Any Admin API endpoint...**

- [ ] Authenticates_and_authorizes_request_with_secret_API_key_as_XApiKey_header
- [ ] Authenticates_but_does_not_authorize_request_with_demo_API_key_as_XApiKeyy_header
- [ ] Does_not_authenticate_request_with_unrecognized_API_key_as_XApiKey_header
- [ ] Does_not_authenticate_request_without_XApiKey_header
