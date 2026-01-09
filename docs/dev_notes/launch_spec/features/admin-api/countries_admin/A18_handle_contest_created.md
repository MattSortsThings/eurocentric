# A18 Handle contest created

This document is part of the *Eurocentric* [launch specification](../../../README.md).

- [A18 Handle contest created](#a18-handle-contest-created)
  - [User story](#user-story)
  - [Acceptance criteria](#acceptance-criteria)

## User story

- **As the Admin**, when I am creating a contest
- **I want** every participating country and global televote voting country to update itself with a contest role in the created contest
- **So that** none of the participating/voting countries can be deleted.

## Acceptance criteria

**HandleContestCreated event handler...**

- should add contest roles to countries when contest without global televote created
- should add contest roles to countries when contest with global televote created
- should update no countries when contest creation fails
