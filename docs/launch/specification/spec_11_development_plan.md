# 11. Development plan

This document is part of the [launch specification](../README.md#launch-specification).

- [11. Development plan](#11-development-plan)
  - [Development phases](#development-phases)
    - [1. API framework](#1-api-framework)
    - [2. Admin API v1.0](#2-admin-api-v10)
    - [3. Public API v1.0](#3-public-api-v10)
    - [4. Tidy-up](#4-tidy-up)
  - [Testing](#testing)
    - [Acceptance tests](#acceptance-tests)
    - [Unit tests](#unit-tests)
    - [Architecture tests](#architecture-tests)
  - [Continuous integration](#continuous-integration)

## Development phases

The launch development is divided into four phases.

### 1. API framework

Define versions 0.1 and 0.2 of the Admin API and the Public API, using placeholder types and `v0` db schema. Implement all non-functional features (feature scopes: *error-handling*, *open-api*, *security* and *versioning*).

### 2. Admin API v1.0

Define version 1.0 of the Admin API. Implement all functional features with *admin-api* feature scope.

### 3. Public API v1.0

Define version 1.0 of the Public API. Implement all functional features with *public-api* feature scope.

### 4. Tidy-up

Refactor and deprecate versions 0.1 and 0.2 of the Admin API and the Public API. Remove placeholder types and `v0` db schema.

## Testing

The system is developed using **acceptance test driven development**.

All tests use [TUnit](https://tunit.dev/).

### Acceptance tests

Acceptance tests use an in-memory web application, with a real SQL Server database running in a Docker container. The workflow is as follows:

1. Select a feature for development.
2. List happy and sad path scenarios for the feature.
3. Write scenario specifications using Gherkin syntax.
4. Write a failing acceptance test for each specification.
5. Implement the feature using unit tests where necessary.
6. When all acceptance tests pass, the feature is implemented.
7. Tidy up code.
8. Add or update acceptance tests that need to be modified or created given the new feature.

### Unit tests

Unit tests are written for all four class libraries.

### Architecture tests

Architecture tests are written for all four class libraries, using ArchUnitNET.

## Continuous integration

A GitHub action executes the following job on every push to the remote repository main branch:

- publish_and_deploy:
  1. Restore the solution.
  2. Build the solution.
  3. Run all tests in the solution.
  4. Publish the `Eurocentric.WebApp` web application.
  5. Deploy the web application to the Azure Web App service.
