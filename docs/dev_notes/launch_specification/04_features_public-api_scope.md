# 04: Features: *public-api* scope

This document is part of the [launch specification](../README.md#launch-specification).

- [04: Features: *public-api* scope](#04-features-public-api-scope)
  - [Competitor Rankings](#competitor-rankings)
    - [pa01: Get competitor points average rankings](#pa01-get-competitor-points-average-rankings)
    - [pa02: Get competitor points in range rankings](#pa02-get-competitor-points-in-range-rankings)
    - [pa03: Get competitor points share rankings](#pa03-get-competitor-points-share-rankings)
    - [pa04: Get competitor points similarity rankings](#pa04-get-competitor-points-similarity-rankings)
  - [Country Rankings](#country-rankings)
    - [pa05: Get country given points average rankings](#pa05-get-country-given-points-average-rankings)
    - [pa06: Get country given points in range rankings](#pa06-get-country-given-points-in-range-rankings)
    - [pa07: Get country given points share rankings](#pa07-get-country-given-points-share-rankings)
    - [pa08: Get country given points similarity rankings](#pa08-get-country-given-points-similarity-rankings)
    - [pa09: Get country received points average rankings](#pa09-get-country-received-points-average-rankings)
    - [pa10: Get country received points in range rankings](#pa10-get-country-received-points-in-range-rankings)
    - [pa11: Get country received points share rankings](#pa11-get-country-received-points-share-rankings)
    - [pa12: Get country received points similarity rankings](#pa12-get-country-received-points-similarity-rankings)
  - [Points Awards](#points-awards)
    - [pa13: Get given points awards](#pa13-get-given-points-awards)
    - [pa14: Get received points awards](#pa14-get-received-points-awards)
  - [Queryable Broadcasts](#queryable-broadcasts)
    - [pa15: Get queryable broadcasts](#pa15-get-queryable-broadcasts)
  - [Queryable Contests](#queryable-contests)
    - [pa16: Get queryable contests](#pa16-get-queryable-contests)
  - [Queryable Countries](#queryable-countries)
    - [pa17: Get queryable countries](#pa17-get-queryable-countries)
  - [Scoreboard Rows](#scoreboard-rows)
    - [pa18: Get scoreboard rows](#pa18-get-scoreboard-rows)
  - [Segment Rankings](#segment-rankings)
    - [pa19: Get segment points average rankings](#pa19-get-segment-points-average-rankings)
    - [pa20: Get segment points in range rankings](#pa20-get-segment-points-in-range-rankings)
    - [pa21: Get segment points share rankings](#pa21-get-segment-points-share-rankings)
    - [pa22: Get segment points similarity rankings](#pa22-get-segment-points-similarity-rankings)

## Competitor Rankings

### pa01: Get competitor points average rankings

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
    - by contest stages (enum, values = \{'All', 'GrandFinal', 'SemiFinals', 'SemiFinal1', 'SemiFinal2'\}, default value = 'All')
    - by voting method (enum, values = \{'Any', 'Televote', 'Jury'\}, default value = 'Any')
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

### pa02: Get competitor points in range rankings

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
    - by contest stages (enum, values = \{'All', 'GrandFinal', 'SemiFinals', 'SemiFinal1', 'SemiFinal2'\}, default value = 'All')
    - by voting method (enum, values = \{'Any', 'Televote', 'Jury'\}, default value = 'Any')
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

### pa03: Get competitor points share rankings

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
    - by contest stages (enum, values = \{'All', 'GrandFinal', 'SemiFinals', 'SemiFinal1', 'SemiFinal2'\}, default value = 'All')
    - by voting method (enum, values = \{'Any', 'Televote', 'Jury'\}, default value = 'Any')
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

### pa04: Get competitor points similarity rankings

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
    - by contest stages (enum, values = \{'All', 'GrandFinal', 'SemiFinals', 'SemiFinal1', 'SemiFinal2'\}, default value = 'All')
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

### pa05: Get country given points average rankings

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
    - by contest stages (enum, values = \{'All', 'GrandFinal', 'SemiFinals', 'SemiFinal1', 'SemiFinal2'\}, default value = 'All')
    - by voting method (enum, values = \{'Any', 'Televote', 'Jury'\}, default value = 'Any')
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

### pa06: Get country given points in range rankings

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
    - by contest stages (enum, values = \{'All', 'GrandFinal', 'SemiFinals', 'SemiFinal1', 'SemiFinal2'\}, default value = 'All')
    - by voting method (enum, values = \{'Any', 'Televote', 'Jury'\}, default value = 'Any')
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

### pa07: Get country given points share rankings

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
    - by contest stages (enum, values = \{'All', 'GrandFinal', 'SemiFinals', 'SemiFinal1', 'SemiFinal2'\}, default value = 'All')
    - by voting method (enum, values = \{'Any', 'Televote', 'Jury'\}, default value = 'Any')
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

### pa08: Get country given points similarity rankings

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
    - by contest stages (enum, values = \{'All', 'GrandFinal', 'SemiFinals', 'SemiFinal1', 'SemiFinal2'\}, default value = 'All')
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

### pa09: Get country received points average rankings

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
    - by contest stages (enum, values = \{'All', 'GrandFinal', 'SemiFinals', 'SemiFinal1', 'SemiFinal2'\}, default value = 'All')
    - by voting method (enum, values = \{'Any', 'Televote', 'Jury'\}, default value = 'Any')
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

### pa10: Get country received points in range rankings

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
    - by contest stages (enum, values = \{'All', 'GrandFinal', 'SemiFinals', 'SemiFinal1', 'SemiFinal2'\}, default value = 'All')
    - by voting method (enum, values = \{'Any', 'Televote', 'Jury'\}, default value = 'Any')
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

### pa11: Get country received points share rankings

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
    - by contest stages (enum, values = \{'All', 'GrandFinal', 'SemiFinals', 'SemiFinal1', 'SemiFinal2'\}, default value = 'All')
    - by voting method (enum, values = \{'Any', 'Televote', 'Jury'\}, default value = 'Any')
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

### pa12: Get country received points similarity rankings

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
    - by contest stages (enum, values = \{'All', 'GrandFinal', 'SemiFinals', 'SemiFinal1', 'SemiFinal2'\}, default value = 'All')
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

### pa13: Get given points awards

**Endpoint:**

```http request
GET /public/api/v1.0/points-awards/given
```

**User story:**

- As a EuroFan
- I want to retrieve all the points awards given by a specific country in a specific broadcast
  - providing
    - the contest year (int)
    - the contest stage (enum, values = \{'GrandFinal', 'SemiFinal1', 'SemiFinal2'\})
    - the country code (string)
  - receiving
    - a list of given televote points awards, ordered by descending points value then by ascending competing country code
    - a list of given jury points awards, ordered by descending points value then by ascending competing country code
  - a metadata object
- so that I can see exactly how the country voted in the broadcast
  - and I can import the points awards into a spreadsheet or database table to run my own analyses.

### pa14: Get received points awards

**Endpoint:**

```http request
GET /public/api/v1.0/points-awards/received
```

**User story:**

- As a EuroFan
- I want to retrieve all the points awards received by a specific country in a specific broadcast
  - providing
    - the contest year (int)
    - the contest stage (enum, values = \{'GrandFinal', 'SemiFinal1', 'SemiFinal2'\})
    - the country code (string)
  - receiving
    - a list of received televote points awards, ordered by descending points value then by ascending voting country code
    - a list of received jury points awards, ordered by descending points value then by ascending voting country code
- so that I can see exactly how the country performed in the broadcast
  - and I can import the points awards into a spreadsheet or database table to run my own analyses.

## Queryable Broadcasts

### pa15: Get queryable broadcasts

**Endpoint:**

```http request
GET /public/api/v1.0/queryable-broadcasts
```

**User story:**

- As a EuroFan
- I want to retrieve all the queryable broadcasts
  - receiving
    - a list of queryable broadcasts
      - ordered by broadcast date
- so that I can get an overview of the Public API's queryable voting data.

## Queryable Contests

### pa16: Get queryable contests

**Endpoint:**

```http request
GET /public/api/v1.0/queryable-contests
```

**User story:**

- As a EuroFan
- I want to retrieve all the queryable contests
  - receiving
    - a list of queryable contests
      - ordered by contest year
- so that I can get an overview of the Public API's queryable voting data.

## Queryable Countries

### pa17: Get queryable countries

**Endpoint:**

```http request
GET /public/api/v1.0/queryable-countries
```

**User story:**

- As a EuroFan
- I want to retrieve all the queryable countries
  - receiving
    - a list of queryable countries
      - ordered by country code
- so that I can get an overview of the Public API's queryable voting data.

## Scoreboard Rows

### pa18: Get scoreboard rows

**Endpoint:**

```http request
GET /public/api/v1.0/scoreboard-rows
```

**User story:**

- As a EuroFan
- I want to retrieve all the scoreboard rows from a specific broadcast
  - providing
    - the contest year (int)
    - the contest stage (enum, values = \{'GrandFinal', 'SemiFinal1', 'SemiFinal2'\})
  - receiving
    - a list of scoreboard rows
      - ordered by running order spot
  - a metadata object
- so that I can see all the results in the broadcast
  - and I can import the scoreboard rows into a spreadsheet or database table to run my own analyses.

## Segment Rankings

### pa19: Get segment points average rankings

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
    - by contest stages (enum, values = \{'All', 'GrandFinal', 'SemiFinals', 'SemiFinal1', 'SemiFinal2'\}, default value = 'All')
    - by voting method (enum, values = \{'Any', 'Televote', 'Jury'\}, default value = 'Any')
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

### pa20: Get segment points in range rankings

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
    - by contest stages (enum, values = \{'All', 'GrandFinal', 'SemiFinals', 'SemiFinal1', 'SemiFinal2'\}, default value = 'All')
    - by voting method (enum, values = \{'Any', 'Televote', 'Jury'\}, default value = 'Any')
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

### pa21: Get segment points share rankings

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
    - by contest stages (enum, values = \{'All', 'GrandFinal', 'SemiFinals', 'SemiFinal1', 'SemiFinal2'\}, default value = 'All')
    - by voting method (enum, values = \{'Any', 'Televote', 'Jury'\}, default value = 'Any')
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

### pa22: Get segment points similarity rankings

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
    - by contest stages (enum, values = \{'All', 'GrandFinal', 'SemiFinals', 'SemiFinal1', 'SemiFinal2'\}, default value = 'All')
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
