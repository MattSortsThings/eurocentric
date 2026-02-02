# V02. API version URL segments

This document is part of the [*Eurocentric* launch specification](../../../README.md).

- [V02. API version URL segments](#v02-api-version-url-segments)
  - [User story](#user-story)
  - [Acceptance criteria](#acceptance-criteria)

## User story

- **As the Dev**
- **I want** every Admin API and Public API endpoint path to contain a major/minor API version URL segment
- **So that** the user is forced to specify the API version they are using.

## Acceptance criteria

**Admin API endpoint...**

- [ ] Should_handle_request_when_client_uses_API_version_supported_by_endpoint
- [ ] Should_fail_when_client_uses_API_version_unsupported_by_endpoint
- [ ] Should_fail_when_client_uses_API_version_unsupported_by_API
- [ ] Should_fail_when_client_uses_malformed_API_version

**Public API endpoint...**

- [ ] Should_handle_request_when_client_uses_API_version_supported_by_endpoint
- [ ] Should_fail_when_client_uses_API_version_unsupported_by_endpoint
- [ ] Should_fail_when_client_uses_API_version_unsupported_by_API
- [ ] Should_fail_when_client_uses_malformed_API_version
