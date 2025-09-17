# 1. AI honesty box

This [development note](README.md#development-notes) is a record of the uses I made of Copilot during the planning and development for the launch of *Eurocentric*.

## 2025/09/12: Domain events vs explicit handler

Asked Copilot to weigh up the pros and cons of publishing and consuming domain events versus handling all command side effects in the same command handler. Opted to use domain events for clarity.

## 2025/09/12: EF Core behaviour

Asked Copilot how Entity Framework Core handles multiple retrievals of the same entity by the same database context. Satisfied that the same entity instance is used for all retrievals with the same identity.
