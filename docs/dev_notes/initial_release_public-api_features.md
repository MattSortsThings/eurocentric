# Initial release: *public-api* features

This document contains the feature specification for the initial release of *Eurocentric*'s *Public API*.

- [Initial release: *public-api* features](#initial-release-public-api-features)
  - [Competitor Rankings](#competitor-rankings)
    - [P01: Get Points Average Competitor Rankings](#p01-get-points-average-competitor-rankings)
    - [P02: Get Points In Range Competitor Rankings](#p02-get-points-in-range-competitor-rankings)
    - [P03: Get Points Share Competitor Rankings](#p03-get-points-share-competitor-rankings)
    - [P04: Get Points Similarity Competitor Rankings](#p04-get-points-similarity-competitor-rankings)
  - [Data Integrity](#data-integrity)
    - [P05: Handle Contest Completed](#p05-handle-contest-completed)
    - [P06: Handle Contest No Longer Completed](#p06-handle-contest-no-longer-completed)
  - [Individual Rankings](#individual-rankings)
    - [P07: Get Points Average Individual Rankings](#p07-get-points-average-individual-rankings)
    - [P08: Get Points In Range Individual Rankings](#p08-get-points-in-range-individual-rankings)
    - [P09: Get Points Share Individual Rankings](#p09-get-points-share-individual-rankings)
    - [P10: Get Points Similarity Individual Rankings](#p10-get-points-similarity-individual-rankings)
  - [Queryable Broadcasts](#queryable-broadcasts)
    - [P11: Get Queryable Broadcasts](#p11-get-queryable-broadcasts)
  - [Queryable Contests](#queryable-contests)
    - [P12: Get Queryable Contests](#p12-get-queryable-contests)
  - [Queryable Countries](#queryable-countries)
    - [P13: Get Queryable Countries](#p13-get-queryable-countries)
  - [Scoreboards](#scoreboards)
    - [P14: Get First Semi-Final Scoreboard](#p14-get-first-semi-final-scoreboard)
    - [P15: Get Grand Final Scoreboard](#p15-get-grand-final-scoreboard)
    - [P16: Get Second Semi-Final Scoreboard](#p16-get-second-semi-final-scoreboard)
  - [Segment Rankings](#segment-rankings)
    - [P17: Get Points Average Segment Rankings](#p17-get-points-average-segment-rankings)
    - [P18: Get Points In Range Segment Rankings](#p18-get-points-in-range-segment-rankings)
    - [P19: Get Points Share Segment Rankings](#p19-get-points-share-segment-rankings)
    - [P20: Get Points Similarity Segment Rankings](#p20-get-points-similarity-segment-rankings)
  - [Voter Rankings](#voter-rankings)
    - [P21: Get Points Average Voter Rankings](#p21-get-points-average-voter-rankings)
    - [P22: Get Points In Range Voter Rankings](#p22-get-points-in-range-voter-rankings)
    - [P23: Get Points Share Voter Rankings](#p23-get-points-share-voter-rankings)
    - [P24: Get Points Similarity Voter Rankings](#p24-get-points-similarity-voter-rankings)

## Competitor Rankings

### P01: Get Points Average Competitor Rankings

**User Story**

- **As** a Euro-Fan
- **I want to** rank each competing country
  - using the POINTS AVERAGE metric:
    - the average points award value received
  - optionally restricting the queried data to
    - a single voting country
    - and/or a minimum year
    - and/or a maximum year
    - and/or a given set of contest stages (All/SemiFinals/FirstSemiFinal/SecondSemiFinal/GrandFinal)
    - and/or a given voting method (Any/Televote/Jury)
  - optionally specifying
    - ranking direction (HighLow/LowHigh)
  - optionally specifying
    - the pagination page index
    - and/or the pagination page count
- **So that** I can represent the page of rankings
  - in a table
  - or a chart
  - or some other illustration.

**Endpoint**

```
GET /public/api/v1.0/competitor-rankings/points-average
```

### P02: Get Points In Range Competitor Rankings

**User Story**

- **As** a Euro-Fan
- **I want to** rank each competing country
  - using the POINTS IN RANGE metric:
    - the fraction of the points awards received
      - having a value in a certain range
  - specifying
    - a minimum value
    - and a maximum value
  - optionally restricting the queried data to
    - a single voting country
    - and/or a minimum year
    - and/or a maximum year
    - and/or a given set of contest stages (All/SemiFinals/FirstSemiFinal/SecondSemiFinal/GrandFinal)
    - and/or a given voting method (Any/Televote/Jury)
  - optionally specifying
    - ranking direction (HighLow/LowHigh)
  - optionally specifying
    - the pagination page index
    - and/or the pagination page count
- **So that** I can represent the page of rankings
  - in a table
  - or a chart
  - or some other illustration.

**Endpoint**

```
GET /public/api/v1.0/competitor-rankings/points-in-range
```

### P03: Get Points Share Competitor Rankings

**User Story**

- **As** a Euro-Fan
- **I want to** rank each competing country
  - using the POINTS SHARE metric:
    - the sum total points received
    - as a share of the available points
  - optionally restricting the queried data to
    - a single voting country
    - and/or a minimum year
    - and/or a maximum year
    - and/or a given set of contest stages (All/SemiFinals/FirstSemiFinal/SecondSemiFinal/GrandFinal)
    - and/or a given voting method (Any/Televote/Jury)
  - optionally specifying
    - ranking direction (HighLow/LowHigh)
  - optionally specifying
    - the pagination page index
    - and/or the pagination page count
- **So that** I can represent the page of rankings
  - in a table
  - or a chart
  - or some other illustration.

**Endpoint**

```
GET /public/api/v1.0/competitor-rankings/points-share
```

### P04: Get Points Similarity Competitor Rankings

**User Story**

- **As** a Euro-Fan
- **I want to** rank each competing country
  - using the POINTS SIMILARITY metric:
    - the cosine similarity of the televote and jury points received
    - comparing by (broadcast, voter)
  - optionally restricting the queried data to
    - a single voting country
    - and/or a minimum year
    - and/or a maximum year
    - and/or a given set of contest stages (All/SemiFinals/FirstSemiFinal/SecondSemiFinal/GrandFinal)
  - optionally specifying
    - ranking direction (HighLow/LowHigh)
  - optionally specifying
    - the pagination page index
    - and/or the pagination page count
- **So that** I can represent the page of rankings
  - in a table
  - or a chart
  - or some other illustration.

**Endpoint**

```
GET /public/api/v1.0/competitor-rankings/points-similarity
```

## Data Integrity

### P05: Handle Contest Completed

**User Story**

- **As** the Admin
- **I want** all the data for a contest that becomes completed
  - to be added to the queryable data
- **So that** the queryable data always consists of all completed contests in the system and their associated countries and broadcasts.

### P06: Handle Contest No Longer Completed

**User Story**

- **As** the Admin
- **I want** all the data for a completed contest that becomes not completed
  - to be removed from the queryable data
- **So that** the queryable data always consists of all completed contests in the system and their associated countries and broadcasts.

## Individual Rankings

### P07: Get Points Average Individual Rankings

**User Story**

- **As** a Euro-Fan
- **I want to** rank each individual broadcast competitor
  - using the POINTS AVERAGE metric:
    - the average points award value received
  - optionally restricting the queried data to
    - a minimum year
    - and/or a maximum year
    - and/or a given set of contest stages (All/SemiFinals/FirstSemiFinal/SecondSemiFinal/GrandFinal)
    - and/or a given voting method (Any/Televote/Jury)
  - optionally specifying
    - ranking direction (HighLow/LowHigh)
  - optionally specifying
    - the pagination page index
    - and/or the pagination page count
- **So that** I can represent the page of rankings
  - in a table
  - or a chart
  - or some other illustration.

**Endpoint**

```
GET /public/api/v1.0/individual-rankings/points-average
```

### P08: Get Points In Range Individual Rankings

**User Story**

- **As** a Euro-Fan
- **I want to** rank each individual broadcast competitor
  - using the POINTS IN RANGE metric:
    - the fraction of the points awards received
      - having a value in a certain range
  - specifying
    - a minimum value
    - and a maximum value
  - optionally restricting the queried data to
    - a minimum year
    - and/or a maximum year
    - and/or a given set of contest stages (All/SemiFinals/FirstSemiFinal/SecondSemiFinal/GrandFinal)
    - and/or a given voting method (Any/Televote/Jury)
  - optionally specifying
    - ranking direction (HighLow/LowHigh)
  - optionally specifying
    - the pagination page index
    - and/or the pagination page count
- **So that** I can represent the page of rankings
  - in a table
  - or a chart
  - or some other illustration.

**Endpoint**

```
GET /public/api/v1.0/individual-rankings/points-in-range
```

### P09: Get Points Share Individual Rankings

**User Story**

- **As** a Euro-Fan
- **I want to** rank each individual broadcast competitor
  - using the POINTS SHARE metric:
    - the sum total points received
    - as a share of the available points
  - optionally restricting the queried data to
    - a minimum year
    - and/or a maximum year
    - and/or a given set of contest stages (All/SemiFinals/FirstSemiFinal/SecondSemiFinal/GrandFinal)
    - and/or a given voting method (Any/Televote/Jury)
  - optionally specifying
    - ranking direction (HighLow/LowHigh)
  - optionally specifying
    - the pagination page index
    - and/or the pagination page count
- **So that** I can represent the page of rankings
  - in a table
  - or a chart
  - or some other illustration.

**Endpoint**

```
GET /public/api/v1.0/individual-rankings/points-share
```

### P10: Get Points Similarity Individual Rankings

**User Story**

- **As** a Euro-Fan
- **I want to** rank each individual broadcast competitor
  - using the POINTS SIMILARITY metric:
    - the cosine similarity of the televote and jury points received
    - comparing by voter
  - optionally restricting the queried data to
    - a minimum year
    - and/or a maximum year
    - and/or a given set of contest stages (All/SemiFinals/FirstSemiFinal/SecondSemiFinal/GrandFinal)
  - optionally specifying
    - ranking direction (HighLow/LowHigh)
  - optionally specifying
    - the pagination page index
    - and/or the pagination page count
- **So that** I can represent the page of rankings
  - in a table
  - or a chart
  - or some other illustration.

**Endpoint**

```
GET /public/api/v1.0/individual-rankings/points-similarity
```

## Queryable Broadcasts

### P11: Get Queryable Broadcasts

**User Story**

- **As** a Euro-Fan
- **I want to** get a list of all the queryable broadcasts
  - ordered by contest year then by contest stage
- **So that** I can plan my queries.

**Endpoint**

```
GET /public/api/v1.0/queryable-broadcasts
```

## Queryable Contests

### P12: Get Queryable Contests

**User Story**

- **As** a Euro-Fan
- **I want to** get a list of all the queryable contests
  - ordered by contest year
- **So that** I can plan my queries.

**Endpoint**

```
GET /public/api/v1.0/queryable-contests
```

## Queryable Countries

### P13: Get Queryable Countries

**User Story**

- **As** a Euro-Fan
- **I want to** get a list of all the queryable countries
  - ordered by country code
- **So that** I can plan my queries.

**Endpoint**

```
GET /public/api/v1.0/queryable-countries
```

## Scoreboards

### P14: Get First Semi-Final Scoreboard

**User Story**

- **As** a Euro-Fan
- **I want to** get the scoreboard for a First Semi-Final broadcast
  - specifying
    - the contest year
- **So that** I can plan my queries.

**Endpoint**

```
GET /public/api/1.0/scoreboards/{contestYear}/first-semi-final
```

### P15: Get Grand Final Scoreboard

**User Story**

- **As** a Euro-Fan
- **I want to** get the scoreboard for a Grand Final broadcast
  - specifying
    - the contest year
- **So that** I can plan my queries.

**Endpoint**

```
GET /public/api/1.0/scoreboards/{contestYear}/grand-final
```

### P16: Get Second Semi-Final Scoreboard

**User Story**

- **As** a Euro-Fan
- **I want to** get the scoreboard for a Second Semi-Final broadcast
  - specifying
    - the contest year
- **So that** I can plan my queries.

**Endpoint**

```
GET /public/api/1.0/scoreboards/{contestYear}/second-semi-final
```

## Segment Rankings

### P17: Get Points Average Segment Rankings

**User Story**

- **As** a Euro-Fan
- **I want to** rank each running order segment
  - using the POINTS AVERAGE metric:
    - the average points award value received
  - specifying
    - the number of segments (between 2 and 10)
  - optionally restricting the queried data to
    - a single voting country
    - and/or a minimum year
    - and/or a maximum year
    - and/or a given set of contest stages (All/SemiFinals/FirstSemiFinal/SecondSemiFinal/GrandFinal)
    - and/or a given voting method (Any/Televote/Jury)
  - optionally specifying
    - ranking direction (HighLow/LowHigh)
- **So that** I can represent the rankings
  - in a table
  - or a chart
  - or some other illustration.

**Endpoint**

```
GET /public/api/v1.0/segment-rankings/points-average
```

### P18: Get Points In Range Segment Rankings

**User Story**

- **As** a Euro-Fan
- **I want to** rank each running order segment
  - using the POINTS IN RANGE metric:
    - the fraction of the points awards received
      - having a value in a certain range
  - specifying
    - the number of segments (between 2 and 10)
    - and a minimum value
    - and a maximum value
  - optionally restricting the queried data to
    - a single voting country
    - and/or a minimum year
    - and/or a maximum year
    - and/or a given set of contest stages (All/SemiFinals/FirstSemiFinal/SecondSemiFinal/GrandFinal)
    - and/or a given voting method (Any/Televote/Jury)
  - optionally specifying
    - ranking direction (HighLow/LowHigh)
- **So that** I can represent the rankings
  - in a table
  - or a chart
  - or some other illustration.

**Endpoint**

```
GET /public/api/v1.0/segment-rankings/points-in-range
```

### P19: Get Points Share Segment Rankings

**User Story**

- **As** a Euro-Fan
- **I want to** rank each running order segment
  - using the POINTS SHARE metric:
    - the sum total points received
    - as a share of the available points
  - specifying
    - the number of segments (between 2 and 10)
  - optionally restricting the queried data to
    - a single voting country
    - and/or a minimum year
    - and/or a maximum year
    - and/or a given set of contest stages (All/SemiFinals/FirstSemiFinal/SecondSemiFinal/GrandFinal)
    - and/or a given voting method (Any/Televote/Jury)
  - optionally specifying
    - ranking direction (HighLow/LowHigh)
- **So that** I can represent the rankings
  - in a table
  - or a chart
  - or some other illustration.

**Endpoint**

```
GET /public/api/v1.0/segment-rankings/points-share
```

### P20: Get Points Similarity Segment Rankings

**User Story**

- **As** a Euro-Fan
- **I want to** rank each running order segment
  - using the POINTS SIMILARITY metric:
    - the cosine similarity of the televote and jury points received
    - comparing by (broadcast, competitor, voter)
  - specifying
    - the number of segments (between 2 and 10)
  - optionally restricting the queried data to
    - a single voting country
    - and/or a minimum year
    - and/or a maximum year
    - and/or a given set of contest stages (All/SemiFinals/FirstSemiFinal/SecondSemiFinal/GrandFinal)
  - optionally specifying
    - ranking direction (HighLow/LowHigh)
- **So that** I can represent the rankings
  - in a table
  - or a chart
  - or some other illustration.

**Endpoint**

```
GET /public/api/v1.0/segment-rankings/points-similarity
```

## Voter Rankings

### P21: Get Points Average Voter Rankings

**User Story**

- **As** a Euro-Fan
- **I want to** rank each voting country
  - using the POINTS AVERAGE metric:
    - the average points award value given to a certain country
  - specifying
    - the competing country
  - optionally restricting the queried data to
    - a minimum year
    - and/or a maximum year
    - and/or a given set of contest stages (All/SemiFinals/FirstSemiFinal/SecondSemiFinal/GrandFinal)
    - and/or a given voting method (Any/Televote/Jury)
  - optionally specifying
    - ranking direction (HighLow/LowHigh)
  - optionally specifying
    - the pagination page index
    - and/or the pagination page count
- **So that** I can represent the page of rankings
  - in a table
  - or a chart
  - or some other illustration.

**Endpoint**

```
GET /public/api/v1.0/voter-rankings/points-average
```

### P22: Get Points In Range Voter Rankings

**User Story**

- **As** a Euro-Fan
- **I want to** rank each voting country
  - using the POINTS IN RANGE metric:
    - the fraction of the points awards given to a certain country
      - having a value in a certain range
  - specifying
    - the competing country
    - and a minimum value
    - and a maximum value
  - optionally restricting the queried data to
    - a minimum year
    - and/or a maximum year
    - and/or a given set of contest stages (All/SemiFinals/FirstSemiFinal/SecondSemiFinal/GrandFinal)
    - and/or a given voting method (Any/Televote/Jury)
  - optionally specifying
    - ranking direction (HighLow/LowHigh)
  - optionally specifying
    - the pagination page index
    - and/or the pagination page count
- **So that** I can represent the page of rankings
  - in a table
  - or a chart
  - or some other illustration.

**Endpoint**

```
GET /public/api/v1.0/voter-rankings/points-in-range
```

### P23: Get Points Share Voter Rankings

**User Story**

- **As** a Euro-Fan
- **I want to** rank each voting country
  - using the POINTS SHARE metric:
    - the sum total points given to a certain country
    - as a share of the available points
  - specifying
    - the competing country
  - optionally restricting the queried data to
    - a minimum year
    - and/or a maximum year
    - and/or a given set of contest stages (All/SemiFinals/FirstSemiFinal/SecondSemiFinal/GrandFinal)
    - and/or a given voting method (Any/Televote/Jury)
  - optionally specifying
    - ranking direction (HighLow/LowHigh)
  - optionally specifying
    - the pagination page index
    - and/or the pagination page count
- **So that** I can represent the page of rankings
  - in a table
  - or a chart
  - or some other illustration.

**Endpoint**

```
GET /public/api/v1.0/voter-rankings/points-share
```

### P24: Get Points Similarity Voter Rankings

**User Story**

- **As** a Euro-Fan
- **I want to** rank each voting country
  - using the POINTS SIMILARITY metric:
    - the cosine similarity of the televote and jury points given
    - comparing by (broadcast, competitor)
  - optionally restricting the queried data to
    - a single competing country
    - and/or a minimum year
    - and/or a maximum year
    - and/or a given set of contest stages (All/SemiFinals/FirstSemiFinal/SecondSemiFinal/GrandFinal)
  - optionally specifying
    - ranking direction (HighLow/LowHigh)
  - optionally specifying
    - the pagination page index
    - and/or the pagination page count
- **So that** I can represent the page of rankings
  - in a table
  - or a chart
  - or some other illustration.

**Endpoint**

```
GET /public/api/v1.0/voter-rankings/points-similarity
```
