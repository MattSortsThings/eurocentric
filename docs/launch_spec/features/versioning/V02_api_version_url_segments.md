# V02 API version URL segments

This document is part of the [*Eurocentric* launch specification](../../README.md).

**Feature scope:** *versioning*

## User story

- **As the Dev**
- **I want** an Admin or Public API endpoint path to include an API version url segment
- **So that** the API user must specify the API version they wish to use

## Acceptance criteria

**Admin API V0 endpoints...**

- Should_succeed_when_supported_API_version_specified
- Should_fail_with_404_NotFound_when_unsupported_API_version_specified
- Should_fail_with_404_NotFound_when_malformed_API_version_specified

**Admin API V1 endpoints...**

- Should_succeed_when_supported_API_version_specified
- Should_fail_with_404_NotFound_when_unsupported_API_version_specified
- Should_fail_with_404_NotFound_when_malformed_API_version_specified

**Public API V0 endpoints...**

- Should_succeed_when_supported_API_version_specified
- Should_fail_with_404_NotFound_when_unsupported_API_version_specified
- Should_fail_with_404_NotFound_when_malformed_API_version_specified

**Public API V0 endpoints...**

- Should_succeed_when_supported_API_version_specified
- Should_fail_with_404_NotFound_when_unsupported_API_version_specified
- Should_fail_with_404_NotFound_when_malformed_API_version_specified
