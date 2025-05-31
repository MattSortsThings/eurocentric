# Project summary

This document is a high-level summary of the *Eurocentric* project.

- [Project summary](#project-summary)
  - [What is *Eurocentric*?](#what-is-eurocentric)
  - [What is the "queryable data"?](#what-is-the-queryable-data)
  - [User roles and feature scopes](#user-roles-and-feature-scopes)
    - [The Euro-Fan user role and *public-api* feature scope](#the-euro-fan-user-role-and-public-api-feature-scope)
    - [The Admin user role and *admin-api* feature scope](#the-admin-user-role-and-admin-api-feature-scope)
    - [The Dev user role and *shared* feature scope](#the-dev-user-role-and-shared-feature-scope)

## What is *Eurocentric*?

*Eurocentric* is a web application for modelling and (over)analysing the Eurovision Song Contest, 2016-present.

*Eurocentric* is composed of two web APIs with a shared domain and database:

- The *Public API* provides a wide range of configurable, read-only queries that can be run on the queryable data in the system database.
- The *Admin API* provides the means to populate the system database with the queryable data for the *Public API*.

## What is the "queryable data"?

- A contest is queryable, along with all the data for its participating countries, when all three of its member broadcasts (First and Second Semi-Finals and Grand Final) have been created and all their points have been awarded.
- A broadcast is queryable, along with all the data for its competing countries, voting countries, and points awarded, when its parent contest is queryable.
- A country is queryable when it participates in one or more queryable contests.

Any data that does not fall into one of the

## User roles and feature scopes

The following user roles are defined, each with its own feature scope:

| User role | Feature scope |
|:----------|:-------------:|
| Euro-Fan  | *public-api*  |
| Admin     |  *admin-api*  |
| Dev       |   *shared*    |

### The Euro-Fan user role and *public-api* feature scope

The Euro-Fan is an anonymous member of the public located anywhere in the world, who interacts with the system using its *Public API*.

The Euro-Fan has at least some familiarity with the Eurovision Song Contest and how it works.

The Euro-Fan uses the *Public API* because they are interested in discovering insights from the way the votes have been distributed in recent contests.

Example questions that the Euro-Fan may want to answer using the *Public API*:

- "Which competitors received the highest share of the available points?"
- "Which participating countries have received the most similar televote and jury points in Grand Finals?"
- "Which voting countries have the highest frequency of awarding the UK non-zero points?"
- "Which participating countries have received the lowest average individual televote points award from the UK?"

### The Admin user role and *admin-api* feature scope

The Admin is a single software developer (myself), who interacts with the system using its *Admin API*.

As the Admin, I am very familiar with the Eurovision Song Contest and how it works. I am also intimately familiar with the internal workings of the system since I am the sole developer.

As the Admin, I use the *Admin API* because I want to populate the system database with the queryable data to support the *Public API*, that is, the countries, contests and broadcasts from the Eurovision Song Contest, 2016-present.

Additionally, I want to gain some experience in domain-driven design. This is why the *Admin API* is implemented using aggregates, transactions, domain events, etc., when the system database could just as well be populated using a single SQL script.

### The Dev user role and *shared* feature scope

The Dev is a single software developer (myself), who is building the system.

As the Dev, I want to build a system that can accommodate new features (i.e. new API versions) with minimal refactoring.

In particular, I want new endpoints to be discovered automatically, without my having to define and invoke an explicit configuration method for each endpoint.

I want the code to be written in such a way that, when I revisit it in 6-12 months, it does not take me more than an afternoon to reacquaint myself with the architecture and implementation.

I want to use the native ASP.NET libraries as much as possible, rather than 3rd-party libraries.

I want to use free or very low-cost hosting services.

I want the system to be functional enough to accommodate a handful of concurrent users.
