# V01. API version reporting

## User story

- **As the Dev**, having implemented major/minor URL versioning for Admin API and Public API endpoints
- **I want** every Admin API and Public API endpoint HTTP response to include an `"api-supported-versions"` request header containing all supported versions of the API
- **So that** the user can confirm that they are using the most up-to-date API version.

## Acceptance criteria

**Admin API V0 endpoint...**

- [ ] Should_report_all_Admin_API_versions_when_request_succeeds
- [ ] Should_report_all_Admin_API_versions_when_request_fails

**Admin API V1 endpoint...**

- [ ] Should_report_all_Admin_API_versions_when_request_succeeds
- [ ] Should_report_all_Admin_API_versions_when_request_fails

**Public API V0 endpoint...**

- [ ] Should_report_all_Public_API_versions_when_request_succeeds
- [ ] Should_report_all_Public_API_versions_when_request_fails

**Public API V1 endpoint...**

- [ ] Should_report_all_Public_API_versions_when_request_succeeds
- [ ] Should_report_all_Public_API_versions_when_request_fails
