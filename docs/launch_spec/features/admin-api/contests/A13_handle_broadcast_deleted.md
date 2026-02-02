# A13. Handle broadcast deleted

This document is part of the [*Eurocentric* launch specification](../../../README.md).

- [A13. Handle broadcast deleted](#a13-handle-broadcast-deleted)
  - [User story](#user-story)
  - [Event handler logic](#event-handler-logic)
  - [Acceptance tests](#acceptance-tests)
    - [Happy path](#happy-path)

## User story

- **As the Admin**, when I am deleting a broadcast
- **I want** the completed broadcast's parent contest to update itself as part of the same transaction
- **So that** the contest and all its data is no longer queryable, and the contest may be deleted if it no longer has any broadcast memos.

## Event handler logic

1. A deleted broadcast raises a `BroadcastDeletedEvent`.
2. The event is published when the unit of work commit is requested.
3. The event handler receives the event.
4. The event handler retrieves the broadcast's parent contest.
5. The contest updates itself as follows:
   - the corresponding broadcast memo is removed.
   - if the contest is `Queryable=true`:
     - the contest sets itself to `Queryable=false`.
6. The updated contest is added to the unit of work.
7. The unit of work commits.

## Acceptance tests

### Happy path

**HandleBroadcastDeleted event handler...**

- [ ] Should_remove_parent_contest_broadcastMemo_1_of_1
- [ ] Should_remove_parent_contest_broadcastMemo_1_of_2
- [ ] Should_remove_parent_contest_Completed_broadcastMemo_3_of_3_and_set_not_Queryable
