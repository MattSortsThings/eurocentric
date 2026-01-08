# Domain transactions

This document is part of the *Eurocentric* [launch specification](README.md).

- [Domain transactions](#domain-transactions)
  - [Create country](#create-country)
  - [Delete country](#delete-country)
  - [Create contest](#create-contest)
  - [Delete contest](#delete-contest)
  - [Create contest child broadcast](#create-contest-child-broadcast)
  - [Delete broadcast](#delete-broadcast)
  - [Award broadcast televote points](#award-broadcast-televote-points)
  - [Award broadcast jury points](#award-broadcast-jury-points)

## Create country

**Summary:**

- The Admin creates a new `Country`, specifying:
  - the country type,
  - the country code,
  - the country name.

**Primary effects:**

- The created `Country` exists.

**Secondary effects:**

- None.

## Delete country

**Summary:**

- The Admin deletes a `Country`, specifying:
  - the country ID.

**Primary effects:**

- The deleted `Country` no longer exists.

**Secondary effects:**

- None.

## Create contest

**Summary:**

- The Admin creates a new `Contest`, specifying:
  - the contest year,
  - the city name,
  - whether the Semi-Finals are televote-only,
  - the optional global televote voting country ID,
  - the participants, for each:
    - the participating country ID
    - the Semi-Final draw,
    - the act name,
    - the song title.

**Primary effects:**

- The created `Contest` exists, with:
  - `Completed=false`,
  - no broadcast memos.

**Secondary effects:**

- For every participant in the created `Contest`, the participating `Country` has a participant contest role referencing the `Contest`.
- If the created `Contest` has a global televote, the voting `Country` has a global televote contest role referencing the `Contest`.

## Delete contest

**Summary:**

- The Admin deletes a `Contest`, specifying:
  - the contest ID.

**Primary effects:**

- The deleted `Contest` no longer exists.

**Secondary effects:**

- No `Country` has a contest role referencing the deleted `Contest`.

## Create contest child broadcast

**Summary:**

- The Admin creates a child `Broadcast` for a `Contest`, specifying:
  - the contest ID,
  - the contest stage,
  - the broadcast date,
  - a dictionary of running order spots and competing country IDs.

**Primary effects:**

- The created `Broadcast` exists, with:
  - `Completed=false`,
  - competitors in running and finishing order, all with no points awards,
  - all juries (if any) with `PointsAwarded=false`,
  - all televotes with `PointsAwarded=false`.

**Secondary effects:**

- The parent `Contest` has a broadcast memo referencing the created `Broadcast`.

## Delete broadcast

**Summary:**

- The Admin deletes a `Broadcast`, specifying:
  - the broadcast ID.

**Primary effects:**

- The deleted `Broadcast` no longer exists.

**Secondary effects:**

- The parent `Contest` has no broadcast memo referencing the deleted `Broadcast`.
- **If** the parent `Contest` was `Completed=true`, it is now `Completed=false`.

## Award broadcast televote points

**Summary:**

- The Admin awards a set of points from a televote to the competitors in a `Broadcast`, specifying:
  - the broadcast ID,
  - the televote voting country ID,
  - all the competing countries in the `Broadcast`, excluding the voting country ID, in rank order.

**Primary effects:**

- The competitors in the `Broadcast` have an additional points award.
- The televote in the `Broadcast` is `PointsAwarded=true`.
- **If** this was the final set of points to be awarded in the `Broadcast`, it is now `Completed=true`.

**Secondary effects:**

- **If** the `Broadcast` is `Completed=true`, the corresponding broadcast memo in the parent `Contest` is now `Completed=true`.
- **If** the `Contest` now has 3 broadcast memos, all `Completed=true`, it is now `Completed=true`.

## Award broadcast jury points

**Summary:**

- The Admin awards a set of points from a jury to the competitors in a `Broadcast`, specifying:
  - the broadcast ID,
  - the jury voting country ID,
  - all the competing countries in the `Broadcast`, excluding the voting country ID, in rank order.

**Primary effects:**

- The competitors in the `Broadcast` have an additional points award.
- The jury in the `Broadcast` is `PointsAwarded=true`.
- **If** this was the final set of points to be awarded in the `Broadcast`, it is now `Completed=true`.

**Secondary effects:**

- **If** the `Broadcast` is `Completed=true`, the corresponding broadcast memo in the parent `Contest` is now `Completed=true`.
- **If** the `Contest` now has 3 broadcast memos, all `Completed=true`, it is now `Completed=true`.
