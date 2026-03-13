# 8. Database schema

This document is part of the [launch specification](README.md).

- [8. Database schema](#8-database-schema)
  - [`Broadcast` aggregate tables](#broadcast-aggregate-tables)
  - [`Contest` aggregate tables](#contest-aggregate-tables)
  - [`Country` aggregate tables](#country-aggregate-tables)
  - [Logging table](#logging-table)

## `Broadcast` aggregate tables

```mermaid
erDiagram

broadcast {
  UNIQUEIDENTIFIER broadcast_id PK
  DATE broadcast_date UK "BETWEEN '2016-01-01' AND '2030-12-31'"
  UNIQUEIDENTIFIER parent_contest_id
  VARCHAR(10) contest_stage "IN ('SemiFinal1', 'SemiFinal2', 'GrandFinal')"
  VARCHAR(15) voting_format "IN ('JuryAndTelevote', 'TelevoteOnly')"
  BIT completed
}

broadcast_televote {
  UNIQUEIDENTIFIER broadcast_id PK,FK
  UNIQUEIDENTIFIER voting_country_id PK
  BIT points_awarded
}

broadcast_competitor {
  UNIQUEIDENTIFIER broadcast_id PK,FK
  UNIQUEIDENTIFIER competing_country_id PK
  INT performing_spot "&ge; 1"
  VARCHAR(6) broadcast_half "IN ('First', 'Second')"
  INT finishing_spot "&ge; 1"
}

broadcast_competitor_points_award {
  INT row_id PK "IDENTITY"
  UNIQUEIDENTIFIER broadcast_id FK
  UNIQUEIDENTIFIER competing_country_id FK
  UNIQUEIDENTIFIER voting_country_id
  VARCHAR(8) voting_method "IN ('Jury', 'Televote')"
  INT points_value "BETWEEN 0 AND 12"
}

broadcast_jury {
  UNIQUEIDENTIFIER broadcast_id PK,FK
  UNIQUEIDENTIFIER voting_country_id PK
  BIT points_awarded
}

broadcast ||--|{ broadcast_televote : owns
broadcast ||--|{ broadcast_competitor : owns
broadcast ||--o{ broadcast_jury : owns

broadcast_competitor ||--o{ broadcast_competitor_points_award : owns
```

**Notes:**

- All columns are `NOT NULL`
- In the `broadcast` table:
  - there is a unique index on (`parent_contest_id`, `contest_stage`)
- In the `broadcast_competitor` table:
  - there is a unique index on (`broadcast_id`, `performing_spot`)
- In the `broadcast_competitor_points_award` table:
  - there is a unique index on (`broadcast_id`, `competing_country_id`, `voting_country_id`, `voting_method`)
  - there is an index on `competing_country_id`
  - there is an index on `voting_country_id`
  - there is a check constraint that ensures `competing_country_id` != `voting_country_id`

## `Contest` aggregate tables

```mermaid
erDiagram

contest {
  UNIQUEIDENTIFIER contest_id PK
  INT contest_year UK "BETWEEN 2016 AND 2030"
  NVARCHAR(150) city_name
  VARCHAR(15) semi_final_voting_format "IN ('JuryAndTelevote', 'TelevoteOnly')"
  VARCHAR(15) grand_final_voting_format "IN ('JuryAndTelevote', 'TelevoteOnly')"
  BIT queryable
  UNIQUEIDENTIFIER global_televote_voting_country_id "NULL"
}

contest_child_broadcast {
  UNIQUEIDENTIFIER contest_id PK,FK
  UNIQUEIDENTIFIER child_broadcast_id PK
  VARCHAR(10) contest_stage "IN ('SemiFinal1', 'SemiFinal2', 'GrandFinal')"
  BIT completed
}

contest_participant {
  UNIQUEIDENTIFIER contest_id PK,FK
  UNIQUEIDENTIFIER participating_country_id PK
  VARCHAR(10) semi_final_draw "IN ('SemiFinal1', 'SemiFinal2')"
  NVARCHAR(150) act_name
  NVARCHAR(150) song_title
}

contest ||--o{ contest_child_broadcast : owns
contest ||--|{ contest_participant : owns

```

**Notes:**

- All columns are `NOT NULL` unless explicitly labelled `NULL`

## `Country` aggregate tables

```mermaid
erDiagram

country {
  UNIQUEIDENTIFIER country_id PK
  CHAR(2) country_code UK
  NVARCHAR(150) country_name
  VARCHAR(6) country_type "IN ('Real', 'Pseudo')"
}

country_active_contest {
  INT row_id PK "IDENTITY"
  UNIQUEIDENTIFIER country_id FK
  UNIQUEIDENTIFIER contest_id
}

country ||--o{ country_active_contest : owns
```

**Notes:**

- All columns are `NOT NULL`
- In the `country_active_contest`:
  - there is a unique index on (`country_id`, `contest_id`)

## Logging table

```mermaid
erDiagram

  log_entry {
    BIGINT row_id PK "IDENTITY"
    UNIQUEIDENTIFIER correlation_id
    DATETIME2 timestamp_utc
    VARCHAR(11) log_level "IN ( 'Trace', 'Debug', 'Information', 'Warning', 'Error', 'Critical', 'None' )"
    VARCHAR(16) log_entry_type "IN ('HttpRequest', 'HttpResponse', 'InternalRequest', 'InternalResponse', 'Exception')"
    NVARCHAR(2048) http_request_path "SPARSE NULL"
    VARCHAR(10) http_request_method "SPARSE NULL"
    NVARCHAR(2048) http_request_query_string "SPARSE NULL"
    NVARCHAR(200) http_request_endpoint_display_name "SPARSE NULL"
    INT http_response_status_code "SPARSE NULL"
    NVARCHAR(500) internal_request_path "SPARSE NULL"
    BIT internal_response_successful "SPARSE NULL"
    NVARCHAR(MAX) internal_response_domain_error "SPARSE NULL"
    VARCHAR(500) exception_type "SPARSE NULL"
    NVARCHAR(1024) exception_message "SPARSE NULL"
    NVARCHAR(MAX) exception_stack_trace "SPARSE NULL"
  }
```

**Notes:**

- All columns are `NOT NULL` unless explicitly labelled `NULL`
- In the `log_entry` table:
  - `internal_response_domain_error`, when not `NULL`, is a JSON-serialized `DomainError` object
  - there is an index on `timestamp_utc`
  - there is an index on `correlation_id`
  - records are populated using the Table-Per-Hierarchy pattern, with the `log_entry_type` column as the discriminator
  - there is an `AFTER INSERT` trigger that deletes all rows with a `timestamp_utc` more than 60 days earlier than the inserted record
