# Eurovision context

This document describes the real-world context for the *Eurocentric* project: the Eurovision Song Contest, 2016-present.

- [Eurovision context](#eurovision-context)
  - [What is the Eurovision Song Contest?](#what-is-the-eurovision-song-contest)
  - [What countries are involved?](#what-countries-are-involved)
  - [How is a contest organized?](#how-is-a-contest-organized)
    - [Contest stages](#contest-stages)
    - [Qualification](#qualification)
    - [Competing and voting allocations](#competing-and-voting-allocations)
    - [Voting methods](#voting-methods)
      - [Voting rules 2016-2022](#voting-rules-2016-2022)
      - [Voting rules 2023-present](#voting-rules-2023-present)
    - [Disqualification](#disqualification)
      - [Disqualification during a contest](#disqualification-during-a-contest)
      - [Withdrawal before a contest starts](#withdrawal-before-a-contest-starts)
  - [How does a broadcast work?](#how-does-a-broadcast-work)
    - [Awarding points](#awarding-points)
    - [Determining the finishing order](#determining-the-finishing-order)
    - [Substitute televotes](#substitute-televotes)
  - [Key terms](#key-terms)
  - [The future](#the-future)
  - [Data source](#data-source)
  - [Images](#images)

## What is the Eurovision Song Contest?

| ![The logo for the 2025 Eurovision Song Contest in Basel, Switzerland.](media/ESC_2025_logo.png) |
|:------------------------------------------------------------------------------------------------:|
|            The logo for the 2025 Eurovision Song Contest in Basel, Switzerland (EBU).            |

The Eurovision Song Contest is an annual televised song contest organized by the European Broadcasting Union (EBU). The contest is between national broadcasters, each from a different country. Each broadcaster is represented by an act with a song. The winner of the contest customarily hosts the following year's contest.

For example:

The 2024 Eurovision Song Contest was held in Malmö, Sweden. There were 37 participants. The winner was Nemo with the song "The Code", representing the Swiss national broadcaster SRG SSR. The 2025 Eurovision Song Contest is therefore hosted by Switzerland.

## What countries are involved?

A given year's contest has somewhere between 35 and 45 participating countries.

Participating countries are mostly located in Europe. Exceptions include Australia, Israel, Georgia, Azerbaijan and Armenia.

It's customary when talking about Eurovision to refer to acts, songs, performances, televotes and juries by the countries that they represented. For example:

- "Sweden and Ireland have won Eurovision the most times."
- "Luxembourg participated in Eurovision 2024, for the first time since 1993."
- "In the 2023 Grand Final, Austria performed first in the running order and finished 15th with 120 points."
- "The UK received zero televote points in the 2024 Grand Final."
- "Getting votes from your neighbours is a sure way / To get your song disgraced. / But when Sweden gets 12 points from Norway / It's clearly just good taste." \[[YouTube link](https://youtu.be/YuszTGJlRoo?si=IfmnLxw1XJGlxdxg&t=92)\]

## How is a contest organized?

### Contest stages

A given year's contest is divided into three stages:

| Stage             | Competitors |
|:------------------|:-----------:|
| First Semi-Final  |    15-20    |
| Second Semi-Final |    15-20    |
| Grand Final       |    25-26    |

Each stage is a separate TV broadcast.

### Qualification

Each year, five or six countries automatically qualify for the Grand Final and do not have to compete in the Semi-Finals. The automatic qualifiers are the host country and the "Big Five":

- France
- Germany
- Italy
- Spain
- United Kingdom

All other participants must compete in one of the two Semi-Finals. The top 10 competitors from each Semi-Final qualify for the Grand Final.

### Competing and voting allocations

The participants are split 50/50 between each of the two Semi-Finals using a semi-random draw.

Each participant that is not an automatic qualifier is allocated to one of the Semi-Finals, in which it competes and votes.

Each automatic qualifier is allocated to one of the Semi-Finals, in which it votes but does not compete.

The qualifying participants compete in the Grand Final.

Every participant votes in the Grand Final, irrespective of whether it competes or not.

### Voting methods

Two voting methods are used: national televotes and national juries.

#### Voting rules 2016-2022

The following voting rules were used from 2016 to 2022 inclusive:

- In all three contest stages, every voting country has a national televote.
- In all three contest stages, every voting country has a national jury.

#### Voting rules 2023-present

The following voting rules have been used since 2023:

- In all three contest stages, every voting country has a national televote.
- In the First Semi-Final and Second Semi-Final, there are no national juries awarding points.
- In the Grand Final, every voting country has a national jury.
- All three contest stages have an additional "Rest of the World" televote for global viewers outside the participating countries.

### Disqualification

#### Disqualification during a contest

During the 2024 Eurovision Song Contest, the Netherlands qualified from the Second Semi-Final and assigned running order spot 5 in the Grand Final. The Dutch act was subsequently disqualified from the Grand Final.

The Netherlands did not compete in the Grand Final, but they did vote as usual. Running order spot 5 was left vacant.

This is the only time this has happened.

#### Withdrawal before a contest starts

On several occasions, a participating country has had to withdraw before the start of the contest. For example, Romania chose an act and a song for the 2016 Eurovision Song Contest but was later disqualified by the EBU. In these instances, the country is not listed as a participant in that contest.

## How does a broadcast work?

A broadcast is a single stage of a single contest. The competitors perform in a pre-determined running order. The voters (the national televotes and/or national juries) award points to the competitors, which determine their finishing positions.

### Awarding points

A single national televote or jury representing a voting country in a broadcast gives a single points award to each competing country in the broadcast excluding itself if present. It ranks the competitors from first to last. It awards the top ten ranked competitors points with the values \[12, 10, 8, 7, 6, 5, 4, 3, 2, 1\]. It awards all the other competitors 0 points.

### Determining the finishing order

The competitors in a broadcast are assigned a distinct finishing position based on descending total points.

Ties are not permitted. The following tie-break rules are used in order:

1. If two competitors are tied on total points, the competitor with more televote points wins the tie.
2. If they are still tied, the competitor with more non-zero televote points awards wins the tie.
3. If they are still tied, a "count-back" is used: the competitor that received more 12-points televote awards wins the tie, then 10-points televote awards, and so on down to 1-point televote awards.
4. If they are still tied, the competitor with the earlier running order position wins the tie.

### Substitute televotes

On multiple occasions in recent years, a voting country has not been able to award its televote points during a broadcast. Two methods have been used in recent years to create a set of televote points awards for a country that finds itself in this position:

1. An artificial televote ranking of competing countries has been generated from the televote rankings of selected countries that are similar to the affected country.
2. The country has a back-up jury whose competitor ranking is used as a substitute for a televote rankings.

## Key terms

The following key terms are extracted from the above. They are mapped to domain entities and value objects.

| Term        | Definition                                                                                             |
|:------------|:-------------------------------------------------------------------------------------------------------|
| Country     | A single real-world country (or the "Rest of the World")                                               |
| Contest     | A single year's edition of the Eurovision Song Contest                                                 |
| Broadcast   | An event that is a single contest stage in a single contest                                            |
| Participant | A country that has an act with a song in a contest                                                     |
| Competitor  | A country that competes in a broadcast                                                                 |
| Televote    | A country that awards televote points in a broadcast                                                   |
| Jury        | A country that awards jury points in a broadcast                                                       |
| Voter       | A country that is a televote and/or a jury in a broadcast                                              |
| Award       | A single points value awarded to a competitor in a broadcast by a televote or a jury in that broadcast |

## The future

This document describes the Eurovision Song Contest from 2016 to 2024. At the time of writing, no announcements have been made regarding any format changes for the 2025 Contest.

## Data source

The [official Eurovision website](https://eurovision.tv) is the single source for all data used in this project.

## Images

| ![The stage for the 68th Eurovision Song Contest in Malmö, Sweden, 2024](media/ESC_2024_contest_stage.jpg) |
|:----------------------------------------------------------------------------------------------------------:|
|       The stage for the 68th Eurovision Song Contest in Malmö, Sweden, 2024 (Peppe Andersson, SVT).        |


| ![Nemo performing The Code for Switzerland at the 2024 Grand Final](media/ESC_2024_nemo_performance.png) |
|:--------------------------------------------------------------------------------------------------------:|
|     Nemo performing "The Code" for Switzerland at the 2024 Grand Final (Sarah Louise Bennett, EBU).      |


| ![The final results of the 2024 Grand Final](media/ESC_2024_grand_final_results.png) |
|:------------------------------------------------------------------------------------:|
|                   The final results of the 2024 Grand Final (EBU).                   |


| ![Switzerland wins the 68th Eurovision Song Contest with the song The Code by Nemo](media/ESC_2024_nemo_winner.jpg) |
|:-------------------------------------------------------------------------------------------------------------------:|
|     Switzerland wins the 68th Eurovision Song Contest with the song "The Code" by Nemo (Corinne Cumming, EBU).      |


| ![Rendering of the stage design for the 2025 Eurovision Song Contest in Basel, Switzerland.](media/ESC_2025_stage_design.jpg) |
|:-----------------------------------------------------------------------------------------------------------------------------:|
|              Rendering of the stage design for the 2025 Eurovision Song Contest in Basel, Switzerland (SRG SSR).              |
