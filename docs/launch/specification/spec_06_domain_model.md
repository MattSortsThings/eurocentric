# 6. Domain model

This document is part of the [launch specification](../README.md#launch-specification).

- [6. Domain model](#6-domain-model)
  - [Enums](#enums)
    - [*ContestRules* enum](#contestrules-enum)
    - [*ContestRoleType* enum](#contestroletype-enum)
    - [*ContestStage* enum](#conteststage-enum)
    - [*PointsValue* enum](#pointsvalue-enum)
    - [*SemiFinalDraw* enum](#semifinaldraw-enum)
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
    - [*RunningOrderSpot* value object](#runningorderspot-value-object)
    - [*SongTitle* value object](#songtitle-value-object)
  - [Compound value objects](#compound-value-objects)
    - [*ContestRole* value object](#contestrole-value-object)
    - [*JuryAward* value object](#juryaward-value-object)
    - [*TelevoteAward* value object](#televoteaward-value-object)
  - [**COUNTRY** aggregate types](#country-aggregate-types)
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

### *ContestRules* enum

A *ContestRules* value specifies a contest's voting rules.

```cs
public enum ContestRules
{
  Liverpool,
  Stockholm
}
```

### *ContestRoleType* enum

A *ContestRoleType* value specifies a contest role's type.

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
  SemiFinal1,
  SemiFinal2,
  GrandFinal
}
```

### *PointsValue* enum

A *PointsValue* value specifies the numeric value of a points award in a broadcast.

```cs
public enum PointsValue
{
    Zero = 0,
    One = 1,
    Two = 2,
    Three = 3,
    Four = 4,
    Five = 5,
    Six = 6,
    Seven = 7,
    Eight = 8,
    Ten = 10,
    Twelve = 12
}
```

### *SemiFinalDraw* enum

A *SemiFinalDraw* value specifies a contest participant's drawn Semi-Final.

```cs
public enum SemiFinalDraw
{
  SemiFinal1,
  SemiFinal2
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

An *ActName* is a `string` that represents an act's name.

**Invariants:**

1. An *ActName* value is a non-empty string of no more than 200 characters.

### *BroadcastDate* value object

A *BroadcastDate* is a `DateOnly` that represents a broadcast's televised date.

**Invariants:**

1. A *BroadcastDate* value has a year in the range \[2016, 2050\].

### *CityName* value object

A *CityName* is a `string` that represents a city's UK English name.

**Invariants:**

1. A *CityName* value is a non-empty string of no more than 200 characters.

### *ContestYear* value object

A *ContestYear* is an `integer` that represents the year in which a contest is held.

**Invariants:**

1. A *ContestYear* value is in the range \[2016, 2050\].

### *CountryCode* value object

A *CountryCode* is a `string` that represents a country's ISO 3166-1 alpha-2 country code.

**Invariants:**

1. A *CountryCode* value is a string of 2 upper-case letters.

### *CountryName* value object

A *CountryName* is a `string` that represents a country's short UK English name.

**Invariants:**

1. A *CountryName* value is a non-empty string of no more than 200 characters.

### *FinishingPosition* value object

A *FinishingPosition* is an `integer` that represents a competitor's finishing position in a broadcast.

**Invariants:**

1. A *FinishingPosition* value is not less than 1.

### *RunningOrderSpot* value object

A *RunningOrderSpot* is an `integer` that represents a competitor's running order spot in a broadcast.

**Invariants:**

1. A *RunningOrderSpot* value is not less than 1.

### *SongTitle* value object

A *SongTitle* is a `string` that represents a song's name.

**Invariants:**

1. A *SongTitle* value is a non-empty string of no more than 200 characters.

## Compound value objects

### *ContestRole* value object

A *ContestRole* is a (*ContestId*, *ContestRoleType*) tuple that represents a country's role in a contest, where the *ContestId* references the **CONTEST** aggregate.

### *JuryAward* value object

A *JuryAward* is a (*CountryId*, *PointsValue*) tuple that represents a points award from a jury in a broadcast, where the *CountryId* references the voting **COUNTRY** aggregate.

### *TelevoteAward* value object

A *TelevoteAward* is a (*CountryId*, *PointsValue*) tuple that represents a points award from a televote in a broadcast, where the *CountryId* references the voting **COUNTRY** aggregate.

## **COUNTRY** aggregate types

### **COUNTRY** aggregate root entity

A **COUNTRY** represents a single country or pseudo-country.

A **COUNTRY** has:

- a *CountryId*.
- a *CountryCode*.
- a *CountryName*.
- a *ContestRoles* collection (initially empty).

A **COUNTRY** can:

- add a participating *ContestRole*.
- add a voting *ContestRole*.
- remove a *ContestRole*.

**Invariants:**

1. Every **COUNTRY** in the **COUNTRIES_SET** has a unique *CountryId*, which is its system identifier.
2. Every **COUNTRY** in the **COUNTRIES_SET** has a unique *CountryCode*.
3. Each of a **COUNTRY's** *ContestRoles* references a different **CONTEST**.

## **CONTEST** aggregate types

### **ChildBroadcast** entity

A **ChildBroadcast** represents a **BROADCAST** from the perspective of its parent **CONTEST**.

A **ChildBroadcast** has:

- a child *BroadcastId*.
- a *ContestStage*.
- a `Completed` boolean value (initially `false`).

### **Participant** entity

A **Participant** represents a participating **COUNTRY** in a **CONTEST**.

A **Participant** has:

- a participating *CountryId*.
- a *SemiFinalDraw*.
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
- a *ContestRules*.
- a `Queryable` boolean value (initially `false`).
- a **ChildBroadcasts** collection (initially empty).
- a **Participants** collection.
- an optional **GlobalTelevote**.

A **CONTEST** can:

- create a **BROADCAST**.
- add a **ChildBroadcast**.
- remove a **ChildBroadcast**, which may also update its `Queryable` value.
- complete a **ChildBroadcast**, which may also update its `Queryable` value.

**Invariants:**

1. Every **CONTEST** in the **CONTESTS_SET** has a unique *ContestId*, which is its system identifier.
2. Every **CONTEST** in the **CONTESTS_SET** has a unique *ContestYear*.
3. A **CONTEST** is one of the following:
   1. *ContestRules* is `Liverpool` and **GlobalTelevote** is not null.
   2. *ContestRules* is `Stockholm` and **GlobalTelevote** is null.
4. A **CONTEST** has at least 3 **Participants** with a *SemiFinalDraw* of `SemiFinal1`.
5. A **CONTEST** has at least 3 **Participants** with a *SemiFinalDraw* of `SemiFinal2`.
6. Each **Participant** in a **CONTEST**, along with the **GlobalTelevote** if present, references a different **COUNTRY**.
7. Each **ChildBroadcast** in a **CONTEST** references a different **BROADCAST** and mirrors its *ContestStage* and `Completed` values.
8. A **CONTEST** cannot create a **BROADCAST** with a *ContestStage* matching one of its existing **ChildBroadcasts**.
9. A **CONTEST** is `Queryable=true` only if it has 3 **ChildBroadcasts** and they are all `Completed=true`.

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
- a `Completed` boolean value (initially `false`).
- a **Competitors** collection.
- a **Juries** collection.
- a **Televotes** collection.

A **BROADCAST** can:

- award a set of points from a **Jury** to the **Competitors**.
- award a set of points from a **Televote** to the **Competitors**.

**Invariants:**

1. Every **BROADCAST** in the **BROADCASTS_SET** has a unique *BroadcastId*, which is its system identifier.
2. Every **BROADCAST** in the **BROADCASTS_SET** has a unique *BroadcastDate*.
3. Every **BROADCAST** in the **BROADCASTS_SET** has a unique (*ContestId*, *ContestStage*) tuple.
4. A **BROADCAST** has at least 2 **Competitors**.
5. Each **Competitor** in a **BROADCAST** has a different competing *CountryId*, matching the participating *CountryId* of a **Participant** in the parent **CONTEST** eligible to compete in the *ContestStage*.
6. Each **Jury** in a **BROADCAST** has a different voting *CountryId*, matching the participating *CountryId* of a **Participant** in the parent **CONTEST** that awards jury points in the *ContestStage*.
7. Each **Televote** in a **BROADCAST** has a different voting *CountryId*, matching the participating *CountryId* of a **Participant** in the parent **CONTEST** that awards televote points in the *ContestStage*, or the voting *CountryId* of the **GlobalTelevote** if present.
8. A **Jury** cannot award points once it is `PointsAwarded=true`.
9. A **Jury** awards points to each **Competitor** with a competing *CountryId* not equal to its voting *CountryId*.
10. A **Televote** cannot award points once it is `PointsAwarded=true`.
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
