# 3. Commit message rules

This [development note](../README.md#development-notes) is a record of the Git commit message rules used during launch development of *Eurocentric*.

## Conventional commits

1. Commit messages use the [Conventional Commits](https://www.conventionalcommits.org/en/v1.0.0/) standard.
2. Acceptable feature scopes are: *admin-api*, *public-api*, *errors*, *limiting*, *open-api*, *security*, *versioning*.

## Message subject line

1. Subject line is at most 50 characters long.
2. Subject line is all lower-case.
3. Subject line is followed by an empty line.

## Message body

1. Message body lines are at most 72 characters long.
2. Message body may tag one or more GitHub issues, e.g. `Closes: #72`.
3. Message body must use colon character except for issue tagging.
4. Message body may contain an unordered list. This must be preceded and followed by an empty line, and must use hanging indentation.
