# 5. Initial release: *public-api* features

This document lists the features with *public-api* scope for the initial release of *Eurocentric*.

- [5. Initial release: *public-api* features](#5-initial-release-public-api-features)
  - [Competing Country Rankings](#competing-country-rankings)
    - [P01: Get Competing Country Points Average Rankings](#p01-get-competing-country-points-average-rankings)
    - [P02: Get Competing Country Points Consensus Rankings](#p02-get-competing-country-points-consensus-rankings)
    - [P03: Get Competing Country Points In Range Rankings](#p03-get-competing-country-points-in-range-rankings)
    - [P04: Get Competing Country Points Share Rankings](#p04-get-competing-country-points-share-rankings)
  - [Competitor Rankings](#competitor-rankings)
    - [P05: Get Competitor Points Average Rankings](#p05-get-competitor-points-average-rankings)
    - [P06: Get Competitor Points Consensus Rankings](#p06-get-competitor-points-consensus-rankings)
    - [P07: Get Competitor Points In Range Rankings](#p07-get-competitor-points-in-range-rankings)
    - [P08: Get Competitor Points Share Rankings](#p08-get-competitor-points-share-rankings)
  - [Queryables](#queryables)
    - [P09: Get Queryable Broadcasts](#p09-get-queryable-broadcasts)
    - [P10: Get Queryable Contests](#p10-get-queryable-contests)
    - [P11: Get Queryable Contest Stages](#p11-get-queryable-contest-stages)
    - [P12: Get Queryable Countries](#p12-get-queryable-countries)
    - [P13: Get Queryable Voting Methods](#p13-get-queryable-voting-methods)
  - [Voting Country Rankings](#voting-country-rankings)
    - [P14: Get Voting Country Points Average Rankings](#p14-get-voting-country-points-average-rankings)
    - [P15: Get Voting Country Points Consensus Rankings](#p15-get-voting-country-points-consensus-rankings)
    - [P16: Get Voting Country Points In Range Rankings](#p16-get-voting-country-points-in-range-rankings)
    - [P17: Get Voting Country Points Share Rankings](#p17-get-voting-country-points-share-rankings)

## Competing Country Rankings

### P01: Get Competing Country Points Average Rankings

**User story:**

- **AS** a *EuroFan*
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

### P02: Get Competing Country Points Consensus Rankings

**User story:**

- **AS** a *EuroFan*
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

### P03: Get Competing Country Points In Range Rankings

**User story:**

- **AS** a *EuroFan*
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

### P04: Get Competing Country Points Share Rankings

**User story:**

- **AS** a *EuroFan*
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

## Competitor Rankings

### P05: Get Competitor Points Average Rankings

**User story:**

- **AS** a *EuroFan*
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

### P06: Get Competitor Points Consensus Rankings

**User story:**

- **AS** a *EuroFan*
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

### P07: Get Competitor Points In Range Rankings

**User story:**

- **AS** a *EuroFan*
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

### P08: Get Competitor Points Share Rankings

**User story:**

- **AS** a *EuroFan*
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

## Queryables

### P09: Get Queryable Broadcasts

**User story:**

- **AS** a *EuroFan*
- **I WANT** to retrieve a list of all the queryable broadcasts
  - ordered by broadcast date
- **SO THAT** I can plan my queries.

**Endpoint:**

```
GET /public/api/v1.0/queryables/broadcasts
```

### P10: Get Queryable Contests

**User story:**

- **AS** a *EuroFan*
- **I WANT** to retrieve a list of all the queryable contests
  - ordered by contest year
- **SO THAT** I can plan my queries.

**Endpoint:**

```
GET /public/api/v1.0/queryables/contests
```

### P11: Get Queryable Contest Stages

**User story:**

- **AS** a *EuroFan*
- **I WANT** to retrieve a list of all the queryable contest stage enum values
- **SO THAT** I can plan my queries.

**Endpoint:**

```
GET /public/api/v1.0/queryables/contest-stages
```

### P12: Get Queryable Countries

**User story:**

- **AS** a *EuroFan*
- **I WANT** to retrieve a list of all the queryable countries
  - ordered by country code
- **SO THAT** I can plan my queries.

**Endpoint:**

```
GET /public/api/v1.0/queryables/countries
```

### P13: Get Queryable Voting Methods

**User story:**

- **AS** a *EuroFan*
- **I WANT** to retrieve a list of all the queryable voting method enum values
- **SO THAT** I can plan my queries.

**Endpoint:**

```
GET /public/api/v1.0/queryables/voting-methods
```

## Voting Country Rankings

### P14: Get Voting Country Points Average Rankings

**User story:**

- **AS** a *EuroFan*
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

- **AS** a *EuroFan*
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

- **AS** a *EuroFan*
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

- **AS** a *EuroFan*
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
