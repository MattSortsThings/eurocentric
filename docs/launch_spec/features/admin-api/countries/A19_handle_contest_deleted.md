# A19. Handle contest deleted

This document is part of the [launch specification](../../../README.md).

## User story

- **As the Admin**, when I am deleting a contest
- **I want** every participating and voting country in the deleted contest to update itself as part of the same transaction
- **So that** any country with no active contest IDs may be deleted

## Acceptance criteria

**Event handler...**

- [ ] Removes_activeContestId_from_all_countries_in_contest_without_globalTelevote
- [ ] Removes_activeContestId_from_all_countries_in_contest_with_globalTelevote
- [ ] Does_not_activate_when_contest_deletion_fails
