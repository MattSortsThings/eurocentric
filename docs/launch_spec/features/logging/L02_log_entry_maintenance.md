# L02. Log entry maintenance

This document is part of the [*Eurocentric* launch specification](../../../README.md).

- [L02. Log entry maintenance](#l02-log-entry-maintenance)
  - [User story](#user-story)
  - [Acceptance criteria](#acceptance-criteria)

## User story

- **As the Dev**
- **I want** log entries to be stored in the system database for the past 60 days only
- **So that** the system database does not become excessively large.

## Acceptance criteria

**API endpoint...**

- [ ] Should_cause_all_log_entries_more_than_60_days_old_to_be_deleted_when_request_logged
- [ ] Should_cause_no_log_entries_to_be_deleted_when_all_are_60_days_old_or_less
