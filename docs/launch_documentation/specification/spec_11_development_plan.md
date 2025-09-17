# 11. Development plan

This document is part of the [launch specification](../README.md#launch-specification).

- [11. Development plan](#11-development-plan)
  - [Development phases](#development-phases)
    - [Infrastructure](#infrastructure)
    - [Admin API](#admin-api)
    - [Public API](#public-api)
    - [Tidy-up](#tidy-up)
  - [Testing](#testing)
    - [Acceptance tests](#acceptance-tests)
    - [Unit tests](#unit-tests)
    - [Architecture tests](#architecture-tests)
  - [Continuous integration](#continuous-integration)

## Development phases

The launch development is divided into four phases.

### Infrastructure

1. Define versions 0.1 and 0.2 of the Admin API and the Public API, using:
   1. POCO implementations of the domain model.
   2. A `v0` database schema containing tables and stored procedures supporting the v0.x endpoints.
2. Implement non-functional features (feature scopes: *errors*, *open-api*, *security* and *versioning*).

### Admin API

1. Implement version 1.0 of the Admin API (*admin-api* feature scope).

### Public API

1. Implement version 1.0 of the Public API (*public-api* feature scope).

### Tidy-up

1. Refactor versions 0.1 and 0.2 of the Admin API and the Public API to use the source code added in the previous two phases.
2. Remove the `v0` database schema.
3. Remove the POCO domain model implementations.
4. Deprecate all v0.x API versions.

## Testing

The system is developed using **acceptance test driven development**.

All tests use TUnit.

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

Unit tests are written for the `Eurocentric.Domain` and `Eurocentric.Infrastructure` class libraries.

### Architecture tests

Architecture tests are written for all four class libraries, using ArchUnitNET.

## Continuous integration

A GitHub action executes the following steps on every push to the remote repository main branch:

1. Build the solution.
2. Run all tests in the solution.
3. Publish the `Eurocentric.WebApp` assembly.
4. Deploy the web app to the Azure Web App service.
