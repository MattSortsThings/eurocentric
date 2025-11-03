# 1. AI honesty box

This [development note](../README.md#development-notes) is a record of the uses I made of Microsoft's Copilot during the planning and development for the launch of *Eurocentric*.

## 2025/10/01: Domain events vs explicit handler

Asked Copilot to weigh up the pros and cons of publishing and consuming domain events while persisting changes to the database versus handling all command side effects in the same command handler then persisting changes. Opted to use domain events for clarity.

## 2025/10/01: EF Core behaviour

Asked Copilot how Entity Framework Core handles multiple retrievals of the same entity by the same database context. Satisfied that the same entity instance is used for all retrievals with the same identity.

## 2025/10/07: Dapper type mapping behaviour

Asked Copilot to explain how to configure Dapper to map a `Date` database type column to a `DateOnly` property type.

## 2025/10/19: Azure Web App service query

The web app, when deployed to the Azure Web App service, failed to start. Asked Copilot for advice on how to debug this. Followed Copilot's recommendation to check the Azure Portal log, which revealed the problem: the Azure Web App service is still using .NET 10 RC1, whereas the system is uses RC2. Decided to wait until the official release of .NET 10 in November.

## 2025/10/20: Combining authorization policies

Asked Copilot to explain how to combine multiple existing authorization policies for an endpoint group.

## 2025/11/02: Dependency injection

Asked Copilot to demonstrate several patterns for registering a given service class as each of the interfaces that it implements.
