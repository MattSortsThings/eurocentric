# V02. Version reporting

This document is part of the [launch specification](../../README.md).

## User story

- **As the Dev**
- **I want** every API endpoint path to include a major/minor URL version segment
- **So that** the user must specify the API version they want to use.

## Acceptance criteria

**Any Admin API endpoint...**

- [ ] Handles_request_when_client_uses_API_version_supported_by_endpoint
- [ ] Fails_when_client_uses_API_version_unsupported_by_endpoint
- [ ] Fails_when_client_uses_malformed_API_version

**Any Public API endpoint...**

- [ ] Handles_request_when_client_uses_API_version_supported_by_endpoint
- [ ] Fails_when_client_uses_API_version_unsupported_by_endpoint
- [ ] Fails_when_client_uses_malformed_API_version
