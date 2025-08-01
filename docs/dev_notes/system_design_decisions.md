# System design decisions

This document outlines system design decisions taken during development of the *Eurocentric* project.

- [System design decisions](#system-design-decisions)
  - [Technical specification](#technical-specification)
  - [Assembly architecture](#assembly-architecture)
  - [API architecture](#api-architecture)
    - [Vertical slices](#vertical-slices)
    - [Request-endpoint-response (REPR pattern)](#request-endpoint-response-repr-pattern)
    - [Command/query responsibility segregation](#commandquery-responsibility-segregation)
    - [Railway oriented programming](#railway-oriented-programming)
    - [Request handling workflow](#request-handling-workflow)
  - [API versioning](#api-versioning)
  - [HTTP responses](#http-responses)
    - [Successful responses](#successful-responses)
    - [Unsuccessful responses](#unsuccessful-responses)
  - [Domain model rules](#domain-model-rules)
    - [Identity](#identity)
    - [Instantiation](#instantiation)
    - [Mutability](#mutability)
    - [Enforcement of invariants](#enforcement-of-invariants)
  - [Acceptance test driven development (ATDD)](#acceptance-test-driven-development-atdd)
    - [Acceptance tests](#acceptance-tests)
    - [Unit tests](#unit-tests)
    - [Architecture tests](#architecture-tests)
  - [Version control](#version-control)
  - [Continuous integration and continuous delivery (CI/CD)](#continuous-integration-and-continuous-delivery-cicd)
  - [Data access](#data-access)
  - [Database concurrency](#database-concurrency)

## Technical specification

- The system is written using .NET version 9.
- The APIs are implemented using the ASP.NET *minimal API* technique.
- The system aims for level 2 REST maturity.
- As far as possible, the native ASP.NET libraries are used to implement the APIs.
- The system is hosted in the cloud as an Azure Web App.
- The system uses an Azure SQL Database, hosted in the cloud.
- The language used by the system is UK English.

## Assembly architecture

The system is composed of four .NET assemblies:

| Name                         | .NET project type | Role                                                                                                                  |
|:-----------------------------|:-----------------:|:----------------------------------------------------------------------------------------------------------------------|
| `Eurocentric.WebApp`         |      Web API      | composition root and executable                                                                                       |
| `Eurocentric.Features`       |   Class library   | *admin-api*, *public-api* and *shared* features (i.e. clean architecture application + presentation layers)           |
| `Eurocentric.Infrastructure` |   Class library   | data access, timing, other services that reach outside the application (i.e. clean architecture infrastructure layer) |
| `Eurocentric.Domain`         |   Class library   | domain types                                                                                                          |

- `Eurocentric.Domain` depends on nothing.
- `Eurocentric.Infrastructure` depends on `Eurocentric.Domain`.
- `Eurocentric.Features` depends on `Eurocentric.Infrastructure`.
- `Eurocentric.WebApp` depends on `Eurocentric.Features`.

The four assemblies are illustrated in the below diagram, in which arrows indicate the directions of dependencies.

```mermaid
block-beta

columns 2

a["Eurocentric.WebApp"]:2
space:2
b["Eurocentric.Features"]:2
space:2
c["Eurocentric.Infrastructure"]:2
space
blockArrowId6<["data"]>(up, down)
e["Eurocentric.Domain"] d[("database")]

a --> b
b --> c
c --> e
```

## API architecture

Each of the two APIs is structured using the following patterns:

### Vertical slices

All the types for a feature are stored together in a single file, named after the feature. Each feature has at most one endpoint, defined using the Minimal API syntax.

### Request-endpoint-response (REPR pattern)

Each API endpoint defines a request type and/or a response type. All the requests and responses for an API have properties that are *either* native .NET types *or* public types defined in the API's section of the `Eurocentric.Features` assembly, but *never* in the `Eurocentric.Domain` assembly.

### Command/query responsibility segregation

A request type is *either* a command (which changes the state of the system) *or* a query (which only reads data from the system).

### Railway oriented programming

Every request is handled on the server and *either* succeeds and returns a response *or* fails and returns a list of errors.

### Request handling workflow

The following diagram illustrates the request handling workflow used for every feature that has an endpoint.

1. The client sends an HTTP request to the API endpoint.
2. The API endpoint sends the request object to the app pipeline.
3. The app pipeline routes the request object to its request handler.
4. The request handler handles the request, which is *either* successful (go to step 5) *or* unsuccessful (go to step 8).
5. The request handler returns the response object to the app pipeline.
6. The app pipeline returns the response object to the API endpoint.
7. The API endpoint sends the response object to the client as an HTTP response with a successful status code \[END\].
8. The request handler returns errors to the app pipeline.
9. The app pipeline returns the errors to the API endpoint.
10. The API endpoint creates a problem details object from the first error and sends the problem details to the client as an HTTP response with an unsuccessful status code \[END\].

```mermaid
sequenceDiagram
autonumber

participant C as Client
participant E as API<br/>Endpoint
participant A as App<br/>Pipeline
participant H as Request<br/>Handler

C->>E : sends HTTP request
E->>A : sends request object
A->>H : routes request object
H->>H : handles request
alt successful
H->>A : returns response object
A->>E : returns response object
E->>C : sends HTTP response<br/>with response object
else unsuccessful
H->>A : returns errors
A->>E : returns errors
E->>C : sends HTTP response<br/>with problem details
end
```

## API versioning

The *Admin API* and the *Public API* both use major+minor API versioning. The API version is passed as a URL segment, e.g. `/admin/api/v1.0/countries`.

Each *major* version of an API is a separate unit, not expected to be compatible with earlier or later major versions.

A later *minor* version of a *major* version of an API is backwards-compatible with all earlier minor versions of the major version. In other words, a new *minor* version of an API can only introduce new endpoints; it cannot remove or modify existing endpoints.

## HTTP responses

### Successful responses

When an API endpoint successfully handles a request, the response follows the examples in the table below.

| HTTP method | Response status code | Response body                  | Response headers  |
|:-----------:|:--------------------:|:-------------------------------|:------------------|
|    `GET`    |         200          | Requested data                 | None              |
|   `POST`    |         201          | Full model of created resource | Resource location |
|  `DELETE`   |         204          | None                           | None              |
|   `PATCH`   |         204          | None                           | None              |

### Unsuccessful responses

When an API unsuccessfully handles a request, the response follows the examples in the table below.

| Response status code | Meaning                                                                                                                                                                                                                                                       | Example(s)                                                                                                                                          |
|:--------------------:|:--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|:----------------------------------------------------------------------------------------------------------------------------------------------------|
|         400          | "I can't understand what the request is asking me to do. This includes situations when a `BadHttpRequestException` or `InvalidEnumArgumentException` was thrown."                                                                                             | Query string missing required parameter. Request body missing required property. Passing an illegal string value for an enum property or parameter. |
|         401          | "I can't authenticate the client."                                                                                                                                                                                                                            | Using an unrecognized API key.                                                                                                                      |
|         403          | "I have authenticated the client but they are not authorized to make the request. "                                                                                                                                                                           | Not using secret API key.                                                                                                                           |
|         404          | "The request is referencing a resource by ID that doesn't exist."                                                                                                                                                                                             | Creating a **CONTEST** with a **Participant** referencing a non-existent **COUNTRY**.                                                               |
|         409          | "I've understood the request, but I can't execute it because doing so would break one or more business rules given the current state of the resource being modified and/or all existing resources. In other words, the request is **extrinsically illegal**." | Creating a **CONTEST** with a non-unique contest year. Awarding points in a **BROADCAST** for a **Jury** that has already awarded its points.       |
|         422          | "I've understood the request, but I can't execute it because one or more of the elements of the request are by themselves incompatible with one or more business rules. In other words, the request is **intrinsically illegal**."                            | Creating a **COUNTRY** with an illegal country code value. Creating a **CONTEST** with two **Participants** referencing the same **COUNTRY**.       |
|         500          | "An unexpected exception was thrown, which is not a `BadHttpRequestException` or an `InvalidEnumArgumentException` or a `SqlException` caused by a database connection timing out."                                                                           | Divide by zero.                                                                                                                                     |
|         503          | "A `SqlException` was thrown because the database connection timed out. Try again after \[duration\] seconds."                                                                                                                                                | Database is unavailable.                                                                                                                            |

## Domain model rules

This section describes the rules for domain aggregate, entity, and value object types.

### Identity

1. An aggregate is assigned an ID on the server when it is first created, before it is persisted to the database. The [RFC 9562 Version 7](https://learn.microsoft.com/en-us/dotnet/api/system.guid.createversion7?view=net-9.0) GUID specification is used.
2. An entity has no ID of its own, but has a property that uniquely identifies it within its aggregate. The entity therefore has a composite ID of its identifying property and the ID of its aggregate.
3. A value object has no identity.

### Instantiation

1. An aggregate can be instantiated using its public API and queried by its ID.
2. An entity cannot be instantiated or queried except as part of its aggregate.
3. A value object can be instantiated anywhere using a factory method.

### Mutability

1. An aggregate is mutable through its public API.
2. An entity can only be mutated through the public API of its aggregate.
3. A value object is immutable.

### Enforcement of invariants

1. An aggregate enforces its internal invariants, including those of all its entities, at all times.
   1. An aggregate cannot be instantiated in a state that violates any of its internal invariants.
   2. An aggregate cannot be mutated if doing so would violate any of its internal invariants.
2. A value object cannot be instantiated in an illegal state.
3. An instantiated or updated aggregate cannot be persisted to the system if it violates any inter-aggregate invariants given the aggregates that currently exist in the system.
4. If one or more aggregates in a system need to be updated as a result of a given aggregate being created, updated or deleted, all updates must be carried out as part of the original transaction. The transaction must be rolled back if any invariant is violated.

## Acceptance test driven development (ATDD)

The development loop is as follows:

1. Choose a feature for development.
2. Write failing acceptance tests covering every happy path and sad path from the user's point of view.
3. Implement the feature using unit tests for domain/feature functionality.
4. Make the acceptance tests pass.
5. Refactor code.
6. Maintain code style using architecture tests that must always pass.

### Acceptance tests

Acceptance tests are written for the features in the `Eurocentric.Features` assembly.

Features are tested using a web application fixture with a containerized database that is reset to an empty state after every test.

As far as possible, acceptance tests only use existing API endpoints to interact with the web application fixture. When a test requires some functionality that has not yet been implemented, an explicit "backdoor" method is used to modify the web application fixture directly.

Acceptance tests are divided into separate collections, each with its own fixture and database. Tests within a collection are run sequentially. Test collections are run in parallel to each other.

### Unit tests

Unit tests are written for domain types in the `Eurocentric.Domain` assembly and for utility class in the `Eurocentric.Features` assembly.

### Architecture tests

Architecture tests are written to enforce design rules in the `Eurocentric.Domain`, `Eurocentric.Features` and `Eurocentric.Infrastructure` assembly.

## Version control

Git is used for version control of source code.

Commit messages are written using the [Conventional Commits](https://www.conventionalcommits.org/en/v1.0.0/) standard.

## Continuous integration and continuous delivery (CI/CD)

At an early stage in development, an action is added to the GitHub source code repository that automatically publishes and deploys the application to the Azure App Service. This action is triggered every time source code is pushed to the main branch in the remote repository. The working cadence is generally to push to the repository after each feature is implemented.

## Data access

The following technologies are used for interacting with the system database:

- All *admin-api* features interact with the database using an **EF Core** database context.
- The *public-api* filters and scoreboards features interact with the database using the **EF Core** database context.
- The *public-api* rankings features interact with the database using stored procedures, using **Dapper** to access the database.

## Database concurrency

This project **does not** implement any kind of locking system to avoid database race conditions. This is because there is only a single user who is authorized to create, update and delete records in the database, and transactions will not be concurrent.
