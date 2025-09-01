# 1. Project overview

This document is a high-level summary of the *Eurocentric* project.

- [1. Project overview](#1-project-overview)
  - [What is *Eurocentric*?](#what-is-eurocentric)
  - [What is the "queryable voting data"?](#what-is-the-queryable-voting-data)
  - [User roles and feature scopes](#user-roles-and-feature-scopes)
    - [The EuroFan user role and *public-api* feature scope](#the-eurofan-user-role-and-public-api-feature-scope)
    - [The Admin user role and *admin-api* feature scope](#the-admin-user-role-and-admin-api-feature-scope)
    - [The Dev user role and *shared* feature scope](#the-dev-user-role-and-shared-feature-scope)

## What is *Eurocentric*?

*Eurocentric* is a web application for modelling and (over)analysing the Eurovision Song Contest, 2016-2025.

*Eurocentric* is composed of two web APIs with a shared domain and database:

- The *Public API* provides a wide range of configurable, read-only rankings queries that can be run on the queryable voting data in the system database.
- The *Admin API* provides the means to populate the system database with the queryable voting data for the *Public API*.

## What is the "queryable voting data"?

The queryable voting data comprises every points award for every competitor and voter in every stage of every contest from 2016 to 2025.

A **contest** is queryable when all three of its member broadcasts (First and Second Semi-Finals and Grand Final) have been created and all their points have been awarded.

A **broadcast** is queryable when its parent contest is queryable.

A **country** is queryable when it participates in one or more queryable contests.

## User roles and feature scopes

The following user roles are defined, each with its own feature scope:

| User role | Feature scope |
|:----------|:-------------:|
| EuroFan   | *public-api*  |
| Admin     |  *admin-api*  |
| Dev       |   *shared*    |

### The EuroFan user role and *public-api* feature scope

The EuroFan is an anonymous member of the public located anywhere in the world, who interacts with the system using its *Public API*.

The EuroFan has at least some familiarity with the Eurovision Song Contest and how it works.

The EuroFan uses the *Public API* because they are interested in discovering insights from the way the votes have been distributed in recent contests.

Example questions that the EuroFan may want to answer using the *Public API*:

- "Which competitors received the highest average individual points award value in their broadcasts?"
- "Which competing countries received the most similar jury and televote points across grand final broadcasts between 2023 and 2025?"
- "Which voting countries awarded the UK non-zero points most frequently across broadcasts?"
- "Which competing countries received the lowest televote points share from the UK across broadcasts?"

### The Admin user role and *admin-api* feature scope

The Admin is a single software developer (myself), who interacts with the system using its *Admin API*.

As the Admin, I am very familiar with the Eurovision Song Contest and how it works. I am also intimately familiar with the internal workings of the system since I am the sole developer.

As the Admin, I use the *Admin API* because I want to populate the system database with the queryable data to support the *Public API*, that is, the countries, contests and broadcasts from the Eurovision Song Contest, 2016-2025.

Additionally, I want to gain some experience in domain-driven design. This is why the *Admin API* is implemented using aggregates, transactions, domain events, etc., when the system database could just as well be populated using a single SQL script.

### The Dev user role and *shared* feature scope

The Dev is a single software developer (myself), who is building the system.

As the Dev, I want to build a system that can accommodate new features (i.e. new API versions) with minimal refactoring.

I want the source code *and* test code to be written in such a way that, when I revisit it in 6-12 months, it does not take me more than an afternoon to reacquaint myself with how the system and its tests work.

I want to use the native ASP.NET libraries as much as possible, rather than 3rd-party libraries.

I want to use free or very low-cost hosting services.

I want to document each API release using the OpenAPI 3.0 standard, so that users can generate client applications in any language.
