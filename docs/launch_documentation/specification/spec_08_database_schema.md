# 8. Database schema

This document is part of the [launch specification](../README.md#launch-specification).

- [8. Database schema](#8-database-schema)
  - [Conventions](#conventions)
  - [**BROADCAST** aggregate tables](#broadcast-aggregate-tables)
  - [**CONTEST** aggregate tables](#contest-aggregate-tables)
  - [**COUNTRY** aggregate tables](#country-aggregate-tables)

## Conventions

1. All columns are `not null` unless specified.
2. Any column named `row_id` contains a database-generated integer row identifier that is not part of the domain model.
3. Enum values are stored as integers.

## **BROADCAST** aggregate tables

```mermaid
erDiagram

broadcast {
  broadcast_id uniqueidentifier PK
  broadcast_date date UK "between 2016-01-01 and 2050-12-31"
  parent_contest_id uniqueidentifier
  contest_stage int "in (0,1,2)"
  completed bit
}

jury {
  broadcast_id uniqueidentifier PK,FK
  voting_country_id uniqueidentifier PK
  points_awarded bit
}

competitor {
  broadcast_id uniqueidentifier PK,FK
  competing_country_id uniqueidentifier PK
  running_order_spot int "\>= 1"
  finishing_position int "\>= 1"
}

televote {
  broadcast_id uniqueidentifier PK,FK
  voting_country_id uniqueidentifier PK
  points_awarded bit
}

jury_award {
  row_id int PK "generated on add"
  broadcast_id uniqueidentifier FK
  competing_country_id uniqueidentifier FK
  voting_country_id uniqueidentifier
  points_value int "in (0,1,2,3,4,5,6,7,8,10,12)"
}

televote_award {
  row_id int PK "generated on add"
  broadcast_id uniqueidentifier FK
  competing_country_id uniqueidentifier FK
  voting_country_id uniqueidentifier
  points_value int "in (0,1,2,3,4,5,6,7,8,10,12)"
}

broadcast ||--o{ jury : owns
broadcast ||--o{ competitor : owns
broadcast ||--o{ televote : owns
competitor ||--o{ jury_award : owns
competitor ||--o{ televote_award : owns
```

Additional constraints:

- In the `broadcast` table:
  - (`parent_contest_id`, `contest_stage`) is unique.
- In the `competitor` table:
  - (`broadcast_id`, `running_order_spot`) is unique.
- In the `jury_award` table:
  - `competing_country_id` <> `voting_country_id`.
  - (`broadcast_id`, `competing_country_id`, `voting_country_id`) is unique.
- In the `televote_award` table:
  - `competing_country_id` <> `voting_country_id`.
  - (`broadcast_id`, `competing_country_id`, `voting_country_id`) is unique.

## **CONTEST** aggregate tables

```mermaid
erDiagram

contest {
  contest_id uniqueidentifier PK
  contest_year int UK "between 2016 and 2050"
  city_name nvarchar(200)
  contest_rules int "in (0,1)"
  queryable bit
  global_televote_voting_country_id uniqueidentifier "allows null"
}

participant {
  contest_id uniqueidentifier PK,FK
  participating_country_id uniqueidentifier PK
  semi_final_draw int "in (0,1)"
  act_name nvarchar(200)
  song_title nvarchar(200)
}

child_broadcast {
  contest_id uniqueidentifier PK,FK
  child_broadcast_id uniqueidentifier PK
  contest_stage int "in (0,1,2)"
  completed bit
}

contest ||--o{ participant : owns
contest ||--o{ child_broadcast : owns
```

Additional constraints:

- In the `contest` table:
  - *either* `contest_rules` = 0 and `global_televote_voting_country_id` is not `null`
  - *or* `contest_rules` = 1 and `global_televote_voting_country_id` is `null`.
- In the `child_broadcast` table:
  - (`contest_id`, `contest_stage`) is unique.

## **COUNTRY** aggregate tables

```mermaid
erDiagram

country {
  country_id uniqueidentifier PK
  country_code nchar(2) UK
  country_name nvarchar(200)
}

contest_role {
  row_id int PK "generated on add"
  country_id uniqueidentifier FK
  contest_id uniqueidentifier
  contest_role_type int "in (0,1)"
}

country ||--o{ contest_role : owns
```

Additional constraints:

- In the `contest_role` table:
  - (`country_id`, `country_id`) is unique.
