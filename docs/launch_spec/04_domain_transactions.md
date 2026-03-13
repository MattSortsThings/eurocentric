# 4. Domain transactions

This document is part of the [launch specification](README.md).

- [4. Domain transactions](#4-domain-transactions)
  - [Create country](#create-country)
  - [Delete country](#delete-country)
  - [Create contest](#create-contest)
  - [Delete contest](#delete-contest)
  - [Create contest broadcast](#create-contest-broadcast)
  - [Delete broadcast](#delete-broadcast)
  - [Award broadcast jury points](#award-broadcast-jury-points)
  - [Award broadcast televote points](#award-broadcast-televote-points)

## Create country

**Summary:**

- The Admin creates a new `Country`, specifying:
  - the country type
  - the country code
  - the country name

**Primary effects:**

- The created `Country` exists

**Secondary effects:**

- None

## Delete country

**Summary:**

- The Admin deletes a `Country`, specifying:
  - the country ID

**Primary effects:**

- The deleted `Country` no longer exists

**Secondary effects:**

- None

## Create contest

**Summary:**

- The Admin creates a new `Contest`, specifying:
  - the contest year
  - the city name
  - the Semi-Final voting format
  - the optional global televote country ID
  - the participants, for each:
    - the participating country ID
    - the Semi-Final draw
    - the act name
    - the song title

**Primary effects:**

- The created `Contest` exists, with:
  - `Queryable=false`
  - no child broadcasts

**Secondary effects:**

- For every participant in the created `Contest`, the participating `Country` holds the ID of the `Contest` in its active `ContestId` collection
- If the created `Contest` has a global televote, the global televote voting `Country` holds the ID of the `Contest` in its active `ContestId` collection

## Delete contest

**Summary:**

- The Admin deletes a `Contest`, specifying:
  - the contest ID

**Primary effects:**

- The deleted `Contest` no longer exists

**Secondary effects:**

- No `Country` holds the ID of the deleted `Contest` in its active `ContestId` collection

## Create contest broadcast

**Summary:**

- The Admin creates a `Broadcast` for a specified `Contest`, specifying:
  - the contest ID
  - the contest stage
  - the broadcast date
  - the first performing spot after the interval
  - the competing country IDs in performing order, with a disqualified competitor represented by a null value

**Primary effects:**

- The created `Broadcast` exists, with:
  - `Completed=false`
  - competitors in performing order, all with no points awards
  - competitors' finishing spots are initially assigned sequentially based on their performing order
  - all juries (if any) with `PointsAwarded=false`
  - all televotes with `PointsAwarded=false`

**Secondary effects:**

- The parent `Contest` has a child broadcast referencing the created `Broadcast`

## Delete broadcast

**Summary:**

- The Admin deletes a `Broadcast`, specifying:
  - the broadcast ID

**Primary effects:**

- The deleted `Broadcast` no longer exists

**Secondary effects:**

- The parent `Contest` has no child broadcast referencing the deleted `Broadcast`
- **If** the parent `Contest` was `Queryable=true`, it is now `Queryable=false`

## Award broadcast jury points

**Summary:**

- The Admin awards a set of points from a jury to the competitors in a `Broadcast`, specifying:
  - the broadcast ID
  - the jury voting country ID
  - all the competing countries in the `Broadcast`, excluding the voting country ID, in rank order

**Primary effects:**

- The competitors in the `Broadcast` have an additional points award
- The competitors in the `Broadcast` have updated finishing spots
- The jury in the `Broadcast` is `PointsAwarded=true`
- **If** this was the final set of points to be awarded in the `Broadcast`, it is now `Completed=true`

**Secondary effects:**

- **If** the `Broadcast` is `Completed=true`, the corresponding child broadcast in the parent `Contest` is now `Completed=true`
- **If** the `Contest` now has 3 child broadcasts, all `Completed=true`, it is now `Queryable=true`

## Award broadcast televote points

**Summary:**

- The Admin awards a set of points from a televote to the competitors in a `Broadcast`, specifying:
  - the broadcast ID
  - the televote voting country ID
  - all the competing countries in the `Broadcast`, excluding the voting country ID, in rank order

**Primary effects:**

- The competitors in the `Broadcast` have an additional points award
- The competitors in the `Broadcast` have updated finishing spots
- The televote in the `Broadcast` is `PointsAwarded=true`
- **If** this was the final set of points to be awarded in the `Broadcast`, it is now `Completed=true`

**Secondary effects:**

- **If** the `Broadcast` is `Completed=true`, the corresponding child broadcast in the parent `Contest` is now `Completed=true`
- **If** the `Contest` now has 3 child broadcasts, all `Completed=true`, it is now `Queryable=true`
