# 4. Functional requirements

This document is part of the [launch specification](../README.md#launch-specification).

- [4. Functional requirements](#4-functional-requirements)
  - [*admin-api* feature scope](#admin-api-feature-scope)
    - [Broadcasts](#broadcasts)
      - [fa01: Award broadcast jury points](#fa01-award-broadcast-jury-points)
      - [fa02: Award broadcast televote points](#fa02-award-broadcast-televote-points)
      - [fa03: Delete broadcast](#fa03-delete-broadcast)
      - [fa04: Get broadcast](#fa04-get-broadcast)
      - [fa05: Get broadcasts](#fa05-get-broadcasts)
    - [Contests](#contests)
      - [fa06: Create contest](#fa06-create-contest)
      - [fa07: Create contest broadcast](#fa07-create-contest-broadcast)
      - [fa08: Delete contest](#fa08-delete-contest)
      - [fa09: Get contest](#fa09-get-contest)
      - [fa10: Get contests](#fa10-get-contests)
      - [fa11: Handle broadcast completed](#fa11-handle-broadcast-completed)
      - [fa12: Handle broadcast created](#fa12-handle-broadcast-created)
      - [fa13: Handle broadcast deleted](#fa13-handle-broadcast-deleted)
    - [Countries](#countries)
      - [fa14: Create country](#fa14-create-country)
      - [fa15: Delete country](#fa15-delete-country)
      - [fa16: Get countries](#fa16-get-countries)
      - [fa17: Get country](#fa17-get-country)
      - [fa18: Handle contest created](#fa18-handle-contest-created)
      - [fa19: Handle contest deleted](#fa19-handle-contest-deleted)
  - [*public-api* feature scope](#public-api-feature-scope)
    - [Competing country rankings](#competing-country-rankings)
      - [fp01: Get competing country points average rankings](#fp01-get-competing-country-points-average-rankings)
      - [fp02: Get competing country points consensus rankings](#fp02-get-competing-country-points-consensus-rankings)
      - [fp03: Get competing country points in range rankings](#fp03-get-competing-country-points-in-range-rankings)
      - [fp04: Get competing country points share rankings](#fp04-get-competing-country-points-share-rankings)
    - [Competitor rankings](#competitor-rankings)
      - [fp05: Get competitor points average rankings](#fp05-get-competitor-points-average-rankings)
      - [fp06: Get competitor points consensus rankings](#fp06-get-competitor-points-consensus-rankings)
      - [fp07: Get competitor points in range rankings](#fp07-get-competitor-points-in-range-rankings)
      - [fp08: Get competitor points share rankings](#fp08-get-competitor-points-share-rankings)
    - [Listings](#listings)
      - [fp09: Get broadcast result listings](#fp09-get-broadcast-result-listings)
      - [fp10: Get voting country points listings](#fp10-get-voting-country-points-listings)
      - [fp11: Get competing country points listings](#fp11-get-competing-country-points-listings)
      - [fp12: Get competing country result listings](#fp12-get-competing-country-result-listings)
    - [Queryables](#queryables)
      - [fp13: Get queryable broadcasts](#fp13-get-queryable-broadcasts)
      - [fp14: Get queryable contests](#fp14-get-queryable-contests)
      - [fp15: Get queryable countries](#fp15-get-queryable-countries)
    - [Voting country rankings](#voting-country-rankings)
      - [fp16: Get voting country points average rankings](#fp16-get-voting-country-points-average-rankings)
      - [fp17: Get voting country points consensus rankings](#fp17-get-voting-country-points-consensus-rankings)
      - [fp18: Get voting country points in range rankings](#fp18-get-voting-country-points-in-range-rankings)
      - [fp19: Get voting country points share rankings](#fp19-get-voting-country-points-share-rankings)

## *admin-api* feature scope

### Broadcasts

#### fa01: Award broadcast jury points

- As the Admin
- I want to award a set of jury points in a broadcast
  - specifying
    - the broadcast's ID
    - the jury voting country ID
    - the ranked competing country IDs
- So I can add to the voting data that will eventually be queryable.

#### fa02: Award broadcast televote points

- As the Admin
- I want to award a set of televote points in a broadcast
  - specifying
    - the broadcast's ID
    - the televote voting country ID
    - the ranked competing country IDs
- So I can add to the voting data that will eventually be queryable.

#### fa03: Delete broadcast

- As the Admin
- I want to delete a single broadcast
  - specifying
    - the broadcast's ID
- So that the deleted broadcast is completely removed
  - and I am free to create a new broadcast with the same broadcast date if I wish
  - and I am free to create a new broadcast with the same contest stage for its parent contest if I wish.

#### fa04: Get broadcast

- As the Admin
- I want to retrieve a single broadcast
  - specifying
    - the broadcast's ID
- So that I can review its status.

#### fa05: Get broadcasts

- As the Admin
- I want to retrieve a list of all existing broadcasts
  - ordered by broadcast date
- So that I can verify the behaviour of features that create, update, or delete one or more broadcasts.

### Contests

#### fa06: Create contest

- As the Admin
- I want to create a new contest
  - specifying
    - the contest year
    - the city name
    - the contest rules
    - the participants, for each:
      - the participating country ID
      - the Semi-Final draw
      - the act name
      - the song title
  - optionally specifying
    - the global televote voting country ID
- So that I can create the broadcasts for the created contest.

#### fa07: Create contest broadcast

- As the Admin
- I want to create a new child broadcast for an existing contest
  - specifying
    - the contest's ID
    - the broadcast date
    - the contest stage
    - the competing country IDs
  - optionally specifying
    - vacant running order spots for disqualified competitors
- So that I can start awarding the points in the created broadcast.

#### fa08: Delete contest

- As the Admin
- I want to delete a single contest
  - specifying
    - the contest's ID
- So that the deleted contest is completely removed
  - and I am free to create a new contest with the same contest year if I wish.

#### fa09: Get contest

- As the Admin
- I want to retrieve a single contest
  - specifying
    - the contest's ID
- So that I can review its status.

#### fa10: Get contests

- As the Admin
- I want to retrieve a list of all existing contests
  - ordered by contest year
- So that I can verify the behaviour of features that create, update, or delete one or more contests.

#### fa11: Handle broadcast completed

- As the Admin
  - who has completed a broadcast
- I want the parent contest to update itself in the same transaction
  - completing the child broadcast referencing the created broadcast
  - and setting itself to queryable if it owns 3 child broadcasts, all completed
- So that all contests' child broadcasts are always up to date
  - and contests with one or more child broadcasts cannot be deleted
  - and only contests with 3 child broadcasts, all completed, are queryable.

#### fa12: Handle broadcast created

- As the Admin
  - who has created a new broadcast
- I want the parent contest to update itself in the same transaction
  - adding a new child broadcast referencing the created broadcast
- So that all contests' child broadcasts are always up to date
  - and contests with one or more child broadcasts cannot be deleted
  - and only contests with 3 child broadcasts, all completed, are queryable.

#### fa13: Handle broadcast deleted

- As the Admin
  - who has deleted a broadcast
- I want the parent contest to update itself in the same transaction
  - removing the child broadcast referencing the created broadcast
  - and setting itself to not queryable
- So that all contests' child broadcasts are always up to date
  - and contests with one or more child broadcasts cannot be deleted
  - and only contests with 3 child broadcasts, all completed, are queryable.

### Countries

#### fa14: Create country

- As the Admin
- I want to create a new country
  - specifying
    - the country code
    - the country name
- So that I can create contests in which the created country is a participant or global televote.

#### fa15: Delete country

- As the Admin
- I want to delete a single country
  - specifying
    - the country's ID
- So that the deleted country is completely removed
  - and I am free to create a new country with the same country code if I wish.

#### fa16: Get countries

- As the Admin
- I want to retrieve a list of all existing countries
  - ordered by country code
- So that I can verify the behaviour of features that create, update, or delete one or more countries.

#### fa17: Get country

- As the Admin
- I want to retrieve a single country
  - specifying
    - the country's ID
- So that I can review its status.

#### fa18: Handle contest created

- As the Admin
  - who has created a new contest
- I want every participating and voting country to update itself in the same transaction
  - adding a contest role referencing the created contest
- So that all countries' contest roles are always up to date
  - and countries with one or more contest roles cannot be deleted.

#### fa19: Handle contest deleted

- As the Admin
  - who has deleted a contest
- I want every participating and voting country to update itself in the same transaction
  - removing the contest role referencing the created contest
- So that all countries' contest roles are always up to date
  - and countries with one or more contest roles cannot be deleted.

## *public-api* feature scope

### Competing country rankings

#### fp01: Get competing country points average rankings

- As a EuroFan
- I want to rank each competing country by its descending *points average* metric
  - i.e. the average individual points value it received across broadcasts
  - optionally filtering the voting data
    - by contest year range
    - by contest stage
    - by voting country
    - by voting method
  - optionally overriding
    - the pre-pagination sorting
    - the pagination page index
    - the pagination page size
  - retrieving
    - a page of rankings
    - the query metadata
- So that I can learn from the rankings
  - and I can represent the rankings in a table or chart
  - and I can import the rankings into a spreadsheet or database table.

#### fp02: Get competing country points consensus rankings

- As a EuroFan
- I want to rank each competing country by its descending *points consensus* metric
  - i.e. the cosine similarity between jury and televote points it received across broadcasts, using each voting country in each broadcast as a vector dimension
  - optionally filtering the voting data
    - by contest year range
    - by contest stage
    - by voting country
  - optionally overriding
    - the pre-pagination sorting
    - the pagination page index
    - the pagination page size
  - retrieving
    - a page of rankings
    - the query metadata
- So that I can learn from the rankings
  - and I can represent the rankings in a table or chart
  - and I can import the rankings into a spreadsheet or database table.

#### fp03: Get competing country points in range rankings

- As a EuroFan
- I want to rank each competing country by its descending *points in range* metric
  - i.e. the frequency of points awards within a defined range it received across broadcasts, relative to the number of points awards it received
  - specifying
    - the points range
  - optionally filtering the voting data
    - by contest year range
    - by contest stage
    - by voting country
    - by voting method
  - optionally overriding
    - the pre-pagination sorting
    - the pagination page index
    - the pagination page size
  - retrieving
    - a page of rankings
    - the query metadata
- So that I can learn from the rankings
  - and I can represent the rankings in a table or chart
  - and I can import the rankings into a spreadsheet or database table.

#### fp04: Get competing country points share rankings

- As a EuroFan
- I want to rank each competing country by its descending *points share* metric
  - i.e. the total points it received across broadcasts as a fraction of the maximum available points
  - optionally filtering the voting data
    - by contest year range
    - by contest stage
    - by voting country
    - by voting method
  - optionally overriding
    - the pre-pagination sorting
    - the pagination page index
    - the pagination page size
  - retrieving
    - a page of rankings
    - the query metadata
- So that I can learn from the rankings
  - and I can represent the rankings in a table or chart
  - and I can import the rankings into a spreadsheet or database table.

### Competitor rankings

#### fp05: Get competitor points average rankings

- As a EuroFan
- I want to rank each broadcast competitor by their descending *points average* metric
  - i.e. the average individual points value they received in their broadcast
  - optionally filtering the voting data
    - by contest year range
    - by contest stage
    - by voting method
  - optionally overriding
    - the pre-pagination sorting
    - the pagination page index
    - the pagination page size
  - retrieving
    - a page of rankings
    - the query metadata
- So that I can learn from the rankings
  - and I can represent the rankings in a table or chart
  - and I can import the rankings into a spreadsheet or database table.

#### fp06: Get competitor points consensus rankings

- As a EuroFan
- I want to rank each broadcast competitor by their descending *points consensus* metric
  - i.e. the cosine similarity between jury and televote points they received in their broadcast, using each voting country in the broadcast as a vector dimension
  - optionally filtering the voting data
    - by contest year range
    - by contest stage
  - optionally overriding
    - the pre-pagination sorting
    - the pagination page index
    - the pagination page size
  - retrieving
    - a page of rankings
    - the query metadata
- So that I can learn from the rankings
  - and I can represent the rankings in a table or chart
  - and I can import the rankings into a spreadsheet or database table.

#### fp07: Get competitor points in range rankings

- As a EuroFan
- I want to rank each broadcast competitor by their descending *points in range* metric
  - i.e. the frequency of points awards within a defined range they received in their broadcast, relative to the number of points awards they received
  - specifying
    - the points range
  - optionally filtering the voting data
    - by contest year range
    - by contest stage
    - by voting method
  - optionally overriding
    - the pre-pagination sorting
    - the pagination page index
    - the pagination page size
  - retrieving
    - a page of rankings
    - the query metadata
- So that I can learn from the rankings
  - and I can represent the rankings in a table or chart
  - and I can import the rankings into a spreadsheet or database table.

#### fp08: Get competitor points share rankings

- As a EuroFan
- I want to rank each broadcast competitor by their descending *points share* metric
  - i.e. the total points they received in their broadcast as a fraction of the maximum available points
  - optionally filtering the voting data
    - by contest year range
    - by contest stage
    - by voting method
  - optionally overriding
    - the pre-pagination sorting
    - the pagination page index
    - the pagination page size
  - retrieving
    - a page of rankings
    - the query metadata
- So that I can learn from the rankings
  - and I can represent the rankings in a table or chart
  - and I can import the rankings into a spreadsheet or database table.

### Listings

#### fp09: Get broadcast result listings

- As a EuroFan
- I want to retrieve a list of all the results in a single broadcast
  - specifying
    - the contest year
    - the contest stage
  - retrieving
    - a complete list of broadcast results
      - for each
        - the finishing position
        - the running order spot
        - the country code
        - the country name
        - the act name
        - the song title
        - the jury points
        - the televote points
        - the total points
        - the jury rank
        - the televote rank
      - ordered by finishing position
    - the query metadata
- So that I see how all the competitors fared in the broadcast, including the best and worst results.

#### fp10: Get voting country points listings

- As a EuroFan
- I want to retrieve a list of all the points given by a single voting country in a single broadcast
  - specifying
    - the country code
    - the contest year
    - the contest stage
  - retrieving
    - a complete list of the jury points given by the country in the broadcast
      - for each
        - the points value
        - the competing country code
        - the competing country name
        - the act name
        - the song title
      - ordered by descending points value, then by competing country code
    - a complete list of the televote points given by the country in the broadcast
      - for each
        - the points value
        - the competing country code
        - the competing country name
        - the act name
        - the song title
      - ordered by descending points value, then by competing country code
    - the query metadata
- So that I see all the points given by the country, including the highest and lowest points
  - and I can compare the jury and televote points.

#### fp11: Get competing country points listings

- As a EuroFan
- I want to retrieve a list of all the points received by a single competing country in a single broadcast
  - specifying
    - the country code
    - the contest year
    - the contest stage
  - retrieving
    - a complete list of the jury points received by the country in the broadcast
      - for each
        - the points value
        - the voting country code
        - the voting country name
      - ordered by descending points value, then by voting country code
    - a complete list of the televote points received by the country in the broadcast
      - for each
        - the points value
        - the voting country code
        - the voting country name
      - ordered by descending points value, then by voting country code
    - the query metadata, including a summary of the competitor
- So that I see all the points received by the country, including the highest and lowest points
  - and I can compare the jury and televote points.

#### fp12: Get competing country result listings

- As a EuroFan
- I want to retrieve a list of all the results achieved by a single competing country
  - specifying
    - the country code
  - retrieving
    - a complete list of results for the country
      - for each
        - the broadcast date
        - the contest year
        - the contest stage
        - the running order spot
        - the act name
        - the song title
        - the jury points
        - the televote points
        - the total points
        - the finishing position
        - the jury rank
        - the televote rank
      - ordered by broadcast date
- So that I see all the results achieved by the country, including its best and worst results.

### Queryables

#### fp13: Get queryable broadcasts

- As a EuroFan
- I want to retrieve all the queryable broadcasts
  - for each
    - the broadcast date
    - the contest year
    - the city name
    - the contest stage
    - the number of competitors
    - the number of televotes
    - the number of juries
  - in broadcast date order
- So that I can plan my queries.

#### fp14: Get queryable contests

- As a EuroFan
- I want to retrieve all the queryable contests
  - for each
    - the contest year
    - the city name
    - the number of participants
    - whether it uses the global televote
  - in contest year order
- So that I can plan my queries.

#### fp15: Get queryable countries

- As a EuroFan
- I want to retrieve all the queryable countries
  - for each
    - the country code
    - the country name
  - in country code order
- So that I can plan my queries.

### Voting country rankings

#### fp16: Get voting country points average rankings

- As a EuroFan
- I want to rank each voting country by its descending *points average* metric
  - i.e. the average individual points value it gave to a defined country across broadcasts
  - specifying
    - the competing country
  - optionally filtering the voting data
    - by contest year range
    - by contest stage
    - by voting method
  - optionally overriding
    - the pre-pagination sorting
    - the pagination page index
    - the pagination page size
  - retrieving
    - a page of rankings
    - the query metadata
- So that I can learn from the rankings
  - and I can represent the rankings in a table or chart
  - and I can import the rankings into a spreadsheet or database table.

#### fp17: Get voting country points consensus rankings

- As a EuroFan
- I want to rank each voting country by its descending *points consensus* metric
  - i.e. the cosine similarity between jury and televote points it awarded across broadcasts, using each competing country in each broadcast as a vector dimension
  - optionally filtering the voting data
    - by competing country
    - by contest year range
    - by contest stage
  - optionally overriding
    - the pre-pagination sorting
    - the pagination page index
    - the pagination page size
  - retrieving
    - a page of rankings
    - the query metadata
- So that I can learn from the rankings
  - and I can represent the rankings in a table or chart
  - and I can import the rankings into a spreadsheet or database table.

#### fp18: Get voting country points in range rankings

- As a EuroFan
- I want to rank each voting country by its descending *points in range* metric
  - i.e. the frequency of points awards within a defined range it gave to a defined country across broadcasts, relative to the number of points awards it gave to that country
  - specifying
    - the competing country
    - the points range
  - optionally filtering the voting data
    - by contest year range
    - by contest stage
    - by voting method
  - optionally overriding
    - the pre-pagination sorting
    - the pagination page index
    - the pagination page size
  - retrieving
    - a page of rankings
    - the query metadata
- So that I can learn from the rankings
  - and I can represent the rankings in a table or chart
  - and I can import the rankings into a spreadsheet or database table.

#### fp19: Get voting country points share rankings

- As a EuroFan
- I want to rank each voting country by its descending *points share* metric
  - i.e. the total points it gave to a defined country across broadcasts as a fraction of the maximum available points
  - specifying
    - the competing country
  - optionally filtering the voting data
    - by contest year range
    - by contest stage
    - by voting method
  - optionally overriding
    - the pre-pagination sorting
    - the pagination page index
    - the pagination page size
  - retrieving
    - a page of rankings
    - the query metadata
- So that I can learn from the rankings
  - and I can represent the rankings in a table or chart
  - and I can import the rankings into a spreadsheet or database table.
