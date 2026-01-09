# A19 Handle contest deleted

This document is part of the *Eurocentric* [launch specification](../../../README.md).

- [A19 Handle contest deleted](#a19-handle-contest-deleted)
  - [User story](#user-story)
  - [Acceptance criteria](#acceptance-criteria)

## User story

- **As the Admin**, when I am deleting a contest
- **I want** every participating country and global televote voting country to update itself by removing the contest role referencing the deleted contest
- **So that** countries with no contest roles can be deleted

## Acceptance criteria

**HandleContestDeleted event handler...**

- should remove contest role from every participating country when contest without global televote deleted
- should remove contest role from every voting and participating country when contest with global televote deleted
- should update no countries when contest deletion fails
