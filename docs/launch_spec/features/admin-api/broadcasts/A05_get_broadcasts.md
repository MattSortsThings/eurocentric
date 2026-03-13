# A05. Get broadcasts

This document is part of the [launch specification](../../../README.md).

## User story

- **As the Admin**
- **I want** to retrieve all existing broadcasts, ordered by broadcast date
- **So that** I can verify the behaviour of features that create, update, or delete one or more broadcasts

## API contract

### HTTP request

```http request
GET /admin/api/{apiVersion}/broadcasts
```

### HTTP response

```http request
200 OK
```

```json
{
  "broadcasts": [
    {
      "id": "00000000-0000-0000-0000-000000000000",
      "broadcastDate": "2025-01-01",
      "parentContestId": "00000000-0000-0000-0000-000000000000",
      "contestStage": "GrandFinal",
      "votingFormat": "JuryAndTelevote",
      "completed": false,
      "competitors": [
        {
          "competingCountryId": "00000000-0000-0000-0000-000000000000",
          "performingSpot": 1,
          "broadcastHalf": "First",
          "finishingSpot": 1,
          "pointsAwards": [
            {
              "votingCountryId": "00000000-0000-0000-0000-000000000000",
              "votingMethod": "Jury",
              "pointsValue": 12
            }
          ]
        }
      ],
      "juries": [
        {
          "votingCountryId": "00000000-0000-0000-0000-000000000000",
          "pointsAwarded": false
        }
      ],
      "televotes": [
        {
          "votingCountryId": "00000000-0000-0000-0000-000000000000",
          "pointsAwarded": false
        }
      ]
    }
  ]
}
```

## Acceptance criteria

### Happy Path : Baseline

**Endpoint...**

- [ ] Succeeds_and_retrieves_all_broadcasts_in_broadcastDate_order

### Happy Path : Zero Broadcasts

**Endpoint...**

- [ ] Succeeds_with_0_broadcasts_when_no_broadcasts_exist
