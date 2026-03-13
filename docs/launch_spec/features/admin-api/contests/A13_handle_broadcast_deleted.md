# A13. Handle broadcast deleted

This document is part of the [launch specification](../../../README.md).

## User story

- **As the Admin**, when I am deleting a broadcast
- **I want** the deleted broadcast's parent contest to update itself as part of the same transaction
- **So that** the contest is no longer part of the queryable voting data
- **and** any contest with no child broadcasts may be deleted

## Acceptance criteria

**Event handler...**

- [ ] Removes_childBroadcast_from_parent_contest
- [ ] Sets_queryable_parent_contest_to_not_queryable
