# L02. Log entry curation

This document is part of the [launch specification](../../README.md).

## User story

- **As the Dev**
- **I want** log entries to be stored in the system database for the past 60 days only
- **So that** the system database does not become excessively large.

## Acceptance criteria

**Any API endpoint...**

- [ ] Triggers_all_log_entries_more_than_60_days_old_to_be_deleted_when_request_is_logged
- [ ] Triggers_no_log_entries_to_be_deleted_when_all_are_60_days_old_or_less
