# System requirements (initial release)

This document outlines the required features for the initial release of the *Eurocentric* project.

- [System requirements (initial release)](#system-requirements-initial-release)
  - [Summary](#summary)
  - [Features and user roles](#features-and-user-roles)
    - [*Admin API* features and Admin user role](#admin-api-features-and-admin-user-role)
    - [*Public API* features and Euro-Fan user role](#public-api-features-and-euro-fan-user-role)
    - [Shared features and Dev user role](#shared-features-and-dev-user-role)
  - [A: *admin-api* scoped features](#a-admin-api-scoped-features)
    - [A1: Broadcasts features](#a1-broadcasts-features)
      - [A1a: Award jury points](#a1a-award-jury-points)
      - [A1b: Award televote points](#a1b-award-televote-points)
      - [A1c: Create broadcast for contest](#a1c-create-broadcast-for-contest)
      - [A1d: Delete broadcast](#a1d-delete-broadcast)
      - [A1e: Get broadcast](#a1e-get-broadcast)
      - [A1f: Get broadcasts for contest](#a1f-get-broadcasts-for-contest)
    - [A2: Contests features](#a2-contests-features)
      - [A2a: Create contest](#a2a-create-contest)
      - [A2b: Delete contest](#a2b-delete-contest)
      - [A2c: Get contest](#a2c-get-contest)
      - [A2d: Get contests](#a2d-get-contests)
      - [A2e: Handle broadcast completed](#a2e-handle-broadcast-completed)
      - [A2f: Handle broadcast deleted](#a2f-handle-broadcast-deleted)
    - [A3: Countries features](#a3-countries-features)
      - [A3a: Create country](#a3a-create-country)
      - [A3b: Delete country](#a3b-delete-country)
      - [A3c: Get countries](#a3c-get-countries)
      - [A3d: Get country](#a3d-get-country)
      - [A3e: Handle broadcast created](#a3e-handle-broadcast-created)
      - [A3f: Handle broadcast deleted](#a3f-handle-broadcast-deleted)
      - [A3g: Handle contest created](#a3g-handle-contest-created)
      - [A3h: Handle contest deleted](#a3h-handle-contest-deleted)
  - [P: *public-api* scoped features](#p-public-api-scoped-features)
    - [P1: Broadcasts features](#p1-broadcasts-features)
      - [P1a: Get queryable broadcasts](#p1a-get-queryable-broadcasts)
    - [P2: Contests features](#p2-contests-features)
      - [P2a: Get queryable contests](#p2a-get-queryable-contests)
    - [P3: Countries features](#p3-countries-features)
      - [P3a: Get queryable countries](#p3a-get-queryable-countries)
    - [P4: Competing country rankings features](#p4-competing-country-rankings-features)
      - [P4a: Get competing country points average rankings](#p4a-get-competing-country-points-average-rankings)
      - [P4b: Get competing country points consensus rankings](#p4b-get-competing-country-points-consensus-rankings)
      - [P4c: Get competing country points in range rankings](#p4c-get-competing-country-points-in-range-rankings)
      - [P4d: Get competing country points share rankings](#p4d-get-competing-country-points-share-rankings)
    - [P5: Competitor rankings features](#p5-competitor-rankings-features)
      - [P5a: Get competitor points average rankings](#p5a-get-competitor-points-average-rankings)
      - [P5b: Get competitor points consensus rankings](#p5b-get-competitor-points-consensus-rankings)
      - [P5c: Get competitor points in range rankings](#p5c-get-competitor-points-in-range-rankings)
      - [P5d: Get competitor points share rankings](#p5d-get-competitor-points-share-rankings)
    - [P6: Grand final spot rankings features](#p6-grand-final-spot-rankings-features)
      - [P6a: Get grand final spot points average rankings](#p6a-get-grand-final-spot-points-average-rankings)
      - [P6b: Get grand final spot points share rankings](#p6b-get-grand-final-spot-points-share-rankings)
    - [P7: Semi-final spot rankings features](#p7-semi-final-spot-rankings-features)
      - [P7a: Get semi-final spot points average rankings](#p7a-get-semi-final-spot-points-average-rankings)
      - [P7b: Get semi-final spot points share rankings](#p7b-get-semi-final-spot-points-share-rankings)
    - [P8: Voting country rankings features](#p8-voting-country-rankings-features)
      - [P8a: Get voting country points average rankings](#p8a-get-voting-country-points-average-rankings)
      - [P8b: Get voting country points consensus rankings](#p8b-get-voting-country-points-consensus-rankings)
      - [P8c: Get voting country points in range rankings](#p8c-get-voting-country-points-in-range-rankings)
      - [P8d: Get voting country points share rankings](#p8d-get-voting-country-points-share-rankings)
  - [S: *shared* scoped features](#s-shared-scoped-features)
    - [S1: API versioning](#s1-api-versioning)
      - [S1a: Versioned API releases](#s1a-versioned-api-releases)
    - [S2: Error handling](#s2-error-handling)
      - [S2a: Global exception handling](#s2a-global-exception-handling)
      - [S2b: Problem details responses](#s2b-problem-details-responses)
    - [S3: OpenAPI](#s3-openapi)
      - [S3a: OpenAPI document per release](#s3a-openapi-document-per-release)
      - [S3b: OpenAPI user interface](#s3b-openapi-user-interface)
    - [S4: Security](#s4-security)
      - [S4a: Api key security](#s4a-api-key-security)
  - [Technical requirements](#technical-requirements)
  - [Features out of scope](#features-out-of-scope)

## Summary

- *Eurocentric* is a .NET minimal web API for analysing voting patterns in the Eurovision Song Contest, 2016-present.
- *Eurocentric* is composed of two separate APIs with a shared domain and database: the *Public API* and the *Admin API*.
- The *Public API* provides a wide range of configurable analytics queries that can be run on the queryable data held in the system database.
- The *Admin API* provides the means to populate the system database with the queryable data.

## Features and user roles

Features are grouped into three scopes, each with their own user role.

### *Admin API* features and Admin user role

The Admin is a single software developer (myself), who interacts with the system using its *Admin API*.

As the Admin, I am very familiar with the Eurovision Song Contest and how it works. I am also intimately familiar with the internal workings of the system since I am the sole developer.

As the Admin, I use the *Admin API* because I want to populate the system database with the queryable data to support the *Public API*, that is, the countries, contests and broadcasts from the Eurovision Song Contest, 2016-present.

Additionally, I wished to gain some experience in domain-driven design. This is why the *Admin API* is implemented using aggregates, transactions, domain events, etc., when the system database could just as well be populated using a single SQL script.

### *Public API* features and Euro-Fan user role

The Euro-Fan is an anonymous member of the public located anywhere in the world, who interacts with the system using its *Public API*.

The Euro-Fan has at least some familiarity with the Eurovision Song Contest and how it works.

The Euro-Fan uses the *Public API* because they are interested in discovering insights from the way the votes have been distributed in recent contests.

Questions posed by the Euro-Fan may include:

- "Of all the competing countries in all the broadcasts, which competitors received the highest average points award value?"
- "Of all the competing countries in the 2022 contest, which competitor received the highest share of the maximum possible points they could have received?"
- "Of all the competing countries overall, which competitor has received the highest relative frequency of non-zero televote points?"
- "Of all the competing countries overall, in the grand finals of the contests between 2023 and the present, which competitor has had the highest consensus between the jury points and the televote points they received?"
- "Of all the voting countries overall, which voter has awarded jury points most similar to those awarded by the UK?"

### Shared features and Dev user role

The Dev is a single software developer (myself), who is building the system.

As the Dev, I want to build a system that can accommodate new features (i.e. new API versions) with minimal refactoring.

I want the code to be written in such a way that, when I revisit it in 6-12 months, it does not take me more than an afternoon to reacquaint myself with the architecture and implementation.

I want to use the native ASP.NET libraries as much as possible, rather than 3rd-party libraries.

I want to use free or very low-cost hosting services.

I want the system to be functional enough for a single user; that is, I am not anticipating Amazon-level traffic and I do not intend to design the system for such.

## A: *admin-api* scoped features

Features with the *admin-api* scope belong to the *Admin API*.

### A1: Broadcasts features

Features relating to the [**BROADCAST** aggregate](domain_model.md#broadcasts-subdomain).

#### A1a: Award jury points

**User story:**

```
As the Admin,

I want to award the points for a jury voting country in a broadcast in the system
  specifying the broadcast ID
  and the jury voting country ID
  and the top-ranked competing countries' IDs,

so that I can add to the queryable data for the Public API.
```

#### A1b: Award televote points

**User story:**

```
As the Admin,

I want to award the points for a televoting country in a broadcast in the system
  specifying the broadcast ID
  and the televoting country ID
  and the top-ranked competing countries' IDs,

so that I can add to the queryable data for the Public API.
```

#### A1c: Create broadcast for contest

**User story:**

```
As the Admin,

I want to create a new broadcast for a contest in the system
  specifying the contest ID
  and the broadcast's contest stage
  and the transmission date
  and the competing country IDs in performing order,

so that I can award the points in the broadcast,
  and the contest cannot be deleted.
```

#### A1d: Delete broadcast

**User story:**

```
As the Admin,

I want to delete a broadcast from the system
  specifying the broadcast ID,

so that I can re-create it with amended details.
```

#### A1e: Get broadcast

**User story:**

```
As the Admin,

I want to retrieve a broadcast from the system
  specifying the broadcast ID,

so that I can review its details.
```

#### A1f: Get broadcasts for contest

**User story:**

```
As the Admin,

I want to retrieve a list of all the broadcasts for a contest in the system
  specifying the contest ID
  ordered by broadcast transmission date,

so that I can review their details.
```

### A2: Contests features

Features relating to the [**CONTEST** aggregate](domain_model.md#countries-subdomain).

#### A2a: Create contest

**User story:**

```
As the Admin,

I want to create a new contest in the system
  specifying the contest year
  and the host city name
  and the voting rules
  and the participating countries,

so that I can create the broadcasts for the contest
  and award the points in the broadcasts.
```

#### A2b: Delete contest

**User story:**

```
As the Admin,

I want to delete a contest from the system
  specifying the contest ID,

so that I can re-create it with amended details.
```

#### A2c: Get contest

**User story:**

```
As the Admin,

I want to retrieve a contest from the system
  specifying the contest ID,

so that I can review its details.
```

#### A2d: Get contests

**User story:**

```
As the Admin,

I want to retrieve a list of all the contests in the system
  ordered by contest year,

so that I can see which contests I need to create.
```

#### A2e: Handle broadcast completed

**User story:**

```
As the Admin,

I want a contest to update itself
  whenever one of its constituent broadcasts is completed,

so that the Public API's queryable data is up to date.
```

#### A2f: Handle broadcast deleted

**User story:**

```
As the Admin,

I want a contest to update itself
  whenever one of its constituent broadcasts is deleted,

so that the Public API's queryable data is up to date,
  and the contest can be deleted
  provided it has no constituent broadcasts after the update.
```

### A3: Countries features

Features relating to the [**COUNTRY** aggregate](domain_model.md#countries-subdomain).

#### A3a: Create country

**User story:**

```
As the Admin,

I want to create a new country in the system
  specifying the country code
  and the country name,

so that I can reference the country in contests and broadcasts.
```

#### A3b: Delete country

**User story:**

```
As the Admin,

I want to delete a country from the system
  specifying the country ID,

so that I can re-create it with amended details.
```

#### A3c: Get countries

**User story:**

```
As the Admin,

I want to retrieve a list of all the countries in the system
  ordered by country code,

so that I can see which countries I need to create.
```

#### A3d: Get country

**User story:**

```
As the Admin,

I want to retrieve a country from the system
  specifying the country ID,

so that I can review its details.
```

#### A3e: Handle broadcast created

**User story:**

```
As the Admin,

I want a country to update itself
  whenever one of its competing broadcasts is created
  or one of its televoting broadcasts is created
  or one of its jury voting broadcasts is created,

so that the country cannot be deleted.
```

#### A3f: Handle broadcast deleted

**User story:**

```
As the Admin,

I want a country to update itself
  whenever one of its competing broadcasts is deleted
  or one of its televoting broadcasts is deleted
  or one of its jury voting broadcasts is deleted,

so that the country can be deleted
  provided it has no participant contests after the update
  and it has no competing broadcasts after the update
  and it has no televoting broadcasts after the update
  and it has no jury voting broadcasts after the update.
```

#### A3g: Handle contest created

**User story:**

```
As the Admin,

I want a country to update itself
  whenever one of its participating contests is created,

so that the country cannot be deleted.
```

#### A3h: Handle contest deleted

**User story:**

```
As the Admin,

I want a country to update itself
  whenever one of its participating contests is deleted,

so that the country can be deleted
  provided it has no participant contests after the update
  and it has no competing broadcasts after the update
  and it has no televoting broadcasts after the update
  and it has no jury voting broadcasts after the update.
```

## P: *public-api* scoped features

Features with the *public-api* scope belong to the *Public API*.

### P1: Broadcasts features

A **BROADCAST** is queryable by the *Public API* if it is a **ConstituentBroadcast** of a [queryable **CONTEST**](#p2-contests-features).

#### P1a: Get queryable broadcasts

**User story:**

```
As a Euro-Fan,

I want to get a list of all the queryable broadcasts in the system
  ordered by transmission date,

so that I can plan my queries.
```

### P2: Contests features

A **CONTEST** is queryable by the *Public API* if it is [completed](domain_model.md#contests-subdomain-invariants), that is, it has 3 constituent broadcasts, all of which are completed.

#### P2a: Get queryable contests

**User story:**

```
As a Euro-Fan,

I want to get a list of all the queryable contests in the system
  ordered by contest year,

so that I can plan my queries.
```

### P3: Countries features

A **COUNTRY** is queryable by the *Public API* if it is a **ParticipatingCountry** in a [queryable **CONTEST**](#p2-contests-features).

#### P3a: Get queryable countries

**User story:**

```
As a Euro-Fan,

I want to get a list of all the queryable countries in the system
  ordered by country code,

so that I can plan my queries.
```

### P4: Competing country rankings features

Features that rank each country overall based on the points it received across all the broadcasts in which it competed.

#### P4a: Get competing country points average rankings

**User story:**

```
As a Euro-Fan,

I want to rank every competing country
  by the average value of all the points awards it received
  and get a page of rankings,

so that I can represent the page in a table, chart, etc.
```

#### P4b: Get competing country points consensus rankings

**User story:**

```
As a Euro-Fan,

I want to rank every competing country
  by the cosine similarity of all the televote points awards it received
  compared with all the jury points awards it received
  using each voting country in each broadcast as a vector dimension
  and get a page of rankings,

so that I can represent the page in a table, chart, etc.
```

#### P4c: Get competing country points in range rankings

**User story:**

```
As a Euro-Fan,

I want to rank every competing country
  by the percentage of all the points awards it received
  having a value in a specified range
  and get a page of rankings,

so that I can represent the page in a table, chart, etc.
```

#### P4d: Get competing country points share rankings

**User story:**

```
As a Euro-Fan,

I want to rank every competing country
  by the sum total points it received
  as a percentage of the available points
  and get a page of rankings,

so that I can represent the page in a table, chart, etc.
```

### P5: Competitor rankings features

Features that rank each competitor in each broadcast based on the points it received in the broadcast.

#### P5a: Get competitor points average rankings

**User story:**

```
As a Euro-Fan,

I want to rank every competitor
  by the average value of all the points awards it received
  and get a page of rankings,

so that I can represent the page in a table, chart, etc.
```

#### P5b: Get competitor points consensus rankings

**User story:**

```
As a Euro-Fan,

I want to rank every competing country
  by the cosine similarity of all the televote points awards it received
  compared with all the jury points awards it received
  using each voting country in its broadcast as a vector dimension
  and get a page of rankings,

so that I can represent the page in a table, chart, etc.
```

#### P5c: Get competitor points in range rankings

**User story:**

```
As a Euro-Fan,

I want to rank every competing country
  by the percentage of all the points awards it received
  having a value in a specified range
  and get a page of rankings,

so that I can represent the page in a table, chart, etc.
```

#### P5d: Get competitor points share rankings

**User story:**

```
As a Euro-Fan,

I want to rank every competing country
  by the sum total points it received
  as a percentage of the available points
  and get a page of rankings,

so that I can represent the page in a table, chart, etc.
```

### P6: Grand final spot rankings features

Features that rank each spot in the grand final performing order based on the points received by the competitors in that spot across all the grand final broadcasts.

#### P6a: Get grand final spot points average rankings

**User story:**

```
As a Euro-Fan,

I want to rank every grand final running order spot
  by the average value of all the points awards received by competitors in the spot
  and get a page of rankings,

so that I can represent the page in a table, chart, etc.
```

#### P6b: Get grand final spot points share rankings

**User story:**

```
As a Euro-Fan,

I want rank every grand final running order spot
  by the sum total points received by competitors in the spot
  as a percentage of the available points
  and get a page of rankings,

so that I can represent the page in a table, chart, etc.
```

### P7: Semi-final spot rankings features

Features that rank each spot in the semi-final performing order based on the points received by the competitors in that spot across all the semi-final broadcasts.

#### P7a: Get semi-final spot points average rankings

**User story:**

```
As a Euro-Fan,

I want to rank every semi-final running order spot
  by the average value of all the points awards received by competitors in the spot
  and get a page of rankings,

so that I can represent the page in a table, chart, etc.
```

#### P7b: Get semi-final spot points share rankings

**User story:**

```
As a Euro-Fan,

I want rank every semi-final running order spot
  by the sum total points received by competitors in the spot
  as a percentage of the available points
  and get a page of rankings,

so that I can represent the page in a table, chart, etc.
```

### P8: Voting country rankings features

Features that rank each country overall based on the points it gave across all the broadcasts in which it voted.

#### P8a: Get voting country points average rankings

**User story:**

```
As a Euro-Fan,

I want to rank every voting country
  by the average value of all the points awards it gave
  to a specified competing country
  and get a page of rankings,

so that I can represent the page in a table, chart, etc.
```

#### P8b: Get voting country points consensus rankings

**User story:**

```
As a Euro-Fan,

I want to rank every voting country
  by the cosine similarity of all the televote points awards it received
  compared with all the jury points awards it received
  using each competing country in each broadcast as a vector dimension
  and get a page of rankings,

so that I can represent the page in a table, chart, etc.
```

#### P8c: Get voting country points in range rankings

**User story:**

```
As a Euro-Fan,

I want to rank every voting country
  by the percentage of all the points awards it gave
  to a specified competing country
  having a value in a specified range
  and get a page of rankings,

so that I can represent the page in a table, chart, etc.
```

#### P8d: Get voting country points share rankings

**User story:**

```
As a Euro-Fan,

I want to rank every voting country
  by the sum total points it gave
  to the specified competing country
  as a percentage of the available points
  and get a page of rankings,

so that I can represent the page in a table, chart, etc.
```

## S: *shared* scoped features

Features with the *shared* scope are shared across all the APIs in the system.

### S1: API versioning

#### S1a: Versioned API releases

**User story:**

```
As the Dev,

I want to release each minor version of each API separately,
  as a whole,
  with the version number as a URL segment,

so that the client must specify the version of the API.
```

### S2: Error handling

#### S2a: Global exception handling

**User story:**

```
As the Dev,

I want any uncaught exception to be mapped to an HTTP response
  with the appropriate unsuccessful status code (e.g. 400, 500),
  and a serialized problem details object,

so that the client is informed that something went wrong.
```

#### S2b: Problem details responses

**User story:**

```
As the Dev,

I want any unsuccessful application command or query to be mapped to an HTTP response
  with the appropriate unsuccessful status code (e.g. 400, 404, 409),
  and a serialized problem details object,

so that the client can use the information to send a successful request.
```

### S3: OpenAPI

#### S3a: OpenAPI document per release

**User story:**

```
As the Dev,

I want every API release to serve an OpenAPI JSON document,
  in development and production,
  with a standardized document name format,

so that clients can see how to work with the API.
```

#### S3b: OpenAPI user interface

**User story:**

```
As the Dev,

I want every API release's OpenAPI document to be served as a web page,
  in development and production,
  with documentation for every API endpoint,

so that clients can learn how to work with the API,
  and I can experiment with the endpoints during development.
```

### S4: Security

#### S4a: Api key security

**User story:**

```
As the Dev,

I want the APIs to be protected by role-based authorization schemes,
  with API key authentication,

so that I can restrict the Admin API to my own personal use as the Admin,
  and I can block all anonymous clients from the Public API.
```

## Technical requirements

1. The system uses the **.NET 9** SDK.
2. The system is hosted in the cloud as an Azure Web App.
3. The system database is hosted in the cloud as an Azure SQL Database.
4. The language of the system is UK English.
5. Countries and host cities are referred to using their English-language names following the [official Eurovision website](https://www.eurovision.tv/).
6. The system's APIs adhere to [Level 2 REST maturity](https://martinfowler.com/articles/richardsonMaturityModel.html#level2).

## Features out of scope

The following features will not be implemented in the initial release:

- logging,
- rate limiting,
- response caching.

The system should be designed in such a way that the features *could* be introduced in a future release with a minimum of restructuring.
