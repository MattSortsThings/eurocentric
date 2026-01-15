# A18 Handle contest created

This document is part of the [*Eurocentric* launch specification](../../README.md).

**Feature scope:** *admin-api*

## User story

- **As the Admin**, when I am creating a new contest
- **I want** every participating/voting country to update itself by adding a contest role referencing the created contest
- **So that** none of the participating/voting countries can be deleted

## Acceptance criteria

**ContestCreatedEventHandler...**

- Should_add_contest_role_to_participating_countries_when_contest_created
- Should_add_contest_role_to_participating_and_voting_countries_when_contest_created
- Should_update_no_countries_when_contest_creation_fails
