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
3. Enum values are stored as their string names.

## **BROADCAST** aggregate tables

```mermaid
erDiagram

broadcast {
  broadcast_id uniqueidentifier PK
  broadcast_date date UK "between 2016-01-01 and 2050-12-31"
  parent_contest_id uniqueidentifier
  contest_stage nvarchar(20) "in ('GrandFinal','SemiFinal1','SemiFinal2')"
  voting_rules nvarchar(20) "in ('TelevoteAndJury','TelevoteOnly')"
  completed bit
}

broadcast_jury {
  broadcast_id uniqueidentifier PK,FK
  voting_country_id uniqueidentifier PK
  points_awarded bit
}

broadcast_competitor {
  broadcast_id uniqueidentifier PK,FK
  competing_country_id uniqueidentifier PK
  running_order_spot int "\>= 1"
  finishing_position int "\>= 1"
}

broadcast_televote {
  broadcast_id uniqueidentifier PK,FK
  voting_country_id uniqueidentifier PK
  points_awarded bit
}

broadcast_competitor_jury_award {
  row_id int PK "generated on add"
  broadcast_id uniqueidentifier FK
  competing_country_id uniqueidentifier FK
  voting_country_id uniqueidentifier
  points_value int "\>= 0"
}

broadcast_competitor_televote_award {
  row_id int PK "generated on add"
  broadcast_id uniqueidentifier FK
  competing_country_id uniqueidentifier FK
  voting_country_id uniqueidentifier
  points_value int "\>= 0"
}

broadcast ||--o{ broadcast_jury : owns
broadcast ||--o{ broadcast_competitor : owns
broadcast ||--o{ broadcast_televote : owns
broadcast_competitor ||--o{ broadcast_competitor_jury_award : owns
broadcast_competitor ||--o{ broadcast_competitor_televote_award : owns
```

**Additional constraints:**

- In the `broadcast` table:
  - (`parent_contest_id`, `contest_stage`) is unique.
- In the `broadcast_competitor` table:
  - (`broadcast_id`, `running_order_spot`) is unique.
- In the `broadcast_competitor_jury_award` table:
  - `competing_country_id` <> `voting_country_id`.
  - (`broadcast_id`, `competing_country_id`, `voting_country_id`) is unique.
- In the `broadcast_competitor_televote_award` table:
  - `competing_country_id` <> `voting_country_id`.
  - (`broadcast_id`, `competing_country_id`, `voting_country_id`) is unique.

## **CONTEST** aggregate tables

```mermaid
erDiagram

contest {
  contest_id uniqueidentifier PK
  contest_year int UK "between 2016 and 2050"
  city_name nvarchar(200)
  grand_final_voting_rules nvarchar(20) "in ('TelevoteAndJury','TelevoteOnly')"
  semi_final_voting_rules nvarchar(20) "in ('TelevoteAndJury','TelevoteOnly')"
  global_televote_voting_country_id uniqueidentifier "allows null"
  queryable bit
}

contest_participant {
  contest_id uniqueidentifier PK,FK
  participating_country_id uniqueidentifier PK
  semi_final_draw nvarchar(10) "in ('SemiFinal1','SemiFinal2')"
  act_name nvarchar(200)
  song_title nvarchar(200)
}

contest_child_broadcast {
  contest_id uniqueidentifier PK,FK
  child_broadcast_id uniqueidentifier PK
  contest_stage nvarchar(20) "in ('GrandFinal','SemiFinal1','SemiFinal2')"
  completed bit
}

contest ||--o{ contest_participant : owns
contest ||--o{ contest_child_broadcast : owns
```

**Additional constraints:**

- In the `contest_child_broadcast` table:
  - (`contest_id`, `contest_stage`) is unique.

## **COUNTRY** aggregate tables

```mermaid
erDiagram

country {
  country_id uniqueidentifier PK
  country_code nchar(2) UK
  country_name nvarchar(200)
}

country_contest_role {
  row_id int PK "generated on add"
  country_id uniqueidentifier FK
  contest_id uniqueidentifier
  contest_role_type nvarchar(20) "in ('Participant','GlobalTelevote')"
}

country ||--o{ country_contest_role : owns
```

Additional constraints:

- In the `country_contest_role` table:
  - (`country_id`, `contest_id`) is unique.
