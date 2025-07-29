# Initial release: *public-api* features

This document lists the features with *public-api* scope for the initial release of *Eurocentric*.

- [Initial release: *public-api* features](#initial-release-public-api-features)
  - [Queryables](#queryables)
    - [P01: Get Queryable Broadcasts](#p01-get-queryable-broadcasts)
    - [P02: Get Queryable Contests](#p02-get-queryable-contests)
    - [P03: Get Queryable Contest Stages](#p03-get-queryable-contest-stages)
    - [P04: Get Queryable Countries](#p04-get-queryable-countries)
    - [P05: Get Queryable Voting Methods](#p05-get-queryable-voting-methods)
  - [Rankings](#rankings)
    - [P06: Get Competing Country Points Average Rankings](#p06-get-competing-country-points-average-rankings)
    - [P07: Get Competing Country Points Consensus Rankings](#p07-get-competing-country-points-consensus-rankings)
    - [P08: Get Competing Country Points In Range Rankings](#p08-get-competing-country-points-in-range-rankings)
    - [P09: Get Competing Country Points Share Rankings](#p09-get-competing-country-points-share-rankings)
    - [P10: Get Competitor Points Average Rankings](#p10-get-competitor-points-average-rankings)
    - [P11: Get Competitor Points Consensus Rankings](#p11-get-competitor-points-consensus-rankings)
    - [P12: Get Competitor Points In Range Rankings](#p12-get-competitor-points-in-range-rankings)
    - [P13: Get Competitor Points Share Rankings](#p13-get-competitor-points-share-rankings)
    - [P14: Get Voting Country Points Average Rankings](#p14-get-voting-country-points-average-rankings)
    - [P15: Get Voting Country Points Consensus Rankings](#p15-get-voting-country-points-consensus-rankings)
    - [P16: Get Voting Country Points In Range Rankings](#p16-get-voting-country-points-in-range-rankings)
    - [P17: Get Voting Country Points Share Rankings](#p17-get-voting-country-points-share-rankings)

## Queryables

### P01: Get Queryable Broadcasts

**User story:**

- **AS** a *Euro-Fan*
- **I WANT** to retrieve a list of all the queryable broadcasts
  - ordered by broadcast date
- **SO THAT** I can plan my queries.

**Endpoint:**

```
GET /public/api/v1.0/queryables/broadcasts
```

### P02: Get Queryable Contests

**User story:**

- **AS** a *Euro-Fan*
- **I WANT** to retrieve a list of all the queryable contests
  - ordered by contest year
- **SO THAT** I can plan my queries.

**Endpoint:**

```
GET /public/api/v1.0/queryables/contests
```

### P03: Get Queryable Contest Stages

**User story:**

- **AS** a *Euro-Fan*
- **I WANT** to retrieve a list of all the queryable contest stage filter enum values
- **SO THAT** I can plan my queries.

**Endpoint:**

```
GET /public/api/v1.0/queryables/contest-stages
```

### P04: Get Queryable Countries

**User story:**

- **AS** a *Euro-Fan*
- **I WANT** to retrieve a list of all the queryable countries
  - ordered by country code
- **SO THAT** I can plan my queries.

**Endpoint:**

```
GET /public/api/v1.0/queryables/countries
```

### P05: Get Queryable Voting Methods

**User story:**

- **AS** a *Euro-Fan*
- **I WANT** to retrieve a list of all the queryable voting method filter enum values
- **SO THAT** I can plan my queries.

**Endpoint:**

```
GET /public/api/v1.0/queryables/voting-methods
```

## Rankings

### P06: Get Competing Country Points Average Rankings

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

### P07: Get Competing Country Points Consensus Rankings

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

### P08: Get Competing Country Points In Range Rankings

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

### P09: Get Competing Country Points Share Rankings

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

### P10: Get Competitor Points Average Rankings

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

### P11: Get Competitor Points Consensus Rankings

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

### P12: Get Competitor Points In Range Rankings

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

### P13: Get Competitor Points Share Rankings

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

### P14: Get Voting Country Points Average Rankings

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

### P15: Get Voting Country Points Consensus Rankings

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

### P16: Get Voting Country Points In Range Rankings

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

### P17: Get Voting Country Points Share Rankings

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
