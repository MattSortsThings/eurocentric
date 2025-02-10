# Public API requirements

This document outlines the requirements for version 1.0 of *Eurocentric*'s *Public API*.

Refer to the [project summary](project_summary.md) dev note for an overview of the *Public API* and the Euro-Fan user role.

- [Public API requirements](#public-api-requirements)
  - [P1: Broadcasts](#p1-broadcasts)
    - [P101: Get Broadcasts](#p101-get-broadcasts)
  - [P2: Competitor Rankings](#p2-competitor-rankings)
    - [P201: Rank Competitors By Points Average](#p201-rank-competitors-by-points-average)
    - [P202: Rank Competitors By Points In Range](#p202-rank-competitors-by-points-in-range)
    - [P203: Rank Competitors By Points Share](#p203-rank-competitors-by-points-share)
    - [P204: Rank Competitors By Points Similarity](#p204-rank-competitors-by-points-similarity)
  - [P3: Contests](#p3-contests)
    - [P301: Get Contests](#p301-get-contests)
  - [P4: Countries](#p4-countries)
    - [P401: Get Countries](#p401-get-countries)
  - [P5: Participant Rankings](#p5-participant-rankings)
    - [P501: Rank Participants By Points Average](#p501-rank-participants-by-points-average)
    - [P502: Rank Participants By Points In Range](#p502-rank-participants-by-points-in-range)
    - [P503: Rank Participants By Points Share](#p503-rank-participants-by-points-share)
    - [P504: Rank Participants By Points Similarity](#p504-rank-participants-by-points-similarity)
  - [P6: Scoreboards](#p6-scoreboards)
    - [P601: Get Scoreboard](#p601-get-scoreboard)
  - [P7: Voter Rankings](#p7-voter-rankings)
    - [P701: Rank Voters By Points Average](#p701-rank-voters-by-points-average)
    - [P702: Rank Voters By Points In Range](#p702-rank-voters-by-points-in-range)
    - [P703: Rank Voters By Points Share](#p703-rank-voters-by-points-share)
    - [P704: Rank Voters By Points Similarity](#p704-rank-voters-by-points-similarity)

## P1: Broadcasts

### P101: Get Broadcasts

```
As a Euro-Fan,

I want to request a list of all the queryable broadcasts,
  ordered by transmission date,

So that I can plan my queries.
```

## P2: Competitor Rankings

### P201: Rank Competitors By Points Average

```
As a Euro-Fan,

I want to rank every competitor by its POINTS AVERAGE,
  that is,
    the average value of all the points awards it received,
  and retrieve a page of rankings,

so that I can represent the rankings
  in a table,
  or a chart,
  or some other illustrative purpose.
```

```
I want to be able to restrict the queried data to points awarded
  in a specific year, and/or
  in a specific contest stage, and/or
  using a specific method.
```

```
I want to be able to control the ordering and pagination of the retrieved rankings.
```

### P202: Rank Competitors By Points In Range

```
As a Euro-Fan,

I want to rank every competitor by its POINTS IN RANGE,
  that is,
    the relative frequency of all the points awards it received,
    having a value within a specified range,
  and retrieve a page of rankings,

so that I can represent the rankings
  in a table,
  or a chart,
  or some other illustrative purpose.
```

```
I want to be able to restrict the queried data to points awarded
  in a specific year, and/or
  in a specific contest stage, and/or
  using a specific method.
```

```
I want to be able to control the ordering and pagination of the retrieved rankings.
```

### P203: Rank Competitors By Points Share

```
As a Euro-Fan,

I want to rank every competitor by its POINTS SHARE,
  that is,
    the sum total points it received,
    as a fraction of the maximum possible points,
  and retrieve a page of rankings,

so that I can represent the rankings
  in a table,
  or a chart,
  or some other illustrative purpose.
```

```
I want to be able to restrict the queried data to points awarded
  in a specific year, and/or
  in a specific contest stage, and/or
  using a specific method.
```

```
I want to be able to control the ordering and pagination of the retrieved rankings.
```

### P204: Rank Competitors By Points Similarity

```
As a Euro-Fan,

I want to rank every competitor by its POINTS SIMILARITY,
  that is,
    the cosine similarity of all the televote points awards it received,
    compared with all the jury points awards it received,
    using each voting country as a vector dimension,
  and retrieve a page of rankings,

so that I can represent the rankings
  in a table,
  or a chart,
  or some other illustrative purpose.
```

```
I want to be able to restrict the queried data to points awarded
  in a specific year, and/or
  in a specific contest stage.
```

```
I want to be able to control the ordering and pagination of the retrieved rankings.
```

## P3: Contests

### P301: Get Contests

```
As a Euro-Fan,

I want to request a list of all the queryable contests,
  ordered by contest year,

So that I can plan my queries.
```

## P4: Countries

### P401: Get Countries

```
As a Euro-Fan,

I want to request a list of all the queryable countries,
  ordered by country code,

So that I can plan my queries.
```

## P5: Participant Rankings

### P501: Rank Participants By Points Average

```
As a Euro-Fan,

I want to rank every participating country by its POINTS AVERAGE,
  that is,
    the average value of all the points awards it received,
    across all the broadcasts in which it competed,
  and retrieve a page of rankings,

so that I can represent the rankings
  in a table,
  or a chart,
  or some other illustrative purpose.
```

```
I want to be able to restrict the queried data to points awarded
  in a specific range of years, and/or
  in Grand Finals only, and/or
  using a specific method, and/or
  by a specific voting country.
```

```
I want to be able to control the ordering and pagination of the retrieved rankings.
```

### P502: Rank Participants By Points In Range

```
As a Euro-Fan,

I want to rank every participating country by its POINTS IN RANGE,
  that is,
    the relative frequency of all the points awards it received,
    across all the broadcasts in which it competed,
    having a value within a specified range,
  and retrieve a page of rankings,

so that I can represent the rankings
  in a table,
  or a chart,
  or some other illustrative purpose.
```

```
I want to be able to restrict the queried data to points awarded
  in a specific range of years, and/or
  in Grand Finals only, and/or
  using a specific method, and/or
  by a specific voting country.
```

```
I want to be able to control the ordering and pagination of the retrieved rankings.
```

### P503: Rank Participants By Points Share

```
As a Euro-Fan,

I want to rank every participating country by its POINTS SHARE,
  that is,
    the sum total points it received,
    across all the broadcasts in which it competed,
    as a fraction of the maximum possible points,
  and retrieve a page of rankings,

so that I can represent the rankings
  in a table,
  or a chart,
  or some other illustrative purpose.
```

```
I want to be able to restrict the queried data to points awarded
  in a specific range of years, and/or
  in Grand Finals only, and/or
  using a specific method, and/or
  by a specific voting country.
```

```
I want to be able to control the ordering and pagination of the retrieved rankings.
```

### P504: Rank Participants By Points Similarity

```
As a Euro-Fan,

I want to rank every participating country by its POINTS SIMILARITY,
  that is,
    the cosine similarity of all the televote points awards it received,
    compared with all the jury points awards it received,
    across all the braodcasts in whcih it competed
    using each voting country in each broadcast as a vector dimension,
  and retrieve a page of rankings,

so that I can represent the rankings
  in a table,
  or a chart,
  or some other illustrative purpose.
```

```
I want to be able to restrict the queried data to points awarded
  in a specific range of years, and/or
  in Grand Finals only, and/or
  by a specific voting country.
```

```
I want to be able to control the ordering and pagination of the retrieved rankings.
```

## P6: Scoreboards

### P601: Get Scoreboard

```
As a Euro-Fan,

I want to retrieve the results scoreboard,
  for a specified contest year,
  and a specified contest stage,
  including the voting countries,

so that I can get an overview of the data I can query.
```

## P7: Voter Rankings

### P701: Rank Voters By Points Average

```
As a Euro-Fan,

I want to rank every voting country by its POINTS AVERAGE,
  that is,
    the average value of all the points awards it gave,
    to a specified competing country,
    across all the broadcasts in which it voted,
    and the specified country competed,
  and retrieve a page of rankings,

so that I can represent the rankings
  in a table,
  or a chart,
  or some other illustrative purpose.
```

```
I want to be able to restrict the queried data to points awarded
  in a specific range of years, and/or
  in Grand Finals only, and/or
  using a specific method.
```

```
I want to be able to control the ordering and pagination of the retrieved rankings.
```

### P702: Rank Voters By Points In Range

```
As a Euro-Fan,

I want to rank every voting country by its POINTS IN RANGE,
  that is,
    the relative frequency of all the points awards it gave,
    to a specified competing country,
    across all the broadcasts in which it voted,
    having a value within a specified range,
    and the specified country competed,
  and retrieve a page of rankings,

so that I can represent the rankings
  in a table,
  or a chart,
  or some other illustrative purpose.
```

```
I want to be able to restrict the queried data to points awarded
  in a specific range of years, and/or
  in Grand Finals only, and/or
  using a specific method.
```

```
I want to be able to control the ordering and pagination of the retrieved rankings.
```

### P703: Rank Voters By Points Share

```
As a Euro-Fan,

I want to rank every voting country by its POINTS SHARE,
  that is,
    the sum total points it gave,
    to a specifed competing country
    across all the broadcasts in which it voted,
    and the specified country competed,
    as a fraction of the maximum possible points,
  and retrieve a page of rankings,

so that I can represent the rankings
  in a table,
  or a chart,
  or some other illustrative purpose.
```

```
I want to be able to restrict the queried data to points awarded
  in a specific range of years, and/or
  in Grand Finals only, and/or
  using a specific method.
```

```
I want to be able to control the ordering and pagination of the retrieved rankings.
```

### P704: Rank Voters By Points Similarity

```
As a Euro-Fan,

I want to rank every voting country by its POINTS SIMILARITY,
  that is,
    the cosine similarity of all the televote points awards it gave,
    to a specified competing country,
    compared with all the jury points awards it gave,
    to the specified country,
    across all the braodcasts in which it voted,
    and the specified country competed,
    using each voting country in each broadcast as a vector dimension,
  and retrieve a page of rankings,

so that I can represent the rankings
  in a table,
  or a chart,
  or some other illustrative purpose.
```

```
I want to be able to restrict the queried data to points awarded
  in a specific range of years, and/or
  in Grand Finals only.
```

```
I want to be able to control the ordering and pagination of the retrieved rankings.
```
