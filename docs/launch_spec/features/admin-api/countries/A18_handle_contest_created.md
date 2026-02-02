# A18. Handle contest created

This document is part of the [*Eurocentric* launch specification](../../../README.md).

- [A18. Handle contest created](#a18-handle-contest-created)
  - [User story](#user-story)
  - [Event handler logic](#event-handler-logic)
  - [Acceptance tests](#acceptance-tests)
    - [Happy path](#happy-path)
    - [Sad path](#sad-path)

## User story

- **As the Admin**, when I am creating a contest
- **I want** the created contest's participating and voting countries to update themselves as part of the same transaction
- **So that** the countries cannot be deleted.

## Event handler logic

1. A created contest raises a `ContestCreatedEvent`.
2. The event is published when the unit of work commit is requested.
3. The event handler receives the event.
4. The event handler retrieves all the contest's participating countries and its optional global televote voting country.
5. Each country updates itself as follows:
   - the ID of the created contest is added.
6. The updated countries are added to the unit of work.
7. The unit of work commits.

## Acceptance tests

### Happy path

**HandleContestCreated event handler...**

- [ ] Should_add_contestId_to_all_participating_countries
- [ ] Should_add_contestId_to_all_participating_countries_and_global_televote_voting_country

### Sad path

**HandleContestCreated event handler...**

- [ ] Should_not_activate_when_contest_creation_fails
