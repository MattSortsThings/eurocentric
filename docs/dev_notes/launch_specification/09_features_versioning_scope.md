# 09: Features: *versioning* scope

This document is part of the [launch specification](../README.md#launch-specification).

- [09: Features: *versioning* scope](#09-features-versioning-scope)
  - [API Versioning](#api-versioning)
    - [cv01: Versioned Endpoints](#cv01-versioned-endpoints)
    - [cv02: Version Reporting](#cv02-version-reporting)

## API Versioning

### cv01: Versioned Endpoints

**Details:**

- Each API uses major/minor semantic versioning.
- The client must specify the API version in a URL segment.

### cv02: Version Reporting

**Details:**

- An API HTTP response includes an `"api-supported-versions"` header that lists all the versions of the API.
