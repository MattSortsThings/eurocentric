# 10: Domain model

This document is part of the [launch specification](../README.md#launch-specification).

- [10: Domain model](#10-domain-model)
  - [Enums](#enums)
    - [*ContestRoleType* enum](#contestroletype-enum)
    - [*ContestStage* enum](#conteststage-enum)
    - [*SemiFinalDraw* enum](#semifinaldraw-enum)
    - [*VotingRules* enum](#votingrules-enum)
  - [Identifier value objects](#identifier-value-objects)
    - [*BroadcastId* value object](#broadcastid-value-object)
    - [*ContestId* value object](#contestid-value-object)
    - [*CountryId* value object](#countryid-value-object)
  - [Atomic value objects](#atomic-value-objects)
    - [*ActName* value object](#actname-value-object)
    - [*BroadcastDate* value object](#broadcastdate-value-object)
    - [*CityName* value object](#cityname-value-object)
    - [*ContestYear* value object](#contestyear-value-object)
    - [*CountryCode* value object](#countrycode-value-object)
    - [*CountryName* value object](#countryname-value-object)
    - [*FinishingPosition* value object](#finishingposition-value-object)
    - [*PointsValue* value object](#pointsvalue-value-object)
    - [*RunningOrderSpot* value object](#runningorderspot-value-object)
    - [*SongTitle* value object](#songtitle-value-object)
  - [Compound value objects](#compound-value-objects)
    - [*ContestRole* value object](#contestrole-value-object)
    - [*JuryAward* value object](#juryaward-value-object)
    - [*TelevoteAward* value object](#televoteaward-value-object)
  - [**COUNTRY** aggregate root entity](#country-aggregate-root-entity)
  - [**CONTEST** aggregate types](#contest-aggregate-types)
    - [**ChildBroadcast** entity](#childbroadcast-entity)
    - [**Participant** entity](#participant-entity)
    - [**GlobalTelevote** entity](#globaltelevote-entity)
    - [**CONTEST** aggregate root entity](#contest-aggregate-root-entity)
  - [**BROADCAST** aggregate types](#broadcast-aggregate-types)
    - [**Competitor** entity](#competitor-entity)
    - [**Jury** entity](#jury-entity)
    - [**Televote** entity](#televote-entity)
    - [**BROADCAST** aggregate root entity](#broadcast-aggregate-root-entity)
  - [Transactions](#transactions)

## Enums

### *ContestRoleType* enum

A *ContestRoleType* value specifies the country's role in a contest.

```cs
public enum ContestRoleType
{
  Participant,
  GlobalTelevote
}
```

### *ContestStage* enum

A *ContestStage* value specifies a broadcast's stage in its parent contest.

```cs
public enum ContestStage
{
  GrandFinal,
  SemiFinal1,
  SemiFinal2
}
```

### *SemiFinalDraw* enum

A *SemiFinalDraw* value specifies the Semi-Final a participant has drawn in its contest.

```cs
public enum SemiFinalDraw
{
    First,
    Second
}
```

### *VotingRules* enum

A *VotingRules* value specifies the voting rules in a broadcast.

```cs
public enum VotingRules
{
    TelevoteAndJury,
    TelevoteOnly
}
```

## Identifier value objects

### *BroadcastId* value object

A *BroadcastId* is a `Guid` that identifies a **BROADCAST** aggregate in the system.

### *ContestId* value object

A *ContestId* is a `Guid` that identifies a **CONTEST** aggregate in the system.

### *CountryId* value object

A *CountryId* is a `Guid` that identifies a **COUNTRY** aggregate in the system.

## Atomic value objects

### *ActName* value object

An *ActName* is a string that represents an act's name.

**Invariants:**

1. An *ActName* value is a non-empty, non-whitespace string of no more than 200 characters.

### *BroadcastDate* value object

A *BroadcastDate* is a `DateOnly` that represents a broadcast's televised date.

**Invariants:**

1. A *BroadcastDate* value has a year in the range \[2016, 2050\].

### *CityName* value object

A *CityName* is a string that represents a city's UK English name.

**Invariants:**

1. A *CityName* value is a non-empty, non-whitespace string of no more than 200 characters.

### *ContestYear* value object

A *ContestYear* is an integer that represents the year in which a contest is held.

**Invariants:**

1. A *ContestYear* value is in the range \[2016, 2050\].

### *CountryCode* value object

A *CountryCode* is a string that represents a country's ISO 3166-1 alpha-2 country code.

**Invariants:**

1. A *CountryCode* value is a string of 2 upper-case letters.

### *CountryName* value object

A *CountryName* is a string that represents a country's short UK English name.

**Invariants:**

1. A *CountryName* value is a non-empty, non-whitespace string of no more than 200 characters.

### *FinishingPosition* value object

A *FinishingPosition* is an integer that represents a competitor's finishing position in their broadcast.

**Invariants:**

1. A *FinishingPosition* value is not less than 1.

### *PointsValue* value object

A *PointsValue* is an integer that represents the value of a single points award in a broadcast.

**Invariants:**

1. A *PointsValue* value is a non-negative integer.
2. The only values used in the domain are \{0, 1, 2, 3, 4, 5, 6, 7, 8, 10, 12\}.

### *RunningOrderSpot* value object

A *RunningOrderSpot* is an integer that represents a competitor's running order spot in their broadcast.

**Invariants:**

1. A *RunningOrderSpot* value is not less than 1.

### *SongTitle* value object

A *SongTitle* is a string that represents a song's name.

**Invariants:**

1. A *SongTitle* value is a non-empty, non-whitespace string of no more than 200 characters.

## Compound value objects

### *ContestRole* value object

A *ContestRole* is a (*ContestId*, *ContestRoleType*) tuple that represents a country's role in a contest, where the *ContestId* references the **CONTEST** aggregate.

### *JuryAward* value object

A *JuryAward* is a (*CountryId*, *PointsValue*) tuple that represents a points award from a jury in a broadcast, where the *CountryId* references the voting **COUNTRY** aggregate.

### *TelevoteAward* value object

A *TelevoteAward* is a (*CountryId*, *PointsValue*) tuple that represents a points award from a televote in a broadcast, where the *CountryId* references the voting **COUNTRY** aggregate.

## **COUNTRY** aggregate root entity

A **COUNTRY** represents a single country or pseudo-country.

A **COUNTRY** has:

- a *CountryId*.
- a *CountryCode*.
- a *CountryName*.
- a *ContestRoles* collection (initially empty).

A **COUNTRY** can:

- add a *ContestRole*.
- remove a *ContestRole*.

**Invariants:**

1. Every **COUNTRY** has a unique *CountryId*, which is its system identifier.
2. Every **COUNTRY** has a unique *CountryCode*.
3. Each of a **COUNTRY's** *ContestRoles* references a different **CONTEST**.
4. A **COUNTRY** with one or more *ContestRoles* cannot be deleted.

## **CONTEST** aggregate types

### **ChildBroadcast** entity

A **ChildBroadcast** represents a **BROADCAST** from the perspective of its parent **CONTEST**.

A **ChildBroadcast** has:

- a child *BroadcastId*.
- a *ContestStage* value.
- a `Completed` boolean value (initially `false`).

### **Participant** entity

A **Participant** represents a participating **COUNTRY** in a **CONTEST**.

A **Participant** has:

- a participating *CountryId*.
- a *SemiFinalDraw* value.
- an *ActName*.
- a *SongTitle*.

### **GlobalTelevote** entity

A **GlobalTelevote** represents a global televote voting **COUNTRY** in a **CONTEST**.

A **GlobalTelevote** has:

- a voting *CountryId*.

### **CONTEST** aggregate root entity

A **CONTEST** represents a single year's edition of the Eurovision Song Contest.

A **CONTEST** has:

- a *ContestId*.
- a *ContestYear*.
- a *CityName*.
- a Grand Final *VotingRules* value (always `TelevoteAndJury`).
- a Semi-Final *VotingRules* value.
- a `Queryable` boolean value (initially `false`).
- a **ChildBroadcasts** collection (initially empty).
- a **Participants** collection.
- an optional **GlobalTelevote**.

A **CONTEST** can:

- create a **BROADCAST**.
- add a **ChildBroadcast**.
- remove a **ChildBroadcast**, which may also update its own `Queryable` value.
- set a **ChildBroadcast** as `Completed=true`, which may also update its own `Queryable` value.

**Invariants:**

1. Every **CONTEST** has a unique *ContestId*, which is its system identifier.
2. Every **CONTEST** has a unique *ContestYear*.
3. A **CONTEST** has at least 3 **Participants** with *SemiFinalDraw* = `First`.
4. A **CONTEST** has at least 3 **Participants** with *SemiFinalDraw* = `Second`.
5. Each **Participant** in a **CONTEST**, along with the **GlobalTelevote** if present, references a different existing **COUNTRY**.
6. Each **ChildBroadcast** in a **CONTEST** references a different existing **BROADCAST** and mirrors its *ContestStage* and `Completed` values.
7. A **CONTEST** cannot create a **BROADCAST** with a *ContestStage* matching one of its existing **ChildBroadcasts**.
8. A **CONTEST** is `Queryable=true` only if it has 3 **ChildBroadcasts** and they are all `Completed=true`.
9. A **CONTEST** with one or more **ChildBroadcasts** cannot be deleted.

## **BROADCAST** aggregate types

### **Competitor** entity

A **Competitor** represents a competing **COUNTRY** in a **BROADCAST**.

A **Competitor** has:

- a competing *CountryId*.
- a *RunningOrderSpot*.
- a *FinishingPosition*.
- a *JuryAwards* collection (initially empty).
- a *TelevoteAwards* collection (initially empty).

### **Jury** entity

A **Jury** represents a voting **COUNTRY** that awards a set of jury points in a **BROADCAST**.

A **Jury** has:

- a voting *CountryId*.
- a `PointsAwarded` boolean value (initially `false`).

### **Televote** entity

A **Televote** represents a voting **COUNTRY** that awards a set of jury points in a **BROADCAST**.

A **Televote** has:

- a voting *CountryId*.
- a `PointsAwarded` boolean value (initially `false`).

### **BROADCAST** aggregate root entity

A **BROADCAST** represents a single broadcast stage of a contest.

A **BROADCAST** has:

- a *BroadcastId*.
- a *BroadcastDate*.
- a parent *ContestId*.
- a *ContestStage*.
- a *VotingRules* value.
- a `Completed` boolean value (initially `false`).
- a **Competitors** collection.
- a **Juries** collection.
- a **Televotes** collection.

A **BROADCAST** can:

- award a set of points from a **Jury** to the **Competitors**.
- award a set of points from a **Televote** to the **Competitors**.

**Invariants:**

1. Every **BROADCAST** has a unique *BroadcastId*, which is its system identifier.
2. Every **BROADCAST** has a unique *BroadcastDate*.
3. Every **BROADCAST** has a unique (*ContestId*, *ContestStage*) tuple.
4. A **BROADCAST** has at least 2 **Competitors**.
5. Each **Competitor** in a **BROADCAST** has a different competing *CountryId*, matching the participating *CountryId* of a **Participant** in the parent **CONTEST** eligible to compete in the *ContestStage*.
6. Each **Jury** in a **BROADCAST** has a different voting *CountryId*, matching the participating *CountryId* of a **Participant** in the parent **CONTEST** required to award jury points in the *ContestStage*.
7. Each **Televote** in a **BROADCAST** has a different voting *CountryId*, matching the participating *CountryId* of a **Participant** in the parent **CONTEST** required to award televote points in the *ContestStage*, or the voting *CountryId* of the **GlobalTelevote** if present.
8. A **Jury** cannot award points when it is `PointsAwarded=true`.
9. A **Jury** awards points to each **Competitor** with a competing *CountryId* not equal to its voting *CountryId*.
10. A **Televote** cannot award points when it is `PointsAwarded=true`.
11. A **Televote** awards points to each **Competitor** with a competing *CountryId* not equal to its voting *CountryId*.
12. A **BROADCAST** is `Completed=true` once all of its **Juries** and **Televotes** are `PointsAwarded=true`.

## Transactions

The domain model has 8 transactions that change the system state.

| Transaction                                     | Effect                | Side effects                                            |
|:------------------------------------------------|:----------------------|:--------------------------------------------------------|
| Creating a **COUNTRY**                          | **COUNTRY** created   | None                                                    |
| Deleting a **COUNTRY**                          | **COUNTRY** deleted   | None                                                    |
| Creating a **CONTEST**                          | **CONTEST** created   | Participating/voting **COUNTRIES** updated              |
| Deleting a **CONTEST**                          | **CONTEST** deleted   | Participating/voting **COUNTRIES** updated              |
| Creating a **BROADCAST** for a **CONTEST**      | **BROADCAST** created | Parent **CONTEST** updated                              |
| Awarding **Jury** points in a **BROADCAST**     | **BROADCAST** updated | Parent **CONTEST** updated *if* **BROADCAST** completed |
| Awarding **Televote** points in a **BROADCAST** | **BROADCAST** updated | Parent **CONTEST** updated *if* **BROADCAST** completed |
| Deleting a **BROADCAST**                        | **BROADCAST** deleted | Parent **CONTEST** updated                              |
