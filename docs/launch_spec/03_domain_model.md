# 3 Domain model

This document is part of the [*Eurocentric* launch specification](README.md).

- [3 Domain model](#3-domain-model)
  - [Enums](#enums)
    - [`BroadcastHalf` enum](#broadcasthalf-enum)
    - [`ContestRoleType` enum](#contestroletype-enum)
    - [`ContestStage` enum](#conteststage-enum)
    - [`CountryType` enum](#countrytype-enum)
    - [`SemiFinalDraw` enum](#semifinaldraw-enum)
    - [`VotingMethod` enum](#votingmethod-enum)
    - [`VotingRules` enum](#votingrules-enum)
  - [`Guid` atomic value objects](#guid-atomic-value-objects)
    - [`BroadcastId` value object](#broadcastid-value-object)
    - [`ContestId` value object](#contestid-value-object)
    - [`CountryId` value object](#countryid-value-object)
  - [`DateOnly` atomic value objects](#dateonly-atomic-value-objects)
    - [`BroadcastDate` value object](#broadcastdate-value-object)
  - [`int` atomic value objects](#int-atomic-value-objects)
    - [`ContestYear` value object](#contestyear-value-object)
    - [`FinishingSpot` value object](#finishingspot-value-object)
    - [`PerformingSpot` value object](#performingspot-value-object)
    - [`PointsValue` value object](#pointsvalue-value-object)
  - [`string` atomic value objects](#string-atomic-value-objects)
    - [`ActName` value object](#actname-value-object)
    - [`CityName` value object](#cityname-value-object)
    - [`CountryCode` value object](#countrycode-value-object)
    - [`CountryName` value object](#countryname-value-object)
    - [`SongTitle` value object](#songtitle-value-object)
  - [Compound value objects](#compound-value-objects)
    - [`BroadcastMemo` value object](#broadcastmemo-value-object)
    - [`ContestRole` value object](#contestrole-value-object)
    - [`PointsAward` value object](#pointsaward-value-object)
  - [`Country` aggregates](#country-aggregates)
    - [`Country` aggregate root entity](#country-aggregate-root-entity)
  - [Contest aggregates](#contest-aggregates)
    - [`GlobalTelevote` owned entity](#globaltelevote-owned-entity)
    - [`Participant` owned entity](#participant-owned-entity)
    - [`Contest` aggregate root entity](#contest-aggregate-root-entity)
  - [Broadcast aggregates](#broadcast-aggregates)
    - [`Televote` owned entity](#televote-owned-entity)
    - [`Jury` owned entity](#jury-owned-entity)
    - [`Competitor` owned entity](#competitor-owned-entity)
    - [`Broadcast` aggregate root entity](#broadcast-aggregate-root-entity)

## Enums

### `BroadcastHalf` enum

- A `BroadcastHalf` enum value is one of `{ First, Second }`.
- It specifies a half in a Broadcast.

### `ContestRoleType` enum

- A `ContestRoleType` enum value is one of `{ Participant, GlobalTelevote }`.
- It specifies a Contest role's type.

### `ContestStage` enum

- A `ContestStage` enum value is one of `{ GrandFinal, SemiFinal1, SemiFinal2 }`.
- It specifies a Broadcast's stage in its parent Contest.

### `CountryType` enum

- A `CountryType` enum value is one of `{ Real, Pseudo }`.
- It specifies a Country's type.

### `SemiFinalDraw` enum

- A `SemiFinalDraw` enum value is one of `{ SemiFinal1, SemiFinal2 }`.
- It specifies the Semi-Final a participant has drawn in their Contest.

### `VotingMethod` enum

- A `VotingMethod` enum value is one of `{ Televote, Jury }`.
- It specifies the voting method used to determine the value of a points award in a Broadcast.

### `VotingRules` enum

- A `VotingRules` enum value is one of `{ TelevoteAndJury, TelevoteOnly }`.
- It specifies the voting rules in a Broadcast.

## `Guid` atomic value objects

### `BroadcastId` value object

- A `BroadcastId` value object is a `(Guid Value)`.
- It identifies a Broadcast aggregate in the system.

### `ContestId` value object

- A `ContestId` value object is a `(Guid Value)`.
- It identifies a Contest aggregate in the system.

### `CountryId` value object

- A `CountryId` value object is a `(Guid Value)`.
- It identifies a Country aggregate in the system.

## `DateOnly` atomic value objects

### `BroadcastDate` value object

- A `BroadcastDate` value object is a `(DateOnly Value)`.
- It represents the date on which a Broadcast is shown.

## `int` atomic value objects

### `ContestYear` value object

- A `ContestYear` value object is an `(int Value)`.
- It represents the year in which a Contest is held.

### `FinishingSpot` value object

- A `FinishingSpot` value object is an `(int Value)`.
- It represents a Competitor's spot in the finishing order of their Broadcast.

### `PerformingSpot` value object

- A `PerformingSpot` value object is an `(int Value)`.
- It represents a Competitor's spot in the performing order of their Broadcast.

### `PointsValue` value object

- A `PointsValue` value object is an `(int Value)`.
- It represents the numeric value of a points award in a Broadcast.

## `string` atomic value objects

### `ActName` value object

- An `ActName` value object is a `(string Value)`.
- It represents an act's performing name.

### `CityName` value object

- A `CityName` value object is a `(string Value)`.
- It represents a city's short UK English name.

### `CountryCode` value object

- A `CountryCode` value object is a `(string Value)`.
- It represents a Country's ISO 3166-1 alpha-2 country code.

### `CountryName` value object

- A `CountryName` value object is a `(string Value)`.
- It represents a Country's short UK English name.

### `SongTitle` value object

- A `SongTitle` value object is a `(string Value)`.
- It represents a song's title.

## Compound value objects

### `BroadcastMemo` value object

- A `BroadcastMemo` value object is a `(BroadcastId BroadcastId, ContestStage ContestStage, bool Completed)`.
- It is a reduced summary of a Broadcast.
- It belongs to a single Contest.

### `ContestRole` value object

- A `ContestRole` value object is a `(ContestId ContestId, ContestRoleType ContestRoleType)`.
- It represents a role in a Contest.
- It belongs to a single Country.

### `PointsAward` value object

- A `PointsAward` value object is a `(CountryId VotingCountryId, PointsValue PointsValue, VotingMethod VotingMethod)`.
- It represents an award of points from a voting Country in a Broadcast.
- It belongs to a single Competitor in a single Broadcast.

## `Country` aggregates

### `Country` aggregate root entity

- A `Country` aggregate root entity represents a single real or pseudo Country.
- It is responsible for tracking the Country's roles in Contests.

The code block below is a sketch of the `Country` class's API:

```csharp
public sealed class Country : AggregateRoot<CountryId>
{
    public CountryCode CountryCode { get; }
    public CountryName CountryName { get; }
    public CountryType CountryType { get; }
    public IReadOnlyList<ContestRole> ContestRoles { get; }

    public void AddContestRole(ContestRole) { }
    public void RemoveContestRole(ContestId) { }

    public static ICountryBuilder Create() { }
}
```

The `ICountryBuilder` interface is a fluent builder for the `Country` class.

## Contest aggregates

### `GlobalTelevote` owned entity

- A `GlobalTelevote` entity represents a single Country acting as a global Televote in a Contest.
- A `Contest` aggregate owns 0 or 1 `GlobalTelevote` entity.
- It is responsible for creating a Televote entity in each of the Contest's child Broadcast aggregates.

The code block below is a sketch of the `GlobalTelevote` class's API:

```csharp
public sealed class GlobalTelevote : Entity
{
    public CountryId VotingCountryId { get; }

    internal Televote CreateTelevote() { }
}
```

### `Participant` owned entity

- A `Participant` entity represents a single Country acting as a participant in a Contest.
- A `Contest` aggregate owns multiple `Participant` entities.
- A `Participant` is identified within its aggregate by its participating `CountryId`.
- It is responsible for creating Competitors, Juries and Televotes in one or more of the Contest's child Broadcast aggregates.

The code block below is a sketch of the `Participant` class's API:

```csharp
public sealed class Participant : Entity
{
    public CountryId ParticipatingCountryId { get; }
    public SemiFinalDraw SemiFinalDraw { get; }
    public ActName ActName { get; }
    public SongTitle SongTitle { get; }

    internal bool MayCompeteIn(ContestStage) { }
    internal bool MustVoteIn(ContestStage) { }
    internal Competitor CreateCompetitor(PerformingSpot) { }
    internal Jury CreateJury() { }
    internal Televote CreateTelevote() { }
}
```

### `Contest` aggregate root entity

- A `Contest` aggregate entity represents a single Contest.
- It is responsible for creating its three child Broadcasts and tracking their status.
- It is also responsible for indicating whether it, and all its associated data, is queryable.

The code block below is a sketch of the `Contest` class's API:

```csharp
public sealed class Contest : AggregateRoot<ContestId>
{
    public ContestYear ContestYear { get; }
    public CityName CityName { get; }
    public VotingRules SemiFinalVotingRules { get; }
    public VotingRules GrandFinalVotingRules { get; }
    public bool Queryable { get; }
    public IReadOnlyList<BroadcastMemo> BroadcastMemos { get; }
    public GlobalTelevote? GlobalTelevote { get; }
    public IReadOnlyList<Participant> Participants { get; }

    public Result<IBroadcastBuilder, DomainError> CreateSemiFinal1ChildBroadcast() { }
    public Result<IBroadcastBuilder, DomainError> CreateSemiFinal2ChildBroadcast() { }
    public Result<IBroadcastBuilder, DomainError> CreateGrandFinalChildBroadcast() { }
    public void AddBroadcastMemo(BroadcastMemo) { }
    public void ReplaceBroadcastMemo(BroadcastMemo) { }
    public void RemoveBroadcastMemo(BroadcastId) { }

    public static IContestBuilder Create() {}
}
```

The `IBroadcastBuilder` interface is a fluent builder for the `Broadcast` class.
The `IContestBuilder` interface is a fluent builder for the `Contest` class.

## Broadcast aggregates

### `Televote` owned entity

- A `Televote` entity represents a single Country voting by Televote in a Broadcast.
- A `Broadcast` aggregate owns 0 or multiple `Televote` entities.
- A `Televote` is identified within its aggregate by its voting `CountryId`.
- It is responsible for awarding a single set of points to the Competitors in the Broadcast.

The code block below is a sketch of the `Televote` class's API:

```csharp
public sealed class Televote : Entity
{
    public CountryId VotingCountryId { get; }
    public bool PointsAwarded { get; }

    internal void GivePointsAwards(IEnumerable<Competitor>) { }
}
```

### `Jury` owned entity

- A `Jury` entity represents a single Country voting by Jury in a Broadcast.
- A `Broadcast` aggregate owns 0 or multiple `Jury` entities.
- A `Jury` is identified within its aggregate by its voting `CountryId`.
- It is responsible for awarding a single set of points to the Competitors in the Broadcast.

The code block below is a sketch of the `Jury` class's API:

```csharp
public sealed class Jury : Entity
{
    public CountryId VotingCountryId { get; }
    public bool PointsAwarded { get; }

    internal void GivePointsAwards(IEnumerable<Competitor>) { }
}
```

### `Competitor` owned entity

- A `Competitor` entity represents a single Country competing in a Broadcast.
- A `Broadcast` aggregate owns multiple `Competitor` entities.
- A `Competitor` is identified within its aggregate by its competing `CountryId`.
- It is responsible for receiving points awards from the Televotes and Juries in the Broadcast.

The code block below is a sketch of the `Competitor` class's API:

```csharp
public sealed class Competitor : Entity
{
    public CountryId CompetingCountryId { get; }
    public PerformingSpot PerformingSpot { get; }
    public BroadcastHalf BroadcastHalf { get; }
    public FinishingSpot FinishingSpot { get; internal set; }
    public IReadOnlyList<PointsAward> PointsAwards { get; }

    public static IComparer<Competitor> FinishingComparer { get; }

    internal void ReceivePointsAward(PointsAward) { }
}
```

### `Broadcast` aggregate root entity

- A `Broadcast` aggregate root entity represents a single Broadcast.
- It is responsible for co-ordinating the points awarded from its Juries and Televotes to its Competitors, and for keeping its Competitors' finishing spots up to date.
- It is also responsible for creating a `BroadcastMemo` of itself.

The code block below is a sketch of the `Broadcast` class's API:

```csharp
public sealed class Broadcast : AggregateRoot<BroadcastId>
{
    public BroadcastDate BroadcastDate { get; }
    public ContestId ParentContestId { get; }
    public ContestStage ContestStage { get; }
    public VotingRules VotingRules { get; }
    public bool Completed { get; }
    public IReadOnlyList<Competitor> Competitors { get; }
    public IReadOnlyList<Televote> Televotes { get; }
    public IReadOnlyList<Jury> Juries { get; }

    public UnitResult<DomainError> AwardTelevotePoints(CountryId, IReadOnlyList<CountryId>) { }
    public UnitResult<DomainError> AwardJuryPoints(CountryId, IReadOnlyList<CountryId>) { }
    public BroadcastMemo ToMemo() { }
}
```
