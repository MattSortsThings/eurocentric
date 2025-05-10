# Initial release: *admin-api* features

This document lists the features with *admin-api* scope for the initial release of *Eurocentric*.

- [Initial release: *admin-api* features](#initial-release-admin-api-features)
  - [Broadcasts](#broadcasts)
    - [A01: Award Jury Points](#a01-award-jury-points)
    - [A02: Award Televote Points](#a02-award-televote-points)
    - [A03: Delete Broadcast](#a03-delete-broadcast)
    - [A04: Disqualify Competitor](#a04-disqualify-competitor)
    - [A05: Get Broadcast](#a05-get-broadcast)
  - [Contests](#contests)
    - [A06: Create Contest](#a06-create-contest)
    - [A07: Create Child Broadcast](#a07-create-child-broadcast)
    - [A08: Delete Contest](#a08-delete-contest)
    - [A09: Get Contest](#a09-get-contest)
    - [A10: Get Contests](#a10-get-contests)
    - [A11: Handle Broadcast Status Updated](#a11-handle-broadcast-status-updated)
    - [A12: Handle Broadcast Deleted](#a12-handle-broadcast-deleted)
  - [Countries](#countries)
    - [A13: Create Country](#a13-create-country)
    - [A14: Delete Country](#a14-delete-country)
    - [A15: Get Country](#a15-get-country)
    - [A16: Get Countries](#a16-get-countries)
    - [A17: Handle Contest Created](#a17-handle-contest-created)
    - [A18: Handle Contest Deleted](#a18-handle-contest-deleted)
    - [A19: Handle Contest Status Updated](#a19-handle-contest-status-updated)


## Broadcasts

### A01: Award Jury Points

**User Story:**

- **AS** the *Admin*
- **I WANT** to award a set of points for a jury in a broadcast in the system
  - providing
    - the broadcast ID
    - and the voting country ID
    - and the ranked competing country IDs
- **SO THAT** I can add to the voting data that will eventually become queryable by the *Public API*.

### A02: Award Televote Points

**User Story:**

- **AS** the *Admin*
- **I WANT** to award a set of points for a televote in a broadcast in the system
  - providing
    - the broadcast ID
    - and the voting country ID
    - and the ranked competing country IDs
- **SO THAT** I can add to the voting data that will eventually become queryable by the *Public API*.

### A03: Delete Broadcast

**User Story:**

- **AS** the *Admin*
- **I WANT** to delete a broadcast from the system
  - providing
    - the broadcast ID
- **SO THAT** I can re-create it with amended details.

### A04: Disqualify Competitor

**User Story:**

- **AS** the *Admin*
- **I WANT** to disqualify a competitor from a broadcast in the system
  - providing
    - the broadcast ID
    - and the competing country ID
- **SO THAT** no points can be awarded to the disqualified competitor.

### A05: Get Broadcast

**User Story:**

- **AS** the *Admin*
- **I WANT** to retrieve a broadcast from the system
  - providing
    - the broadcast ID
- **SO THAT** I can see which juries and/or contests still need to award points.

## Contests

### A06: Create Contest

**User Story:**

- **AS** the *Admin*
- **I WANT** to create a new contest in the system
  - providing
    - the contest year
    - and the city name
    - and the contest format
    - and the group 1 participants, for each
      - its country ID
      - and its act name
      - and its song title
    - and the group 2 participants, for each
      - its country ID
      - and its act name
      - and its song title
  - optionally providing
    - the group 0 participant country ID
- **SO THAT** I can create the broadcasts for the contest.

### A07: Create Child Broadcast

**User Story:**

- **AS** the *Admin*
- **I WANT** to create a child broadcast for a contest in the system
  - providing
    - the contest ID
    - and the contest stage
    - and the competing country IDs
- **SO THAT** I can start awarding points in the broadcast.

### A08: Delete Contest

**User Story:**

- **AS** the *Admin*
- **I WANT** to delete a contest from the system
  - providing
    - the contest ID
- **SO THAT** I can re-create it with amended details.

### A09: Get Contest

**User Story:**

- **AS** the *Admin*
- **I WANT** to retrieve a contest from the system
  - providing
    - the contest ID
- **SO THAT** I can review its details.

### A10: Get Contests

**User Story:**

- **AS** the *Admin*
- **I WANT** to retrieve all the contests from the system
  - ordered by contest year
- **SO THAT** I can see which contests I need to create.

### A11: Handle Broadcast Status Updated

**User Story:**

- **AS** the *Admin*
  - who has just modified a broadcast causing its status to change
- **I WANT** the parent contest for the broadcast
  - to replace the corresponding broadcast memo
- **SO THAT** the *Public API*'s queryable data is up to date.

### A12: Handle Broadcast Deleted

**User Story:**

- **AS** the *Admin*
  - who has deleted a broadcast
- **I WANT** the parent contest for the broadcast
  - to remove the corresponding broadcast memo
- **SO THAT** contests with no broadcast memos can be deleted.

## Countries

### A13: Create Country

**User Story:**

- **AS** the *Admin*
- **I WANT** to create a new country in the system
  - providing
    - the country code
    - and the country name
- **SO THAT** I can reference the country in contests I will create.

### A14: Delete Country

**User Story:**

- **AS** the *Admin*
- **I WANT** to delete a country from the system
  - providing
    - the country ID
- **SO THAT** I can re-create it with amended details.

### A15: Get Country

**User Story:**

- **AS** the *Admin*
- **I WANT** to retrieve a country from the system
  - providing
    - the country ID
- **SO THAT** I can review its details.

### A16: Get Countries

**User Story:**

- **AS** the *Admin*
- **I WANT** to retrieve all the countries from the system
  - ordered by country code
- **SO THAT** I can see which countries I need to create.

### A17: Handle Contest Created

**User Story:**

- **AS** the *Admin*
  - who has created a new contest in the system
- **I WANT** every country that participates in the contest
  - to add a contest memo
- **SO THAT** the referenced countries cannot be deleted from the system.

### A18: Handle Contest Deleted

**User Story:**

- **AS** the *Admin*
  - who has deleted a contest from the system
- **I WANT** every country that participates in the contest
  - to remove the corresponding contest memo
- **SO THAT** countries with no contest memos can be deleted from the system.

### A19: Handle Contest Status Updated

**User Story:**

- **AS** the *Admin*
  - who has just caused a contest in the system to change its status
- **I WANT** every country that participates in the contest
  - to replace the corresponding contest memo
- **SO THAT** the *Public API* has an up-to-date list of queryable countries.
