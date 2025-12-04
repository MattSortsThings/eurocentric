# 2. System overview

This document is part of the [launch specification](../README.md#launch-specification).

- [2. System overview](#2-system-overview)
  - [System description](#system-description)
    - [The Public API](#the-public-api)
    - [The Admin API](#the-admin-api)
  - [User roles](#user-roles)
    - [The EuroFan](#the-eurofan)
    - [The Admin](#the-admin)
    - [Operational context](#operational-context)

## System description

The system is a .NET 10 web API for modelling and (over)analysing the Eurovision Song Contest, 2016-present.

The system is composed of two APIs with a shared domain and database: the Public API and the Admin API.

### The Public API

The Public API is a set of GET endpoints for analysing the queryable voting data in the system.

Its feature groups are:

- Ranking competitors by metrics calculated from the points they received in their broadcasts.
- Ranking countries by metrics calculated from the points they gave and received across broadcasts.
- Ranking broadcast running order segments by metrics calculated from points received by competitors across broadcasts.
- Retrieving the scoreboard rows from a broadcast.
- Retrieving the points awards given and/or received by a country in a broadcast.
- Retrieving lists of all queryable broadcasts, contests and countries.

### The Admin API

The Admin API is a set of GET/POST/PATCH/DELETE endpoints for populating the system with the queryable voting data in a manner that imitates the way the real-world voting is organized.

Its feature groups are:

- Creating, retrieving, updating and deleting countries in the system.
- Creating, retrieving, updating and deleting contests in the system.
- Creating, retrieving, updating and deleting broadcasts in the system.

## User roles

Two user roles - the EuroFan and the Admin - are defined for the purposes of defining user stories and writing acceptance test specifications.

### The EuroFan

The EuroFan is an authenticated user of the Public API. They are:

- A fan of the Eurovision Song Contest.
- Located anywhere in the world.
- Familiar with the structure and recent history of the Eurovision Song Contest.
- Knowledgeable about how the voting system works.
- Able to send HTTP requests and read responses.
- Interested in discovering insights from the voting data.

### The Admin

The Admin is the single authorized user of the Admin API. They are:

- A single software developer (Matt Tantony).
- The designer of the system, including its domain model and database schema.
- Located in the UK.
- Very familiar with the structure and recent history of the Eurovision Song Contest.
- Knowledgeable about how the voting system works.
- Able to send HTTP requests and read responses.
- Authorized to configure the web application and the database.

### Operational context

The system operates in the following context:

- A client communicates with the system using HTTP requests and responses.
- The language of the system is UK English.
- The web application is hosted in the cloud using the Azure Web App service.
- The system database is hosted in the cloud using an Azure SQL Database service.
- The web application and the database use free or very low-cost service tiers.
- Due to the service tier limitations, **the system database may not always be immediately available**. If this happens, the client must be directed to retry their request after 120 seconds.
