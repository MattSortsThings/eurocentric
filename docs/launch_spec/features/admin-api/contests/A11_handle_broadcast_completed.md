# A11. Handle broadcast completed

This document is part of the [launch specification](../../../README.md).

## User story

- **As the Admin**, when I am awarding the final set of points in a broadcast
- **I want** the completed broadcast's parent contest to update itself as part of the same transaction
- **So that** the contest and all its data become queryable once all three of its child broadcasts are created and completed

## Acceptance criteria

**Event handler...**

- [ ] Sets_childBroadcast_completed_in_parent_contest
- [ ] Sets_parent_contest_to_queryable_when_3rd_of_3_childBroadcasts_completed
- [ ] Does_not_activate_when_broadcast_points_award_fails
