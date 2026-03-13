# 10. Development plan

This document is part of the [launch specification](README.md).

- [10. Development plan](#10-development-plan)
  - [Development phases](#development-phases)
    - [Phase 1: API framework](#phase-1-api-framework)
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

### Phase 1: API framework

Define versions 0.1 and 0.2 of the Admin API and the Public API, each with a few features implemented using placeholder domain types and database schema. Implement all cross-cutting features, i.e. those with feature scopes *error-handling*, *logging*, *open-api*, *security* and *versioning*.

### Phase 2: Admin API v1.0

Define version 1.0 of the Admin API, then implement all *admin-api* features.

### Phase 3: Public API v1.0

Define version 1.0 of the Public API, then implement all *public-api* features.

### Phase 4: Tidy-up

Refactor the version 0.x API releases to use the same database schema and domain types as the version 1.x API releases. Deprecate the version 0.x API releases. Finally, remove all redundant code.

## Testing strategy

All tests use [TUnit](https://tunit.dev/). Three separate test projects are used.

### Acceptance tests

This is the primary test project. Acceptance tests are written for every feature, using an in-memory web application and a containerized SQL Server instance.

Acceptance tests for *admin-api* and *public-api* features are written in Given/When/Then Gherkin syntax, using actor objects that emulate the Admin, EuroFan and Dev user personas.

Acceptance tests for cross-cutting features use the standard Arrange/Act/Assert test method layout and no user personas.

### Unit tests

Unit tests are written to verify the behaviour of domain aggregate public methods and of utility methods across the source code.

### Architecture tests

Code design rules are enforced across all source code assemblies using architecture tests. These are written using [ArchUnitNET](https://github.com/TNG/ArchUnitNET).

## Version control

Commit messages use the [Conventional Commits](https://www.conventionalcommits.org/en/v1.0.0/) standard.

Permitted *commit types* are:

- `build`
- `chore`
- `ci`
- `docs`
- `feat`
- `fix`
- `perf`
- `refactor`
- `style`
- `test`

Permitted *commit scopes* are:

- `admin-api`
- `error-handling`
- `logging`
- `open-api`
- `public-api`
- `security`
- `versioning`

The commit type decision table below is adapted from [this GitHub Gist comment](https://gist.github.com/qoomon/5dfcdf8eec66a051ecd85625518cfd13?permalink_comment_id=5893039#gistcomment-5893039).

| Order | Question                                                              | If Yes → Type |
|------:|:----------------------------------------------------------------------|:--------------|
|     1 | Updated source code to fix a bug?                                     | `fix`         |
|     2 | Updated source code to add or modify a feature?                       | `feat`        |
|     3 | Updated source code to improve performance?                           | `perf`        |
|     4 | Updated source code structure without changing behaviour              | `refactor`    |
|     5 | Updated source code formatting without changing structure?            | `style`       |
|     6 | Updated test code?                                                    | `test`        |
|     7 | Updated documentation?                                                | `docs`        |
|     8 | Updated build files, 3rd-party dependencies, or SDK version?          | `build`       |
|     9 | Updated continuous integration files?                                 | `ci`          |
|    10 | Last resort, e.g. initial commit, .gitignore update, anything else... | `chore`       |


### Commit message subject line rules

1. Subject line is at most 50 characters long
2. Subject line's first word after colon is an *infinitive verb*, such that the subject can be read as "When you apply the changes in this commit, you will \<subject\>"
3. Subject line is all lower-case, except for type, package and product names
4. Subject line is followed by an empty line

### Commit message body rules

1. Message body lines are at most 72 characters long
2. Message body is written in the past tense
3. Message body may tag one or more GitHub issues, e.g. `Closes: #72`
4. Message body must never use the colon character except for issue tagging
5. Message body may contain an unordered list, which must be preceded by and followed by an empty line, and must use hanging indentation
6. The name of a software type, namespace, or package is enclosed in backticks
7. The name of a feature scope is enclosed in backticks
8. A feature is referenced by its code and name, enclosed in double quotation marks

### Examples

```git
build: bump TUnit to 1.3.25

Updated the `TUnit` package to version 1.3.25.
```

```git
feat(admin-api): add country creation

Implemented the feature "A14. Create country".

The Admin is now able to create a new country in the system using
version 1.0 of the Admin API.

Closes: #40
```
