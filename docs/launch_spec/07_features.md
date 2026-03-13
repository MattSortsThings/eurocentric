# 7. Features

This document is part of the [launch specification](README.md).

- [7. Features](#7-features)
  - [*admin-api* feature scope](#admin-api-feature-scope)
    - [Broadcasts](#broadcasts)
    - [Contests](#contests)
    - [Countries](#countries)
    - [Observability](#observability)
  - [*error-handling* feature scope](#error-handling-feature-scope)
  - [*logging* feature scope](#logging-feature-scope)
  - [*open-api* feature scope](#open-api-feature-scope)
  - [*public-api* feature scope](#public-api-feature-scope)
    - [Competitor rankings](#competitor-rankings)
    - [Country rankings](#country-rankings)
    - [Points awards](#points-awards)
    - [Queryable voting data](#queryable-voting-data)
    - [Scoreboard rows](#scoreboard-rows)
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
- [A07. Create contest broadcast](features/admin-api/contests/A07_create_contest_broadcast.md)
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
- [E02. Domain error responses](features/error-handling/E02_domain_error_responses.md)

## *logging* feature scope

- [L01. Correlated log entries](features/logging/L01_correlated_log_entries.md)
- [L02. Log entry curation](features/logging/L02_log_entry_curation.md)

## *open-api* feature scope

- [O01. OpenAPI documents](features/open-api/O01_open-api_documents.md)
- [O02. Scalar pages](features/open-api/O02_scalar_pages.md)

## *public-api* feature scope

### Competitor rankings

- [P01. Get competitor points from all average rankings](features/public-api/competitor_rankings/P01_get_competitor_points_from_all_average_rankings.md)
- [P02. Get competitor points from all fraction rankings](features/public-api/competitor_rankings/P02_get_competitor_points_from_all_fraction_rankings.md)
- [P03. Get competitor points from all in range rankings](features/public-api/competitor_rankings/P03_get_competitor_points_from_all_in_range_rankings.md)
- [P04. Get competitor points from all similarity rankings](features/public-api/competitor_rankings/P04_get_competitor_points_from_all_similarity_rankings.md)

### Country rankings

- [P05. Get country points from all average rankings](features/public-api/country_rankings/P05_get_country_points_from_all_average_rankings.md)
- [P06. Get country points from all fraction rankings](features/public-api/country_rankings/P06_get_country_points_from_all_fraction_rankings.md)
- [P07. Get country points from all in range rankings](features/public-api/country_rankings/P07_get_country_points_from_all_in_range_rankings.md)
- [P08. Get country points from all similarity rankings](features/public-api/country_rankings/P08_get_country_points_from_all_similarity_rankings.md)
- [P09. Get country points from single average rankings](features/public-api/country_rankings/P09_get_country_points_from_single_average_rankings.md)
- [P10. Get country points from single fraction rankings](features/public-api/country_rankings/P10_get_country_points_from_single_fraction_rankings.md)
- [P11. Get country points from single in range rankings](features/public-api/country_rankings/P11_get_country_points_from_single_in_range_rankings.md)
- [P12. Get country points from single similarity rankings](features/public-api/country_rankings/P12_get_country_points_from_single_similarity_rankings.md)
- [P13. Get country points to all similarity rankings](features/public-api/country_rankings/P13_get_country_points_to_all_similarity_rankings.md)
- [P14. Get country points to single average rankings](features/public-api/country_rankings/P14_get_country_points_to_single_average_rankings.md)
- [P15. Get country points to single fraction rankings](features/public-api/country_rankings/P15_get_country_points_to_single_fraction_rankings.md)
- [P16. Get country points to single in range rankings](features/public-api/country_rankings/P16_get_country_points_to_single_in_range_rankings.md)
- [P17. Get country points to single similarity rankings](features/public-api/country_rankings/P17_get_country_points_to_single_similarity_rankings.md)

### Points awards

- [P18. Get broadcast competitor points awards](features/public-api/points_awards/P18_get_broadcast_competitor_points_awards.md)
- [P19. Get broadcast voter points awards](features/public-api/points_awards/P19_get_broadcast_voter_points_awards.md)

### Queryable voting data

- [P20. Get queryable broadcasts](features/public-api/queryable_voting_data/P20_get_queryable_broadcasts.md)
- [P21. Get queryable contests](features/public-api/queryable_voting_data/P21_get_queryable_contests.md)
- [P22. Get queryable countries](features/public-api/queryable_voting_data/P22_get_queryable_countries.md)

### Scoreboard rows

- [P23. Get broadcast scoreboard rows](features/public-api/scoreboard_rows/P23_get_broadcast_scoreboard_rows.md)

## *security* feature scope

- [S01. Admin API security](features/security/S01_admin_api_security.md)
- [S02. Public API security](features/security/S02_public_api_security.md)

## *versioning* feature scope

- [V01. URL path versioning](features/versioning/V01_url_path_versioning.md)
- [V02. Version reporting](features/versioning/V02_version_reporting.md)
