# Domain model

This document is part of the *Eurocentric* [launch specification](README.md).

- [Domain model](#domain-model)
  - [Enums](#enums)
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
    - [`FinishingPosition` value object](#finishingposition-value-object)
    - [`PointsValue` value object](#pointsvalue-value-object)
    - [`RunningOrderPosition` value object](#runningorderposition-value-object)
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

### `ContestRoleType` enum

- A `ContestRoleType` enum value is one of `{ Participant, GlobalTelevote }`.
- It specifies a contest role's type.

### `ContestStage` enum

- A `ContestStage` enum value is one of `{ GrandFinal, SemiFinal1, SemiFinal2 }`.
- It specifies a broadcast's stage in its parent contest.

### `CountryType` enum

- A `CountryType` enum value is one of `{ Real, Pseudo }`.
- It specifies a country's type.

### `SemiFinalDraw` enum

- A `SemiFinalDraw` enum value is one of `{ SemiFinal1, SemiFinal2 }`.
- It specifies the Semi-Final a participant has drawn in their contest.

### `VotingMethod` enum

- A `VotingMethod` enum value is one of `{ Televote, Jury }`.
- It specifies the voting method used to determine the value of a points award in a broadcast.

### `VotingRules` enum

- A `VotingRules` enum value is one of `{ TelevoteAndJury, TelevoteOnly }`.
- It specifies the voting rules in a broadcast.

## `Guid` atomic value objects

### `BroadcastId` value object

- A `BroadcastId` value object is a `(Guid Value)`.
- It identifies a broadcast aggregate in the system.

### `ContestId` value object

- A `ContestId` value object is a `(Guid Value)`.
- It identifies a contest aggregate in the system.

### `CountryId` value object

- A `CountryId` value object is a `(Guid Value)`.
- It identifies a country aggregate in the system.

## `DateOnly` atomic value objects

### `BroadcastDate` value object

- A `BroadcastDate` value object is a `(DateOnly Value)`.
- It represents the date on which a broadcast is shown.

## `int` atomic value objects

### `ContestYear` value object

- A `ContestYear` value object is an `(int Value)`.
- It represents the year in which a contest is held.

### `FinishingPosition` value object

- A `FinishingPosition` value object is an `(int Value)`.
- It represents a competitor's finishing position in their broadcast.

### `PointsValue` value object

- A `PointsValue` value object is an `(int Value)`.
- It represents the numeric value of a points award in a broadcast.

### `RunningOrderPosition` value object

- A `RunningOrderPosition` value object is an `(int Value)`.
- It represents a competitor's position in the running order of their broadcast.

## `string` atomic value objects

### `ActName` value object

- An `ActName` value object is a `(string Value)`.
- It represents an act's performing name.

### `CityName` value object

- A `CityName` value object is a `(string Value)`.
- It represents a city's short UK English name.

### `CountryCode` value object

- A `CountryCode` value object is a `(string Value)`.
- It represents a country's ISO 3166-1 alpha-2 country code.

### `CountryName` value object

- A `CountryName` value object is a `(string Value)`.
- It represents a country's short UK English name.

### `SongTitle` value object

- A `SongTitle` value object is a `(string Value)`.
- It represents a song's title.

## Compound value objects

### `BroadcastMemo` value object

- A `BroadcastMemo` value object is a `(BroadcastId BroadcastId, ContestStage ContestStage, bool Completed)`.
- It represents a broadcast from the perspective of its parent contest.
- It belongs to a single contest.

### `ContestRole` value object

- A `ContestRole` value object is a `(ContestId ContestId, ContestRoleType ContestRoleType)`.
- It represents a role in a contest.
- It belongs to a single country.

### `PointsAward` value object

- A `PointsAward` value object is a `(CountryId VotingCountryId, PointsValue PointsValue, VotingMethod VotingMethod)`.
- It represents an award of points from a voting country in a broadcast.
- It belongs to a single competitor in a single broadcast.

## `Country` aggregates

### `Country` aggregate root entity

- A `Country` aggregate root entity represents a single real or pseudo country.
- It is responsible for tracking the country's roles in contests.

The code block below is a sketch of the `Country` class's API:

```csharp
public sealed class Country : AggregateRoot<CountryId>
{
    public CountryCode CountryCode { get; }
    public CountryName CountryName { get; }
    public CountryType CountryType { get; }
    public IReadOnlyList<ContestRole> ContestRoles { get; }

    public void AddParticipantContestRole(ContestId) { }
    public void AddGlobalTelevoteContestRole(ContestId) { }
    public void RemoveContestRole(ContestId) { }
}
```

## Contest aggregates

### `GlobalTelevote` owned entity

- A `GlobalTelevote` entity represents a single country acting as a global televote in a contest.
- A `Contest` aggregate owns 0 or 1 `GlobalTelevote` entity.
- It is responsible for creating a televote entity in each of the contest's child broadcast aggregates.

The code block below is a sketch of the `GlobalTelevote` class's API:

```csharp
public sealed class GlobalTelevote : Entity
{
    public CountryId VotingCountryId { get; }

    internal Televote CreateTelevote() { }
}
```

### `Participant` owned entity

- A `Participant` entity represents a single country acting as a participant in a contest.
- A `Contest` aggregate owns multiple `Participant` entities.
- A `Participant` is identified within its aggregate by its participating `CountryId`.
- It is responsible for creating competitors, juries and televotes in one or more of the contest's child broadcast aggregates.

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
    internal Competitor CreateCompetitor(RunningOrderPosition) { }
    internal Jury CreateJury() { }
    internal Televote CreateTelevote() { }
}
```

### `Contest` aggregate root entity

- A `Contest` aggregate entry represents a single contest.
- It is responsible for creating its three child broadcasts and tracking their status.

The code block below is a sketch of the `Contest` class's API:

```csharp
public sealed class Contest : AggregateRoot<ContestId>
{
    public ContestYear ContestYear { get; }
    public CityName CityName { get; }
    public VotingRules SemiFinalVotingRules { get; }
    public VotingRules GrandFinalVotingRules { get; }
    public bool Completed { get; }
    public GlobalTelevote? GlobalTelevote { get; }
    public IReadOnlyList<BroadcastMemo> BroadcastMemos { get; }
    public IReadOnlyList<Participant> Participants { get; }

    public Result<IBroadcastBuilder, DomainError> CreateBroadcast() { }
    public void AddBroadcastMemo(BroadcastMemo) { }
    public void ReplaceBroadcastMemo(BroadcastMemo) { }
    public void RemoveBroadcastMemo(BroadcastId) { }
}
```

## Broadcast aggregates

### `Televote` owned entity

- A `Televote` entity represents a single country voting by televote in a broadcast.
- A `Broadcast` aggregate owns 0 or multiple `Televote` entities.
- A `Televote` is identified within its aggregate by its voting `CountryId`.
- It is responsible for awarding a single set of points to the competitors in the broadcast.

The code block below is a sketch of the `Televote` class's API:

```csharp
public sealed class Televote : Entity
{
    public CountryId VotingCountryId { get; }
    public bool PointsAwarded { get; }

    internal void AwardPoints(IEnumerable<Competitor>) { }
}
```

### `Jury` owned entity

- A `Jury` entity represents a single country voting by jury in a broadcast.
- A `Broadcast` aggregate owns 0 or multiple `Jury` entities.
- A `Jury` is identified within its aggregate by its voting `CountryId`.
- It is responsible for awarding a single set of points to the competitors in the broadcast.

The code block below is a sketch of the `Jury` class's API:

```csharp
public sealed class Jury : Entity
{
    public CountryId VotingCountryId { get; }
    public bool PointsAwarded { get; }

    internal void AwardPoints(IEnumerable<Competitor>) { }
}
```

### `Competitor` owned entity

- A `Competitor` entity represents a single country competing in a broadcast.
- A `Broadcast` aggregate owns multiple `Competitor` entities.
- A `Competitor` is identified within its aggregate by its competing `CountryId`.
- It is responsible for receiving points awards from the televotes and juries in the broadcast.

The code block below is a sketch of the `Competitor` class's API:

```csharp
public sealed class Competitor : Entity
{
    public CountryId CompetingCountryId { get; }
    public RunningOrderPosition RunningOrderPosition { get; }
    public FinishingPosition FinishingPosition { get; internal set; }
    public IReadOnlyList<PointsAward> PointsAwards { get; }

    public static IComparer<Competitor> FinishingComparer { get; }

    internal void ReceiveAward(PointsAward) { }
}
```

### `Broadcast` aggregate root entity

- A `Broadcast` aggregate root entity represents a single broadcast.
- It is responsible for co-ordinating the points awarded from its juries and televotes to its competitors, and for keeping its competitors' finishing positions up to date.

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
}
```
