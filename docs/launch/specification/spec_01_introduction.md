# 1. Introduction

This document is part of the [launch specification](../README.md#launch-specification).

- [1. Introduction](#1-introduction)
  - [Summary](#summary)
  - [Launch scope](#launch-scope)
    - [In scope](#in-scope)
    - [Out of scope](#out-of-scope)
  - [Data source](#data-source)
  - [Conventions](#conventions)

## Summary

*Eurocentric* is a web API for modelling and (over)analysing the Eurovision Song Contest, 2016-2025.

*Eurocentric* ("the system") is composed of two APIs - the Public API and the Admin API - with a shared domain and database.

## Launch scope

The launch scope comprises:

- Version 1.0 of the Public API.
- Version 1.0 of the Admin API.
- A single web application that serves both APIs.

### In scope

The launch release will include the following key components:

**Public API v1.0** functionality:

- The Public API lets a EuroFan run a range of queries on voting data from the Eurovision Song Contest 2016-2025.

**Admin API v1.0** functionality:

- The Admin API lets the Admin populate the system database with the queryable voting data for the Public API in a manner that imitates the real-world Eurovision voting format.

**Error handling** functionality:

- Every HTTP request received by an API returns *either* a successful HTTP response *or* an unsuccessful HTTP response with a serialized problem details object.
- Exceptions are caught by a global exception handler.

**OpenAPI** functionality:

- The web application serves an OpenAPI JSON document for each version of each API.
- The web application serves a documentation web page based on each OpenAPI document.

**Security** functionality:

- APIs use API key authentication with role-based authorization.
- The Public API is only accessible by authenticated users.
- The Admin API is only accessible by the Admin.

**Versioning** functionality:

- APIs use semantic major/minor versioning.
- HTTP responses report supported API versions.

### Out of scope

The following functionality is not included in the initial release:

- HTTP request rate limiting
- HTTP response caching
- Logging
- Database concurrency management

## Data source

The [official Eurovision website](https://www.eurovision.tv) is the source of all data for votes, act names, song titles, etc.

## Conventions

Features are classified as follows:

| Number | Requirement type | Feature scope    |
|:-------|:-----------------|:-----------------|
| fa##   | Functional       | *admin-api*      |
| fp##   | Functional       | *public-api*     |
| ne##   | Non-functional   | *error-handling* |
| no##   | Non-functional   | *open-api*       |
| ns##   | Non-functional   | *security*       |
| nv##   | Non-functional   | *versioning*     |

Domain model types are formatted as follows:

- **AGGREGATE**
- **Entity**
- *ValueObject* or *Enum*
- `value` or `Property=value`
