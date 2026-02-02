# 6. Features

This document is part of the [*Eurocentric* launch specification](README.md).

- [6. Features](#6-features)
  - [*admin-api* feature scope](#admin-api-feature-scope)
    - [Broadcasts Admin](#broadcasts)
    - [Contests Admin](#contests)
    - [Countries Admin](#countries)
    - [Observability](#observability)
  - [*error-handling* feature scope](#error-handling-feature-scope)
  - [*logging* feature scope](#logging-feature-scope)
  - [*open-api* feature scope](#open-api-feature-scope)
  - [*public-api* feature scope](#public-api-feature-scope)
    - [Broadcast competitor rankings](#broadcast-competitor-rankings)
    - [Broadcasts](#broadcasts)
    - [Competing country rankings](#competing-country-rankings)
    - [Contests](#contests)
    - [Countries](#countries)
    - [Points Awards](#points-awards)
    - [Scoreboard Rows](#scoreboard-rows)
    - [Voting country rankings](#voting-country-rankings)
  - [*security* feature scope](#security-feature-scope)
  - [*versioning* feature scope](#versioning-feature-scope)

## *admin-api* feature scope

### Broadcasts

- [A01. Award broadcast jury points](features/admin-api/broadcasts/A01_award_broadcast_jury_points.md)
- [A02. Award broadcast televote points](features/admin-api/broadcasts/A02_award_broadcast_televote_points.md)
- [A03. Delete broadcast](features/admin-api/broadcasts/A03_delete_broadcast.md)
- [A04. Get broadcast](features/admin-api/broadcasts/A04_get_broadcast.md)
- [A05. Get broadcasts](features/admin-api/broadcasts/A05_get_broadcasts.md)

### Contests

- [A06. Create contest](features/admin-api/contests/A06_create_contest.md)
- [A07. Create contest child broadcast](features/admin-api/contests/A07_create_contest_child_broadcast.md)
- [A08. Delete contest](features/admin-api/contests/A08_delete_contest.md)
- [A09. Get contest](features/admin-api/contests/A09_get_contest.md)
- [A10. Get contests](features/admin-api/contests/A10_get_contests.md)
- [A11. Handle broadcast completed](features/admin-api/contests/A11_handle_broadcast_completed.md)
- [A12. Handle broadcast created](features/admin-api/contests/A12_handle_broadcast_created.md)
- [A13. Handle broadcast deleted](features/admin-api/contests/A13_handle_broadcast_deleted.md)

### Countries

- [A14. Create country](features/admin-api/countries/A14_create_country.md)
- [A15. Delete country](features/admin-api/countries/A15_delete_country.md)
- [A16. Get countries](features/admin-api/countries/A16_get_countries.md)
- [A17. Get country](features/admin-api/countries/A17_get_country.md)
- [A18. Handle contest created](features/admin-api/countries/A18_handle_contest_created.md)
- [A19. Handle contest deleted](features/admin-api/countries/A19_handle_contest_deleted.md)

### Observability

- [A20. Get correlated log entries](features/admin-api/observability/A20_get_correlated_log_entries.md)

## *error-handling* feature scope

- [E01. Exception handlers](features/error-handling/E01_exception_handlers.md)
- [E02. Problem details](features/error-handling/E02_problem_details.md)

## *logging* feature scope

- [L01. Correlated log entries](features/logging/L01_correlated_log_entries.md)
- [L02. Log entry maintenance](features/logging/L02_log_entry_maintenance.md)

## *open-api* feature scope

- [O01. OpenAPI documents](features/open-api/O01_open_api_documents.md)
- [O02. Scalar UI pages](features/open-api/O02_scalar_ui_pages.md)

## *public-api* feature scope

### Broadcast competitor rankings

- [P01. Get broadcast competitor points average rankings](features/public-api/broadcast-competitor-rankings/P01_get_broadcast_competitor_points_average_rankings.md)
- [P02. Get broadcast competitor points in range rankings](features/public-api/broadcast-competitor-rankings/P02_get_broadcast_competitor_points_in_range_rankings.md)
- [P03. Get broadcast competitor points share rankings](features/public-api/broadcast-competitor-rankings/P03_get_broadcast_competitor_points_share_rankings.md)
- [P04. Get broadcast competitor points similarity rankings](features/public-api/broadcast-competitor-rankings/P04_get_broadcast_competitor_points_similarity_rankings.md)

### Competing country rankings

- [P05. Get competing country points average rankings](features/public-api/competing-country-rankings/P05_get_competing_country_points_average_rankings.md)
- [P06. Get competing country points in range rankings](features/public-api/competing-country-rankings/P06_get_competing_country_points_in_range_rankings.md)
- [P07. Get competing country points share rankings](features/public-api/competing-country-rankings/P07_get_competing_country_points_share_rankings.md)
- [P08. Get competing country points similarity rankings](features/public-api/competing-country-rankings/P08_get_competing_country_points_similarity_rankings.md)

### Points Awards

- [P09. Get broadcast competing country points awards](features/public-api/points-awards/P09_get_broadcast_competing_country_points_awards.md)
- [P10. Get broadcast voting country points awards](features/public-api/points-awards/P10_get_broadcast_voting_country_points_awards.md)

### Queryable broadcasts

- [P11. Get queryable broadcasts](features/public-api/queryable-broadcasts/P11_get_queryable_broadcasts.md)

### Queryable contests

- [P12. Get queryable contests](features/public-api/queryable-contests/P12_get_queryable_contests.md)

### Queryable countries

- [P13. Get queryable countries](features/public-api/queryable-countries/P13_get_queryable_countries.md)

### Scoreboard Rows

- [P14. Get broadcast scoreboard rows](features/public-api/scoreboard-rows/P14_get_broadcast_scoreboard_rows.md)
- [P15. Get competing country scoreboard rows](features/public-api/scoreboard-rows/P15_get_competing_country_scoreboard_rows.md)

### Voting country rankings

- [P16. Get voting country points average rankings](features/public-api/voting-country-rankings/P16_get_voting_country_points_average_rankings.md)
- [P17. Get voting country points in range rankings](features/public-api/voting-country-rankings/P17_get_voting_country_points_in_range_rankings.md)
- [P18. Get voting country points share rankings](features/public-api/voting-country-rankings/P18_get_voting_country_points_share_rankings.md)
- [P19. Get voting country points similarity rankings](features/public-api/voting-country-rankings/P19_get_voting_country_points_similarity_rankings.md)

## *security* feature scope

- [S01. API key authentication](features/security/S01_api_key_authentication.md)
- [S02. API key authorization](features/security/S02_api_key_authorization.md)

## *versioning* feature scope

- [V01. API version reporting](features/versioning/V01_api_version_reporting.md)
- [V02. API version URL segments](features/versioning/V02_api_version_url_segments.md)
