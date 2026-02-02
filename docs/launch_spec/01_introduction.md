# 1. Introduction

This document is part of the [*Eurocentric* launch specification](README.md).

- [1. Introduction](#1-introduction)
  - [System overview](#system-overview)
  - [The queryable voting data](#the-queryable-voting-data)
  - [Operational context](#operational-context)
  - [Feature scopes](#feature-scopes)
  - [User personas](#user-personas)
  - [Out of scope](#out-of-scope)

## System overview

*Eurocentric* (the system) is a web API for modelling and analysing the Eurovision Song Contest, 2016-present.

The system is a web application that hosts two separate web APIs - the Public API and the Admin API - operating on a shared database and domain.

The Public API exposes a large number of GET endpoints, each of which allows the user to execute an analytical query on voting data stored in the system database.

The Admin API exposes GET/POST/PATCH/DELETE endpoints that collectively allow the user to populate the system with its queryable voting data and monitor its use.

## The queryable voting data

The Admin API populates the system with voting data split across three aggregate types: Countries, Contests and Broadcasts. The Public API operates on a subset of this data, known as the *queryable voting data*.

A Contest is queryable if and only if all 3 of its child Broadcasts have been created and all their points have been awarded.

A Broadcast is queryable if and only if its parent Contest is queryable.

A Country is queryable if and only if it has a role in one or more queryable Contests.

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

|       Name       | Definition                               |
|:----------------:|:-----------------------------------------|
|   *admin-api*    | Admin API endpoints and event handlers   |
|   *public-api*   | Public API endpoints                     |
| *error-handling* | User error and exception handling        |
|    *logging*     | API request/response logging             |
|    *open-api*    | OpenAPI documents and Scalar pages       |
|    *security*    | API key authentication and authorization |
|   *versioning*   | API endpoint versioning                  |

## User personas

The following user personas are defined:

|  Name   | Feature scopes                                                    | Definition                                                                                                                                                                                                                                                                                  |
|:-------:|:------------------------------------------------------------------|:--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
|  Admin  | *admin-api*                                                       | The owner and developer of the system, with sole administrative privileges. Aims to populate the system with accurate voting data using the Admin API. Has detailed knowledge of recent Eurovision history. Knows the system domain model and its invariants.                               |
| EuroFan | *public-api*                                                      | An anonymous web user located anywhere in the world, with reader privileges only. Aims to learn about recent Eurovision history using the Public API. Reasonably familiar with Eurovision's voting rules and recent history. Does not know about the system domain model or its invariants. |
|   Dev   | *error-handling*, *logging*, *open-api*, *security*, *versioning* | The owner and developer of the system. Aims to create a robust, well-structured web application that hosts the Admin API and the Public API.                                                                                                                                                |

## Out of scope

The following features are *out of scope* for the launch of *Eurocentric*:

| Feature                         | Reason                                                                                                                |
|:--------------------------------|:----------------------------------------------------------------------------------------------------------------------|
| Database concurrency management | Only a single user (the Admin) will ever cause INSERT/UPDATE/DELETE statements to be executed in the system database. |
| Rate limiting                   | The Azure Container App is configured to scale to zero when it is not in use.                                         |
| Response caching                | The Azure Container App is configured to scale to zero when it is not in use.                                         |
