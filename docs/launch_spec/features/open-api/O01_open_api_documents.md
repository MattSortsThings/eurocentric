# O01. OpenApi documents

This document is part of the [*Eurocentric* launch specification](../../../README.md).

- [O01. OpenApi documents](#o01-openapi-documents)
  - [User story](#user-story)
  - [Acceptance criteria](#acceptance-criteria)
    - [Happy path](#happy-path)
    - [Sad path](#sad-path)

## User story

- **As the Dev**
- **I want** the system to serve an OpenAPI v3.1 JSON document for every version of every API, from an anonymous endpoint
- **So that** a developer can generate a client to work with an API.

## Acceptance criteria

### Happy path

**OpenAPI endpoint...**

- [ ] Should_succeed_with_200_OK_and_requested_OpenAPI_JSON_document_when_request_is_valid
- [ ] Should_return_OpenAPI_document_containing_correct_info_for_API_version
- [ ] Should_return_OpenAPI_document_containing_correct_server_URL_for_API_version
- [ ] Should_return_OpenAPI_document_containing_correct_operations_for_API_version
- [ ] Should_return_OpenAPI_document_containing_correct_paths_for_API_version
- [ ] Should_return_OpenAPI_document_containing_correct_schemas_for_API_version

### Sad path

**OpenAPI endpoint...**

- [ ] Should_fail_when_OpenAPI_document_does_not_exist
