# Initial release: *admin-api* features

This document contains the feature specification for the initial release of *Eurocentric*'s *Admin API*.

- [Initial release: *admin-api* features](#initial-release-admin-api-features)
  - [Broadcasts](#broadcasts)
    - [A01: Award Jury Points](#a01-award-jury-points)
    - [A02: Award Televote Points](#a02-award-televote-points)
    - [A03: Delete Broadcast](#a03-delete-broadcast)
    - [A04: Disqualify Competing Country](#a04-disqualify-competing-country)
    - [A05: Get Broadcast](#a05-get-broadcast)
  - [Contests](#contests)
    - [A06: Create Contest](#a06-create-contest)
    - [A07: Create Contest Broadcast](#a07-create-contest-broadcast)
    - [A08: Delete Contest](#a08-delete-contest)
    - [A09: Get Contest](#a09-get-contest)
    - [A10: Get Contest Broadcasts](#a10-get-contest-broadcasts)
    - [A11: Get Contests](#a11-get-contests)
    - [A12: Handle Broadcast Completed](#a12-handle-broadcast-completed)
    - [A13: Handle Broadcast Deleted](#a13-handle-broadcast-deleted)
  - [Countries](#countries)
    - [A14: Create Country](#a14-create-country)
    - [A15: Delete Country](#a15-delete-country)
    - [A16: Get Country](#a16-get-country)
    - [A17: Get Countries](#a17-get-countries)
    - [A18: Handle Contest Created](#a18-handle-contest-created)
    - [A19: Handle Contest Deleted](#a19-handle-contest-deleted)

## Broadcasts

### A01: Award Jury Points

**User Story**

- **As** the Admin
- **I want to** award a set of jury points in a broadcast in the system
  - specifying
    - the broadcast's ID
    - and the voting country's ID
    - and the ranked competing countries' IDs
- **So that** I can add to the voting data for the broadcast
  - which will become queryable when its parent contest is completed.

**Endpoint**

```
PATCH /admin/api/v1.0/broadcasts/{broadcastId}/jury-points
```

### A02: Award Televote Points

**User Story**

- **As** the Admin
- **I want to** award a set of televote points in a broadcast in the system
  - specifying
    - the broadcast's ID
    - and the voting country's ID
    - and the ranked competing countries' IDs
- **So that** I can add to the voting data for the broadcast
  - which will become queryable when its parent contest is completed.

**Endpoint**

```
PATCH /admin/api/v1.0/broadcasts/{broadcastId}/televote-points
```

### A03: Delete Broadcast

**User Story**

- **As** the Admin
- **I want to** delete a broadcast from the system
  - specifying
    - the broadcast's ID
- **So that** I can re-create it with amended details.

**Endpoint**

```
DELETE /admin/api/v1.0/broadcasts/{broadcastId}
```

### A04: Disqualify Competing Country

**User Story**

- **As** the Admin
- **I want to** disqualify a competing country from a broadcast in the system
  - specifying
    - the broadcast's ID
    - and the competing country's ID
- **So that** it is not possible to award any points to the disqualified country.

**Endpoint**

```
PATCH /admin/api/v1.0/broadcasts/{broadcastId}/disqualifications
```

### A05: Get Broadcast

**User Story**

- **As** the Admin
- **I want to** get a broadcast in the system
  - specifying
    - the broadcast's ID
- **So that** I can review its details.

**Endpoint**

```
GET /admin/api/v1.0/broadcasts/{broadcastId}
```

## Contests

### A06: Create Contest

**User Story**

- **As** the Admin
- **I want to** create a contest in the system
  - specifying
    - the contest's year
    - and its city name
    - and whether it uses no juries in the semi-finals
    - and its participating countries allocated to the First Semi-Final
    - and its participating countries allocated to the Second Semi-Final
    - and its televote-only participating country IDs (if present)
- **So that** I can create its broadcasts and award points.

**Endpoint**

```
POST /admin/api/v1.0/contests
```

### A07: Create Contest Broadcast

**User Story**

- **As** the Admin
- **I want to** create a broadcast for a contest in the system
  - specifying
    - the contest's ID
    - and the broadcast's contest stage
    - and its competing country IDs
- **So that** I can award points in the broadcast
  - and the contest cannot be deleted.

**Endpoint**

```
POST /admin/api/v1.0/contests/{contestId}/broadcasts
```

### A08: Delete Contest

**User Story**

- **As** the Admin
- **I want to** delete a contest from the system
  - specifying
    - the contest's ID
- **So that** I can re-create it with amended details.

**Endpoint**

```
DELETE /admin/api/v1.0/contests/{contestId}
```

### A09: Get Contest

**User Story**

- **As** the Admin
- **I want to** get a contest in the system
  - specifying
    - the contest's ID
- **So that** I can review its details.

**Endpoint**

```
GET /admin/api/v1.0/contests/{contestId}
```

### A10: Get Contest Broadcasts

**User Story**

- **As** the Admin
- **I want to** get all the broadcasts for a contest in the system
  - ordered by contest stage
  - specifying
    - the contest's ID
- **So that** I can review their details.

**Endpoint**

```
GET /admin/api/v1.0/contests/{contestId}/broadcasts
```

### A11: Get Contests

**User Story**

- **As** the Admin
- **I want to** get all the contests in the system
  - ordered by contest year
- **So that** I can see which contests I need to create.

**Endpoint**

```
GET /admin/api/v1.0/contests
```

### A12: Handle Broadcast Completed

**User Story**

- **As** the Admin
- **I want to** be sure that a contest updates itself
  - when one of its broadcasts is completed
    - setting the corresponding member broadcast to completed
    - and setting its status to complete
      - if it has 3 member broadcasts and they are all completed
- **So that** completed contests are included in the system's queryable data.

### A13: Handle Broadcast Deleted

**User Story**

- **As** the Admin
- **I want to** be sure that a contest updates itself
  - when one of its broadcasts is deleted
    - removing the corresponding member broadcast
    - and setting its status to incomplete
- **So that** no incomplete contest is included in the system's queryable data
  - and the updated contest can be deleted
    - if it has 0 member broadcasts.

## Countries

### A14: Create Country

**User Story**

- **As** the Admin
- **I want to** create a country in the system
  - specifying
    - the country's country code
    - and its name
- **So that** I can reference it in contests I will create.

**Endpoint**

```
POST /admin/api/v1.0/countries
```

### A15: Delete Country

**User Story**

- **As** the Admin
- **I want to** delete a country from the system
  - specifying
    - the country's ID
- **So that** I can re-create it with amended details.

**Endpoint**

```
DELETE /admin/api/v1.0/countries/{countryId}
```

### A16: Get Country

**User Story**

- **As** the Admin
- **I want to** get a country in the system
  - specifying
    - the country's ID
- **So that** I can review its details.

**Endpoint**

```
GET /admin/api/v1.0/countries/{countryId}
```

### A17: Get Countries

**User Story**

- **As** the Admin
- **I want to** get all the countries in the system
  - ordered by country code
- **So that** I can see which countries I need to create.

**Endpoint**

```
GET /admin/api/v1.0/countries
```

### A18: Handle Contest Created

**User Story**

- **As** the Admin
- **I want to** be sure that every country in a new contest updates itself
  - adding a reference to the created contest
- **So that** the updated countries cannot be deleted.

### A19: Handle Contest Deleted

**User Story**

- **As** the Admin
- **I want to** be sure that every country in a deleted contest updates itself
  - removing the reference to the deleted contest
- **So that** each updated country can be deleted
  - if it contains 0 contest references.
