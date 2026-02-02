# 9 Development plan

This document is part of the [*Eurocentric* launch specification](README.md).

- [9 Development plan](#9-development-plan)
  - [Development phases](#development-phases)
    - [Phase 1: Cross-cutting features](#phase-1-cross-cutting-features)
    - [Phase 2: Admin API v1.0](#phase-2-admin-api-v10)
    - [Phase 3: Public API v1.0](#phase-3-public-api-v10)
    - [Phase 4: Tidy-up](#phase-4-tidy-up)
  - [Testing strategy](#testing-strategy)
    - [Acceptance tests](#acceptance-tests)
    - [Unit tests](#unit-tests)
    - [Architecture tests](#architecture-tests)
  - [Version control](#version-control)
    - [Commit message subject line rules](#commit-message-subject-line-rules)
    - [Commit message body rules](#commit-message-body-rules)
    - [Examples](#examples)

## Development phases

Development is split into 4 phases.

### Phase 1: Cross-cutting features

Define versions 0.1 and 0.2 of the Admin API and the Public API, each with a few features. The features should be fully functional, using minimal placeholder domain types and a temporary database schema.

Once the version 0.x API versions are up and running, implement all cross-cutting features, i.e. those with feature scopes *error-handling*, *logging*, *open-api*, *security* and *versioning*.

### Phase 2: Admin API v1.0

Define version 1.0 of the Admin API, then implement all *admin-api* features.

### Phase 3: Public API v1.0

Define version 1.0 of the Public API, then implement all *public-api* features.

### Phase 4: Tidy-up

Refactor the version 0.x API versions to use the same database schema and domain types as the version 1.x API releases. Finally, remove all redundant code.

## Testing strategy

All tests use [TUnit](https://tunit.dev/). Three separate test projects are used.

### Acceptance tests

This is the primary test project. Acceptance tests are written for every feature, using an in-memory web application and a containerized SQL Server instance.

Acceptance tests for `admin-api` and `public-api` features are written in first-person Gherkin syntax, using the Admin and EuroFan [user personas](01_introduction.md/#user-personas) respectively.

Acceptance tests for cross-cutting features are written using arrange/act/assert test methods and no explicit user persona.

### Unit tests

Unit tests are written to verify the behaviour of domain aggregate public methods and of utility methods across the source code.

### Architecture tests

Code design rules are enforced across all source code assemblies using architecture tests. These are written using [ArchUnitNET](https://github.com/TNG/ArchUnitNET).

## Version control

Commit messages use the [Conventional Commits](https://www.conventionalcommits.org/en/v1.0.0/) standard.

Acceptable commit types are: *feat*, *fix*, *refactor*, *test*, *ci*, *deps*, *style*, *docs*, *chore*.

Acceptable feature scopes are: *admin-api*, *public-api*, *error-handling*, *logging*, *open-api*, *security* and *versioning*.

### Commit message subject line rules

1. Subject line is at most 50 characters long.
2. Subject line's first word after colon is an *infinitive verb*, such that the subject can be read as "When you apply the changes in this commit, you will \<subject\>".
3. Subject line is all lower-case, except for package and product names.
4. Subject line is followed by an empty line.

### Commit message body rules

1. Message body lines are at most 72 characters long.
2. Message body is written in the past tense.
3. Message body may tag one or more GitHub issues, e.g. `Closes: #72.`
4. Message body must never use the colon character except for issue tagging.
5. Message body may contain an unordered list, which must be preceded by and followed by an empty line, and must use hanging indentation.
6. The name of a software type, namespace, or package is enclosed in backticks.
7. The name of a feature scope is enclosed in double quotation marks
8. A feature is referenced by its code and name, enclosed in double quotation marks.

### Examples

```git
deps: bump TUnit to 1.3.25

Updated the `TUnit` package to version 1.3.25.
```

```git
feat(admin-api): add country creation

Implemented the feature "A14. Create country".

The Admin is now able to create a new country in the system using
version 1.0 of the Admin API.

Closes: #40
```
