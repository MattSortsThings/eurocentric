# Scraped data

This directory contains country and contest data scraped from the official Eurovision website.

- [Scraped data](#scraped-data)
  - [Countries info](#countries-info)
  - [Contest info](#contest-info)

## Countries info

**File:**

- [all 53 countries info](all_53_countries_info.json)

**JSON schema:**

```json
{
  "$schema": "https://json-schema.org/draft/2020-12/schema",
  "title": "CountriesInfo",
  "type": "object",
  "properties": {
    "realCountries": {
      "type": "array",
      "items": { "$ref": "#/$defs/country" }
    },
    "pseudoCountries": {
      "type": "array",
      "items": { "$ref": "#/$defs/country" }
    }
  },
  "required": ["realCountries", "pseudoCountries"],
  "$defs": {
    "country": {
      "type": "object",
      "properties": {
        "countryCode": { "type": "string" },
        "countryName": { "type": "string" }
      },
      "required": ["countryCode", "countryName"]
    }
  }
}
```

## Contest info

**Files:**

- [2016 contest info](2016_contest_info.json)
- [2017 contest info](2017_contest_info.json)
- [2018 contest info](2018_contest_info.json)
- [2019 contest info](2019_contest_info.json)
- [2021 contest info](2021_contest_info.json)
- [2022 contest info](2022_contest_info.json)
- [2023 contest info](2023_contest_info.json)
- [2024 contest info](2024_contest_info.json)
- [2025 contest info](2025_contest_info.json)

**JSON schema:**

```json
{
  "$schema": "https://json-schema.org/draft/2020-12/schema",
  "title": "ContestInfo",
  "type": "object",
  "properties": {
    "contest": { "$ref": "#/$defs/contest" },
    "semiFinal1Broadcast": { "$ref": "#/$defs/broadcast" },
    "semiFinal2Broadcast": { "$ref": "#/$defs/broadcast" },
    "grandFinalBroadcast": { "$ref": "#/$defs/broadcast" }
  },
  "required": [
    "contest",
    "semiFinal1Broadcast",
    "semiFinal2Broadcast",
    "grandFinalBroadcast"
  ],
  "$defs": {
    "contest": {
      "type": "object",
      "properties": {
        "contestYear": { "type": "integer" },
        "cityName": { "type": "string" },
        "televoteOnlySemiFinals": { "type": "boolean" },
        "globalTelevoteVotingCountryCode": {
          "type": ["string", "null"]
        },
        "semiFinal1Participants": {
          "type": "array",
          "items": { "$ref": "#/$defs/participant" }
        },
        "semiFinal2Participants": {
          "type": "array",
          "items": { "$ref": "#/$defs/participant" }
        }
      },
      "required": [
        "contestYear",
        "cityName",
        "televoteOnlySemiFinals",
        "semiFinal1Participants",
        "semiFinal2Participants"
      ]
    },

    "participant": {
      "type": "object",
      "properties": {
        "participatingCountryCode": { "type": "string" },
        "actName": { "type": "string" },
        "songTitle": { "type": "string" }
      },
      "required": [
        "participatingCountryCode",
        "actName",
        "songTitle"
      ]
    },

    "broadcast": {
      "type": "object",
      "properties": {
        "broadcastDate": {
          "type": "string",
          "format": "date"
        },
        "firstHalfCompetitors": {
          "type": "array",
          "items": { "$ref": "#/$defs/competitor" }
        },
        "secondHalfCompetitors": {
          "type": "array",
          "items": { "$ref": "#/$defs/competitor" }
        },
        "televotes": {
          "type": "array",
          "items": { "$ref": "#/$defs/voter" }
        },
        "juries": {
          "type": "array",
          "items": { "$ref": "#/$defs/voter" }
        }
      },
      "required": [
        "broadcastDate",
        "firstHalfCompetitors",
        "secondHalfCompetitors",
        "televotes",
        "juries"
      ]
    },

    "competitor": {
      "type": "object",
      "properties": {
        "performingSpot": { "type": "integer" },
        "competingCountryCode": { "type": "string" },
        "finishingSpot": { "type": "integer" }
      },
      "required": [
        "performingSpot",
        "competingCountryCode",
        "finishingSpot"
      ]
    },

    "voter": {
      "type": "object",
      "properties": {
        "votingCountryCode": { "type": "string" },
        "givenPointsAwards": {
          "type": "array",
          "items": { "$ref": "#/$defs/pointsAward" }
        }
      },
      "required": ["votingCountryCode", "givenPointsAwards"]
    },

    "pointsAward": {
      "type": "object",
      "properties": {
        "pointsValue": { "type": "integer" },
        "competingCountryCode": { "type": "string" }
      },
      "required": ["pointsValue", "competingCountryCode"]
    }
  }
}
```
