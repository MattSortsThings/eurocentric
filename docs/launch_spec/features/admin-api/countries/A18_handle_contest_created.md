# A18. Handle contest created

This document is part of the [launch specification](../../../README.md).

## User story

- **As the Admin**, when I am creating a new contest
- **I want** every participating and voting country in the created contest to update itself as part of the same transaction
- **So that** none of the countries can be deleted

## Acceptance criteria

**Event handler...**

- [ ] Adds_activeContestId_to_all_countries_in_contest_without_globalTelevote
- [ ] Adds_activeContestId_to_all_countries_in_contest_with_globalTelevote
- [ ] Does_not_activate_when_contest_creation_fails
