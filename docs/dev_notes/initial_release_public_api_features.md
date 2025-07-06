# Initial release: *public-api* features

This document lists the features with *public-api* scope for the initial release of *Eurocentric*.

- [Initial release: *public-api* features](#initial-release-public-api-features)
  - [Filters](#filters)
    - [P01: Get Broadcasts](#p01-get-broadcasts)
    - [P02: Get Contests](#p02-get-contests)
    - [P03: Get Contest Stages](#p03-get-contest-stages)
    - [P04: Get Countries](#p04-get-countries)
    - [P05: Get Voting Methods](#p05-get-voting-methods)
  - [Queryable Data](#queryable-data)
    - [P06: Handle Queryable Data Refreshed](#p06-handle-queryable-data-refreshed)
  - [Rankings](#rankings)
    - [P07: Get Competing Country Points Average Rankings](#p07-get-competing-country-points-average-rankings)
    - [P08: Get Competing Country Points Consensus Rankings](#p08-get-competing-country-points-consensus-rankings)
    - [P09: Get Competing Country Points In Range Rankings](#p09-get-competing-country-points-in-range-rankings)
    - [P10: Get Competing Country Points Share Rankings](#p10-get-competing-country-points-share-rankings)
    - [P11: Get Competitor Points Average Rankings](#p11-get-competitor-points-average-rankings)
    - [P12: Get Competitor Points Consensus Rankings](#p12-get-competitor-points-consensus-rankings)
    - [P13: Get Competitor Points In Range Rankings](#p13-get-competitor-points-in-range-rankings)
    - [P14: Get Competitor Points Share Rankings](#p14-get-competitor-points-share-rankings)
    - [P15: Get Voting Country Points Average Rankings](#p15-get-voting-country-points-average-rankings)
    - [P16: Get Voting Country Points Consensus Rankings](#p16-get-voting-country-points-consensus-rankings)
    - [P17: Get Voting Country Points In Range Rankings](#p17-get-voting-country-points-in-range-rankings)
    - [P18: Get Voting Country Points Share Rankings](#p18-get-voting-country-points-share-rankings)
  - [Scoreboards](#scoreboards)
    - [P19: Get Scoreboard](#p19-get-scoreboard)

## Filters

### P01: Get Broadcasts

**User story:**

- **AS** a *Euro-Fan*
- **I WANT** to retrieve a list of all the available broadcasts
  - ordered by broadcast date
- **SO THAT** I can plan my queries.

**Endpoint:**

```
GET /public/api/v1.0/filters/broadcasts
```

### P02: Get Contests

**User story:**

- **AS** a *Euro-Fan*
- **I WANT** to retrieve a list of all the available contests
  - ordered by contest year
- **SO THAT** I can plan my queries.

**Endpoint:**

```
GET /public/api/v1.0/filters/contests
```

### P03: Get Contest Stages

**User story:**

- **AS** a *Euro-Fan*
- **I WANT** to retrieve a list of all the contest stage filter enum values
- **SO THAT** I can plan my queries.

**Endpoint:**

```
GET /public/api/v1.0/filters/contest-stages
```

### P04: Get Countries

**User story:**

- **AS** a *Euro-Fan*
- **I WANT** to retrieve a list of all the available countries
  - ordered by country code
- **SO THAT** I can plan my queries.

**Endpoint:**

```
GET /public/api/v1.0/filters/countries
```

### P05: Get Voting Methods

**User story:**

- **AS** a *Euro-Fan*
- **I WANT** to retrieve a list of all the voting method filter enum values
- **SO THAT** I can plan my queries.

**Endpoint:**

```
GET /public/api/v1.0/filters/voting-methods
```

## Queryable Data

### P06: Handle Queryable Data Refreshed

**User Story:**

- **AS** a *Euro-Fan*
- **I WANT** the system's queryable data to be repopulated every time it has been refreshed by the *Admin*
- **SO THAT** the *Public API*'s queryable data is always up-to-date.

## Rankings

### P07: Get Competing Country Points Average Rankings

**User story:**

- **AS** a *Euro-Fan*
- **I WANT** to rank every competing country by its POINTS AVERAGE,
  - that is
    - the average value of all the individual points awards it received
  - and receive a page of rankings
  - optionally filtering the queried data
    - by contest year(s), and/or
    - by contest stage(s), and/or
    - by voting method(s), and/or
    - by voting country
  - optionally specifying
    - the page index, and/or
    - the page size, and/or
    - whether the rankings should be in descending rank order
- **SO THAT** I can represent the page of rankings
  - in a table
  - or a chart
  - or some other illustrative purpose.

**Endpoint:**

```
GET /public/api/v1.0/rankings/competing-countries/points-average
```

### P08: Get Competing Country Points Consensus Rankings

**User story:**

- **AS** a *Euro-Fan*
- **I WANT** to rank every competing country by its POINTS CONSENSUS,
  - that is
    - the cosine similarity of the televote and jury points awards it received
  - and receive a page of rankings
  - optionally filtering the queried data
    - by contest year(s), and/or
    - by contest stage(s), and/or
    - by voting country
  - optionally specifying
    - the page index, and/or
    - the page size, and/or
    - whether the rankings should be in descending rank order
- **SO THAT** I can represent the page of rankings
  - in a table
  - or a chart
  - or some other illustrative purpose.

**Endpoint:**

```
GET /public/api/v1.0/rankings/competing-countries/points-consensus
```

### P09: Get Competing Country Points In Range Rankings

**User story:**

- **AS** a *Euro-Fan*
- **I WANT** to rank every competing country by its POINTS IN RANGE,
  - that is
    - the relative frequency of all the individual points awards it received having a value in a specified range
  - and receive a page of rankings
  - providing
    - the minimum points value, and
    - the maximum points value
  - optionally filtering the queried data
    - by contest year(s), and/or
    - by contest stage(s), and/or
    - by voting method(s), and/or
    - by voting country
  - optionally specifying
    - the page index, and/or
    - the page size, and/or
    - whether the rankings should be in descending rank order
- **SO THAT** I can represent the page of rankings
  - in a table
  - or a chart
  - or some other illustrative purpose.

**Endpoint:**

```
GET /public/api/v1.0/rankings/competing-countries/points-in-range
```

### P10: Get Competing Country Points Share Rankings

**User story:**

- **AS** a *Euro-Fan*
- **I WANT** to rank every competing country by its POINTS SHARE,
  - that is
    - the sum total points it received as a fraction of the maximum possible points
  - and receive a page of rankings
  - optionally filtering the queried data
    - by contest year(s), and/or
    - by contest stage(s), and/or
    - by voting method(s), and/or
    - by voting country
  - optionally specifying
    - the page index, and/or
    - the page size, and/or
    - whether the rankings should be in descending rank order
- **SO THAT** I can represent the page of rankings
  - in a table
  - or a chart
  - or some other illustrative purpose.

**Endpoint:**

```
GET /public/api/v1.0/rankings/competing-countries/points-share
```

### P11: Get Competitor Points Average Rankings

**User story:**

- **AS** a *Euro-Fan*
- **I WANT** to rank every competitor in every broadcast by its POINTS AVERAGE,
  - that is
    - the average value of all the individual points awards it received
  - and receive a page of rankings
  - optionally filtering the queried data
    - by contest year(s), and/or
    - by contest stage(s), and/or
    - by voting method(s)
  - optionally specifying
    - the page index, and/or
    - the page size, and/or
    - whether the rankings should be in descending rank order
- **SO THAT** I can represent the page of rankings
  - in a table
  - or a chart
  - or some other illustrative purpose.

**Endpoint:**

```
GET /public/api/v1.0/rankings/competitors/points-average
```

### P12: Get Competitor Points Consensus Rankings

**User story:**

- **AS** a *Euro-Fan*
- **I WANT** to rank every competitor in every broadcast by its POINTS CONSENSUS,
  - that is
    - the cosine similarity of the televote and jury points awards it received
  - and receive a page of rankings
  - optionally filtering the queried data
    - by contest year(s), and/or
    - by contest stage(s)
  - optionally specifying
    - the page index, and/or
    - the page size, and/or
    - whether the rankings should be in descending rank order
- **SO THAT** I can represent the page of rankings
  - in a table
  - or a chart
  - or some other illustrative purpose.

**Endpoint:**

```
GET /public/api/v1.0/rankings/competitors/points-consensus
```

### P13: Get Competitor Points In Range Rankings

**User story:**

- **AS** a *Euro-Fan*
- **I WANT** to rank every competitor in every broadcast by its POINTS IN RANGE,
  - that is
    - the relative frequency of all the individual points awards it received having a value in a specified range
  - and receive a page of rankings
  - providing
    - the minimum points value, and
    - the maximum points value
  - optionally filtering the queried data
    - by contest year(s), and/or
    - by contest stage(s), and/or
    - by voting method(s)
  - optionally specifying
    - the page index, and/or
    - the page size, and/or
    - whether the rankings should be in descending rank order
- **SO THAT** I can represent the page of rankings
  - in a table
  - or a chart
  - or some other illustrative purpose.

**Endpoint:**

```
GET /public/api/v1.0/rankings/competitors/points-in-range
```

### P14: Get Competitor Points Share Rankings

**User story:**

- **AS** a *Euro-Fan*
- **I WANT** to rank every competitor in every broadcast by its POINTS SHARE,
  - that is
    - the sum total points it received as a fraction of the maximum possible points
  - and receive a page of rankings
  - optionally filtering the queried data
    - by contest year(s), and/or
    - by contest stage(s), and/or
    - by voting method(s)
  - optionally specifying
    - the page index, and/or
    - the page size, and/or
    - whether the rankings should be in descending rank order
- **SO THAT** I can represent the page of rankings
  - in a table
  - or a chart
  - or some other illustrative purpose.

**Endpoint:**

```
GET /public/api/v1.0/rankings/competitors/points-share
```

### P15: Get Voting Country Points Average Rankings

**User story:**

- **AS** a *Euro-Fan*
- **I WANT** to rank every voting country by its POINTS AVERAGE,
  - that is
    - the average value of all the individual points awards it gave to a specified competing country
  - and receive a page of rankings
  - providing
    - the competing country
  - optionally filtering the queried data
    - by contest year(s), and/or
    - by contest stage(s), and/or
    - by voting method(s)
  - optionally specifying
    - the page index, and/or
    - the page size, and/or
    - whether the rankings should be in descending rank order
- **SO THAT** I can represent the page of rankings
  - in a table
  - or a chart
  - or some other illustrative purpose.

**Endpoint:**

```
GET /public/api/v1.0/rankings/voting-countries/points-average
```

### P16: Get Voting Country Points Consensus Rankings

**User story:**

- **AS** a *Euro-Fan*
- **I WANT** to rank every voting country by its POINTS CONSENSUS,
  - that is
    - the cosine similarity of the televote and jury points awards it gave to a specified competing country
  - and receive a page of rankings
  - providing
    - the competing country
  - optionally filtering the queried data
    - by contest year(s), and/or
    - by contest stage(s)
  - optionally specifying
    - the page index, and/or
    - the page size, and/or
    - whether the rankings should be in descending rank order
- **SO THAT** I can represent the page of rankings
  - in a table
  - or a chart
  - or some other illustrative purpose.

**Endpoint:**

```
GET /public/api/v1.0/rankings/voting-countries/points-consensus
```

### P17: Get Voting Country Points In Range Rankings

**User story:**

- **AS** a *Euro-Fan*
- **I WANT** to rank every voting country by its POINTS IN RANGE,
  - that is
    - the relative frequency of all the individual points awards it gave to a specified competing country having a value in a specified range
  - and receive a page of rankings
  - providing
    - the competing country, and
    - the minimum points value, and
    - the maximum points value
  - optionally filtering the queried data
    - by contest year(s), and/or
    - by contest stage(s), and/or
    - by voting method(s)
  - optionally specifying
    - the page index, and/or
    - the page size, and/or
    - whether the rankings should be in descending rank order
- **SO THAT** I can represent the page of rankings
  - in a table
  - or a chart
  - or some other illustrative purpose.

**Endpoint:**

```
GET /public/api/v1.0/rankings/voting-countries/points-in-range
```

### P18: Get Voting Country Points Share Rankings

**User story:**

- **AS** a *Euro-Fan*
- **I WANT** to rank every competitor in every broadcast by its POINTS SHARE,
  - that is
    - the sum total points it gave to a specified competing country as a fraction of the maximum possible points
  - and receive a page of rankings
  - providing
    - the competing country
  - optionally filtering the queried data
    - by contest year(s), and/or
    - by contest stage(s), and/or
    - by voting method(s)
  - optionally specifying
    - the page index, and/or
    - the page size, and/or
    - whether the rankings should be in descending rank order
- **SO THAT** I can represent the page of rankings
  - in a table
  - or a chart
  - or some other illustrative purpose.

**Endpoint:**

```
GET /public/api/v1.0/rankings/voting-countries/points-share
```

## Scoreboards

### P19: Get Scoreboard

- **AS** a *Euro-Fan*
- **I WANT** to retrieve the scoreboard from a single broadcast
  - containing the total points, televote points, jury points, running order position and finishing position of each competitor in the broadcast
  - providing
    - the contest year, and
    - the contest stage
- **SO THAT** I can plan my queries.

**Endpoint:**

```
GET /public/api/v1.0/scoreboards
```
