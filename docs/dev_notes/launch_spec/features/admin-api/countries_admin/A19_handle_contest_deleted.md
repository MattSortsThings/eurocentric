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

**HandleContestCreated event handler...**

- should remove matching contest role from all participating and voting countries when contest deleted
- should update no countries when contest deletion fails
