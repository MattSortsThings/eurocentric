# 7. Domain invariant enforcement

This document is part of the [launch specification](../README.md#launch-specification).

- [7. Domain invariant enforcement](#7-domain-invariant-enforcement)
  - [Identity](#identity)
    - [Aggregate identity](#aggregate-identity)
    - [Entity identity](#entity-identity)
    - [Value object identity](#value-object-identity)
  - [Instantiation outside domain assembly](#instantiation-outside-domain-assembly)
    - [Aggregate instantiation](#aggregate-instantiation)
    - [Entity instantiation](#entity-instantiation)
    - [Value object instantiation](#value-object-instantiation)
  - [Mutability outside domain assembly](#mutability-outside-domain-assembly)
    - [Aggregate mutability](#aggregate-mutability)
    - [Entity mutability](#entity-mutability)
    - [Value object mutability](#value-object-mutability)
  - [Transactions](#transactions)

## Identity

### Aggregate identity

An aggregate is assigned an ID on the server when it is first created, before it is persisted to the database. The [RFC 9562 Version 7](https://learn.microsoft.com/en-us/dotnet/api/system.guid.createversion7?view=net-9.0) GUID specification is used.

### Entity identity

An entity has no ID of its own. It has a property that uniquely identifies it within its aggregate. The entity therefore has a composite ID of its identifying property and the ID of its aggregate.

### Value object identity

A value object has no identity.

## Instantiation outside domain assembly

### Aggregate instantiation

An aggregate can only be instantiated using a public static builder on the aggregate type.

An aggregate builder fails and returns an error if any domain invariants are violated by the specified aggregate.

Therefore, an aggregate can never be instantiated in an illegal state.

### Entity instantiation

An entity cannot be instantiated except as part of the aggregate that owns it.

### Value object instantiation

A value object can be instantiated using a public static factory method on the value object type.

The factory method fails and returns an error if any domain invariant is violated by the specified value object.

Therefore, an illegal value object can never be instantiated.

## Mutability outside domain assembly

### Aggregate mutability

An aggregate has one or more public methods that may be used to mutate its state as part of a transaction.

An aggregate's mutator method fails, rolls back, and returns an error if the proposed changes would leave it in an illegal state.

A mutator method throws an exception instead of returning an error if it is to be used as part of a domain event handler.

### Entity mutability

An entity has no public methods, which means it can only be mutated by the aggregate that owns it.

### Value object mutability

A value object is immutable.

## Transactions

The domain model has [8 transactions](spec_06_domain_model.md#transactions). Domain invariants are enforced in the application, with a fallback enforcement layer in the database.

A transaction's effect is one of the following:

- adds a single aggregate to an aggregates set.
- updates a single aggregate in an aggregates set.
- removes a single aggregate from an aggregates set.

A transaction with side effects raises a domain event in the aggregate. Handling the domain event:

- updates one or more aggregates in an aggregates set.

A successful transaction therefore commits its effect and all side effects to the database as a single unit.

A transaction that violates one or more domain invariants fails and returns an error. It does not save changes to the database.

Domain invariants are replicated in the database schema using table constraints. In the event that a transaction that violates one or more domain events is committed, the saving changes operation throws a `SqlException`. This situation should only arise if application invariant enforcement is incorrectly implemented.

Domain invariants are not enforced in the API layer, i.e. by validating request body or query string properties.
