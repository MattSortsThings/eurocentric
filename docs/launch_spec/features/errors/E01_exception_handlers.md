# E01 Exception handlers

This document is part of the [*Eurocentric* launch specification](../../README.md).

**Feature scope:** *errors*

## User story

- **As the Dev**
- **I want** an exception thrown on the server to return an unsuccessful HTTP response with a `ProblemDetails` object
- **So that** the API user can understand why their request was unsuccessful

## Acceptance criteria

**API endpoints...**

- Should_return_400_BadRequest_on_BadHttpRequestException_missing_required_request_body_property
- Should_return_400_BadRequest_on_BadHttpRequestException_invalid_enum_request_body_property
- Should_return_400_BadRequest_on_BadHttpRequestException_integer_enum_request_body_property
- Should_return_400_BadRequest_on_BadHttpRequestException_missing_required_query_parameter
- Should_return_400_BadRequest_on_BadHttpRequestException_invalid_enum_query_parameter
- Should_return_400_BadRequest_on_BadHttpRequestException_integer_enum_query_parameter
- Should_return_503_ServiceUnavailable_on_DbTimeoutException
- Should_return_500_InternalServerError_as_fallback_on_Exception_of_unspecified_type
