# Project summary

- [Project summary](#project-summary)
  - [What is *Eurocentric*?](#what-is-eurocentric)
  - [User roles](#user-roles)
    - [Euro-Fan](#euro-fan)
    - [Admin](#admin)
    - [Dev](#dev)

## What is *Eurocentric*?

*Eurocentric* is a web application that lets users run a range of analytical queries on voting data for the Eurovision Song Contest, 2016-present.

*Eurocentric* is composed of two web APIs with a shared domain and database:

- The *Public API* provides a wide range of configurable analytics queries that can be run on the queryable data in the system database.
- The *Admin API* provides the means to populate the system database with the queryable data for the *Public API*.

## User roles

The following user roles are defined, each with its own feature scope:

| User role | Feature scope                             |
|:----------|:------------------------------------------|
| Euro-Fan  | *Public API* features                     |
| Admin     | *Admin API* features                      |
| Dev       | Shared features for the system as a whole |

### Euro-Fan

The Euro-Fan is an anonymous member of the public located anywhere in the world, who interacts with the system using its *Public API*.

The Euro-Fan has at least some familiarity with the Eurovision Song Contest and how it works.

The Euro-Fan uses the *Public API* because they are interested in discovering insights from the way the votes have been distributed in recent contests.

Example questions that the Euro-Fan may want to answer using the *Public API*:

- "Which competitors received the highest share of the available points?"
- "Which participating countries have received the most similar points from the televotes and the juries in Grand Finals?"
- "Which voting countries have the highest frequency of awarding the UK non-zero points?"
- "Which participating countries have received the lowest average individual televote points award from the UK?"

### Admin

The Admin is a single software developer (myself), who interacts with the system using its *Admin API*.

As the Admin, I am very familiar with the Eurovision Song Contest and how it works. I am also intimately familiar with the internal workings of the system since I am the sole developer.

As the Admin, I use the *Admin API* because I want to populate the system database with the queryable data to support the *Public API*, that is, the countries, contests and broadcasts from the Eurovision Song Contest, 2016-present.

Additionally, I want to gain some experience in domain-driven design. This is why the *Admin API* is implemented using aggregates, transactions, domain events, etc., when the system database could just as well be populated using a single SQL script.

### Dev

The Dev is a single software developer (myself), who is building the system.

As the Dev, I want to build a system that can accommodate new features (i.e. new API versions) with minimal refactoring.

I want the code to be written in such a way that, when I revisit it in 6-12 months, it does not take me more than an afternoon to reacquaint myself with the architecture and implementation.

I want to use the native ASP.NET libraries as much as possible, rather than 3rd-party libraries.

I want to use free or very low-cost hosting services.

I want the system to be functional enough to accommodate a handful of concurrent users.
