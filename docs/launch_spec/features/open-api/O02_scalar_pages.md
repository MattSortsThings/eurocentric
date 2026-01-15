# O02 Scalar pages

This document is part of the [*Eurocentric* launch specification](../../README.md).

**Feature scope:** *open-api*

## User story

- **As the Dev**
- **I want** the system to serve a Scalar documentation UI page for every OpenAPI document, from an endpoint that bypasses authentication
- **So that** a user can get an overview of how an API release is used

## Acceptance criteria

### Happy path

**Docs endpoint...**

- Should_succeed_with_200_OK_and_HTML_doc_when_AdminApiV0Point1_doc_requested_by_anonymous
- Should_succeed_with_200_OK_and_HTML_doc_when_AdminApiV0Point2_doc_requested_by_anonymous
- Should_succeed_with_200_OK_and_HTML_doc_when_AdminApiV1Point0_doc_requested_by_anonymous
- Should_succeed_with_200_OK_and_HTML_doc_when_PublicApiV0Point1_doc_requested_by_anonymous
- Should_succeed_with_200_OK_and_HTML_doc_when_PublicApiV0Point2_doc_requested_by_anonymous
- Should_succeed_with_200_OK_and_HTML_doc_when_PublicApiV1Point0_doc_requested_by_anonymous
- Should_succeed_with_200_OK_and_HTML_doc_when_non_existent_doc_requested_by_anonymous
