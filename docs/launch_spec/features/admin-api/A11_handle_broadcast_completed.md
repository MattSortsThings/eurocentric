# A11 Handle broadcast completed

This document is part of the [*Eurocentric* launch specification](../../README.md).

**Feature scope:** *admin-api*

## User story

- **As the Admin**, when I am awarding the final set of points in a broadcast
- **I want** the parent contest to update itself by replacing the broadcast memo referencing the created broadcast and setting itself to queryable if this is the final child broadcast to be completed
- **So that** all the data from the contest is now queryable using the Public API.

## Acceptance criteria

**BroadcastCompletedEventHandler...**

- Should_replace_broadcastMemo_in_parent_contest_when_child_broadcast_completed
- Should_set_parent_contest_to_Queryable_when_final_child_broadcast_completed
- Should_update_no_contests_when_broadcast_completion_fails
