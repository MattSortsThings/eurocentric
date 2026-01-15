# O01 OpenAPI docs

This document is part of the [*Eurocentric* launch specification](../../README.md).

**Feature scope:** *open-api*

## User story

- **As the Dev**
- **I want** the system to serve an OpenAPI JSON document for every Admin API and Public API release from an endpoint that bypasses authentication
- **So that** a developer can generate an API client from an OpenAPI document

## Acceptance criteria

### Happy path

**OpenApi endpoint...**

- Should_succeed_with_200_OK_and_JSON_doc_when_AdminApiV0Point1_doc_requested_by_anonymous
- Should_succeed_with_200_OK_and_JSON_doc_when_AdminApiV0Point2_doc_requested_by_anonymous
- Should_succeed_with_200_OK_and_JSON_doc_when_AdminApiV1Point0_doc_requested_by_anonymous
- Should_succeed_with_200_OK_and_JSON_doc_when_PublicApiV0Point1_doc_requested_by_anonymous
- Should_succeed_with_200_OK_and_JSON_doc_when_PublicApiV0Point2_doc_requested_by_anonymous
- Should_succeed_with_200_OK_and_JSON_doc_when_PublicApiV1Point0_doc_requested_by_anonymous

### Sad path

**OpenApi endpoint...**

- Should_fail_when_requested_doc_does_not_exist
