# L01. Correlated log entries

This document is part of the [*Eurocentric* launch specification](../../../README.md).

- [L01. Correlated log entries](#l01-correlated-log-entries)
  - [User story](#user-story)
  - [Acceptance criteria](#acceptance-criteria)

## User story

- **As the Dev**
- **I want** every API HTTP request to generate a timestamped log entry for its HTTP request, internal request, internal result, HTTP response and/or exception, all with a single correlation ID returned to the user an `"X-Correlation-Id"` HTTP response header
- **So that** endpoint usage is tracked and I can trace any request's path through the system.

## Acceptance criteria

**API endpoint...**

- [ ] Should_create_correlated_log_entries_when_HTTP_request_succeeds
- [ ] Should_create_correlated_log_entries_when_HTTP_request_is_not_authenticated
- [ ] Should_create_correlated_log_entries_when_HTTP_request_is_not_authorized
- [ ] Should_create_correlated_log_entries_when_HTTP_request_fails_due_to_domain_error
- [ ] Should_create_correlated_log_entries_when_HTTP_request_fails_due_to_BadHttpRequestException
- [ ] Should_create_correlated_log_entries_when_HTTP_request_fails_due_to_DbTimeoutException
- [ ] Should_create_correlated_log_entries_when_HTTP_request_fails_due_to_general_exception
