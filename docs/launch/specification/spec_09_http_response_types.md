# 9. HTTP response types

This document is part of the [launch specification](../README.md#launch-specification).

- [9. HTTP response types](#9-http-response-types)
  - [Successful responses](#successful-responses)
  - [Unsuccessful responses](#unsuccessful-responses)

## Successful responses

When an API endpoint successfully handles a request, the response follows the examples in the table below.

| HTTP method | Response status code | Response body                  | Response headers  |
|:-----------:|:--------------------:|:-------------------------------|:------------------|
|    `GET`    |         200          | Requested data                 | None              |
|   `POST`    |         201          | Full model of created resource | Resource location |
|  `DELETE`   |         204          | None                           | None              |
|   `PATCH`   |         204          | None                           | None              |

## Unsuccessful responses

When an API unsuccessfully handles a request, the response follows the examples in the table below.

| Response status code | Meaning                                                                                                                                                                                                                                                       | Example(s)                                                                                                                                          |
|:--------------------:|:--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|:----------------------------------------------------------------------------------------------------------------------------------------------------|
|         400          | "I can't understand what the request is asking me to do. This includes situations when a `BadHttpRequestException` or `InvalidEnumArgumentException` was thrown."                                                                                             | Query string missing required parameter. Request body missing required property. Passing an illegal string value for an enum property or parameter. |
|         401          | "I can't authenticate the client."                                                                                                                                                                                                                            | Using an unrecognized API key.                                                                                                                      |
|         403          | "I have authenticated the client but they are not authorized to make the request. "                                                                                                                                                                           | Not using SECRET_API_KEY.                                                                                                                           |
|         404          | "The request is referencing a resource by ID that doesn't exist."                                                                                                                                                                                             | Creating a **CONTEST** with a **Participant** referencing a non-existent **COUNTRY**.                                                               |
|         409          | "I've understood the request, but I can't execute it because doing so would break one or more business rules given the current state of the resource being modified and/or all existing resources. In other words, the request is **extrinsically illegal**." | Creating a **CONTEST** with a non-unique contest year. Awarding points in a **BROADCAST** for a **Jury** that has already awarded its points.       |
|         422          | "I've understood the request, but I can't execute it because one or more of the elements of the request are by themselves incompatible with one or more business rules. In other words, the request is **intrinsically illegal**."                            | Creating a **COUNTRY** with an illegal country code value. Creating a **CONTEST** with two **Participants** referencing the same **COUNTRY**.       |
|         429          | "The authenticated client has sent too many requests. Try again after \[duration\] seconds."                                                                                                                                                                  | Client using DEMO_API_KEY sends excessive requests to Public API inside 60 seconds.                                                                 |                                                                                        |
|         500          | "An unexpected exception was thrown, which is not a `BadHttpRequestException` or an `InvalidEnumArgumentException` or a `SqlException` caused by a database connection timing out."                                                                           | Divide by zero.                                                                                                                                     |
|         503          | "A `SqlException` was thrown because the database connection timed out. Try again after \[duration\] seconds."                                                                                                                                                | Database is unavailable.                                                                                                                            |
