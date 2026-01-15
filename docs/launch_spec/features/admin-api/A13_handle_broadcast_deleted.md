# A13 Handle broadcast deleted

This document is part of the [*Eurocentric* launch specification](../../README.md).

**Feature scope:** *admin-api*

## User story

- **As the Admin**, when I am deleting a broadcast
- **I want** the parent contest to update itself by removing the broadcast memo referencing the deleted broadcast and setting itself to not queryable if it is currently queryable
- **So that** the Public API's queryable voting data is always up to date, and any contest with no child broadcasts can be deleted

## Acceptance criteria

**BroadcastDeletedEventHandler...**

- Should_remove_broadcastMemo_from_parent_contest_when_child_broadcast_deleted
- Should_set_Queryable_parent_contest_to_not_Queryable_when_child_broadcast_deleted
