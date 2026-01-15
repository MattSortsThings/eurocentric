# 8 Database schema

This document is part of the [*Eurocentric* launch specification](README.md).

- [8 Database schema](#8-database-schema)
  - [`Country` aggregate tables](#country-aggregate-tables)
  - [`Contest` aggregate tables](#contest-aggregate-tables)
  - [`Broadcast` aggregate tables](#broadcast-aggregate-tables)
  - [`HttpRequestLogEntry` table](#httprequestlogentry-table)
  - [`HttpResponseLogEntry` table](#httpresponselogentry-table)
  - [`InternalRequestLogEntry` table](#internalrequestlogentry-table)
  - [`InternalResultLogEntry` table](#internalresultlogentry-table)
  - [`ExceptionLogEntry` table](#exceptionlogentry-table)

## `Country` aggregate tables

```mermaid
erDiagram

    country {
      UNIQUEIDENTIFIER country_id PK
      CHAR(2) country_code UK
      NVARCHAR(200) country_name
      VARCHAR(6) country_type "IN ('Real', 'Pseudo')"
    }

    country_contest_role {
      INT row_id PK
      UNIQUEIDENTIFIER country_id FK
      UNIQUEIDENTIFIER contest_id
      VARCHAR(14) contest_role_type "IN ('Participant', 'GlobalTelevote')"
    }

    country ||--o{ country_contest_role : owns
```

**Notes:**

- all columns are `NOT NULL` unless otherwise specified
- `country_contest_role.row_id` is generated on add

## `Contest` aggregate tables

```mermaid
erDiagram

  contest {
    UNIQUEIDENTIFIER contest_id PK
    INT contest_year UK "BETWEEN 2016 AND 2050"
    NVARCHAR(200) city_name
    VARCHAR(15) semi_final_voting_rules "IN ('TelevoteAndJury', 'TelevoteOnly')"
    VARCHAR(15) grand_final_voting_rules "IN ('TelevoteAndJury', 'TelevoteOnly')"
    BIT queryable
    UNIQUEIDENTIFIER global_televote_voting_country_id
  }

  contest_competitor {
    UNIQUEIDENTIFIER contest_id PK,FK
    UNIQUEIDENTIFIER competing_country_id PK
    VARCHAR(10) semi_final_draw "IN ('SemiFinal1', 'SemiFinal2')"
    NVARCHAR(200) act_name
    NVARCHAR(200) song_title
  }

  contest_broadcast_memo {
    INT row_id PK
    UNIQUEIDENTIFIER contest_id FK
    UNIQUEIDENTIFIER broadcast_id
    VARCHAR(10) contest_stage "IN ('SemiFinal1', 'SemiFinal2', 'GrandFinal')"
    BIT completed
  }

  contest ||--|{ contest_competitor : owns
  contest ||--o{ contest_broadcast_memo : owns
```

**Notes:**

- all columns are `NOT NULL` unless otherwise specified
- `contest.global_televote_voting_country_id` allows `NULL`
- `contest_broadcast_memo.row_id` is generated on add

## `Broadcast` aggregate tables

```mermaid
erDiagram

  broadcast {
    UNIQUEIDENTIFIER broadcast_id PK
    DATE broadcast_date UK
    UNIQUEIDENTIFIER parent_contest_id
    VARCHAR(10) contest_stage "IN ('SemiFinal1', 'SemiFinal2', 'GrandFinal')"
    VARCHAR(15) voting_rules "IN ('TelevoteAndJury', 'TelevoteOnly')"
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
    INT performing_spot ">= 1"
    VARCHAR(6) broadcast_half "IN ('First', 'Second')"
    INT finishing_spot ">= 1"
  }

  broadcast_competitor_points_award {
    INT row_id PK
    UNIQUEIDENTIFIER broadcast_id FK
    UNIQUEIDENTIFIER competing_country_id FK
    UNIQUEIDENTIFIER voting_country_id
    VARCHAR(8) voting_method "IN ('Televote', 'Jury')"
    INT points_value ">= 0"
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

- all columns are `NOT NULL` unless otherwise specified
- (`broadcast.parent_contest_id`, `broadcast.contest_stage`) is unique
- (`broadcast_competitor.broadcast_id`, `broadcast_competitor.performing_spot`) is unique
- `broadcast_competitor_points_award.row_id` is generated on add
- `broadcast_competitor_points_award.competing_country_id` must not equal `broadcast_competitor_points_award.voting_country_id`
- (`broadcast_competitor_points_award.broadcast_id`, `broadcast_competitor_points_award.competing_country_id`, `broadcast_competitor_points_award.voting_country_id`, `broadcast_competitor_points_award.voting_method`) is unique

## `HttpRequestLogEntry` table

```mermaid
erDiagram

  http_request_log_entry {
    BIGINT row_id PK
    DATETIME2 timestamp_utc
    UNIQUEIDENTIFIER correlation_id
    NVARCHAR(10) http_request_method
    NVARCHAR(MAX) http_request_path
    NVARCHAR(MAX) http_request_query_string
  }
```

**Notes:**

- all columns are `NOT NULL` unless otherwise specified
- `http_request_log_entry.row_id` is generated on add
- `http_request_log_entry.http_request_query_string` allows `NULL`

## `HttpResponseLogEntry` table

```mermaid
erDiagram

  http_response_log_entry {
    BIGINT row_id PK
    DATETIME2 timestamp_utc
    UNIQUEIDENTIFIER correlation_id
    INT http_response_status_code
  }
```

**Notes:**

- all columns are `NOT NULL`
- `http_response_log_entry.row_id` is generated on add

## `InternalRequestLogEntry` table

```mermaid
erDiagram

  internal_request_log_entry {
    BIGINT row_id PK
    DATETIME2 timestamp_utc
    UNIQUEIDENTIFIER correlation_id
    VARCHAR(200) internal_request_path
    VARCHAR(200) internal_request_type
  }
```

**Notes:**

- all columns are `NOT NULL`
- `internal_request_log_entry.row_id` is generated on add

## `InternalResultLogEntry` table

```mermaid
erDiagram

  internal_result_log_entry {
    BIGINT row_id PK
    DATETIME2 timestamp_utc
    UNIQUEIDENTIFIER correlation_id
    BIT internal_result_successful
    NVARCHAR(MAX) internal_result_domain_error
  }
```

**Notes:**

- all columns are `NOT NULL`
- `internal_result_log_entry.row_id` is generated on add
- `internal_result_log_entry.internal_result_domain_error` allows `NULL`
- `internal_result_log_entry.internal_result_domain_error` is a JSON-serialized `DomainError` object when it is not `NULL`

## `ExceptionLogEntry` table

```mermaid
erDiagram

  exception_log_entry {
    BIGINT row_id PK
    DATETIME2 timestamp_utc
    UNIQUEIDENTIFIER correlation_id
    VARCHAR(200) exception_type
    NVARCHAR(MAX) exception_message
    NVARCHAR(MAX) exception_stack_trace
  }
```

**Notes:**

- all columns are `NOT NULL` unless otherwise specified
- `exception_log_entry.row_id` is generated on add
- `exception_stack_trace` allows `NULL`
