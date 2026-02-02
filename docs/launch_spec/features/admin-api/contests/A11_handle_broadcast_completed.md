# A11. Handle broadcast completed

This document is part of the [*Eurocentric* launch specification](../../../README.md).

- [A11. Handle broadcast completed](#a11-handle-broadcast-completed)
  - [User story](#user-story)
  - [Event handler logic](#event-handler-logic)
  - [Acceptance tests](#acceptance-tests)
    - [Happy path](#happy-path)
    - [Sad path](#sad-path)

## User story

- **As the Admin**, when I am awarding the final set of points in a broadcast
- **I want** the completed broadcast's parent contest to update itself as part of the same transaction
- **So that** the contest and all its data becomes queryable once all three of its child broadcasts are created and completed.

## Event handler logic

1. A completed broadcast raises a `BroadcastCompletedEvent`.
2. The event is published when the unit of work commit is requested.
3. The event handler receives the event.
4. The event handler retrieves the broadcast's parent contest.
5. The contest updates itself as follows:
   - the corresponding broadcast memo is replaced with a new memo that is `Completed=true`.
   - if the contest has 3 broadcast memos and they are all `Completed=true`:
     - the contest sets itself to `Queryable=true`.
6. The updated contest is added to the unit of work.
7. The unit of work commits.

## Acceptance tests

### Happy path

**HandleBroadcastCompleted event handler...**

- [ ] Should_replace_parent_contest_broadcastMemo_1_of_1_with_Completed
- [ ] Should_replace_parent_contest_broadcastMemo_1_of_2_with_Completed
- [ ] Should_replace_parent_contest_broadcastMemo_2_of_2_with_Completed
- [ ] Should_replace_parent_contest_broadcastMemo_1_of_3_with_Completed
- [ ] Should_replace_parent_contest_broadcastMemo_2_of_3_with_Completed
- [ ] Should_replace_parent_contest_broadcastMemo_3_of_3_with_Completed_and_set_Queryable

### Sad path

**HandleBroadcastCompleted event handler...**

- [ ] Should_not_activate_when_final_broadcast_points_award_fails
