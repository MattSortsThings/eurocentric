# 04a: Initial release *admin-api* features

This document contains the feature specification for the initial release of *Eurocentric*'s *Admin API*.

- [04a: Initial release *admin-api* features](#04a-initial-release-admin-api-features)
  - [Broadcasts](#broadcasts)
    - [A01: Award Jury Points](#a01-award-jury-points)
    - [A02: Award Televote Points](#a02-award-televote-points)
    - [A03: Create Broadcast For Contest](#a03-create-broadcast-for-contest)
    - [A04: Delete Broadcast](#a04-delete-broadcast)
    - [A05: Disqualify Competitor](#a05-disqualify-competitor)
    - [A06: Get Broadcast](#a06-get-broadcast)
  - [Contests](#contests)
    - [A07: Create Contest](#a07-create-contest)
    - [A08: Delete Contest](#a08-delete-contest)
    - [A09: Get Contest](#a09-get-contest)
    - [A10: Get Contests](#a10-get-contests)
    - [A11: Handle Broadcast Completed](#a11-handle-broadcast-completed)
    - [A12: Handle Broadcast Deleted](#a12-handle-broadcast-deleted)
  - [Countries](#countries)
    - [A13: Create Country](#a13-create-country)
    - [A14: Delete Country](#a14-delete-country)
    - [A15: Get Countries](#a15-get-countries)
    - [A16: Get Country](#a16-get-country)
    - [A17: Handle Contest Created](#a17-handle-contest-created)
    - [A18: Handle Contest Deleted](#a18-handle-contest-deleted)

## Broadcasts

### A01: Award Jury Points

**User Story**

- **As** the Admin
- **I want to** award the points for a jury in a broadcast
  - providing the broadcast's ID
  - and the jury country ID
  - and the ranked competitor country IDs
- **So that** I can add to the voting data that will eventually be queryable.

### A02: Award Televote Points

**User Story**

- **As** the Admin
- **I want to** award the points for a televote in a broadcast
  - providing the broadcast's ID
  - and the televote country ID
  - and the ranked competitor country IDs
- **So that** I can add to the voting data that will eventually be queryable.

### A03: Create Broadcast For Contest

**User Story**

- **As** the Admin
- **I want to** create a broadcast for a contest
  - providing the contest's ID
  - and the broadcast date
  - and its contest stage
  - and its competing participant country IDs
    - in running order
- **So that** I can start awarding points in the broadcast.

### A04: Delete Broadcast

**User Story**

- **As** the Admin
- **I want to** delete a broadcast
  - providing its ID
- **So that** I can recreate it with different details.

### A05: Disqualify Competitor

**User Story**

- **As** the Admin
- **I want to** disqualify a competitor from a broadcast
  - providing the broadcast's ID
  - and the competitor country ID
  - given that no points have been awarded in the broadcast
- **So that** the disqualified competitor is removed from the broadcast
  - and it is not possible for it to receive any points.

### A06: Get Broadcast

**User Story**

- **As** the Admin
- **I want to** retrieve a broadcast
  - providing its ID
- **So that** I can see which jury/televote points I need to award.

## Contests

### A07: Create Contest

**User Story**

- **As** the Admin
- **I want to** create a contest
  - providing its contest year
  - and its host city name
  - and its voting rules
  - and for each of its participants
    - providing its country ID
    - and its act name
    - and its song title
    - and its allocated semi-final
    - and its qualification route
  - and optionally its global televote country ID
- **So that** I can create the broadcasts for the contest
  - and award the points in the broadcasts.

### A08: Delete Contest

**User Story**

- **As** the Admin
- **I want to** delete a contest
  - providing its ID
- **So that** I can recreate it with different details.

### A09: Get Contest

**User Story**

- **As** the Admin
- **I want to** retrieve a contest
  - providing its ID
- **So that** I can review its details.

### A10: Get Contests

**User Story**

- **As** the Admin
- **I want to** retrieve all the contests
  - ordered by contest year
- **So that** I can see which contests I need to create.

### A11: Handle Broadcast Completed

- **As** the Admin
- **I want** a contest to update itself
  - when I award points in one of its broadcasts
  - causing the broadcast to be completed
- **So that** the queryable data is kept up to date.

### A12: Handle Broadcast Deleted

- **As** the Admin
- **I want** a contest to update itself
  - when I delete one of its broadcasts
- **So that** the queryable data is kept up to date.

## Countries

### A13: Create Country

**User Story**

- **As** the Admin
- **I want to** create a country
  - providing its country code
  - and its country name
  - and its country type
- **So that** I can reference it in contests I will create.

### A14: Delete Country

**User Story**

- **As** the Admin
- **I want to** delete a country
  - providing its ID
- **So that** I can recreate it with different details.

### A15: Get Countries

**User Story**

- **As** the Admin
- **I want to** retrieve all the countries
  - ordered by country code
- **So that** I can see which countries I need to create.

### A16: Get Country

**User Story**

- **As** the Admin
- **I want to** retrieve a country
  - providing its ID
- **So that** I can review its details.

### A17: Handle Contest Created

- **As** the Admin
- **I want** a country to update itself
  - when I create a contest in which it is involved
- **So that** the queryable data is kept up to date
  - and it is impossible to delete any country that is now involved in a contest.

### A18: Handle Contest Deleted

- **As** the Admin
- **I want** a country to update itself
  - when I delete a contest in which it is involved
- **So that** the queryable data is kept up to date
  - and it is possible to delete any country that is now involved in no contests.
