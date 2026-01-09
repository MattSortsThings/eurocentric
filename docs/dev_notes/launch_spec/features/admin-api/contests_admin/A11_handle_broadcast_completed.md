# A11 Handle broadcast completed

This document is part of the *Eurocentric* [launch specification](../../../README.md).

- [A11 Handle broadcast created](#a11-handle-broadcast-completed)
  - [User story](#user-story)
  - [Acceptance criteria](#acceptance-criteria)

## User story

- **As the Admin**, when I am awarding the final set of points in a broadcast
- **I want** the parent contest to update itself by replacing the broadcast memo referencing the completed broadcast, which may in turn complete the contest
- **So that** the Public API's queryable voting data is always up to date.

## Acceptance criteria

**HandleBroadcastCreated event handler...**

- should replace broadcast memo in parent contest when child broadcast 1 of 1 completed
- should replace broadcast memo in parent contest when child broadcast 1 of 2 completed
- should replace broadcast memo in parent contest when child broadcast 2 of 2 completed
- should replace broadcast memo in parent contest when child broadcast 1 of 3 completed
- should replace broadcast memo in parent contest when child broadcast 2 of 3 completed
- should replace broadcast memo in parent contest and completed contest when child broadcast 3 of 3 completed
- should update no contests when broadcast completion fails
