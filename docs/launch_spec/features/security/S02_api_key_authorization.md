# S02. API key authorization

## User story

- **As the Dev**, having defined user roles and associated them with the demo and secret API keys
- **I want** every request to an Admin API and Public API endpoint to be authorized based on the role(s) granted from the API key
- **So that** only the Admin can use the Admin API.

## Acceptance criteria

**Admin API endpoint...**

- [ ] Should_not_authorize_request_using_demo_API_key
- [ ] Should_authorize_request_using_secret_API_key

**Public API endpoint...**

- [ ] Should_authorize_request_using_demo_API_key
- [ ] Should_authorize_request_using_secret_API_key
