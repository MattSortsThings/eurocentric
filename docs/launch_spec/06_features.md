# 6. Features

This document is part of the [*Eurocentric* launch specification](README.md).

- [6. Features](#6-features)
  - [*admin-api* feature scope](#admin-api-feature-scope)
    - [Broadcasts Admin](#broadcasts-admin)
    - [Contests Admin](#contests-admin)
    - [Countries Admin](#countries-admin)
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

### Broadcasts Admin

- [A01. Award broadcast jury points](features/admin-api/broadcasts-admin/A01_award_broadcast_jury_points.md)
- [A02. Award broadcast televote points](features/admin-api/broadcasts-admin/A02_award_broadcast_televote_points.md)
- [A03. Delete broadcast](features/admin-api/broadcasts-admin/A03_delete_broadcast.md)
- [A04. Get broadcast](features/admin-api/broadcasts-admin/A04_get_broadcast.md)
- [A05. Get broadcasts](features/admin-api/broadcasts-admin/A05_get_broadcasts.md)

### Contests Admin

- A06. Create contest
- A07. Create contest child broadcast
- A08. Delete contest
- A09. Get contest
- A10. Get contests
- A11. Handle broadcast completed
- A12. Handle broadcast created
- A13. Handle broadcast deleted

### Countries Admin

- A14. Create country
- [A15. Delete country](features/admin-api/countries-admin/A15_delete_country.md)
- [A16. Get countries](features/admin-api/countries-admin/A16_get_countries.md)
- [A17. Get country](features/admin-api/countries-admin/A17_get_country.md)
- A18. Handle contest created
- A19. Handle contest deleted

### Observability

- A20. Get endpoint usage metrics
- A21. Get log entries

## *error-handling* feature scope

- E01. Exception handlers
- E02. Problem details

## *logging* feature scope

- L01. Correlated log entries

## *open-api* feature scope

- O01. OpenAPI documents
- O02. Scalar UI pages

## *public-api* feature scope

### Broadcast competitor rankings

- P01. Get broadcast competitor points average rankings
- P02. Get broadcast competitor points in range rankings
- P03. Get broadcast competitor points share rankings
- P04. Get broadcast competitor points similarity rankings

### Broadcasts

- P05. Get queryable broadcasts

### Competing country rankings

- [P06. Get competing country points average rankings](features/public-api/competing-country-rankings/P06_get_competing_country_points_average_rankings.md)
- [P07. Get competing country points in range rankings](features/public-api/competing-country-rankings/P07_get_competing_country_points_in_range_rankings.md)
- [P08. Get competing country points share rankings](features/public-api/competing-country-rankings/P08_get_competing_country_points_share_rankings.md)
- [P09. Get competing country points similarity rankings](features/public-api/competing-country-rankings/P09_get_competing_country_points_similarity_rankings.md)

### Contests

- P10. Get queryable contests

### Countries

- P11. Get queryable countries

### Points Awards

- P12. Get broadcast competing country points awards
- P13. Get broadcast voting country points awards

### Scoreboard Rows

- P14. Get broadcast scoreboard rows
- P15. Get competing country scoreboard rows

### Voting country rankings

- [P16. Get voting country points average rankings](features/public-api/voting-country-rankings/P16_get_voting_country_points_average_rankings.md)
- [P17. Get voting country points in range rankings](features/public-api/voting-country-rankings/P17_get_voting_country_points_in_range_rankings.md)
- [P18. Get voting country points share rankings](features/public-api/voting-country-rankings/P18_get_voting_country_points_share_rankings.md)
- [P19. Get voting country points similarity rankings](features/public-api/voting-country-rankings/P19_get_voting_country_points_similarity_rankings.md)

## *security* feature scope

- S01. API key authentication
- S02. API key authorization

## *versioning* feature scope

- V01. API version reporting
- V02. API version URL segments
