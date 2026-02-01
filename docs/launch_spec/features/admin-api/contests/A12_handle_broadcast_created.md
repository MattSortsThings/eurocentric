# A12. Handle broadcast created

## User story

- **As the Admin**, when I am creating a broadcast
- **I want** the created broadcast's parent contest to update itself as part of the same transaction
- **So that** the contest cannot be deleted.

## Event handler logic

1. A created broadcast raises a `BroadcastCreatedEvent`.
2. The event is published when the unit of work commit is requested.
3. The event handler receives the event.
4. The event handler retrieves the broadcast's parent contest.
5. The contest updates itself as follows:
   - a broadcast memo referencing the created broadcast is added.
6. The updated contest is added to the unit of work.
7. The unit of work commits.

## Acceptance tests

**HandleBroadcastCreated event handler...**

- [ ] Should_add_parent_contest_broadcastMemo_1_of_1
- [ ] Should_add_parent_contest_broadcastMemo_1_of_2
- [ ] Should_not_activate_when_broadcast_creation_fails

