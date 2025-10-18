# 1. AI honesty box

This [development note](../README.md#development-notes) is a record of the uses I made of Microsoft's Copilot during the planning and development for the launch of *Eurocentric*.

## 2025/10/01: Domain events vs explicit handler

Asked Copilot to weigh up the pros and cons of publishing and consuming domain events while persisting changes to the database versus handling all command side effects in the same command handler then persisting changes. Opted to use domain events for clarity.

## 2025/10/01: EF Core behaviour

Asked Copilot how Entity Framework Core handles multiple retrievals of the same entity by the same database context. Satisfied that the same entity instance is used for all retrievals with the same identity.

## 2025/10/07: Dapper type mapping behaviour

Asked Copilot to explain how to configure Dapper to map a `Date` database type column to a `DateOnly` property type.
