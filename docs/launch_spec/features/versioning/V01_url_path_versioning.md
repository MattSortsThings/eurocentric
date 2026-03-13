# V01. URL path versioning

This document is part of the [launch specification](../../README.md).

## User story

- **As the Dev**, having implemented major/minor URL versioning for Admin API and Public API endpoints
- **I want** every Admin API and Public API endpoint HTTP response to include an `"api-supported-versions"` header and an optional `"api-deprecated-versions"` header
- **So that** the user can confirm that they are using the most up-to-date API version

## Acceptance criteria

**Any Admin API endpoint...**

- [ ] Reports_all_Admin_API_versions_when_request_succeeds
- [ ] Reports_all_Admin_API_versions_when_request_is_not_authenticated
- [ ] Reports_all_Admin_API_versions_when_request_fails_with_DomainError

**Any Public API endpoint...**

- [ ] Reports_all_Public_API_versions_when_request_succeeds
- [ ] Reports_all_Public_API_versions_when_request_is_not_authenticated
- [ ] Reports_all_Public_API_versions_when_request_fails_with_DomainError
