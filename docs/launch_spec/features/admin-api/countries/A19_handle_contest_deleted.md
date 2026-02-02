# A19. Handle contest deleted

This document is part of the [*Eurocentric* launch specification](../../../README.md).

- [A19. Handle contest deleted](#a19-handle-contest-deleted)
  - [User story](#user-story)
  - [Event handler logic](#event-handler-logic)
  - [Acceptance tests](#acceptance-tests)
    - [Happy path](#happy-path)
    - [Sad path](#sad-path)

## User story

- **As the Admin**, when I am deleting a contest
- **I want** the created contest's participating and voting countries to update themselves as part of the same transaction
- **So that** any country holding no contest IDs can be deleted.

## Event handler logic

1. A deleted contest raises a `ContestDeletedEvent`.
2. The event is published when the unit of work commit is requested.
3. The event handler receives the event.
4. The event handler retrieves all the contest's participating countries and its optional global televote voting country.
5. Each country updates itself as follows:
   - the ID of the deleted contest is removed.
6. The updated countries are added to the unit of work.
7. The unit of work commits.

## Acceptance tests

### Happy path

**HandleContestDeleted event handler...**

- [ ] Should_remove_contestId_from_all_participating_countries
- [ ] Should_remove_contestId_from_all_participating_countries_and_global_televote_voting_country

### Sad path

**HandleContestDeleted event handler...**

- [ ] Should_not_activate_when_contest_deletion_fails
