# A12 Handle broadcast created

This document is part of the *Eurocentric* [launch specification](../../../README.md).

- [A12 Handle broadcast created](#a12-handle-broadcast-created)
  - [User story](#user-story)
  - [Acceptance criteria](#acceptance-criteria)

## User story

- **As the Admin**, when I am creating a broadcast
- **I want** the parent contest to update itself by adding a broadcast memo referencing the created broadcast
- **So that** the parent contest cannot be deleted

## Acceptance criteria

**HandleBroadcastCreated event handler...**

- should add broadcast memo to parent contest when GrandFinal broadcast created
- should add broadcast memo to parent contest when SemiFinal1 broadcast created
- should add broadcast memo to parent contest when SemiFinal2 broadcast created
- should update no contests when broadcast creation fails
