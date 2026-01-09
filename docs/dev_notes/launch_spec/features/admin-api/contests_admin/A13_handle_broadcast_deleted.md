# A13 Handle broadcast deleted

This document is part of the *Eurocentric* [launch specification](../../../README.md).

- [A13 Handle broadcast deleted](#a13-handle-broadcast-deleted)
  - [User story](#user-story)
  - [Acceptance criteria](#acceptance-criteria)

## User story

- **As the Admin**, when I am deleting a broadcast
- **I want** the parent contest to update itself by removing the broadcast memo referencing the deleted broadcast
- **So that** the Public API's queryable voting data is always up to date, and contests with no broadcast memos can be deleted

## Acceptance criteria

**HandleBroadcastDeleted event handler...**

- should remove broadcast memo from not completed parent contest when child broadcast deleted
- should remove broadcast memo from completed parent contest and set not completed when child broadcast deleted
