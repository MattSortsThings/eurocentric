# Initial release: *public-api* features

This document lists the features with *public-api* scope for the initial release of *Eurocentric*.

- [Initial release: *public-api* features](#initial-release-public-api-features)
  - [Competing Country Rankings](#competing-country-rankings)
    - [P01: Get Points Average Competing Country Rankings](#p01-get-points-average-competing-country-rankings)
    - [P02: Get Points Consensus Competing Country Rankings](#p02-get-points-consensus-competing-country-rankings)
    - [P03: Get Points In Range Competing Country Rankings](#p03-get-points-in-range-competing-country-rankings)
    - [P04: Get Points Share Competing Country Rankings](#p04-get-points-share-competing-country-rankings)
  - [Competitor Rankings](#competitor-rankings)
    - [P05: Get Points Average Competitor Rankings](#p05-get-points-average-competitor-rankings)
    - [P06: Get Points Consensus Competitor Rankings](#p06-get-points-consensus-competitor-rankings)
    - [P07: Get Points In Range Competitor Rankings](#p07-get-points-in-range-competitor-rankings)
    - [P08: Get Points Share Competitor Rankings](#p08-get-points-share-competitor-rankings)
  - [Filters](#filters)
    - [P09: Get Available Broadcasts](#p09-get-available-broadcasts)
    - [P10: Get Available Contests](#p10-get-available-contests)
    - [P11: Get Available Contest Stages](#p11-get-available-contest-stages)
    - [P12: Get Available Countries](#p12-get-available-countries)
    - [P13: Get Available Voting Methods](#p13-get-available-voting-methods)
  - [Scoreboards](#scoreboards)
    - [P14: Get Scoreboard Detail](#p14-get-scoreboard-detail)
    - [P15: Get Scoreboard Summary](#p15-get-scoreboard-summary)
  - [Voting Country Rankings](#voting-country-rankings)
    - [P16: Get Points Average Voting Country Rankings](#p16-get-points-average-voting-country-rankings)
    - [P17: Get Points Consensus Voting Country Rankings](#p17-get-points-consensus-voting-country-rankings)
    - [P18: Get Points In Range Voting Country Rankings](#p18-get-points-in-range-voting-country-rankings)
    - [P19: Get Points Share Voting Country Rankings](#p19-get-points-share-voting-country-rankings)

## Competing Country Rankings

### P01: Get Points Average Competing Country Rankings

**User story:**

- **AS** a Euro-Fan
- **I WANT** to rank every competing country by its POINTS AVERAGE,
  - that is
    - the average value of all the individual points awards it received
  - and receive a page of rankings
  - optionally filtering the queried data
    - by contest year(s), and/or
    - by contest stage(s), and/or
    - by voting method, and/or
    - by voting country
  - optionally specifying
    - the page index, and/or
    - the page size, and/or
    - whether the rankings should be in descending rank order
- **SO THAT** I can represent the page of rankings
  - in a table
  - or a chart
  - or some other illustrative purpose.

### P02: Get Points Consensus Competing Country Rankings

**User story:**

- **AS** a Euro-Fan
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

### P03: Get Points In Range Competing Country Rankings

**User story:**

- **AS** a Euro-Fan
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
    - by voting method, and/or
    - by voting country
  - optionally specifying
    - the page index, and/or
    - the page size, and/or
    - whether the rankings should be in descending rank order
- **SO THAT** I can represent the page of rankings
  - in a table
  - or a chart
  - or some other illustrative purpose.

### P04: Get Points Share Competing Country Rankings

**User story:**

- **AS** a Euro-Fan
- **I WANT** to rank every competing country by its POINTS SHARE,
  - that is
    - the sum total points it received as a fraction of the maximum possible points
  - and receive a page of rankings
  - optionally filtering the queried data
    - by contest year(s), and/or
    - by contest stage(s), and/or
    - by voting method, and/or
    - by voting country
  - optionally specifying
    - the page index, and/or
    - the page size, and/or
    - whether the rankings should be in descending rank order
- **SO THAT** I can represent the page of rankings
  - in a table
  - or a chart
  - or some other illustrative purpose.

## Competitor Rankings

### P05: Get Points Average Competitor Rankings

**User story:**

- **AS** a Euro-Fan
- **I WANT** to rank every competitor in every broadcast by its POINTS AVERAGE,
  - that is
    - the average value of all the individual points awards it received
  - and receive a page of rankings
  - optionally filtering the queried data
    - by contest year(s), and/or
    - by contest stage(s), and/or
    - by voting method
  - optionally specifying
    - the page index, and/or
    - the page size, and/or
    - whether the rankings should be in descending rank order
- **SO THAT** I can represent the page of rankings
  - in a table
  - or a chart
  - or some other illustrative purpose.

### P06: Get Points Consensus Competitor Rankings

**User story:**

- **AS** a Euro-Fan
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

### P07: Get Points In Range Competitor Rankings

**User story:**

- **AS** a Euro-Fan
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
    - by voting method
  - optionally specifying
    - the page index, and/or
    - the page size, and/or
    - whether the rankings should be in descending rank order
- **SO THAT** I can represent the page of rankings
  - in a table
  - or a chart
  - or some other illustrative purpose.

### P08: Get Points Share Competitor Rankings

**User story:**

- **AS** a Euro-Fan
- **I WANT** to rank every competitor in every broadcast by its POINTS SHARE,
  - that is
    - the sum total points it received as a fraction of the maximum possible points
  - and receive a page of rankings
  - optionally filtering the queried data
    - by contest year(s), and/or
    - by contest stage(s), and/or
    - by voting method
  - optionally specifying
    - the page index, and/or
    - the page size, and/or
    - whether the rankings should be in descending rank order
- **SO THAT** I can represent the page of rankings
  - in a table
  - or a chart
  - or some other illustrative purpose.

## Filters

### P09: Get Available Broadcasts

**User story:**

- **AS** a Euro-Fan
- **I WANT** to retrieve a list of all the available broadcasts
  - ordered by contest year
  - then by contest stage
- **SO THAT** I can plan my queries.

### P10: Get Available Contests

**User story:**

- **AS** a Euro-Fan
- **I WANT** to retrieve a list of all the available contests
  - ordered by contest year
- **SO THAT** I can plan my queries.

### P11: Get Available Contest Stages

**User story:**

- **AS** a Euro-Fan
- **I WANT** to retrieve a list of all the available contest stages
- **SO THAT** I can plan my queries.

### P12: Get Available Countries

**User story:**

- **AS** a Euro-Fan
- **I WANT** to retrieve a list of all the available countries
  - ordered by country code
- **SO THAT** I can plan my queries.

### P13: Get Available Voting Methods

**User story:**

- **AS** a Euro-Fan
- **I WANT** to retrieve a list of all the available voting methods
- **SO THAT** I can plan my queries.

## Scoreboards

### P14: Get Scoreboard Detail

- **AS** a Euro-Fan
- **I WANT** to retrieve a detailed section of the scoreboard from a specified broadcast
  - containing the points given and/or received by a specified country in the broadcast
  - providing
    - the contest year, and
    - the contest stage, and
    - the country
- **SO THAT** I can plan my queries.

### P15: Get Scoreboard Summary

- **AS** a Euro-Fan
- **I WANT** to retrieve a summary of the scoreboard from a specified broadcast
  - containing the total points, running order position and finishing position of each competitor in the broadcast
  - providing
    - the contest year, and
    - the contest stage
- **SO THAT** I can plan my queries.

## Voting Country Rankings

### P16: Get Points Average Voting Country Rankings

**User story:**

- **AS** a Euro-Fan
- **I WANT** to rank every voting country by its POINTS AVERAGE,
  - that is
    - the average value of all the individual points awards it gave to a specified competing country
  - and receive a page of rankings
  - providing
    - the competing country
  - optionally filtering the queried data
    - by contest year(s), and/or
    - by contest stage(s), and/or
    - by voting method
  - optionally specifying
    - the page index, and/or
    - the page size, and/or
    - whether the rankings should be in descending rank order
- **SO THAT** I can represent the page of rankings
  - in a table
  - or a chart
  - or some other illustrative purpose.

### P17: Get Points Consensus Voting Country Rankings

**User story:**

- **AS** a Euro-Fan
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

### P18: Get Points In Range Voting Country Rankings

**User story:**

- **AS** a Euro-Fan
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
    - by voting method
  - optionally specifying
    - the page index, and/or
    - the page size, and/or
    - whether the rankings should be in descending rank order
- **SO THAT** I can represent the page of rankings
  - in a table
  - or a chart
  - or some other illustrative purpose.

### P19: Get Points Share Voting Country Rankings

**User story:**

- **AS** a Euro-Fan
- **I WANT** to rank every competitor in every broadcast by its POINTS SHARE,
  - that is
    - the sum total points it gave to a specified competing country as a fraction of the maximum possible points
  - and receive a page of rankings
  - providing
    - the competing country
  - optionally filtering the queried data
    - by contest year(s), and/or
    - by contest stage(s), and/or
    - by voting method
  - optionally specifying
    - the page index, and/or
    - the page size, and/or
    - whether the rankings should be in descending rank order
- **SO THAT** I can represent the page of rankings
  - in a table
  - or a chart
  - or some other illustrative purpose.
