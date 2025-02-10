# Admin API requirements

This document outlines the requirements for version 1.0 of *Eurocentric*'s *Admin API*.

Refer to the [project summary](project_summary.md) dev note for an overview of the *Admin API* and the Admin user role.

- [Admin API requirements](#admin-api-requirements)
  - [A1: Broadcasts](#a1-broadcasts)
    - [A101: Award Jury Points](#a101-award-jury-points)
    - [A102: Award Televote Points](#a102-award-televote-points)
    - [A103: Delete Broadcast](#a103-delete-broadcast)
    - [A104: Disqualify Competitor](#a104-disqualify-competitor)
    - [A105: Get Broadcast](#a105-get-broadcast)
    - [A106: Get Broadcast Summaries](#a106-get-broadcast-summaries)
    - [A107: Initialize Broadcast For Contest](#a107-initialize-broadcast-for-contest)
  - [A2: Contests](#a2-contests)
    - [A201: Create Contest](#a201-create-contest)
    - [A202: Delete Contest](#a202-delete-contest)
    - [A203: Get Contest](#a203-get-contest)
    - [A204: Get Contests](#a204-get-contests)
    - [A205: Handle Broadcast Completed](#a205-handle-broadcast-completed)
    - [A206: Handle Broadcast Deleted](#a206-handle-broadcast-deleted)
  - [A3: Countries](#a3-countries)
    - [A301: Create Country](#a301-create-country)
    - [A302: Delete Country](#a302-delete-country)
    - [A303: Get Countries](#a303-get-countries)
    - [A304: Get Country](#a304-get-country)
    - [A305: Handle Contest Created](#a305-handle-contest-created)
    - [A306: Handle Contest Deleted](#a306-handle-contest-deleted)

## A1: Broadcasts

### A101: Award Jury Points

```
As the Admin,

I want to award a set of points,
  for a jury,
  identified by its country code,
  in a broadcast in the system,
  identified by its contest year and contest stage,
  supplying the ranked competitor country codes,

so that I can add to the voting data that will eventually be queryable.
```

### A102: Award Televote Points

```
As the Admin,

I want to award a set of points,
  for a televote,
  identified by its country code,
  in a broadcast in the system,
  identified by its contest year and contest stage,
  supplying the ranked competitor country codes,

so that I can add to the voting data that will eventually be queryable.
```

### A103: Delete Broadcast

```
As the Admin,

I want to delete a broadcast from the system,
  identified by its contest year and contest stage,

so that I can re-initialize with with no points awarded.
```

### A104: Disqualify Competitor

```
As the Admin,

I want to disqualify a competitor,
  identified by its country code,
  in a broadcast in the system,
  identified by its contest year and country code,
  for which no points have yet been awarded,

so that the disqualified competitor is removed from the broadcast.
```

### A105: Get Broadcast

```
As the Admin,

I want to retrieve a broadcast from the system,
  identified by its contest year and country code,

so that I can see which points I need to award.
```

### A106: Get Broadcast Summaries

```
As the Admin,

I want to retrieve a list of all the broadcasts in the system,
  in summary format,
  ordered by transmission date,

so that I can see which broadcasts I need to create.
```

### A107: Initialize Broadcast For Contest

```
As the Admin,

I want to initialize a broadcast,
  for a contest in the system,
  identified by its contest year,
  supplying the contest stage,
  and the transmission date,
  and the competitor country codes in running order,

so that I can start awarding points in the broadcast.
```

## A2: Contests

### A201: Create Contest

```
As the Admin,

I want to create a new contest in the system,
  supplying its contest year,
  and its host city name,
  and its participants,
  and its contest format,

so that I can initialize the broadcasts for the contest.
```

### A202: Delete Contest

```
As the Admin,

I want to delete a contest from the system,
  identified by its contest year,

so that I can re-create it with amended details.
```

### A203: Get Contest

```
As the Admin,

I want to retrieve a contest from the system,
  identified by its contest year,

so that I can review its details.
```

### A204: Get Contests

```
As the Admin,

I want to retrieve a list of all the contests in the system,
  ordered by contest year,

so that I can see which contests and broadcasts I need to create.
```

### A205: Handle Broadcast Completed

```
As the Admin,

I want a contest to update itself,
  whenever one of its constituent broadcasts is completed,

so that all data for the contest becomes queryable,
  provided the contest is complete.
```

### A206: Handle Broadcast Deleted

```
As the Admin,

I want a contest to update itself,
  whenever one of its constituent broadcasts is deleted,

so that all data for the contest is not queryable,
  until the contest is complete.
```

## A3: Countries

### A301: Create Country

```
As the Admin,

I want to create a new country in the system,
  supplying its country code,
  and its country name,

so that I can reference it in contests and broadcasts.
```

### A302: Delete Country

```
As the Admin,

I want to delete a country from the system,
  identified by its country code,

so that I can re-create it with amended details.
```

### A303: Get Countries

```
As the Admin,

I want to retrieve a list of all the countries in the system,
  ordered by country code,

so that I can see which countries I need to create.
```

### A304: Get Country

```
As the Admin,

I want to retrieve a country from the system,
  identified by its country code,

so that I can review its details.
```

### A305: Handle Contest Created

```
As the Admin,

I want all the countries involved in a newly created contest
  to update themselves,
  removing adding the contest year,

so that it is no longer possible to delete any of the affected countries.
```

### A306: Handle Contest Deleted

```
As the Admin,

I want all the countries involved in a deleted contest
  to update themselves,
  removing the deleted contest year,

so that I may delete countries involved in no contests.
```
