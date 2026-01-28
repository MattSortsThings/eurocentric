# 2. Real world context

This document is part of the [*Eurocentric* launch specification](README.md).

- [2. Real world context](#2-real-world-context)
  - [What is the Eurovision Song Contest?](#what-is-the-eurovision-song-contest)
  - [Project scope](#project-scope)
  - [Contest structure](#contest-structure)
    - [Contest stages](#contest-stages)
    - [The Semi-Final draw](#the-semi-final-draw)
    - [Qualification](#qualification)
    - [Contest stage voting formats](#contest-stage-voting-formats)
  - [Broadcast structure](#broadcast-structure)
    - [How a Televote or Jury awards points](#how-a-televote-or-jury-awards-points)
    - [Determining the result](#determining-the-result)
  - [Unusual events](#unusual-events)
    - [A Participant withdraws from a Contest before it starts](#a-participant-withdraws-from-a-contest-before-it-starts)
    - [A Competitor is disqualified from a Broadcast](#a-competitor-is-disqualified-from-a-broadcast)
    - [Substitute Televote points are used](#substitute-televote-points-are-used)
    - [Substitute Jury points are used](#substitute-jury-points-are-used)
  - [Images from the Eurovision official website](#images-from-the-eurovision-official-website)

## What is the Eurovision Song Contest?

| ![Logo comprising the text "Eurovision Song Contest Basel 2025", with "V" in "Eurovision" rendered as a heart containing the Swiss flag, on a black background, surrounded by pink and blue hearts.](media/ESC-2025-logo.jpg) |
|:-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------:|
|                                                                         The logo for the 2025 Eurovision Song Contest in Basel, Switzerland (© EBU).                                                                          |

The Eurovision Song Contest is an annual televised song contest between national broadcasters, organized by the European Broadcasting Union (EBU).

A Contest has 40&pm;5 Participants, each of which is an act with a song representing a participating Country. From 2023 onwards, a Contest also has a Global Televote representing viewers who do not reside in a participating Country.

A Contest is made up of 3 Broadcasts: two Semi-Finals and a Grand Final.

A Broadcast has multiple Competitors, each of which represents a competing Country, as well as Televotes and Juries, each of which represents a voting Country.

The Televotes and Juries award points to the Competitors to determine the finishing order in the Broadcast.

The competing Country finishing in 1st place in the Grand Final Broadcast is the overall winner of the Contest and customarily hosts the event the following year.

For example:

> The 2025 Eurovision Song Contest was held in Basel, Switzerland. There were 37 participating Countries, plus an additional "Rest of the World" Televote. The winner was the singer JJ with the song "Wasted Love", representing the Austrian national broadcaster ORF. The 2026 Eurovision Song Contest was announced as to be held in Vienna, Austria.

## Project scope

The *Eurocentric* project is restricted to editions of the Eurovision Song Contest from 2016 to the present (2026 at time of writing). It is not concerned with the cancelled 2020 edition.

## Contest structure

### Contest stages

A Contest's three stages, in Broadcast order, are:

| Contest stage | Competitors |
|:--------------|------------:|
| Semi-Final 1  |       15-18 |
| Semi-Final 2  |       15-18 |
| Grand Final   |       25-26 |

**In *Eurocentric*:**

- A Broadcast has 3 or more Competitors.
- Therefore, a Contest has 6 or more Participants.

### The Semi-Final draw

Before the start of the Contest, each Participant is randomly drawn into one of two Semi-Final groups.

- Participants who draw Semi-Final 1 *must* vote in and *may* compete in the Semi-Final 1 Broadcast.
- Participants who draw Semi-Final 2 *must* vote in and *may* compete in the Semi-Final 2 Broadcast.
- All Participants must vote in and may compete in the Grand Final Broadcast.

### Qualification

Each Semi-Final group contains 2-3 Participants who automatically qualify for the Grand Final. These Participants do not compete in their allocated Semi-Final, but still vote.

The remaining Participants in each group must compete in their allocated Semi-Final. The top 10 finishers in the Semi-Final, along with the automatic qualifiers, go on to compete in the Grand Final.

**In *Eurocentric*:**

- Qualification is not explicitly modelled

### Contest stage voting formats

From **2016 to 2022**:

- Every Participant that has drawn Semi-Final 1 in the contest has a Televote and a Jury in the Semi-Final 1 Broadcast.
- Every Participant that has drawn Semi-Final 2 in the contest has a Televote and a Jury in the Semi-Final 2 Broadcast.
- Every Participant has a Televote and a Jury in the Grand Final Broadcast.

From **2023 to 2025**:

- Every Participant that has drawn Semi-Final 1 in the contest has a Televote (and no Jury) in the Semi-Final 1 Broadcast.
- Every Participant that has drawn Semi-Final 2 in the contest has a Televote (and no Jury) in the Semi-Final 2 Broadcast.
- Every Participant has a Televote and a Jury in the Grand Final Broadcast.
- The Contest has a Global Televote representing the "Rest of the World" Pseudo Country.
- The Global Televote has a Televote in all three Broadcasts.

From **2026**:

- Every Participant that has drawn Semi-Final 1 in the contest has a Televote and a Jury in the Semi-Final 1 Broadcast.
- Every Participant that has drawn Semi-Final 2 in the contest has a Televote and a Jury in the Semi-Final 2 Broadcast.
- Every Participant has a Televote and a Jury in the Grand Final Broadcast.
- The Contest has a Global Televote representing the "Rest of the World" Pseudo Country.
- The Global Televote has a Televote in all three Broadcasts.

## Broadcast structure

A Broadcast is split into a first half and a second half. Each Competitor is assigned a performing spot in one of the two halves. After all the Competitors have performed, the Televotes and Juries award their points to the Competitors. A Competitor's points total determines its finishing spot.

### How a Televote or Jury awards points

A voting Country's Televote or Jury ranks all the Competitors from first place to last place, excluding any Competitor whose competing Country is the same as the voting Country.

The ranking determines the value of the points award the Televote/Jury gives to the Competitor. The highest ranked Competitors receive the following points values: 12, 10, 8, 7, 6, 5, 4, 3, 2, 1. All other Competitors receive 0 points.

For example, these are the points awarded by the Icelandic Televote in the 2025 Semi-Final 1 Broadcast, in which Iceland competed and voted:

| Competing Country | Points Value |
|:------------------|-------------:|
| Sweden            |           12 |
| Norway            |           10 |
| Netherlands       |            8 |
| Poland            |            7 |
| Estonia           |            6 |
| Belgium           |            5 |
| Ukraine           |            4 |
| San Marino        |            3 |
| Albania           |            2 |
| Portugal          |            1 |
| Slovenia          |            0 |
| Azerbaijan        |            0 |
| Croatia           |            0 |
| Cyprus            |            0 |
| Iceland           |          n/a |

### Determining the result

The Competitors in a broadcast are ranked in a finishing order based on descending total points. Ties are not permitted. The following tie-break rules are used in order:

1. If two Competitors are tied on total points, the Competitor with more Televote points wins the tie.
2. If they are still tied, the Competitor with more non-zero points Televote awards wins the tie.
3. If they are still tied, a "count-back" is used: the Competitor that received more 12-points Televote awards wins the tie, then 10-points Televote awards, and so on down to 1-point Televote awards.
4. If they are still tied, the Competitor with the earlier performing spot wins the tie.

## Unusual events

This section lists some unusual events that have occurred at the Eurovision Song Contest between 2016 and the present.

### A Participant withdraws from a Contest before it starts

Moldova withdrew from the Basel 2025 Contest before they had selected their Act. Romania was disqualified from the Stockholm 2016 Contest after their Act was selected but before the Contest began.

**In *Eurocentric*:**

- A withdrawn Participant is disregarded when creating a Contest.

### A Competitor is disqualified from a Broadcast

The Netherlands were assigned performing spot 5 in the Malmö 2024 Contest Grand Final, but they were then disqualified, leaving performing spot 5 vacant in the Grand Final Broadcast.

**In *Eurocentric*:**

- A disqualified Competitor is disregarded when creating a Broadcast, but the vacant performing spot is retained.

### Substitute Televote points are used

San Marino's Televote points are usually determined by taking a statistical average of the points awarded by Italy and other neighbouring countries.

**In *Eurocentric*:**

- Televote points awards are taken at face value.

### Substitute Jury points are used

In the Turin 2022 Grand Final Broadcast, six Juries were disqualified and statistical average Jury points were given in their place.

**In *Eurocentric*:**

- Jury points awards are taken at face value.

## Images from the Eurovision official website

| ![Brightly lit area, with spectators seated around central stage with proscenium arch.](media/ESC-2025-arena.jpg) |
|:-----------------------------------------------------------------------------------------------------------------:|
|                   *The Eurovision 2025 stage in St. Jakobshalle, Basel (© EBU/Alma Bengtsson).*                   |

| ![Performers standing in a gallery overlooking a concert arena, being photographed by journalists.](media/ESC-2025-sf2-green-room.jpg) |
|:--------------------------------------------------------------------------------------------------------------------------------------:|
|     *The qualifying acts representing Austria, Latvia and Malta at the Eurovision 2025 Semi-Final 2 (© EBU/Sarah Louise Bennett).*     |

| ![Male singer standing on a stage lit in red and white, holding an Austrian flag, with the name of the country Austria behind him.](media/ESC-2025-jj-flag-parade.jpg) |
|:----------------------------------------------------------------------------------------------------------------------------------------------------------------------:|
|                       *JJ from Austria participating in the flag parade at the start of the Eurovision 2025 Grand Final (© EBU/Alma Bengtsson).*                       |

| ![Black and white image of male solo singer performing in a stage modelled to look like a small boat.](media/ESC-2025-jj-performance.jpg) |
|:-----------------------------------------------------------------------------------------------------------------------------------------:|
|                   *JJ from Austria performing "Wasted Love" in the Eurovision 2025 Grand Final (© EBU/Alma Bengtsson).*                   |

| ![Scoreboard with 26 ranked countries, Austria in first place.](media/ESC-2025-grand-final-scoreboard.jpg) |
|:----------------------------------------------------------------------------------------------------------:|
|                         *Scoreboard from the Eurovision 2025 Grand Final (© EBU).*                         |

| ![Male singer standing on a brightly lit arena stage holding a glass trophy shaped like a microphone.](media/ESC-2025-jj-trophy.jpg) |
|:------------------------------------------------------------------------------------------------------------------------------------:|
