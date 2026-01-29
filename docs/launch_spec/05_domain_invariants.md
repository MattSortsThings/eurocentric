# 5. Domain invariants

This document is part of the [*Eurocentric* launch specification](README.md).

- [5. Domain invariants](#5-domain-invariants)
  - [Value object invariants](#value-object-invariants)
  - [Transaction invariants](#transaction-invariants)
    - [Create country](#create-country)
    - [Delete country](#delete-country)
    - [Create contest](#create-contest)
    - [Delete contest](#delete-contest)
    - [Create contest child broadcast](#create-contest-child-broadcast)
    - [Delete broadcast](#delete-broadcast)
    - [Award broadcast televote points](#award-broadcast-televote-points)
    - [Award broadcast jury points](#award-broadcast-jury-points)

## Value object invariants

Value object invariants are defined in the [domain model](03_domain_model.md).

A value object can *never* exist with an illegal value.

Compound value objects have a public constructor.

Atomic value objects have a private constructor. Most atomic value objects have two public static factory methods:

- `TryCreate` method returns *either* a legal value object *or* a domain error if the provided value was illegal.
- `FromValue` method (used for EF Core value conversion) *either* returns a legal value object *or* throws an `ArgumentException` if the provided value was illegal.

## Transaction invariants

A [domain transaction](04_domain_transactions.md) must satisfy all its invariants (listed below), otherwise it fails and is rolled back.

### Create country

- A `Country`'s ID is its unique system identifier, assigned on the server.
- Every `Country` has a unique country code.

### Delete country

- A `Country` with a non-empty `ContestId` collection cannot be deleted.

### Create contest

- A `Contest`'s ID is its unique system identifier, assigned on the server.
- Every `Contest` has a unique contest year.
- The global televote, if present, must reference an existing pseudo `Country`.
- Each participant must reference an existing real `Country`.
- Each participant must reference a different `Country`.
- There must be at least 3 participants with `SemiFinalDraw=SemiFinal1`.
- There must be at least 3 participants with `SemiFinalDraw=SemiFinal2`.
- Every participating `Country` in the created `Contest` must add its ID to its `ContestId` collection.
- The global televote voting `Country` in the created `Contest`, if present, must add its ID to its `ContestId` collection.

### Delete contest

- A `Contest` with one or more broadcast memos cannot be deleted.
- Every participating `Country` in the deleted `Contest` must remove its ID from its `ContestId` collection.
- The global televote voting `Country` in the deleted `Contest`, if present, must remove its ID from its `ContestId` collection.

### Create contest child broadcast

- A `Broadcast`'s ID is its unique system identifier, assigned on the server.
- Every `Broadcast` has a unique broadcast date.
- Every `Broadcast` has a unique (parent `Contest` ID, contest stage) tuple.
- A `Broadcast`'s broadcast date must match the year of its parent `Contest`.
- Each competitor must reference a `Country` with a participant in the parent `Contest` that is eligible to compete in the contest stage.
- Each competitor must reference a different `Country`.
- There must be at least 3 competitors.
- The difference in length between the two broadcast halves must not be greater than 2.
- The parent `Contest` of the created `Broadcast` must add a corresponding broadcast memo.

### Delete broadcast

- The parent `Contest` of the deleted `Broadcast` must remove the corresponding broadcast memo and set itself to `Queryable=false` if it was previously `Queryable=true`.

### Award broadcast televote points

- The voting `CountryId` must reference a televote voting `Country` in the `Broadcast` that has not awarded its points.
- The ranked competing `CountryId`s must contain no duplicates.
- The ranked competing `CountryId`s must not include the voting `CountryId`.
- The ranked competing `CountryId`s must not include a `CountryId` matching no competitor.
- The ranked competing `CountryId`s must include the competing `CountryId` of every competitor excluding the voting `CountryId`.
- If the `Broadcast` is set to `Completed=true`, the parent `Contest` of the completed `Broadcast` must replace the corresponding broadcast memo and set itself to `Queryable=true` if it now has 3 broadcast memos, all `Queryable=true`.

### Award broadcast jury points

- The voting `CountryId` must reference a jury voting `Country` in the `Broadcast` that has not awarded its points.
- The ranked competing `CountryId`s must contain no duplicates.
- The ranked competing `CountryId`s must not include the voting `CountryId`.
- The ranked competing `CountryId`s must not include a `CountryId` matching no competitor.
- The ranked competing `CountryId`s must include the competing `CountryId` of every competitor excluding the voting `CountryId`.
- If the `Broadcast` is set to `Completed=true`, the parent `Contest` of the completed `Broadcast` must replace the corresponding broadcast memo and set itself to `Queryable=true` if it now has 3 broadcast memos, all `Queryable=true`.
