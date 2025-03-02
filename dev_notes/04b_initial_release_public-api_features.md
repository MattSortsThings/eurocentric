# 04b: Initial release *public-api* features

This document contains the feature specification for the initial release of *Eurocentric*'s *Public API*.

- [04b: Initial release *public-api* features](#04b-initial-release-public-api-features)
  - [Broadcasts](#broadcasts)
    - [P01: Get Queryable Broadcasts](#p01-get-queryable-broadcasts)
  - [Competitor Points Average Rankings](#competitor-points-average-rankings)
    - [P02: Get Competitor Points Average Rankings](#p02-get-competitor-points-average-rankings)
  - [Competitor Points In Range Rankings](#competitor-points-in-range-rankings)
    - [P03: Get Competitor Points In Range Rankings](#p03-get-competitor-points-in-range-rankings)
  - [Competitor Points Share Rankings](#competitor-points-share-rankings)
    - [P04: Get Competitor Points Share Rankings](#p04-get-competitor-points-share-rankings)
  - [Competitor Points Similarity Rankings](#competitor-points-similarity-rankings)
    - [P05: Get Competitor Points Similarity Rankings](#p05-get-competitor-points-similarity-rankings)
  - [Contests](#contests)
    - [P06: Get Queryable Contests](#p06-get-queryable-contests)
  - [Countries](#countries)
    - [P07: Get Queryable Countries](#p07-get-queryable-countries)
  - [Participant Points Average Rankings](#participant-points-average-rankings)
    - [P08: Get Participant Points Average Rankings](#p08-get-participant-points-average-rankings)
  - [Participant Points In Range Rankings](#participant-points-in-range-rankings)
    - [P09: Get Participant Points In Range Rankings](#p09-get-participant-points-in-range-rankings)
  - [Participant Points Share Rankings](#participant-points-share-rankings)
    - [P10: Get Participant Points Share Rankings](#p10-get-participant-points-share-rankings)
  - [Participant Points Similarity Rankings](#participant-points-similarity-rankings)
    - [P11: Get Participant Points Similarity Rankings](#p11-get-participant-points-similarity-rankings)
  - [Scoreboards](#scoreboards)
    - [P12: Get Scoreboard](#p12-get-scoreboard)
  - [Voter Points Average Rankings](#voter-points-average-rankings)
    - [P13: Get Voter Points Average Rankings](#p13-get-voter-points-average-rankings)
  - [Voter Points In Range Rankings](#voter-points-in-range-rankings)
    - [P14: Get Voter Points In Range Rankings](#p14-get-voter-points-in-range-rankings)
  - [Voter Points Share Rankings](#voter-points-share-rankings)
    - [P15: Get Voter Points Share Rankings](#p15-get-voter-points-share-rankings)
  - [Voter Points Similarity Rankings](#voter-points-similarity-rankings)
    - [P16: Get Voter Points Similarity Rankings](#p16-get-voter-points-similarity-rankings)

## Broadcasts

### P01: Get Queryable Broadcasts

**User Story**

- **As** a Euro-Fan
- **I want to** retrieve all the queryable broadcasts
  - ordered by broadcast date
- **So that** I can plan my queries.

## Competitor Points Average Rankings

### P02: Get Competitor Points Average Rankings

**User Story**

- **As** a Euro-Fan
- **I want to** retrieve a page of rankings
  - for each competitor in each broadcast
  - using the POINTS AVERAGE metric: the average value of the points awards it received
  - optionally restricting the queried data to points awarded
    - using a given voting method
    - in a given range of years
    - in a given contest stage
  - optionally specifying the ranking direction
  - optionally specifying the page index and page size
- **So that** I can represent the page
  - in a table
  - or a chart
  - or some other illustration.

## Competitor Points In Range Rankings

### P03: Get Competitor Points In Range Rankings

**User Story**

- **As** a Euro-Fan
- **I want to** retrieve a page of rankings
  - for each competitor in each broadcast
  - using the POINTS IN RANGE metric: the relative frequency of the points awards it received having a value in a specified range
  - optionally restricting the queried data to points awarded
    - using a given voting method
    - in a given range of years
    - in a given set of contest stages
  - optionally specifying the ranking direction
  - optionally specifying the page index and page size
- **So that** I can represent the page
  - in a table
  - or a chart
  - or some other illustration.

## Competitor Points Share Rankings

### P04: Get Competitor Points Share Rankings

**User Story**

- **As** a Euro-Fan
- **I want to** retrieve a page of rankings
  - for each competitor in each broadcast
  - using the POINTS SHARE metric: the sum total points it received as a fraction of the available points
  - optionally restricting the queried data to points awarded
    - using a given voting method
    - in a given range of years
    - in a given set of contest stages
  - optionally specifying the ranking direction
  - optionally specifying the page index and page size
- **So that** I can represent the page
  - in a table
  - or a chart
  - or some other illustration.

## Competitor Points Similarity Rankings

### P05: Get Competitor Points Similarity Rankings

**User Story**

- **As** a Euro-Fan
- **I want to** retrieve a page of rankings
  - for each competitor in each broadcast
  - using the POINTS SIMILARITY metric: the cosine similarity of all the televote points and jury points it received, using each voting country as a vector dimension
  - optionally restricting the queried data to points awarded
    - in a given range of years
    - in a given set of contest stages
  - optionally specifying the ranking direction
  - optionally specifying the page index and page size
- **So that** I can represent the page
  - in a table
  - or a chart
  - or some other illustration.

## Contests

### P06: Get Queryable Contests

**User Story**

- **As** a Euro-Fan
- **I want to** retrieve all the queryable contests
  - ordered by contest year
- **So that** I can plan my queries.

## Countries

### P07: Get Queryable Countries

**User Story**

- **As** a Euro-Fan
- **I want to** retrieve all the queryable countries
  - ordered by country code
- **So that** I can plan my queries.

## Participant Points Average Rankings

### P08: Get Participant Points Average Rankings

**User Story**

- **As** a Euro-Fan
- **I want to** retrieve a page of rankings
  - for each participating country overall
  - using the POINTS AVERAGE metric: the average value of the points awards it received
  - optionally restricting the queried data to points awarded
    - using a given voting method
    - in a given range of years
    - in a given set of contest stages
    - by a given voting country
  - optionally specifying the ranking direction
  - optionally specifying the page index and page size
- **So that** I can represent the page
  - in a table
  - or a chart
  - or some other illustration.

## Participant Points In Range Rankings

### P09: Get Participant Points In Range Rankings

**User Story**

- **As** a Euro-Fan
- **I want to** retrieve a page of rankings
  - for each participating country overall
  - using the POINTS IN RANGE metric: the relative frequency of the points awards it received having a value in a specified range
  - optionally restricting the queried data to points awarded
    - using a given voting method
    - in a given range of years
    - in a given set of contest stages
    - by a given voting country
  - optionally specifying the ranking direction
  - optionally specifying the page index and page size
- **So that** I can represent the page
  - in a table
  - or a chart
  - or some other illustration.

## Participant Points Share Rankings

### P10: Get Participant Points Share Rankings

**User Story**

- **As** a Euro-Fan
- **I want to** retrieve a page of rankings
  - for each participating country overall
  - using the POINTS SHARE metric: the sum total points it received as a fraction of the available points
  - optionally restricting the queried data to points awarded
    - using a given voting method
    - in a given range of years
    - in a given set of contest stages
    - by a given voting country
  - optionally specifying the ranking direction
  - optionally specifying the page index and page size
- **So that** I can represent the page
  - in a table
  - or a chart
  - or some other illustration.

## Participant Points Similarity Rankings

### P11: Get Participant Points Similarity Rankings

**User Story**

- **As** a Euro-Fan
- **I want to** retrieve a page of rankings
  - for each participating country overall
  - using the POINTS SIMILARITY metric: the cosine similarity of all the televote points and jury points it received, using each voting country in each broadcast as a vector dimension
  - optionally restricting the queried data to points awarded
    - in a given range of years
    - in a given set of contest stages
    - by a given voting country
  - optionally specifying the ranking direction
  - optionally specifying the page index and page size
- **So that** I can represent the page
  - in a table
  - or a chart
  - or some other illustration.

## Scoreboards

### P12: Get Scoreboard

**User Story**

- **As** a Euro-Fan
- **I want to** retrieve the scoreboard from a broadcast
  - providing its contest year
  - and its contest stage
- **So that** I can see how every competitor fared overall
  - and which countries awarded jury points
  - and which countries awarded televote points.

## Voter Points Average Rankings

### P13: Get Voter Points Average Rankings

**User Story**

- **As** a Euro-Fan
- **I want to** retrieve a page of rankings
  - for each voting country overall
  - using the POINTS AVERAGE metric: the average value of the points awards it gave to a specified competing country
  - optionally restricting the queried data to points awarded
    - using a given voting method
    - in a given range of years
    - in a given set of contest stages
  - optionally specifying the ranking direction
  - optionally specifying the page index and page size
- **So that** I can represent the page
  - in a table
  - or a chart
  - or some other illustration.

## Voter Points In Range Rankings

### P14: Get Voter Points In Range Rankings

**User Story**

- **As** a Euro-Fan
- **I want to** retrieve a page of rankings
  - for each voting country overall
  - using the POINTS IN RANGE metric: the relative frequency of the points awards it gave to a specified competing country having a value in a specified range
  - optionally restricting the queried data to points awarded
    - using a given voting method
    - in a given range of years
    - in a given set of contest stages
  - optionally specifying the ranking direction
  - optionally specifying the page index and page size
- **So that** I can represent the page
  - in a table
  - or a chart
  - or some other illustration.

## Voter Points Share Rankings

### P15: Get Voter Points Share Rankings

**User Story**

- **As** a Euro-Fan
- **I want to** retrieve a page of rankings
  - for each voting country overall
  - using the POINTS SHARE metric: the sum total points it gave to a specified competing country as a fraction of the available points
  - optionally restricting the queried data to points awarded
    - using a given voting method
    - in a given range of years
    - in a given set of contest stages
  - optionally specifying the ranking direction
  - optionally specifying the page index and page size
- **So that** I can represent the page
  - in a table
  - or a chart
  - or some other illustration.

## Voter Points Similarity Rankings

### P16: Get Voter Points Similarity Rankings

**User Story**

- **As** a Euro-Fan
- **I want to** retrieve a page of rankings
  - for each voting country overall
  - using the POINTS SIMILARITY metric: the cosine similarity of all the televote points and jury points it gave to a specified competing country, using each competitor in each broadcast as a vector dimension
  - optionally restricting the queried data to points awarded
    - in a given range of years
    - in a given set of contest stages
  - optionally specifying the ranking direction
  - optionally specifying the page index and page size
- **So that** I can represent the page
  - in a table
  - or a chart
  - or some other illustration.
