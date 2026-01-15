# 5 Domain invariants

This document is part of the [*Eurocentric* launch specification](README.md).

- [5 Domain invariants](#5-domain-invariants)
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

The following invariants apply to domain value objects.

| Type                   | Value must be                                                                                                              |
|:-----------------------|:---------------------------------------------------------------------------------------------------------------------------|
| `ActName`              | a non-empty, non-whitespace, single-line string of no more than 100 characters, that does not begin or end with whitespace |
| `BroadcastDate`        | a date with a year between 2016 and 2050                                                                                   |
| `CityName`             | a non-empty, non-whitespace, single-line string of no more than 100 characters, that does not begin or end with whitespace |
| `ContestYear`          | an integer between 2016 and 2050                                                                                           |
| `CountryCode`          | a string of 2 upper-case letters                                                                                           |
| `CountryName`          | a non-empty, non-whitespace, single-line string of no more than 100 characters, that does not begin or end with whitespace |
| `FinishingSpot`        | an integer &geq; 1                                                                                                         |
| `PointsValue`          | an integer &geq; 0                                                                                                         |
| `RunningOrderSpot`     | an integer &geq; 1                                                                                                         |
| `SongTitle`            | a non-empty, non-whitespace, single-line string of no more than 100 characters, that does not begin or end with whitespace |

## Transaction invariants

### Create country

- A `Country`'s ID is its unique system identifier, assigned on the server
- Every `Country` has a unique country code

### Delete country

- A `Country` with one or more contest roles cannot be deleted

### Create contest

- A `Contest`'s ID is its unique system identifier, assigned on the server
- Every `Contest` has a unique contest year
- The global televote, if present, must reference an existing pseudo `Country`
- Each participant must reference an existing real `Country`
- Each participant must reference a different `Country`
- There must be at least 3 participants with `SemiFinalDraw=SemiFinal1`
- There must be at least 3 participants with `SemiFinalDraw=SemiFinal2`
- Every participating `Country` in the created `Contest` must add a corresponding contest role
- The global televote voting `Country` in the created `Contest`, if present, must add a corresponding contest role

### Delete contest

- A `Contest` with one or more broadcast memos cannot be deleted
- Every participating `Country` in the deleted `Contest` must remove the corresponding contest role
- The global televote voting `Country` in the deleted `Contest`, if present, must remove the corresponding contest role

### Create contest child broadcast

- A `Broadcast`'s ID is its unique system identifier, assigned on the server
- Every `Broadcast` has a unique broadcast date
- Every `Broadcast` has a unique (parent `Contest` ID, contest stage) tuple
- A `Broadcast`'s broadcast date must match the year of its parent `Contest`
- Each competitor must reference a`Country` with a participant in the parent `Contest` that may compete in the contest stage
- Each competitor must reference a different `Country`
- There must be at least 3 competitors
- The difference in length between the two broadcast halves must not be greater than 2
- The parent `Contest` of the created `Broadcast` must add a corresponding broadcast memo

### Delete broadcast

- The parent `Contest` of the deleted `Broadcast` must remove the corresponding broadcast memo and set itself to `Queryable=false` if it was previously `Queryable=true`

### Award broadcast televote points

- The voting `Country` ID must reference a televote voting `Country` in the `Broadcast` that has not awarded its points
- The ranked competing `Country` IDs must contain no duplicates
- The ranked competing `Country` IDs must not include the voting `Country` ID
- The ranked competing `Country` IDs must be equal to the set of all competing `Country` IDs in the `Broadcast` excluding the voting `Country` ID
- If the `Broadcast` is set to `Completed=true`, the parent `Contest` of the completed `Broadcast` must replace the corresponding broadcast memo and set itself to `Queryable=true` if it now has 3 broadcast memos, all `Queryable=true`

### Award broadcast jury points

- The voting `Country` ID must reference a jury voting `Country` in the `Broadcast` that has not awarded its points
- The ranked competing `Country` IDs must contain no duplicates
- The ranked competing `Country` IDs must not include the voting `Country` ID
- The ranked competing `Country` IDs must be equal to the set of all competing `Country` IDs in the `Broadcast` excluding the voting `Country` ID
- If the `Broadcast` is set to `Completed=true`, the parent `Contest` of the completed `Broadcast` must replace the corresponding broadcast memo and set itself to `Queryable=true` if it now has 3 broadcast memos, all `Queryable=true`
