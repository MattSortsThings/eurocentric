# Introduction

This document is part of the *Eurocentric* [launch specification](README.md).

- [Introduction](#introduction)
  - [System overview](#system-overview)
  - [Operational context](#operational-context)
  - [Feature scopes](#feature-scopes)
  - [User personas](#user-personas)
  - [Out of scope](#out-of-scope)

## System overview

*Eurocentric* (the system) is a web API for modelling and (over)analysing the Eurovision Song Contest, 2016-present.

The system comprises a single web application that hosts two separate web APIs - the Public API and the Admin API - operating on a shared database and domain.

The Public API exposes a large number of GET endpoints, each of which allows the user to execute an analytical query on voting data stored in the system database.

The Admin API exposes GET/POST/PATCH/DELETE endpoints that collectively allow the user to populate the system with queryable voting data and monitor its use.

## Operational context

The system operates in the following context:

- A user communicates with the system using HTTP requests and responses to and from its APIs.
- The language of the system is UK English.
- The web application is published to Docker Hub as a Docker image.
- The containerized web application is hosted in the cloud as an Azure Container App.
- The system database is hosted in the cloud as an Azure SQL Database.
- The web application and the system database use free or very low-cost service tiers.
- Due to service tier limitations, the web application and the system database will scale to zero when they are not in use.

## Feature scopes

The following feature scopes are defined:

|       Name       | Definition                                     |
|:----------------:|:-----------------------------------------------|
|   *admin-api*    | Admin API endpoints and event handlers.        |
|   *public-api*   | Public API endpoints.                          |
| *documentation*  | OpenAPI documents and documentation endpoints. |
| *error-handling* | User error and exception handling.             |
|    *logging*     | API request/response logging.                  |
|    *security*    | API key authentication and authorization.      |
|   *versioning*   | API endpoint versioning.                       |

## User personas

The following user personas are defined:

|  Name   | Feature scopes                                                         | Definition                                                                                                                                                                                                                                                          |
|:-------:|:-----------------------------------------------------------------------|:--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
|  Admin  | *admin-api*                                                            | The owner and developer of the system, with sole administrative privileges. Aims to populate the system with accurate voting data using the Admin API. Knows a lot about recent Eurovision history. Knows the system domain model and its invariants.               |
| EuroFan | *public-api*                                                           | An anonymous web user located anywhere in the world, with reader privileges only. Aims to learn about recent Eurovision history using the Public API. Reasonably familiar with how Eurovision works. Does not know about the system domain model or its invariants. |
|   Dev   | *documentation*, *error-handling*, *logging*, *security*, *versioning* | The owner and developer of the system. Aims to create a robust, well-structured web application that hosts the Admin API and the Public API.                                                                                                                        |

## Out of scope

The following features are *out of scope* for the launch of *Eurocentric*:

| Feature              | Reason                                                                                                                |
|:---------------------|:----------------------------------------------------------------------------------------------------------------------|
| Database concurrency | Only a single user (the Admin) will ever cause INSERT/UPDATE/DELETE statements to be executed in the system database. |
| Rate limiting        | The Azure Container App is configured to scale to zero when it is not in use.                                         |
| Response caching     | The Azure Container App is configured to scale to zero when it is not in use.                                         |
