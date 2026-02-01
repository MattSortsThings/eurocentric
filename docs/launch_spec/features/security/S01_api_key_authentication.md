# S01. API key authentication

## User story

- **As the Dev**, having defined a demo API key and a secret API key
- **I want** every request to an Admin API and Public API endpoint to be authenticated based on the presence of a recognized API key as an `"X-Api-Key"` header
- **So that** I can exert a degree of control over who uses the APIs.

## Acceptance criteria

**Admin API endpoint...**

- [ ] Should_authenticate_request_using_demo_API_key
- [ ] Should_authenticate_request_using_secret_API_key
- [ ] Should_not_authenticate_request_using_unrecognized_API_key
- [ ] Should_not_authenticate_request_using_no_API_key

**Public API endpoint...**

- [ ] Should_authenticate_request_using_demo_API_key
- [ ] Should_authenticate_request_using_secret_API_key
- [ ] Should_not_authenticate_request_using_unrecognized_API_key
- [ ] Should_not_authenticate_request_using_no_API_key
