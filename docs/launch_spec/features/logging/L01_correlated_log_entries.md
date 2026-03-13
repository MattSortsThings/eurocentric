# L01. Correlated log entries

This document is part of the [launch specification](../../README.md).

## User story

- **As the Dev**
- **I want** every API HTTP request handled by the system to generate timestamped log entries at various points, all with a single correlation ID returned to the user an `"X-Correlation-Id"` HTTP response header
- **So that** endpoint usage is tracked and I can trace any request's path through the system.

## Acceptance criteria

### Request Succeeds

**Any API endpoint...**

- [ ] Returns_response_with_XCorrelationId_header_when_request_succeeds
- [ ] Writes_HttpRequest_logEntry_when_request_succeeds
- [ ] Writes_InternalRequest_logEntry_when_request_succeeds
- [ ] Writes_InternalResponse_logEntry_when_request_succeeds
- [ ] Writes_HttpResponse_logEntry_when_request_succeeds
- [ ] Writes_no_Exception_logEntry_when_request_succeeds

### Request Fails : HTTP Pipeline : Not Authenticated

**Any API endpoint...**

- [ ] Returns_response_with_XCorrelationId_header_when_request_is_not_authenticated
- [ ] Writes_HttpRequest_logEntry_when_request_is_not_authenticated
- [ ] Writes_no_InternalRequest_logEntry_when_request_is_not_authenticated
- [ ] Writes_no_InternalResponse_logEntry_when_request_is_not_authenticated
- [ ] Writes_HttpResponse_logEntry_when_request_is_not_authenticated
- [ ] Writes_no_Exception_logEntry_when_request_is_not_authenticated

### Request Fails : HTTP Pipeline : Not Authorized

**Any API endpoint...**

- [ ] Returns_response_with_XCorrelationId_header_when_request_is_not_authorized
- [ ] Writes_HttpRequest_logEntry_when_request_is_not_authorized
- [ ] Writes_no_InternalRequest_logEntry_when_request_is_not_authorized
- [ ] Writes_no_InternalResponse_logEntry_when_request_is_not_authorized
- [ ] Writes_HttpResponse_logEntry_when_request_is_not_authorized
- [ ] Writes_no_Exception_logEntry_when_request_is_not_authorized

### Request Fails : HTTP Pipeline: Endpoint Not Found

**Any API endpoint...**

- [ ] Returns_response_with_XCorrelationId_header_when_request_matches_no_endpoint
- [ ] Writes_HttpRequest_logEntry_when_request_matches_no_endpoint
- [ ] Writes_no_InternalRequest_logEntry_when_request_matches_no_endpoint
- [ ] Writes_no_InternalResponse_logEntry_when_request_matches_no_endpoint
- [ ] Writes_HttpResponse_logEntry_when_request_matches_no_endpoint
- [ ] Writes_no_Exception_logEntry_when_request_matches_no_endpoint

### Request Fails : HTTP Pipeline : BadHttpRequestException

**Any API endpoint...**

- [ ] Returns_response_with_XCorrelationId_header_when_request_fails_on_BadHttpRequestException
- [ ] Writes_HttpRequest_logEntry_when_request_fails_on_BadHttpRequestException
- [ ] Writes_no_InternalRequest_logEntry_when_request_fails_on_BadHttpRequestException
- [ ] Writes_no_InternalResponse_logEntry_when_request_fails_on_BadHttpRequestException
- [ ] Writes_HttpResponse_logEntry_when_request_fails_on_BadHttpRequestException
- [ ] Writes_no_Exception_logEntry_when_request_fails_on_BadHttpRequestException

### Request Fails : Internal Pipeline : InvalidEnumArgumentException

**Any API endpoint...**

- [ ] Returns_response_with_XCorrelationId_header_when_request_fails_on_InvalidEnumArgumentException
- [ ] Writes_HttpRequest_logEntry_when_request_fails_on_InvalidEnumArgumentException
- [ ] Writes_InternalRequest_logEntry_when_request_fails_on_InvalidEnumArgumentException
- [ ] Writes_no_InternalResponse_logEntry_when_request_fails_on_InvalidEnumArgumentException
- [ ] Writes_HttpResponse_logEntry_when_request_fails_on_InvalidEnumArgumentException
- [ ] Writes_no_Exception_logEntry_when_request_fails_on_InvalidEnumArgumentException

### Request Fails : Internal Pipeline : Timeout DbUpdateException

**Any API endpoint...**

- [ ] Returns_response_with_XCorrelationId_header_when_request_fails_on_timeout_DbUpdateException
- [ ] Writes_HttpRequest_logEntry_when_request_fails_on_timeout_DbUpdateException
- [ ] Writes_InternalRequest_logEntry_when_request_fails_on_timeout_DbUpdateException
- [ ] Writes_no_InternalResponse_logEntry_when_request_fails_on_timeout_DbUpdateException
- [ ] Writes_HttpResponse_logEntry_when_request_fails_on_timeout_DbUpdateException
- [ ] Writes_no_Exception_logEntry_when_request_fails_on_timeout_DbUpdateException

### Request Fails : Internal Pipeline : Non Timeout DbUpdateException

**Any API endpoint...**

- [ ] Returns_response_with_XCorrelationId_header_when_request_fails_on_non_timeout_DbUpdateException
- [ ] Writes_HttpRequest_logEntry_when_request_fails_on_non_timeout_DbUpdateException
- [ ] Writes_InternalRequest_logEntry_when_request_fails_on_non_timeout_DbUpdateException
- [ ] Writes_no_InternalResponse_logEntry_when_request_fails_on_non_timeout_DbUpdateException
- [ ] Writes_HttpResponse_logEntry_when_request_fails_on_non_timeout_DbUpdateException
- [ ] Writes_Exception_logEntry_when_request_fails_on_non_timeout_DbUpdateException

### Request Fails : Internal Pipeline : General Exception

**Any API endpoint...**

- [ ] Returns_response_with_XCorrelationId_header_when_request_fails_on_general_Exception
- [ ] Writes_HttpRequest_logEntry_when_request_fails_on_general_Exception
- [ ] Writes_InternalRequest_logEntry_when_request_fails_on_general_Exception
- [ ] Writes_no_InternalResponse_logEntry_when_request_fails_on_general_Exception
- [ ] Writes_HttpResponse_logEntry_when_request_fails_on_general_Exception
- [ ] Writes_Exception_logEntry_when_request_fails_on_general_Exception

### Request Fails : Internal Pipeline : Undefined DomainError

**Any API endpoint...**

- [ ] Returns_response_with_XCorrelationId_header_when_request_fails_with_Undefined_DomainError
- [ ] Writes_HttpRequest_logEntry_when_request_fails_with_Undefined_DomainError
- [ ] Writes_InternalRequest_logEntry_when_request_fails_with_Undefined_DomainError
- [ ] Writes_InternalResponse_logEntry_when_request_fails_with_Undefined_DomainError
- [ ] Writes_HttpResponse_logEntry_when_request_fails_with_Undefined_DomainError
- [ ] Writes_no_Exception_logEntry_when_request_fails_with_Undefined_DomainError

### Request Fails : Internal Pipeline : NotFound DomainError

**Any API endpoint...**

- [ ] Returns_response_with_XCorrelationId_header_when_request_fails_with_NotFound_DomainError
- [ ] Writes_HttpRequest_logEntry_when_request_fails_with_NotFound_DomainError
- [ ] Writes_InternalRequest_logEntry_when_request_fails_with_NotFound_DomainError
- [ ] Writes_InternalResponse_logEntry_when_request_fails_with_NotFound_DomainError
- [ ] Writes_HttpResponse_logEntry_when_request_fails_with_NotFound_DomainError
- [ ] Writes_no_Exception_logEntry_when_request_fails_with_NotFound_DomainError

### Request Fails : Internal Pipeline : Extrinsic DomainError

**Any API endpoint...**

- [ ] Returns_response_with_XCorrelationId_header_when_request_fails_with_Extrinsic_DomainError
- [ ] Writes_HttpRequest_logEntry_when_request_fails_with_Extrinsic_DomainError
- [ ] Writes_InternalRequest_logEntry_when_request_fails_with_Extrinsic_DomainError
- [ ] Writes_InternalResponse_logEntry_when_request_fails_with_Extrinsic_DomainError
- [ ] Writes_HttpResponse_logEntry_when_request_fails_with_Extrinsic_DomainError
- [ ] Writes_no_Exception_logEntry_when_request_fails_with_Extrinsic_DomainError

### Request Fails : Internal Pipeline : Intrinsic DomainError

**Any API endpoint...**

- [ ] Returns_response_with_XCorrelationId_header_when_request_fails_with_Intrinsic_DomainError
- [ ] Writes_HttpRequest_logEntry_when_request_fails_with_Intrinsic_DomainError
- [ ] Writes_InternalRequest_logEntry_when_request_fails_with_Intrinsic_DomainError
- [ ] Writes_InternalResponse_logEntry_when_request_fails_with_Intrinsic_DomainError
- [ ] Writes_HttpResponse_logEntry_when_request_fails_with_Intrinsic_DomainError
- [ ] Writes_no_Exception_logEntry_when_request_fails_with_Intrinsic_DomainError
