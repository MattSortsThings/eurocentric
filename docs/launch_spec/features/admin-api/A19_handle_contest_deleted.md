# A19 Handle contest deleted

This document is part of the [*Eurocentric* launch specification](../../README.md).

**Feature scope:** *admin-api*

## User story

- **As the Admin**, when I am deleting a contest
- **I want** every participating/voting country to update itself by removing the contest role referencing the deleted contest
- **So that** any country with no contest roles can be deleted

## Acceptance criteria

**ContestDeletedEventHandler...**

- Should_remove_contest_role_from_participating_countries_when_contest_deleted
- Should_remove_contest_role_from_participating_and_voting_countries_when_contest_deleted
- Should_update_no_countries_when_contest_deletion_fails
