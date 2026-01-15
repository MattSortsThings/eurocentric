# A12 Handle broadcast created

This document is part of the [*Eurocentric* launch specification](../../README.md).

**Feature scope:** *admin-api*

## User story

- **As the Admin**, when I am creating a new broadcast
- **I want** the parent contest to update itself by adding a broadcast memo referencing the created broadcast
- **So that** the parent contest cannot be deleted

## Acceptance criteria

**BroadcastCreatedEventHandler...**

- Should_add_broadcastMemo_to_parent_contest_when_child_broadcast_created
- Should_update_no_contests_when_broadcast_creation_fails
