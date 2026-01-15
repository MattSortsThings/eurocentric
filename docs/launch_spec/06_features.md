# 6 Features

This document is part of the [*Eurocentric* launch specification](README.md).

- [6 Features](#6-features)
  - [*admin-api* features](#admin-api-features)
    - [Broadcasts Admin](#broadcasts-admin)
    - [Contests Admin](#contests-admin)
    - [Countries Admin](#countries-admin)
    - [Observability](#observability)
  - [*errors* features](#errors-features)
  - [*logging* features](#logging-features)
  - [*open-api* features](#open-api-features)
  - [*public-api* features](#public-api-features)
    - [Broadcast Competitor Rankings](#broadcast-competitor-rankings)
    - [Broadcast Segment Rankings](#broadcast-segment-rankings)
    - [Broadcasts](#broadcasts)
    - [Competing Country Rankings](#competing-country-rankings)
    - [Contests](#contests)
    - [Countries](#countries)
    - [Points Awards](#points-awards)
    - [Results](#results)
    - [Voting Country Rankings](#voting-country-rankings)
  - [*security* features](#security-features)
  - [*versioning* features](#versioning-features)

## *admin-api* features

### Broadcasts Admin

- [A01 Award broadcast jury points](features/admin-api/A01_award_broadcast_jury_points.md)
- [A02 Award broadcast televote points](features/admin-api/A02_award_broadcast_televote_points.md)
- [A03 Delete broadcast](features/admin-api/A03_delete_broadcast.md)
- [A04 Get broadcast](features/admin-api/A04_get_broadcast.md)
- [A05 Get broadcasts](features/admin-api/A05_get_broadcasts.md)

### Contests Admin

- [A06 Create contest](features/admin-api/A06_create_contest.md)
- [A07 Create contest child broadcast](features/admin-api/A07_create_contest_child_broadcast.md)
- [A08 Delete contest](features/admin-api/A08_delete_contest.md)
- [A09 Get contest](features/admin-api/A09_get_contest.md)
- [A10 Get contests](features/admin-api/A10_get_contests.md)
- [A11 Handle broadcast completed](features/admin-api/A11_handle_broadcast_completed.md)
- [A12 Handle broadcast created](features/admin-api/A12_handle_broadcast_created.md)
- [A13 Handle broadcast deleted](features/admin-api/A13_handle_broadcast_deleted.md)

### Countries Admin

- [A14 Create country](features/admin-api/A14_create_country.md)
- [A15 Delete country](features/admin-api/A15_delete_country.md)
- [A16 Get countries](features/admin-api/A16_get_countries.md)
- [A17 Get country](features/admin-api/A17_get_country.md)
- [A18 Handle contest created](features/admin-api/A18_handle_contest_created.md)
- [A19 Handle contest deleted](features/admin-api/A19_handle_contest_deleted.md)

### Observability

- [A20 Get endpoint usage metrics](features/admin-api/A20_get_endpoint_usage_metrics.md)
- [A21 Get log entries](features/admin-api/A21_get_log_entries.md)

## *errors* features

- [E01 Exception handlers](features/errors/E01_exception_handlers.md)
- [E02 Problem details](features/errors/E02_problem_details.md)

## *logging* features

- [L01 Correlated log entries](features/logging/L01_correlated_log_entries.md)

## *open-api* features

- [O01 OpenAPI docs](features/open-api/O01_openapi_docs.md)
- [O02 Scalar pages](features/open-api/O02_scalar_pages.md)

## *public-api* features

### Broadcast Competitor Rankings

- [P01 Get broadcast competitor points average rankings](features/public-api/P01_get_broadcast_competitor_points_average_rankings.md)
- [P02 Get broadcast competitor points in range rankings](features/public-api/P02_get_broadcast_competitor_points_in_range_rankings.md)
- [P03 Get broadcast competitor points share rankings](features/public-api/P03_get_broadcast_competitor_points_share_rankings.md)
- [P04 Get broadcast competitor points similarity rankings](features/public-api/P04_get_broadcast_competitor_points_similarity_rankings.md)

### Broadcast Segment Rankings

- [P05 Get broadcast segment points average rankings](features/public-api/P05_get_broadcast_segment_points_average_rankings.md)
- [P06 Get broadcast segment points in range rankings](features/public-api/P06_get_broadcast_segment_points_in_range_rankings.md)
- [P07 Get broadcast segment points share rankings](features/public-api/P07_get_broadcast_segment_points_share_rankings.md)
- [P08 Get broadcast segment points similarity rankings](features/public-api/P08_get_broadcast_segment_points_similarity_rankings.md)

### Broadcasts

- [P09 Get queryable broadcasts](features/public-api/P09_get_queryable_broadcasts.md)

### Competing Country Rankings

- [P10 Get competing country points average rankings](features/public-api/P10_get_competing_country_points_average_rankings.md)
- [P11 Get competing country points in range rankings](features/public-api/P11_get_competing_country_points_in_range_rankings.md)
- [P12 Get competing country points share rankings](features/public-api/P12_get_competing_country_points_share_rankings.md)
- [P13 Get competing country points similarity rankings](features/public-api/P13_get_competing_country_points_similarity_rankings.md)

### Contests

- [P14 Get queryable contests](features/public-api/P14_get_queryable_contests.md)

### Countries

- [P15 Get queryable countries](features/public-api/P15_get_queryable_countries.md)

### Points Awards

- [P16 Get broadcast competing country points awards](features/public-api/P16_get_broadcast_competing_country_points_awards.md)
- [P17 Get broadcast voting country points awards](features/public-api/P17_get_broadcast_voting_country_points_awards.md)

### Results

- [P18 Get broadcast results](features/public-api/P18_get_broadcast_results.md)
- [P19 Get competing country results](features/public-api/P19_get_competing_country_results.md)

### Voting Country Rankings

- [P20 Get voting country points average rankings](features/public-api/P20_get_voting_country_points_average_rankings.md)
- [P21 Get voting country points in range rankings](features/public-api/P21_get_voting_country_points_in_range_rankings.md)
- [P22 Get voting country points share rankings](features/public-api/P22_get_voting_country_points_share_rankings.md)
- [P23 Get voting country points similarity rankings](features/public-api/P23_get_voting_country_points_similarity_rankings.md)

## *security* features

- [S01 API key authentication](features/security/S01_api_key_authentication.md)
- [S02 API key authorization](features/security/S02_api_key_authorization.md)

## *versioning* features

- [V01 API version reporting](features/versioning/V01_api_version_reporting.md)
- [V02 API version URL segments](features/versioning/V02_api_version_url_segments.md)
