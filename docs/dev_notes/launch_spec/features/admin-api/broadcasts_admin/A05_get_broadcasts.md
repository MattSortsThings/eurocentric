# A05 Get broadcasts

This document is part of the *Eurocentric* [launch specification](../../../README.md).

- [A05 Get broadcasts](#a05-get-broadcasts)
  - [User story](#user-story)
  - [API contract](#api-contract)
    - [HTTP request](#http-request)
    - [HTTP response](#http-response)
  - [Acceptance criteria](#acceptance-criteria)

## User story

- **As the Admin**
- **I want** to retrieve all existing broadcasts in broadcast date order
- **So that** I can test the behaviour of features that create, update or delete one or more broadcasts

## API contract

### HTTP request

```http request
GET /admin/api/{apiVersion}/broadcasts
```

### HTTP response

```http request
200 Ok
```

```json
{
  "broadcasts": [
    {
      "id": "00000000-0000-0000-0000-000000000000",
      "broadcastDate": "2025-05-17",
      "parentContestId": "00000000-0000-0000-0000-000000000000",
      "contestStage": "GrandFinal",
      "votingRules": "TelevoteAndJury",
      "completed": false,
      "competitors": [
        {
          "competingCountryId": "00000000-0000-0000-0000-000000000000",
          "runningOrderPosition": 1,
          "finishingPosition": 1,
          "pointsAwards": [
            {
              "votingCountryId": "00000000-0000-0000-0000-000000000000",
              "votingMethod": "Televote",
              "pointsValue": 12
            }
          ]
        }
      ],
      "televotes": [
        {
          "votingCountryId": "00000000-0000-0000-0000-000000000000",
          "pointsAwarded": false
        }
      ],
      "juries": [
        {
          "votingCountryId": "00000000-0000-0000-0000-000000000000",
          "pointsAwarded": false
        }
      ]
    }
  ]
}
```

**Notes:**

- `broadcastDate` uses the `"yyyy-MM-dd"` date format
- `contestStage` is an [enum value](../../../domain_model.md#conteststage-enum)
- `votingMethod` is an [enum value](../../../domain_model.md#votingmethod-enum)
- `votingRules` is an [enum value](../../../domain_model.md#votingrules-enum)

## Acceptance criteria

**GetBroadcast endpoint...**

- should succeed with 200 and return all existing broadcasts in broadcast date order
- should succeed with 200 and return empty list when no broadcasts exist
