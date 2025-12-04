# 04: Features: *public-api* scope

This document is part of the [launch specification](../README.md#launch-specification).

- [04: Features: *public-api* scope](#04-features-public-api-scope)
  - [Competitor Rankings](#competitor-rankings)
    - [pa01: Get Competitor Points Average Rankings](#pa01-get-competitor-points-average-rankings)
    - [pa02: Get Competitor Points In Range Rankings](#pa02-get-competitor-points-in-range-rankings)
    - [pa03: Get Competitor Points Share Rankings](#pa03-get-competitor-points-share-rankings)
    - [pa04: Get Competitor Points Similarity Rankings](#pa04-get-competitor-points-similarity-rankings)
  - [Country Rankings](#country-rankings)
    - [pa05: Get Country Given Points Average Rankings](#pa05-get-country-given-points-average-rankings)
    - [pa06: Get Country Given Points In Range Rankings](#pa06-get-country-given-points-in-range-rankings)
    - [pa07: Get Country Given Points Share Rankings](#pa07-get-country-given-points-share-rankings)
    - [pa08: Get Country Given Points Similarity Rankings](#pa08-get-country-given-points-similarity-rankings)
    - [pa09: Get Country Received Points Average Rankings](#pa09-get-country-received-points-average-rankings)
    - [pa10: Get Country Received Points In Range Rankings](#pa10-get-country-received-points-in-range-rankings)
    - [pa11: Get Country Received Points Share Rankings](#pa11-get-country-received-points-share-rankings)
    - [pa12: Get Country Received Points Similarity Rankings](#pa12-get-country-received-points-similarity-rankings)
  - [Points Awards](#points-awards)
    - [pa13: Get Points Awards](#pa13-get-points-awards)
  - [Queryable Broadcasts](#queryable-broadcasts)
    - [pa14: Get Queryable Broadcasts](#pa14-get-queryable-broadcasts)
  - [Queryable Contests](#queryable-contests)
    - [pa15: Get Queryable Contests](#pa15-get-queryable-contests)
  - [Queryable Countries](#queryable-countries)
    - [pa16: Get Queryable Countries](#pa16-get-queryable-countries)
  - [Scoreboard Rows](#scoreboard-rows)
    - [pa17: Get Scoreboard Rows](#pa17-get-scoreboard-rows)
  - [Segment Rankings](#segment-rankings)
    - [pa18: Get Segment Points Average Rankings](#pa18-get-segment-points-average-rankings)
    - [pa19: Get Segment Points In Range Rankings](#pa19-get-segment-points-in-range-rankings)
    - [pa20: Get Segment Points Share Rankings](#pa20-get-segment-points-share-rankings)
    - [pa21: Get Segment Points Similarity Rankings](#pa21-get-segment-points-similarity-rankings)

## Competitor Rankings

### pa01: Get Competitor Points Average Rankings

**Endpoint:**

```http request
GET /public/api/v1.0/rankings/competitors/points-average
```

**User story:**

- As a EuroFan
- I want to rank competitors by their *points average*
  - which is a float in the range \[0.0, 12.0\]
    - calculated as the average value of all the points awards the competitor received in their broadcast
  - optionally filtering the queried voting data
    - by minimum contest year (int, default value = 2016)
    - by maximum contest year (int, default value = current year)
    - by contest stages (enum, values = \{'All', 'GrandFinal', 'FirstSemiFinal', 'SecondSemiFinal', 'BothSemiFinals'\}, default value = 'All')
    - by voting methods (enum, values = \{'Any', 'Televote', 'Jury'\}, default value = 'Any')
  - optionally overriding
    - the rank order (enum, values = \{'HighestToLowest', 'LowestToHighest'\}, default value = 'HighestToLowest')
    - the pagination page size (int, default value = 10)
    - the pagination page index (int, default value = 0)
  - receiving
    - a page of rankings
      - ordered by rank
    - a metadata object comprising
      - all the query parameters
      - the total rankings
      - the total pages
- so that I can observe trends and highlights from the rankings
  - and I can import the rankings into a spreadsheet or database table
  - and I can represent the rankings in a table or chart.

### pa02: Get Competitor Points In Range Rankings

**Endpoint:**

```http request
GET /public/api/v1.0/rankings/competitors/points-in-range
```

**User story:**

- As a EuroFan
- I want to rank competitors by their *points in range*
  - which is a float in the range \[0.0, 1.0\]
    - calculated as the frequency of points awards the competitor received in their broadcast having a value within a specified range, relative to the number of points awards received
  - providing
    - the minimum points value (int)
    - the maximum points value (int)
  - optionally filtering the queried voting data
    - by minimum contest year (int, default value = 2016)
    - by maximum contest year (int, default value = current year)
    - by contest stages (enum, values = \{'All', 'GrandFinal', 'FirstSemiFinal', 'SecondSemiFinal', 'BothSemiFinals'\}, default value = 'All')
    - by voting methods (enum, values = \{'Any', 'Televote', 'Jury'\}, default value = 'Any')
  - optionally overriding
    - the rank order (enum, values = \{'HighestToLowest', 'LowestToHighest'\}, default value = 'HighestToLowest')
    - the pagination page size (int, default value = 10)
    - the pagination page index (int, default value = 0)
  - receiving
    - a page of rankings
      - ordered by rank
    - a metadata object comprising
      - all the query parameters
      - the total rankings
      - the total pages
- so that I can observe trends and highlights from the rankings
  - and I can import the rankings into a spreadsheet or database table
  - and I can represent the rankings in a table or chart.

### pa03: Get Competitor Points Share Rankings

**Endpoint:**

```http request
GET /public/api/v1.0/rankings/competitors/points-share
```

**User story:**

- As a EuroFan
- I want to rank competitors by their *points share*
  - which is a float in the range \[0.0, 1.0\]
    - calculated as the total points the competitor received in their broadcast, as a fraction of the available points
  - optionally filtering the queried voting data
    - by minimum contest year (int, default value = 2016)
    - by maximum contest year (int, default value = current year)
    - by contest stages (enum, values = \{'All', 'GrandFinal', 'FirstSemiFinal', 'SecondSemiFinal', 'BothSemiFinals'\}, default value = 'All')
    - by voting methods (enum, values = \{'Any', 'Televote', 'Jury'\}, default value = 'Any')
  - optionally overriding
    - the rank order (enum, values = \{'HighestToLowest', 'LowestToHighest'\}, default value = 'HighestToLowest')
    - the pagination page size (int, default value = 10)
    - the pagination page index (int, default value = 0)
  - receiving
    - a page of rankings
      - ordered by rank
    - a metadata object comprising
      - all the query parameters
      - the total rankings
      - the total pages
- so that I can observe trends and highlights from the rankings
  - and I can import the rankings into a spreadsheet or database table
  - and I can represent the rankings in a table or chart.

### pa04: Get Competitor Points Similarity Rankings

**Endpoint:**

```http request
GET /public/api/v1.0/rankings/competitors/points-similarity
```

**User story:**

- As a EuroFan
- I want to rank competitors by their *points similarity*
  - which is a float in the range \[0.0, 1.0\]
    - calculated as the cosine similarity between the normalized televote and jury points the competitor received in their broadcast, using each voting country as a vector dimension
  - optionally filtering the queried voting data
    - by minimum contest year (int, default value = 2016)
    - by maximum contest year (int, default value = current year)
    - by contest stages (enum, values = \{'All', 'GrandFinal', 'FirstSemiFinal', 'SecondSemiFinal', 'BothSemiFinals'\}, default value = 'All')
  - optionally overriding
    - the rank order (enum, values = \{'HighestToLowest', 'LowestToHighest'\}, default value = 'HighestToLowest')
    - the pagination page size (int, default value = 10)
    - the pagination page index (int, default value = 0)
  - receiving
    - a page of rankings
      - ordered by rank
    - a metadata object comprising
      - all the query parameters
      - the total rankings
      - the total pages
- so that I can observe trends and highlights from the rankings
  - and I can import the rankings into a spreadsheet or database table
  - and I can represent the rankings in a table or chart.

## Country Rankings

### pa05: Get Country Given Points Average Rankings

**Endpoint:**

```http request
GET /public/api/v1.0/rankings/countries/given-points-average
```

**User story:**

- As a EuroFan
- I want to rank countries by their *given points average*
  - which is a float in the range \[0.0, 12.0\]
    - calculated as the average value of all the points awards the country gave across broadcasts to a specified competing country
  - providing
    - the competing country code (string)
  - optionally filtering the queried voting data
    - by minimum contest year (int, default value = 2016)
    - by maximum contest year (int, default value = current year)
    - by contest stages (enum, values = \{'All', 'GrandFinal', 'FirstSemiFinal', 'SecondSemiFinal', 'BothSemiFinals'\}, default value = 'All')
    - by voting methods (enum, values = \{'Any', 'Televote', 'Jury'\}, default value = 'Any')
  - optionally overriding
    - the rank order (enum, values = \{'HighestToLowest', 'LowestToHighest'\}, default value = 'HighestToLowest')
    - the pagination page size (int, default value = 10)
    - the pagination page index (int, default value = 0)
  - receiving
    - a page of rankings
      - ordered by rank
    - a metadata object comprising
      - all the query parameters
      - the total rankings
      - the total pages
- so that I can observe trends and highlights from the rankings
  - and I can import the rankings into a spreadsheet or database table
  - and I can represent the rankings in a table or chart.

### pa06: Get Country Given Points In Range Rankings

**Endpoint:**

```http request
GET /public/api/v1.0/rankings/countries/given-points-in-range
```

**User story:**

- As a EuroFan
- I want to rank countries by their *given points in range*
  - which is a float in the range \[0.0, 1.0\]
    - calculated as the frequency of points awards the country gave across broadcasts to a specified competing country having a value within a specified range, relative to the number of points awards given
  - providing
    - the competing country code (string)
    - the minimum points value (int)
    - the maximum points value (int)
  - optionally filtering the queried voting data
    - by minimum contest year (int, default value = 2016)
    - by maximum contest year (int, default value = current year)
    - by contest stages (enum, values = \{'All', 'GrandFinal', 'FirstSemiFinal', 'SecondSemiFinal', 'BothSemiFinals'\}, default value = 'All')
    - by voting methods (enum, values = \{'Any', 'Televote', 'Jury'\}, default value = 'Any')
  - optionally overriding
    - the rank order (enum, values = \{'HighestToLowest', 'LowestToHighest'\}, default value = 'HighestToLowest')
    - the pagination page size (int, default value = 10)
    - the pagination page index (int, default value = 0)
  - receiving
    - a page of rankings
      - ordered by rank
    - a metadata object comprising
      - all the query parameters
      - the total rankings
      - the total pages
- so that I can observe trends and highlights from the rankings
  - and I can import the rankings into a spreadsheet or database table
  - and I can represent the rankings in a table or chart.

### pa07: Get Country Given Points Share Rankings

**Endpoint:**

```http request
GET /public/api/v1.0/rankings/countries/given-points-share
```

**User story:**

- As a EuroFan
- I want to rank countries by their *given points share*
  - which is a float in the range \[0.0, 1.0\]
    - calculated as the total points the country gave across broadcasts to a specified competing country, as a fraction of the available points
  - providing
    - the competing country code (string)
  - optionally filtering the queried voting data
    - by minimum contest year (int, default value = 2016)
    - by maximum contest year (int, default value = current year)
    - by contest stages (enum, values = \{'All', 'GrandFinal', 'FirstSemiFinal', 'SecondSemiFinal', 'BothSemiFinals'\}, default value = 'All')
    - by voting methods (enum, values = \{'Any', 'Televote', 'Jury'\}, default value = 'Any')
  - optionally overriding
    - the rank order (enum, values = \{'HighestToLowest', 'LowestToHighest'\}, default value = 'HighestToLowest')
    - the pagination page size (int, default value = 10)
    - the pagination page index (int, default value = 0)
  - receiving
    - a page of rankings
      - ordered by rank
    - a metadata object comprising
      - all the query parameters
      - the total rankings
      - the total pages
- so that I can observe trends and highlights from the rankings
  - and I can import the rankings into a spreadsheet or database table
  - and I can represent the rankings in a table or chart.

### pa08: Get Country Given Points Similarity Rankings

**Endpoint:**

```http request
GET /public/api/v1.0/rankings/countries/given-points-similarity
```

**User story:**

- As a EuroFan
- I want to rank countries by their *given points similarity*
  - which is a float in the range \[0.0, 1.0\]
    - calculated as the cosine similarity between the normalized televote and jury points the country gave across broadcasts, using each competing country in each broadcast as a vector dimension
  - providing
    - the competing country code
  - optionally filtering the queried voting data
    - by minimum contest year (int, default value = 2016)
    - by maximum contest year (int, default value = current year)
    - by contest stages (enum, values = \{'All', 'GrandFinal', 'FirstSemiFinal', 'SecondSemiFinal', 'BothSemiFinals'\}, default value = 'All')
  - optionally overriding
    - the rank order (enum, values = \{'HighestToLowest', 'LowestToHighest'\}, default value = 'HighestToLowest')
    - the pagination page size (int, default value = 10)
    - the pagination page index (int, default value = 0)
  - receiving
    - a page of rankings
      - ordered by rank
    - a metadata object comprising
      - all the query parameters
      - the total rankings
      - the total pages
- so that I can observe trends and highlights from the rankings
  - and I can import the rankings into a spreadsheet or database table
  - and I can represent the rankings in a table or chart.

### pa09: Get Country Received Points Average Rankings

**Endpoint:**

```http request
GET /public/api/v1.0/rankings/countries/received-points-average
```

**User story:**

- As a EuroFan
- I want to rank countries by their *received points average*
  - which is a float in the range \[0.0, 12.0\]
    - calculated as the average value of all the points awards the country received across broadcasts
  - optionally filtering the queried voting data
    - by minimum contest year (int, default value = 2016)
    - by maximum contest year (int, default value = current year)
    - by contest stages (enum, values = \{'All', 'GrandFinal', 'FirstSemiFinal', 'SecondSemiFinal', 'BothSemiFinals'\}, default value = 'All')
    - by voting methods (enum, values = \{'Any', 'Televote', 'Jury'\}, default value = 'Any')
    - by voting country code (string)
  - optionally overriding
    - the rank order (enum, values = \{'HighestToLowest', 'LowestToHighest'\}, default value = 'HighestToLowest')
    - the pagination page size (int, default value = 10)
    - the pagination page index (int, default value = 0)
  - receiving
    - a page of rankings
      - ordered by rank
    - a metadata object comprising
      - all the query parameters
      - the total rankings
      - the total pages
- so that I can observe trends and highlights from the rankings
  - and I can import the rankings into a spreadsheet or database table
  - and I can represent the rankings in a table or chart.

### pa10: Get Country Received Points In Range Rankings

**Endpoint:**

```http request
GET /public/api/v1.0/rankings/countries/received-points-in-range
```

**User story:**

- As a EuroFan
- I want to rank countries by their *received points in range*
  - which is a float in the range \[0.0, 1.0\]
    - calculated as the frequency of points awards the country received across broadcasts having a value within a specified range, relative to the number of points awards received
  - providing
    - the minimum points value (int)
    - the maximum points value (int)
  - optionally filtering the queried voting data
    - by minimum contest year (int, default value = 2016)
    - by maximum contest year (int, default value = current year)
    - by contest stages (enum, values = \{'All', 'GrandFinal', 'FirstSemiFinal', 'SecondSemiFinal', 'BothSemiFinals'\}, default value = 'All')
    - by voting methods (enum, values = \{'Any', 'Televote', 'Jury'\}, default value = 'Any')
    - by voting country code (string)
  - optionally overriding
    - the rank order (enum, values = \{'HighestToLowest', 'LowestToHighest'\}, default value = 'HighestToLowest')
    - the pagination page size (int, default value = 10)
    - the pagination page index (int, default value = 0)
  - receiving
    - a page of rankings
      - ordered by rank
    - a metadata object comprising
      - all the query parameters
      - the total rankings
      - the total pages
- so that I can observe trends and highlights from the rankings
  - and I can import the rankings into a spreadsheet or database table
  - and I can represent the rankings in a table or chart.

### pa11: Get Country Received Points Share Rankings

**Endpoint:**

```http request
GET /public/api/v1.0/rankings/countries/received-points-share
```

**User story:**

- As a EuroFan
- I want to rank countries by their *received points share*
  - which is a float in the range \[0.0, 1.0\]
    - calculated as the total points the country received across broadcasts, as a fraction of the available points
  - optionally filtering the queried voting data
    - by minimum contest year (int, default value = 2016)
    - by maximum contest year (int, default value = current year)
    - by contest stages (enum, values = \{'All', 'GrandFinal', 'FirstSemiFinal', 'SecondSemiFinal', 'BothSemiFinals'\}, default value = 'All')
    - by voting methods (enum, values = \{'Any', 'Televote', 'Jury'\}, default value = 'Any')
    - by voting country code (string)
  - optionally overriding
    - the rank order (enum, values = \{'HighestToLowest', 'LowestToHighest'\}, default value = 'HighestToLowest')
    - the pagination page size (int, default value = 10)
    - the pagination page index (int, default value = 0)
  - receiving
    - a page of rankings
      - ordered by rank
    - a metadata object comprising
      - all the query parameters
      - the total rankings
      - the total pages
- so that I can observe trends and highlights from the rankings
  - and I can import the rankings into a spreadsheet or database table
  - and I can represent the rankings in a table or chart.

### pa12: Get Country Received Points Similarity Rankings

**Endpoint:**

```http request
GET /public/api/v1.0/rankings/countries/received-points-similarity
```

**User story:**

- As a EuroFan
- I want to rank countries by their *received points similarity*
  - which is a float in the range \[0.0, 1.0\]
    - calculated as the cosine similarity between the normalized televote and jury points the country received broadcasts, using each voting country in each broadcast as a vector dimension
  - optionally filtering the queried voting data
    - by minimum contest year (int, default value = 2016)
    - by maximum contest year (int, default value = current year)
    - by contest stages (enum, values = \{'All', 'GrandFinal', 'FirstSemiFinal', 'SecondSemiFinal', 'BothSemiFinals'\}, default value = 'All')
    - by voting country code (string)
  - optionally overriding
    - the rank order (enum, values = \{'HighestToLowest', 'LowestToHighest'\}, default value = 'HighestToLowest')
    - the pagination page size (int, default value = 10)
    - the pagination page index (int, default value = 0)
  - receiving
    - a page of rankings
      - ordered by rank
    - a metadata object comprising
      - all the query parameters
      - the total rankings
      - the total pages
- so that I can observe trends and highlights from the rankings
  - and I can import the rankings into a spreadsheet or database table
  - and I can represent the rankings in a table or chart.

## Points Awards

### pa13: Get Points Awards

**Endpoint:**

```http request
GET /public/api/v1.0/points-awards
```

**User story:**

- As a EuroFan
- I want to retrieve all the points awards received and/or given by a specific country in a specific broadcast
  - providing
    - the contest year (int)
    - the contest stage (enum, values = \{'GrandFinal', 'FirstSemiFinal', 'SecondSemiFinal'\})
    - the country code (string)
  - receiving
    - a list of received televote points awards, ordered by descending points value then by ascending voting country code
    - a list of received jury points awards, ordered by descending points value then by ascending voting country code
    - a list of given televote points awards, ordered by descending points value then by ascending competing country code
    - a list of given jury points awards, ordered by descending points value then by ascending competing country code
  - a metadata object
- so that I can see exactly how the country performed and voted in the broadcast
  - and I can import the points awards into a spreadsheet or database table to run my own analyses.

## Queryable Broadcasts

### pa14: Get Queryable Broadcasts

**Endpoint:**

```http request
GET /public/api/v1.0/queryables/broadcasts
```

**User story:**

- As a EuroFan
- I want to retrieve all the queryable broadcasts
  - receiving
    - a list of queryable broadcasts
      - ordered by broadcast date
- so that I can get an overview of the Public API's queryable voting data.

## Queryable Contests

### pa15: Get Queryable Contests

**Endpoint:**

```http request
GET /public/api/v1.0/queryables/contests
```

**User story:**

- As a EuroFan
- I want to retrieve all the queryable contests
  - receiving
    - a list of queryable contests
      - ordered by contest year
- so that I can get an overview of the Public API's queryable voting data.

## Queryable Countries

### pa16: Get Queryable Countries

**Endpoint:**

```http request
GET /public/api/v1.0/queryables/countries
```

**User story:**

- As a EuroFan
- I want to retrieve all the queryable countries
  - receiving
    - a list of queryable countries
      - ordered by country code
- so that I can get an overview of the Public API's queryable voting data.

## Scoreboard Rows

### pa17: Get Scoreboard Rows

**Endpoint:**

```http request
GET /public/api/v1.0/scoreboard-rows
```

**User story:**

- As a EuroFan
- I want to retrieve all the scoreboard rows from a specific broadcast
  - providing
    - the contest year (int)
    - the contest stage (enum, values = \{'GrandFinal', 'FirstSemiFinal', 'SecondSemiFinal'\})
  - receiving
    - a list of scoreboard rows
      - ordered by running order spot
  - a metadata object
- so that I can see all the results in the broadcast
  - and I can import the scoreboard rows into a spreadsheet or database table to run my own analyses.

## Segment Rankings

### pa18: Get Segment Points Average Rankings

**Endpoint:**

```http request
GET /public/api/v1.0/rankings/segments/points-average
```

**User story:**

- As a EuroFan
- I want to rank broadcast running order segments by their *points average*
  - which is a float in the range \[0.0, 12.0\]
    - calculated as the average value of all the points awards received across broadcasts by competitors performing in the segment
  - optionally filtering the queried voting data
    - by minimum contest year (int, default value = 2016)
    - by maximum contest year (int, default value = current year)
    - by contest stages (enum, values = \{'All', 'GrandFinal', 'FirstSemiFinal', 'SecondSemiFinal', 'BothSemiFinals'\}, default value = 'All')
    - by voting methods (enum, values = \{'Any', 'Televote', 'Jury'\}, default value = 'Any')
    - by voting country code (string)
  - optionally overriding
    - the rank order (enum, values = \{'HighestToLowest', 'LowestToHighest'\}, default value = 'HighestToLowest')
    - the pagination page size (int, default value = 10)
    - the pagination page index (int, default value = 0)
  - receiving
    - a page of rankings
      - ordered by rank
    - a metadata object comprising
      - all the query parameters
      - the total rankings
      - the total pages
- so that I can observe trends and highlights from the rankings
  - and I can import the rankings into a spreadsheet or database table
  - and I can represent the rankings in a table or chart.

### pa19: Get Segment Points In Range Rankings

**Endpoint:**

```http request
GET /public/api/v1.0/rankings/segments/points-in-range
```

**User story:**

- As a EuroFan
- I want to rank broadcast running order segments by their *points in range*
  - which is a float in the range \[0.0, 1.0\]
    - calculated as the frequency of points awards received across broadcasts by competitors performing in the segment having a value within a specified range, relative to the number of points awards received
  - providing
    - the minimum points value (int)
    - the maximum points value (int)
  - optionally filtering the queried voting data
    - by minimum contest year (int, default value = 2016)
    - by maximum contest year (int, default value = current year)
    - by contest stages (enum, values = \{'All', 'GrandFinal', 'FirstSemiFinal', 'SecondSemiFinal', 'BothSemiFinals'\}, default value = 'All')
    - by voting methods (enum, values = \{'Any', 'Televote', 'Jury'\}, default value = 'Any')
    - by voting country code (string)
  - optionally overriding
    - the rank order (enum, values = \{'HighestToLowest', 'LowestToHighest'\}, default value = 'HighestToLowest')
    - the pagination page size (int, default value = 10)
    - the pagination page index (int, default value = 0)
  - receiving
    - a page of rankings
      - ordered by rank
    - a metadata object comprising
      - all the query parameters
      - the total rankings
      - the total pages
- so that I can observe trends and highlights from the rankings
  - and I can import the rankings into a spreadsheet or database table
  - and I can represent the rankings in a table or chart.

### pa20: Get Segment Points Share Rankings

**Endpoint:**

```http request
GET /public/api/v1.0/rankings/segments/points-share
```

**User story:**

- As a EuroFan
- I want to rank broadcast running order segments by their *points share*
  - which is a float in the range \[0.0, 1.0\]
    - calculated as the total points received across broadcasts by competitors performing in the segment, as a fraction of the available points
  - optionally filtering the queried voting data
    - by minimum contest year (int, default value = 2016)
    - by maximum contest year (int, default value = current year)
    - by contest stages (enum, values = \{'All', 'GrandFinal', 'FirstSemiFinal', 'SecondSemiFinal', 'BothSemiFinals'\}, default value = 'All')
    - by voting methods (enum, values = \{'Any', 'Televote', 'Jury'\}, default value = 'Any')
    - by voting country code (string)
  - optionally overriding
    - the rank order (enum, values = \{'HighestToLowest', 'LowestToHighest'\}, default value = 'HighestToLowest')
    - the pagination page size (int, default value = 10)
    - the pagination page index (int, default value = 0)
  - receiving
    - a page of rankings
      - ordered by rank
    - a metadata object comprising
      - all the query parameters
      - the total rankings
      - the total pages
- so that I can observe trends and highlights from the rankings
  - and I can import the rankings into a spreadsheet or database table
  - and I can represent the rankings in a table or chart.

### pa21: Get Segment Points Similarity Rankings

**Endpoint:**

```http request
GET /public/api/v1.0/rankings/segments/points-similarity
```

**User story:**

- As a EuroFan
- I want to rank broadcast running order segments by their *points similarity*
  - which is a float in the range \[0.0, 1.0\]
    - calculated as the cosine similarity between the normalized televote and jury points received across broadcasts by competitors performing in the segment, using each competing/voting country pair in each broadcast as a vector dimension
  - optionally filtering the queried voting data
    - by minimum contest year (int, default value = 2016)
    - by maximum contest year (int, default value = current year)
    - by contest stages (enum, values = \{'All', 'GrandFinal', 'FirstSemiFinal', 'SecondSemiFinal', 'BothSemiFinals'\}, default value = 'All')
    - by voting country code (string)
  - optionally overriding
    - the rank order (enum, values = \{'HighestToLowest', 'LowestToHighest'\}, default value = 'HighestToLowest')
    - the pagination page size (int, default value = 10)
    - the pagination page index (int, default value = 0)
  - receiving
    - a page of rankings
      - ordered by rank
    - a metadata object comprising
      - all the query parameters
      - the total rankings
      - the total pages
- so that I can observe trends and highlights from the rankings
  - and I can import the rankings into a spreadsheet or database table
  - and I can represent the rankings in a table or chart.
