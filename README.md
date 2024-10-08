# RobustApiTemplate

This project is for me to test different techniques to use when building an API, to make it more robust and easier to support.

## Things I will be covering
- Secure key management
- Endpoint versioning
- Enforce HTTPS/TLS
- Authentication
- Authorization
- ~~Request size limits~~
- ~~Add CorrelationID to requests, visible downstream, for request/response/error logging~~
- Rate-limiting (to prevent DDoS)
- Logging requests, responses, and exceptions
- Prevent logging sensitive data
- Thorough request data validation and cleansing, with clear error responses
- Error-handling, with clear error responses
- Monitoring the service (including having a heartbeat endpoint)
- Sending out alerts
- Deployment/installation

## Possible feaures
- Replaying requests
 
## Things I won’t be covering (for now) 
- Maxing out performance
- Caching
- Scaling
- Containerization
- Load-balancing 
