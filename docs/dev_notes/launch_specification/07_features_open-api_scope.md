# 07: Features: *open-api* scope

This document is part of the [launch specification](../README.md#launch-specification).

- [07: Features: *open-api* scope](#07-features-open-api-scope)
  - [OpenAPI Documents](#openapi-documents)
    - [co01: OpenAPI Endpoint](#co01-openapi-endpoint)
  - [Scalar Documentation](#scalar-documentation)
    - [co02: Docs Endpoint](#co02-docs-endpoint)

## OpenAPI Documents

### co01: OpenAPI Endpoint

**Endpoint:**

```http
GET /openapi/{docName}.json
```

**Details:**

- The system generates an OpenAPI v3.1 JSON document for every release of each API.
- The system exposes an `/openapi` endpoint, which is accessible by unauthenticated clients.
- The client can retrieve an OpenAPI document by providing its name, as shown in the route template.

## Scalar Documentation

### co02: Docs Endpoint

**Endpoint:**

```http
GET /docs/{docName}
```

**Details:**

- The system serves a Scalar documentation webpage for every generated OpenAPI document.
- The system exposes a `/docs` endpoint, which is accessible by unauthenticated clients.
- The client can retrieve a documentation webpage by providing the OpenAPI document name, as shown in the route template.
