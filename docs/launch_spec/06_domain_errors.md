# 6. Domain errors

This document is part of the [launch specification](README.md).

- [6. Domain errors](#6-domain-errors)
  - [The `DomainError` type](#the-domainerror-type)
  - [`NotFound` domain errors](#notfound-domain-errors)
  - [`Extrinsic` domain errors](#extrinsic-domain-errors)
  - [`Intrinsic` domain errors](#intrinsic-domain-errors)

## The `DomainError` type

```csharp
public sealed record DomainError
{
  public required string Title { get; init; }

  public required DomainErrorType Type { get; init; }

  public required string Description { get; init; }

  public IReadOnlyDictionary<string, object>? AdditionalData { get; init; }
}
```

The `DomainErrorType` enum is described in the [domain model](./03_domain_model.md#domainerrortype-enum).

## `NotFound` domain errors

A `NotFound` domain error occurs when the transaction operates on an aggregate that does not exist.

The following `NotFound` domain errors are defined for domain aggregate transactions:

| Title               | Client tries...                                             |
|:--------------------|:------------------------------------------------------------|
| Broadcast Not Found | To get, update or delete a non-existent broadcast aggregate |
| Contest Not Found   | To get, update or delete a non-existent contest aggregate   |
| Country Not Found   | To get, update or delete a non-existent country aggregate   |

## `Extrinsic` domain errors

An `Extrinsic` domain error occurs when the transaction violates a domain invariant given the current state of all the aggregates.

The following `Extrinsic` domain errors are defined for domain aggregate transactions:

| Title                                         | Client tries...                                                                                                                             |
|:----------------------------------------------|:--------------------------------------------------------------------------------------------------------------------------------------------|
| Broadcast Date Duplicated                     | To create a broadcast with the same broadcast date as an existing broadcast                                                                 |
| Broadcast Jury Not Found                      | To award points in a broadcast with a voting country ID that matches no jury                                                                |
| Broadcast Jury Points Already Awarded         | To award points in a broadcast with a voting country ID that matches a jury that has already awarded its points                             |
| Broadcast Ranked Competitor Not Found         | To award points in a broadcast with a set of ranked competing country IDs that includes an item that matches no competitor in the broadcast |
| Broadcast Competitor Not Ranked               | To award points in a broadcast with a set of ranked competing country IDs that omits a competitor in the broadcast                          |
| Broadcast Televote Not Found                  | To award points in a broadcast with a voting country ID that matches no televote                                                            |
| Broadcast Televote Points Already Awarded     | To award points in a broadcast with a voting country ID that matches a televote that has already awarded its points                         |
| Contest Child Broadcast Competitor Ineligible | To create a child broadcast with a competing country ID that matches a participant in the contest ineligible for the contest stage          |
| Contest Child Broadcast Competitor Not Found  | To create a child broadcast with a competing country ID that does not match a participant in the contest                                    |
| Contest Child Broadcast Date Out Of Range     | To create a child broadcast with a broadcast date outside the contest's year                                                                |
| Contest Child Broadcast Stage Duplicated      | To create a child broadcast with the same contest stage as one of the contest's existing child broadcasts                                   |
| Contest Deletion Disallowed                   | To delete a contest with one or more child broadcasts                                                                                       |
| Contest Participating Country Ineligible      | To create a contest with a participant having a participant country ID that matches an existing country with `CountryType != Real`          |
| Contest Voting Country Ineligible             | To create a contest with a global televote voting country ID that matches an existing country with `CountryType != Pseudo`                  |
| Contest Year Duplicated                       | To create a contest with the same contest year as an existing contest                                                                       |
| Country Code Duplicated                       | To create a country with the same country code as an existing country                                                                       |
| Country Deletion Disallowed                   | To delete a country with one or more active contest IDs                                                                                     |

## `Intrinsic` domain errors

An `Intrinsic` domain error occurs when the transaction in itself violates a domain invariant, irrespective of the current state of any aggregates.

The following `Intrinsic` domain errors are defined for domain value object instantiation:

| Title                      | Client tries...                                                   |
|:---------------------------|:------------------------------------------------------------------|
| Illegal Act Name           | To create an act name object with an illegal value                |
| Illegal Broadcast Date     | To create a broadcast date object with an illegal value           |
| Illegal City Name          | To create a city name object with an illegal value                |
| Illegal Country Code       | To create a country code object with an illegal value             |
| Illegal Country Name       | To create a country name object with an illegal value             |
| Illegal Contest Year       | To create a contest year object with an illegal value             |
| Illegal Contest Year Range | To create a contest year range object with an illegal value range |
| Illegal Page Index         | To create a page index object with an illegal value               |
| Illegal Page Size          | To create a page size object with an illegal value                |
| Illegal Performing Spot    | To create a performing spot object with an illegal value          |
| Illegal Points Value       | To create a points value object with an illegal value             |
| Illegal Points Value Range | To create a points value range object with an illegal value range |
| Illegal Song Title         | To create a song title object with an illegal value               |

The following `Intrinsic` domain errors are defined for domain aggregate transactions:

| Title                                        | Client tries...                                                                                                                 |
|:---------------------------------------------|:--------------------------------------------------------------------------------------------------------------------------------|
| Illegal Broadcast Competitor Count           | To create a broadcast with fewer than 3 competitors                                                                             |
| Illegal Broadcast Half Boundaries            | To create a broadcast with the first performing spot after the interval being performing spot 1 or outside the performing order |
| Duplicated Broadcast Ranked Competitors      | To award points in a broadcast with a list of competing country IDs that contains duplicates                                    |
| Broadcast Voter As Ranked Competitor         | To award points in a broadcast with a list of competing country IDs that includes the voting country ID                         |
| Duplicated Contest Participating Countries   | To create a contest with multiple participants having the same participant country ID                                           |
| Illegal Contest Semi-Final Draw Distribution | To create a contest with fewer than 3 participants assigned Semi-Final 1 and/or fewer than 3 participants assigned Semi-Final 2 |
