# E02 Problem details

This document is part of the [*Eurocentric* launch specification](../../README.md).

**Feature scope:** *errors*

## User story

- **As the Dev**
- **I want** an unsuccessful HTTP request to return an unsuccessful HTTP response with a `ProblemDetails` object
- **So that** the API user can understand what went wrong

## Acceptance criteria

**API endpoints...**

- Should_return_400_BadRequest_with_ProblemDetails_when_BadRequest_error_occurs
- Should_return_404_NotFound_with_ProblemDetails_when_NotFound_error_occurs
- Should_return_409_Conflict_with_ProblemDetails_when_Conflict_error_occurs
- Should_return_422_UnprocessableEntity_with_ProblemDetails_when_Unprocessable_error_occurs
