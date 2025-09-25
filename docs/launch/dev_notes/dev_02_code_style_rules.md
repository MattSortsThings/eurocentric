# 2. Code style rules

This [development note](../README.md#development-notes) is a record of code style rules implemented during launch development of *Eurocentric*.

## Tools

- Code formatting is enforced using CSharpier.
- All other code style rules (e.g. naming, syntax) are enforced using an `.editorconfig` file.

## EF Core migrations

- Migration name is in lower snake case, comprised of letters, digits and underscores only.
- Migration name supplied to EF Core follows the convention "when you apply this migration to the database, it will \[name\]", for example `"add_dbo_tt_contest_stage_enum"`.
