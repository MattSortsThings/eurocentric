# O01. OpenAPI documents

This document is part of the [launch specification](../../README.md).

## User story

- **As the Dev**
- **I want** the web application to serve an OpenAPI document for every API release, from an endpoint that allows anonymous requests
- **So that** users can generate a client for an API release, e.g. by using Kiota

## Acceptance criteria

**OpenAPI endpoint...**

- [ ] Succeeds_with_200_OK_and_requested_OpenAPI_document_for_anonymous_client_when_document_exists
- [ ] Serves_OpenAPI_document_with_info_section_for_API_release
- [ ] Serves_OpenAPI_document_with_server_URL_for_API_release
- [ ] Serves_OpenAPI_document_with_tags_for_API_release
- [ ] Serves_OpenAPI_document_with_paths_for_API_release
- [ ] Serves_OpenAPI_document_with_operations_for_API_release
- [ ] Serves_OpenAPI_document_with_schema_examples_for_API_release_DTO_types
- [ ] Serves_OpenAPI_document_with_schema_examples_for_API_release_RequestBody_types
- [ ] Serves_OpenAPI_document_with_schema_examples_for_API_release_POST_endpoint_ResponseBody_types
- [ ] Serves_OpenAPI_document_with_API_key_security_scheme
- [ ] Fails_when_document_does_not_exist
