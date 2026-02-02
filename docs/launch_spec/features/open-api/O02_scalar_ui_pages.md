# O02. Scalar UI pages

This document is part of the [*Eurocentric* launch specification](../../../README.md).

- [O02. Scalar UI pages](#o02-scalar-ui-pages)
  - [User story](#user-story)
  - [Acceptance criteria](#acceptance-criteria)
    - [Happy path](#happy-path)

## User story

- **As the Dev**
- **I want** the system to serve a Scalar UI page for every OpenAPI document, from an anonymous endpoint
- **So that** a user can see examples and other documentation for an API version.

## Acceptance criteria

### Happy path

**Docs endpoint...**

- [ ] Should_succeed_with_200_OK_and_requested_Scalar_UI_web_page_when_request_is_valid
- [ ] Should_retrieve_empty_Scalar_UI_web_page_when_OpenAPI_document_does_not_exist
