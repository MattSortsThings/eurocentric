# A12. Handle broadcast created

This document is part of the [launch specification](../../../README.md).

## User story

- **As the Admin**, when I am creating a new broadcast
- **I want** the created broadcast's parent contest to update itself as part of the same transaction
- **So that** the contest cannot be deleted

## Acceptance criteria

**Event handler...**

- [ ] Adds_childBroadcast_to_parent_contest
- [ ] Does_not_activate_when_broadcast_creation_fails
