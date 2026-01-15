# Scraped data

This directory contains country and contest data scraped from the official Eurovision website.

- [Scraped data](#scraped-data)
  - [Countries](#countries)
  - [Contests](#contests)

## Countries

**File:**

- [53 countries](53_countries.json)

**JSON schema:**

```json
{
  "$schema": "https://json-schema.org/draft/2020-12/schema",
  "title": "Countries",
  "type": "object",
  "properties": {
    "realCountries": {
      "type": "array",
      "items": {
        "$ref": "#/$defs/country"
      }
    },
    "pseudoCountries": {
      "type": "array",
      "items": {
        "$ref": "#/$defs/country"
      }
    }
  },
  "required": ["realCountries", "pseudoCountries"],
  "$defs": {
    "country": {
      "type": "object",
      "properties": {
        "countryCode": {
          "type": "string",
          "pattern": "^[A-Z]{2}$"
        },
        "countryName": {
          "type": "string"
        }
      },
      "required": ["countryCode", "countryName"],
      "additionalProperties": false
    }
  },
  "additionalProperties": false
}
```

## Contests

**Files:**

- [2016 contest](2016_contest.json)
- [2017 contest](2017_contest.json)
- [2018 contest](2018_contest.json)
- [2019 contest](2019_contest.json)
- [2021 contest](2021_contest.json)
- [2022 contest](2022_contest.json)
- [2023 contest](2023_contest.json)
- [2024 contest](2024_contest.json)
- [2025 contest](2025_contest.json)

**JSON schema:**

```json
{
  "$schema": "https://json-schema.org/draft/2020-12/schema",
  "title": "Contest",
  "type": "object",
  "properties": {
    "overview": {
      "type": "object",
      "properties": {
        "contestYear": { "type": "integer" },
        "cityName": { "type": "string" },
        "televoteOnlySemiFinals": { "type": "boolean" },
        "globalTelevoteVotingCountryCode": {
          "type": ["string", "null"],
          "pattern": "^[A-Z]{2}$"
        }
      },
      "required": [
        "contestYear",
        "cityName",
        "televoteOnlySemiFinals",
        "globalTelevoteVotingCountryCode"
      ],
      "additionalProperties": false
    },
    "semiFinal1Participants": {
      "type": "array",
      "items": { "$ref": "#/$defs/participant" }
    },
    "semiFinal2Participants": {
      "type": "array",
      "items": { "$ref": "#/$defs/participant" }
    },
    "semiFinal1Broadcast": { "$ref": "#/$defs/broadcast" },
    "semiFinal2Broadcast": { "$ref": "#/$defs/broadcast" },
    "grandFinalBroadcast": { "$ref": "#/$defs/broadcast" }
  },
  "required": [
    "overview",
    "semiFinal1Participants",
    "semiFinal2Participants",
    "semiFinal1Broadcast",
    "semiFinal2Broadcast",
    "grandFinalBroadcast"
  ],
  "additionalProperties": false,
  "$defs": {
    "countryCode": {
      "type": "string",
      "pattern": "^[A-Z]{2}$"
    },
    "participant": {
      "type": "object",
      "properties": {
        "participatingCountryCode": { "$ref": "#/$defs/countryCode" },
        "actName": { "type": "string" },
        "songTitle": { "type": "string" }
      },
      "required": [
        "participatingCountryCode",
        "actName",
        "songTitle"
      ],
      "additionalProperties": false
    },
    "broadcast": {
      "type": "object",
      "properties": {
        "broadcastDate": { "type": "string", "format": "date" },
        "firstHalfLength": { "type": "integer" },
        "competitors": {
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
      "required": ["broadcastDate", "competitors", "televotes", "juries"],
      "additionalProperties": false
    },
    "competitor": {
      "type": "object",
      "properties": {
        "runningOrderSpot": { "type": "integer" },
        "competingCountryCode": { "$ref": "#/$defs/countryCode" },
        "totalPoints": { "type": "integer" },
        "finishingPosition": { "type": "integer" }
      },
      "required": [
        "runningOrderSpot",
        "competingCountryCode",
        "totalPoints",
        "finishingPosition"
      ],
      "additionalProperties": false
    },
    "voter": {
      "type": "object",
      "properties": {
        "votingCountryCode": { "$ref": "#/$defs/countryCode" },
        "givenPointsAwards": {
          "type": "array",
          "items": { "$ref": "#/$defs/pointsAward" }
        }
      },
      "required": ["votingCountryCode", "givenPointsAwards"],
      "additionalProperties": false
    },
    "pointsAward": {
      "type": "object",
      "properties": {
        "pointsValue": { "type": "integer" },
        "competingCountryCode": { "$ref": "#/$defs/countryCode" }
      },
      "required": ["pointsValue", "competingCountryCode"],
      "additionalProperties": false
    }
  }
}
```
