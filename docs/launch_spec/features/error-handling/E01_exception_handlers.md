# E01. Exception handlers

## User story

- **As the Dev**
- **I want** an exception thrown on the server to be caught and returned as an unsuccessful HTTP response with a problem details object
- **So that** the user can understand what went wrong.

## Acceptance criteria

**API endpoint...**

- [ ] Should_return_400_BadRequest_with_ProblemDetails_on_missing_required_query_param
- [ ] Should_return_400_BadRequest_with_ProblemDetails_on_invalid_enum_query_param_string_value
- [ ] Should_return_400_BadRequest_with_ProblemDetails_on_invalid_enum_query_param_int_value
- [ ] Should_return_400_BadRequest_with_ProblemDetails_on_missing_required_property
- [ ] Should_return_400_BadRequest_with_ProblemDetails_on_invalid_enum_property_string_value
- [ ] Should_return_400_BadRequest_with_ProblemDetails_on_enum_property_int_value
- [ ] Should_return_503_ServiceUnavailable_with_RetryAfter_and_ProblemDetails_on_db_timeout
- [ ] Should_return_500_InternalServerError_with_ProblemDetails_on_general_exception
