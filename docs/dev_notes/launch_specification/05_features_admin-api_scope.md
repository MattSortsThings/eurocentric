# 05: Features: *admin-api* scope

This document is part of the [launch specification](../README.md#launch-specification).

- [05: Features: *admin-api* scope](#05-features-admin-api-scope)
  - [Broadcasts](#broadcasts)
    - [aa01: Award Broadcast Jury Points](#aa01-award-broadcast-jury-points)
    - [aa02: Award Broadcast Televote Points](#aa02-award-broadcast-televote-points)
    - [aa03: Delete Broadcast](#aa03-delete-broadcast)
    - [aa04: Get Broadcast](#aa04-get-broadcast)
    - [aa05: Get Broadcasts](#aa05-get-broadcasts)
  - [Contests](#contests)
    - [aa06: Create Contest](#aa06-create-contest)
    - [aa07: Create Contest Broadcast](#aa07-create-contest-broadcast)
    - [aa08: Delete Contest](#aa08-delete-contest)
    - [aa09: Get Contest](#aa09-get-contest)
    - [aa10: Get Contests](#aa10-get-contests)
    - [aa11: Handle Broadcast Completed](#aa11-handle-broadcast-completed)
    - [aa12: Handle Broadcast Created](#aa12-handle-broadcast-created)
    - [aa13: Handle Broadcast Deleted](#aa13-handle-broadcast-deleted)
  - [Countries](#countries)
    - [aa14: Create Country](#aa14-create-country)
    - [aa15: Delete Country](#aa15-delete-country)
    - [aa16: Get Countries](#aa16-get-countries)
    - [aa17: Get Country](#aa17-get-country)
    - [aa18: Handle Contest Created](#aa18-handle-contest-created)
    - [aa19: Handle Contest Deleted](#aa19-handle-contest-deleted)

## Broadcasts

### aa01: Award Broadcast Jury Points

**Endpoint:**

```http request
PATCH /admin/api/v1.0/broadcasts/{broadcastId}/award-jury-points
```

**User story:**

- As the Admin
- I want to award the points from a jury to the competitors in a broadcast
  - providing
    - the ID of the requested broadcast
    - the jury's voting country ID
    - the competing country IDs in rank order
- so that I can add to the voting data that will eventually become queryable using the Public API.

### aa02: Award Broadcast Televote Points

**Endpoint:**

```http request
PATCH /admin/api/v1.0/broadcasts/{broadcastId}/award-televote-points
```

**User story:**

- As the Admin
- I want to award the points from a televote to the competitors in a broadcast
  - providing
    - the ID of the requested broadcast
    - the televote's voting country ID
    - the competing country IDs in rank order
- so that I can add to the voting data that will eventually become queryable using the Public API.

### aa03: Delete Broadcast

**Endpoint:**

```http request
DELETE /admin/api/v1.0/broadcasts/{broadcastId}
```

**User story:**

- As the Admin
- I want to delete a single broadcast
  - providing
    - the ID of the requested broadcast
- so that the requested broadcast is completely removed
  - and I am free to create a new broadcast with the same contest stage and parent contest ID, and the same broadcast date, if I wish.

### aa04: Get Broadcast

**Endpoint:**

```http request
GET /admin/api/v1.0/broadcasts/{broadcastId}
```

**User story:**

- As the Admin
- I want to retrieve a single broadcast
  - providing
    - the ID of the requested broadcast
- so that I can review the current status of the requested broadcast.

### aa05: Get Broadcasts

**Endpoint:**

```http request
GET /admin/api/v1.0/broadcasts
```

**User story:**

- As the Admin
- I want to retrieve a list of all existing broadcasts
  - ordered by broadcast date
- so that I can verify the behaviour of features that create, update, or delete one or more broadcasts.

## Contests

### aa06: Create Contest

**Endpoint:**

```http request
POST /admin/api/v1.0/contests
```

**User story:**

- As the Admin
- I want to create a new contest
  - providing
    - the contest year
    - the city name
    - the Semi-Final voting rules
    - the optional global televote voting country ID
    - the group 1 participants, for each
      - the participating country ID
      - the act name
      - the song title
    - the group 2 participants, for each
      - the participating country ID
      - the act name
      - the song title
  - receiving
    - the created contest
    - its location
- so that I can create the broadcasts for the contest.

### aa07: Create Contest Broadcast

**Endpoint:**

```http request
POST /admin/api/v1.0/contests/{contestId}/broadcasts
```

**User story:**

- As the Admin
- I want to create a new broadcast for an existing contest
  - providing
    - the ID of the requested contest
    - the contest stage
    - the broadcast date
    - the competing country IDs in running order
      - including any vacant running order spots
  - receiving
    - the created broadcast
    - its location
- so that I can start awarding points in the broadcast.

### aa08: Delete Contest

**Endpoint:**

```http request
DELETE /admin/api/v1.0/contests/{contestId}
```

**User story:**

- As the Admin
- I want to delete a single contest
  - providing
    - the ID of the requested contest
- so that the requested contest is completely removed
  - and I am free to create a new contest with the same contest year if I wish.

### aa09: Get Contest

**Endpoint:**

```http request
GET /admin/api/v1.0/contests/{contestId}
```

**User story:**

- As the Admin
- I want to retrieve a single contest
  - providing
    - the ID of the requested contest
- so that I can review the current status of the requested contest.

### aa10: Get Contests

**Endpoint:**

```http request
GET /admin/api/v1.0/contests
```

**User story:**

- As the Admin
- I want to retrieve a list of all existing contests
  - ordered by contest year
- so that I can verify the behaviour of features that create, update, or delete one or more contests.

### aa11: Handle Broadcast Completed

**User story:**

- As the Admin
  - when I am awarding the final set of points in a broadcast
- I want the parent contest of the completed broadcast to update itself in the same transaction
  - setting the corresponding child broadcast to completed
  - and setting itself to completed if it owns 3 child broadcasts, all of which are completed
- so that all contests' child broadcasts are always up to date
  - and contests with one or more child broadcasts cannot be deleted
  - and contests with no child broadcasts can be deleted
  - and only data for complete contests constitute the queryable voting data for the Public API.

### aa12: Handle Broadcast Created

**User story:**

- As the Admin
  - when I am creating a broadcast
- I want the parent contest of the created broadcast to update itself in the same transaction
  - adding a child broadcast referencing the created broadcast
- so that all contests' child broadcasts are always up to date
  - and contests with one or more child broadcasts cannot be deleted
  - and contests with no child broadcasts can be deleted
  - and only data for complete contests constitute the queryable voting data for the Public API.

### aa13: Handle Broadcast Deleted

**User story:**

- As the Admin
  - when I am deleting a broadcast
- I want the parent contest of the deleted broadcast to update itself in the same transaction
  - removing the corresponding child broadcast
  - setting itself to not complete
- so that all contests' child broadcasts are always up to date
  - and contests with one or more child broadcasts cannot be deleted
  - and contests with no child broadcasts can be deleted
  - and only data for complete contests constitute the queryable voting data for the Public API.

## Countries

### aa14: Create Country

**Endpoint:**

```http request
POST /admin/api/v1.0/countries
```

**User story:**

- As the Admin
- I want to create a new country
  - providing
    - the ISO 3166 alpha-2 country code
    - the short UK English country name
  - receiving
    - the created country
    - its location
- so that I can create contests in which the created country is a participant or a global televote.

### aa15: Delete Country

**Endpoint:**

```http request
DELETE /admin/api/v1.0/countries/{countryId}
```

**User story:**

- As the Admin
- I want to delete a single country
  - providing
    - the ID of the requested country
- so that the requested country is completely removed
  - and I am free to create a new country with the same country code if I wish.

### aa16: Get Countries

**Endpoint:**

```http request
GET /admin/api/v1.0/countries
```

**User story:**

- As the Admin
- I want to retrieve a list of all existing countries
  - ordered by country code
- so that I can verify the behaviour of features that create, update, or delete one or more countries.

### aa17: Get Country

**Endpoint:**

```http request
GET /admin/api/v1.0/countries/{countryId}
```

**User story:**

- As the Admin
- I want to retrieve a single country
  - providing
    - the ID of the requested country
- so that I can review the current status of the requested country.

### aa18: Handle Contest Created

**User story:**

- As the Admin
  - when I am creating a contest
- I want every country that is a participant or global televote in the created contest to update itself in the same transaction
  - adding a contest role referencing the created contest
- so that all countries' contest roles are always up to date
  - and countries with one or more contest roles cannot be deleted
  - and countries with no contest roles can be deleted.

### aa19: Handle Contest Deleted

**User story:**

- As the Admin
  - when I am deleting a contest
- I want every country that is a participant or global televote in the deleted contest to update itself in the same transaction
  - removing the contest role referencing the deleted contest
- so that all countries' contest roles are always up to date
  - and countries with one or more contest roles cannot be deleted
  - and countries with no contest roles can be deleted.
