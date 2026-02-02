# V01. API version reporting

This document is part of the [*Eurocentric* launch specification](../../../README.md).

- [V01. API version reporting](#v01-api-version-reporting)
  - [User story](#user-story)
  - [Acceptance criteria](#acceptance-criteria)

## User story

- **As the Dev**, having implemented major/minor URL versioning for Admin API and Public API endpoints
- **I want** every Admin API and Public API endpoint HTTP response to include an `"api-supported-versions"` request header containing all supported versions of the API
- **So that** the user can confirm that they are using the most up-to-date API version.

## Acceptance criteria

**Admin API endpoint...**

- [ ] Should_report_all_Admin_API_versions_when_request_succeeds
- [ ] Should_report_all_Admin_API_versions_when_request_fails_on_server
- [ ] Should_report_all_Admin_API_versions_when_client_uses_unsupported_API_version

**Public API endpoint...**

- [ ] Should_report_all_Admin_API_versions_when_request_succeeds
- [ ] Should_report_all_Admin_API_versions_when_request_fails_on_server
- [ ] Should_report_all_Admin_API_versions_when_client_uses_unsupported_API_version
