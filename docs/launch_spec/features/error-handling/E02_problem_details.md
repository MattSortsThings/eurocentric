# E02. Problem details

This document is part of the [*Eurocentric* launch specification](../../../README.md).

- [E02. Problem details](#e02-problem-details)
  - [User story](#user-story)
  - [Acceptance criteria](#acceptance-criteria)

## User story

- **As the Dev**
- **I want** every API endpoint to return *either* a successful HTTP response *or* an unsuccessful HTTP response with a problem details object
- **So that** the user can understand why their unsuccessful request failed.

## Acceptance criteria

**API endpoint...**

- [ ] Should_return_404_NotFound_with_ProblemDetails_on_NotFound_domain_error
- [ ] Should_return_409_Conflict_with_ProblemDetails_on_Extrinsic_domain_error
- [ ] Should_return_422_UnprocessableEntity_with_ProblemDetails_on_Intrinsic_domain_error
