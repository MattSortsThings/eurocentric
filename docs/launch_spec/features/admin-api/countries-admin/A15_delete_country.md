# A15. Delete country

## User story

- **As the Admin**
- **I want** to delete a single country
- **So that** no trace of the deleted country remains, and I am free to create a new country with the same country code if I wish.

## API contract

### HTTP request

```http request
DELETE /admin/api/{apiVersion}/countries/{countryId}
```

**Notes:**

- `apiVersion` is a major-minor API version URL segment, e.g. `"v1.0"`.
- `countryId` is the Guid ID of the requested country aggregate.

### HTTP response

```http request
204 No Content
```

**Notes:**

- The requested country aggregate no longer exists.
- Any secondary effects are scoped to event handler features.

## Acceptance criteria

### Happy path

**DeleteCountry endpoint...**

- [ ] Should_succeed_with_204_NoContent_and_delete_country_when_request_is_valid

### Sad path

**GetCountry endpoint...**

- [ ] Should_fail_when_country_does_not_exist
- [ ] Should_fail_when_country_owns_one_or_more_contestIds
