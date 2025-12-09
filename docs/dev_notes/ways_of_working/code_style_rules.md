# Code style rules

This note is part of the [ways of working](../README.md#ways-of-working) used in the *Eurocentric* project.

It documents the high-level code style rules adopted during development.

## Code files

1. The maximum line length is 120 characters.
2. Text formatting in code files is uniformly imposed using [CSharpier](https://csharpier.com/).
3. All other style rules (e.g. naming, syntax) are enforced using a single root-level `.editorconfig` file.

## EF Core

1. When creating a new migration using the EF Core CLI, the first character is an upper-case letter; all other characters are lower-case letters, digits, or underscores.
2. The first word of the migration name is a *future participle verb*, such that the name can be read as "When you apply the changes in this migration, you will \<name\>".
3. The name of a migration should include the name of the database schema.

**Examples:**

```bash
dotnet ef migrations add Create_dbo_country_aggregate_tables
```

```bash
dotnet ef migrations add Drop_dbo_usp_get_competitor_points_average_rankings
```

## Database names

1. Database tables and columns use lower-case snake-case formatting.
2. The name of a stored procedure begins with `usp_`.
3. The name of a user-defined function begins with `udf_`, irrespective of whether it returns a scalar value or a table.

## XML documentation comments

1. No code shall be committed to version control without proper XML documentation comments.
2. The developer will *never* adopt the "I can add documentation once I know everything else is finished" mindset.
