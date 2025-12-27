# AI honesty box

This note is part of the [ways of working](../README.md#ways-of-working) used in the *Eurocentric* project.

It documents the ways in which the developer made use of AI during planning and development.

**No source code or test code was written by an AI.**

## 2025/12/22: Docker and CI

Asked Copilot to advise me on how to deploy my containerized web app from Docker Hub to the Azure Container Apps service. Copilot provided example instructions for configuring an Azure Container App, creating a User-Assigned Managed Identity on Azure, and granting access to GitHub. This makes it possible to deploy a new revision of the Azure Container App on every push to the remote repository main branch.
