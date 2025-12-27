# Commit message rules

This note is part of the [ways of working](../README.md#ways-of-working) used in the *Eurocentric* project.

It documents the Git commit message rules adopted during development.

## Conventional commits

1. Commit messages use the [Conventional Commits](https://www.conventionalcommits.org/en/v1.0.0/) standard.
2. Acceptable commit types are: *feat*, *fix*, *refactor*, *test*, *ci*, *deps*, *style*, *docs*, *chore*.
3. Acceptable feature scopes are: *admin-api*, *public-api*, *error-handling*, *open-api*, *security*, *versioning*.

## Message subject line

1. Subject line is at most 50 characters long.
2. Subject line's first word after colon is a *future participle verb*, such that the subject can be read as "When you apply the changes in this commit, you will \<subject\>".
3. Subject line is all lower-case, except for package and product names.
4. Subject line is followed by an empty line.

## Message body

1. Message body lines are at most 72 characters long.
2. Message body is written in the past tense.
3. Message body may tag one or more GitHub issues, e.g. `Closes: #72`.
4. Message body must *not* use colon character except for issue tagging.
5. Message body may contain an unordered list. This must be preceded and followed by an empty line, and must use hanging indentation.
6. Message body must enclose the name of a software type, namespace, or package in backticks.
7. Message body must enclose all other quoted strings in double quotation marks.

## Examples

```git
deps: bump TUnit to 1.3.25

Updated the `TUnit` package to version 1.3.25.
```

```git
feat(admin-api): add country creation

Implemented the "Create Country" feature.

The Admin is now able to create a new country in the system using
version 1.0 of the Admin API.

Closes: #40
```
