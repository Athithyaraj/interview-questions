**1. AWS Architecture — Most Important**
# Q1. Explain how you would design a cloud-native payment application on AWS.

Senior-level answer:

I would design it using a combination of API Gateway, Lambda or ECS depending on workload, SQS/EventBridge for asynchronous communication, RDS for transactional relational data, DynamoDB for high-scale NoSQL use cases, S3 for documents, and CloudWatch for observability.

For example, in a payment platform, a request could come through API Gateway and reach a .NET service running on Lambda or ECS. Once a payment is accepted, I wouldn't perform every downstream operation synchronously. I would publish an event such as PaymentInitiated to EventBridge or SQS. Consumers can then independently perform fraud checks, notifications, reconciliation, and audit processing.

This reduces coupling and makes the system more resilient. If a downstream service is temporarily unavailable, the message can remain in SQS and be retried rather than losing the transaction.

Architecture to remember:
Client
   ↓
API Gateway
   ↓
.NET API / Lambda / ECS
   ↓
RDS / DynamoDB
   ↓
EventBridge
   ↓
SQS
   ↓
Consumer Lambda / .NET Worker
   ↓
Notification / FX / Reconciliation


**2. Lambda**
# Q2. What is AWS Lambda?

Lambda is a serverless compute service where AWS runs your code without you managing servers.

For your .NET background:

API Gateway
     ↓
AWS Lambda
     ↓
ASP.NET Core logic
     ↓
DynamoDB / S3 / RDS
Real-time example

Suppose Corpay receives a payment-status event.

Instead of running a server continuously:

Payment event
     ↓
EventBridge
     ↓
Lambda
     ↓
Update payment status
     ↓
Send notification

Lambda runs only when the event occurs.


# Q3. Lambda Cold Start
Q3. What is a cold start?

When Lambda hasn't been invoked recently, AWS needs to create a new execution environment before running the function.

That initial delay is called a cold start.

Subsequent invocations may reuse the environment.

How would you reduce cold starts?
Reduce deployment package size
Avoid unnecessary dependencies
Initialize only what is required
Use provisioned concurrency when appropriate
Optimize Lambda memory
Keep database connections efficient
Senior example

If a payment API has strict latency requirements, I might use provisioned concurrency for the critical Lambda rather than accepting unpredictable cold-start latency.

# Q4. Lambda vs ECS
Q4. When would you use Lambda vs ECS?
Lambda	ECS
Event-driven workloads	Long-running applications
Short-running processing	Long-running services
Serverless	Containerized
Automatic scaling	More control
SQS/EventBridge consumers	.NET APIs/workers
Minimal infrastructure management	Custom runtime requirements
Corpay example

For:

SQS → process payment event → update DB

Lambda can be excellent.

For:

High-throughput .NET API
Complex long-running process
Containerized microservice

ECS/Fargate may be more appropriate.


# Q5. API Gateway
Q5. Why use API Gateway?

API Gateway provides a managed entry point for APIs.

It can provide:

Authentication/authorization
Throttling
Routing
Request validation
Monitoring
Integration with Lambda/backend services
Real-world example
Mobile/Web
   ↓
API Gateway
   ↓
Authentication
   ↓
.NET Lambda
   ↓
Payment service

If thousands of clients suddenly call the API, API Gateway can help control and manage traffic.


# Q6. SQS — VERY IMPORTANT FOR CORPAY
Q6. Why would you use SQS?

SQS provides asynchronous communication between services.

Instead of:

Service A → Service B

we use:

Service A
   ↓
SQS
   ↓
Service B
Real-time payment example

Suppose a payment is created.

You don't want the API request to wait for:

fraud checking
notification
audit logging
reconciliation
reporting

Instead:

Payment API
     ↓
Payment Created
     ↓
SQS
     ↓
Fraud Service
Notification Service
Audit Service

This improves decoupling, scalability and resilience.


# Q7. SQS Visibility Timeout
Q7. What happens when a consumer receives an SQS message?

The message isn't immediately deleted.

It becomes temporarily invisible to other consumers.

This is controlled by:

Visibility Timeout

Example:

SQS message
     ↓
Consumer receives
     ↓
Message invisible for 60 sec
     ↓
Processing succeeds
     ↓
Delete message

If processing fails and the message isn't deleted:

Visibility timeout expires
        ↓
Message becomes visible again
        ↓
Retry


# Q8. Dead Letter Queue — VERY LIKELY
Q8. What is a DLQ?

A Dead Letter Queue stores messages that repeatedly fail processing.

Example:

SQS
 ↓
Lambda
 ↓
Processing fails
 ↓
Retry
 ↓
Retry
 ↓
Retry
 ↓
DLQ
Corpay example

Suppose an FX transaction contains invalid data.

Instead of retrying forever:

Main Queue
    ↓
Lambda
    ↓
Failure
    ↓
3 retries
    ↓
DLQ

Operations team can inspect the failed transaction.

Senior answer

I would configure a DLQ and an appropriate redrive policy so poison messages don't continuously consume processing capacity.

**Amazon Simple Queue Service (SQS)** is a fully managed message queuing service that lets you decouple and scale microservices, distributed systems, and serverless applications. 

How It Works Producers send data payloads (up to 256 KB) into an SQS queue.The Queue stores messages redundantly across multiple servers or Availability Zones.Consumers poll the queue, retrieve the messages, process them, and delete them. 

Key Features
Standard Queues: Offer high throughput and at-least-once delivery, though messages may occasionally arrive out of order.
FIFO Queues: Maintain exact first-in-first-out ordering and process messages strictly once.
Visibility Timeout: Hides a retrieved message temporarily so other workers do not process it twice.
Dead Letter Queues (DLQ): Captures messages that fail processing repeatedly after a set number of attempt


**SNS** most commonly refers to Amazon Simple Notification Service (AWS SNS), a cloud-based messaging and notification service, or a Social Networking Service.
s
Amazon Simple Notification Service (AWS SNS)What it does: Delivers messages from publishers (producers) to subscribers (consumers) using a publish-subscribe model.Key components: Topics (communication channels), publishers (message senders), and subscribers (message receivers like email, SMS, SQS, or Lambda).Use cases: Sending system alerts, mobile push notifications, and triggering event-driven architectures. 

Social Networking Service (SNS)What it does: An online platform that lets users connect, share content, and build social relations based on shared interests.Examples: Platforms like Facebook, Instagram, X (formerly Twitter), and LinkedIn.Business model: Typically driven by targeted online advertising fueled by user data.



# Q9. SNS vs SQS
Q9. Difference between SNS and SQS?

SQS = queue

SNS = publish/subscribe

Example:

Payment Service
      ↓
     SNS
   ↙ ↓ ↘
Email  Audit  Notification

With SQS:

Payment Service
      ↓
     SQS
      ↓
Consumer

SNS is useful when multiple consumers need the same event.

# Q10. What is EventBridge?

EventBridge is an event bus that allows applications and AWS services to communicate using events.

Example:

Payment Service
      ↓
PaymentCompleted event
      ↓
EventBridge
    ↙   ↓   ↘
Audit  FX   Notification
Real-world Corpay scenario

Imagine:

PaymentCompleted

Multiple systems may care about this event:

reconciliation
audit
customer notification
reporting
fraud monitoring

Instead of tightly coupling the payment service to every system, publish one event

# Q11. When would you use EventBridge vs SQS?

EventBridge:

I need event routing and multiple subscribers.

SQS:

I need reliable queue-based processing and workload buffering.

Example:

PaymentCompleted
       ↓
EventBridge
   ↙    ↓    ↘
SQS   SQS   Lambda

Each consumer can have its own queue.


## Amazon SQS vs SNS vs EventBridge

| Category | Amazon SQS | Amazon SNS | EventBridge |
| --- | --- | --- | --- |
| Communication model | Pull-based (consumers poll messages from the queue) | Push-based (subscribers receive messages when published) | Event-driven (rules match events and route to targets) |
| Persistence | Messages persist until consumed or expired | Messages do not persist; delivered in real-time to subscribers | Events do not persist; processed in real-time |
| Delivery guarantees | At-least-once delivery | At-least-once delivery for HTTP/S, exactly-once for Lambda and Amazon SQS | At-least-once delivery |
| Message ordering | FIFO (First-In-First-Out) queues ensure strict ordering | Amazon SNS FIFO topics guarantee order | No ordering guarantees |
| Message filtering | Amazon SQS cannot decide consumer based on message. Use Amazon SNS message filtering with Amazon SQS to achieve. | Message filtering using subscription filter policies based on message metadata and, for FIFO topics, message content | Complex event pattern matching and content-based filtering |
| Supported subscribers | Pull-based consumers (such as Amazon EC2 or Lambda) | HTTP/S endpoints, email, SMS, mobile push, Lambda, Amazon SQS | AWS services, Lambda, API destinations, event buses in other AWS accounts |
| Typical use cases | Decoupling microservices, buffering requests, processing tasks asynchronously | Fanout notifications, pub/sub messaging, mobile push notifications | Event-driven architectures, real-time stream processing, cross-account event sharing |
| Integration with other AWS services | Lambda, CloudWatch, AWS KMS, IAM | Lambda, Amazon SQS, Mobile Push, AWS KMS, IAM | Lambda, Step Functions, Amazon SQS, Amazon SNS, Kinesis, SageMaker AI, CloudWatch, IAM |


# Q12. Event-Driven Architecture
Q12. Explain event-driven architecture with a real example.

Suppose:

PaymentCreated

Instead of:

Payment Service
 ↓
Fraud Service
 ↓
Notification
 ↓
Accounting
 ↓
Audit

we do:

Payment Service
       ↓
PaymentCreated Event
       ↓
EventBridge
   ↓    ↓    ↓
Fraud  Audit  Notification

Each service is independent.

Benefits
Loose coupling
Scalability
Fault isolation
Independent deployments
Asynchronous processing

# Q13. Idempotency — VERY IMPORTANT FOR PAYMENTS
Q13. What is idempotency?

An operation is idempotent when processing the same request multiple times produces the same final result.

This is extremely important in payment systems.

Suppose:

PaymentId = P1001
Amount = ₹10,000

The client retries because of a network timeout.

Without idempotency:

Request 1 → debit ₹10,000
Request 2 → debit ₹10,000

Customer gets charged twice.

With idempotency:

Request 1 → process payment
Request 2 → recognize same idempotency key
             ↓
          return existing result

You can implement this using DynamoDB/RDS depending on architecture.


# Q14. DynamoDB
Q14. When would you use DynamoDB instead of RDS?

DynamoDB is useful when you need:

Very high throughput
Low latency
Flexible schema
Key-value/document access
Serverless architecture

Example:

IdempotencyKey → PaymentResult

could be stored in DynamoDB.

# Q15. DynamoDB Partition Key
Q15. What is a partition key?

The partition key determines where DynamoDB stores data.

Example:

PaymentId

could be the partition key.

PaymentId = P10001
Status = Completed
Amount = 1000

You need to design keys carefully to avoid hot partitions.


# Q16. RDS
Q16. Why use RDS?

RDS is appropriate when you need relational database capabilities.

For payment systems, relational databases can be useful for:

Transactions
Referential integrity
Complex queries
Structured financial data

Example:

Payment
Customer
Account
Transaction
Settlement

with relationships between them.



# Q17. RDS vs DynamoDB
Q17. Explain RDS vs DynamoDB.
RDS	DynamoDB
Relational	NoSQL
SQL	Key-value/document
Joins	No traditional joins
Transactions	Supports transactions
Structured schema	Flexible schema
Complex queries	Key-based access
Traditional applications	High-scale/serverless workloads
Senior answer

I wouldn't choose DynamoDB simply because it's AWS. I would choose based on access patterns and consistency requirements.


# Q18. S3
Q18. Give a real-world S3 use case.

Suppose Corpay needs to store:

invoices
payment documents
reports
reconciliation files

We can use:

.NET API
   ↓
S3
   ↓
Documents

S3 provides highly durable object storage.


# Q19. S3 Security
Q19. How would you secure an S3 bucket?

I would:

Block public access
Use IAM roles
Enable encryption
Use bucket policies carefully
Enable versioning where appropriate
Enable logging/auditing
Apply least privilege
Use presigned URLs when temporary access is needed



# Q20. IAM — VERY IMPORTANT
Q20. What is IAM?

IAM controls:

Who can access what?

Example:

Lambda
 ↓
IAM Role
 ↓
S3:GetObject

Instead of storing:

AWS_ACCESS_KEY
AWS_SECRET_KEY

inside application code.

Senior answer

For AWS workloads, I prefer IAM roles and temporary credentials rather than hardcoding long-lived access keys.


# Q21. IAM Least Privilege
Q21. What does least privilege mean?

Give only the permissions required.

Bad:

Lambda → AdministratorAccess

Good:

Lambda
 ↓
s3:GetObject
sqs:SendMessage
dynamodb:GetItem

Only what the Lambda actually needs.

# Q22. Secrets Manager
Q22. Where would you store database passwords?

Not:

appsettings.json

Not:

GitHub

Instead:

AWS Secrets Manager

Application:

.NET API
   ↓
Secrets Manager
   ↓
DB credentials

Secrets can also be rotated.


# Q23. Parameter Store vs Secrets Manager
Q23. Difference?

Parameter Store

Good for:

configuration
parameters
non-secret values

Secrets Manager

Good for:

passwords
API secrets
database credentials
secret rotation


# Q24. CloudWatch — VERY LIKELY
Q24. How do you monitor AWS applications?

I would use:

CloudWatch

for:

Logs
Metrics
Alarms
Dashboards

Example:

Lambda
 ↓
CloudWatch Logs
 ↓
Exception detected
 ↓
CloudWatch Alarm
 ↓
SNS notification


# Q25. Production Troubleshooting
Q25. Suppose Lambda is failing in production. What would you do?

Senior-level answer:

First I would identify whether the issue is application-level, infrastructure-level, dependency-related, or configuration-related.

Then:

CloudWatch Logs
       ↓
Correlation/request ID
       ↓
Exception
       ↓
Dependency check
       ↓
IAM/network/database check
       ↓
Root cause
       ↓
Fix
       ↓
Monitoring

I would also check:

Error rate
Duration
Memory
Throttling
SQS backlog
DLQ
downstream dependency failures


# Q26. Lambda Timeout
Q26. What happens if Lambda times out?

The invocation fails.

If triggered by SQS, the message can become visible again after visibility timeout and be retried.

Therefore:

Lambda timeout
      ↓
Retry
      ↓
Retry
      ↓
DLQ

This is why timeout + visibility timeout + retry + DLQ should be designed together.

Maximum Execution Time for lambda is Strictly 15 minutes (900 seconds).


# Q27. AWS Lambda Concurrency
Q27. What is concurrency?

Number of Lambda executions running simultaneously.

Example:

100 messages
     ↓
Lambda
     ↓
100 concurrent executions

You can control concurrency to protect downstream systems.

Corpay example

If your database can safely handle only 20 concurrent operations:

Lambda concurrency = 20

This prevents Lambda from overwhelming the database.

The default overall account concurrency limit for AWS Lambda is 1,000 concurrent executions per region.

Request a Limit Increase: If 1,000 is not enough for your entire account, you can request a quota increase from AWS Support for free. Accounts are routinely bumped up to tens of thousands of concurrent executions depending on use case.

# Q28. Reserved vs Provisioned Concurrency
Reserved

Limits/reserves concurrency for a Lambda.

Provisioned

Keeps execution environments initialized to reduce cold starts.

Good senior-level distinction.

# 29. AWS Step Functions — IMPORTANT

The JD mentions:

workflow automation / business process orchestration

This is a strong clue that they may ask Step Functions.

# Q29. What is Step Functions?

AWS Step Functions orchestrates multiple steps in a workflow.

Example payment workflow:

Payment Initiated
      ↓
Validate
      ↓
Fraud Check
      ↓
FX Conversion
      ↓
Payment Processing
      ↓
Settlement
      ↓
Notification

Step Functions can handle:

retries
failures
branching
waiting
parallel execution
state management
loop


# Q30. Step Functions vs EventBridge
Step Functions

Best for:

Workflow orchestration

Step 1 → Step 2 → Step 3 → Step 4
EventBridge

Best for:

Event routing

Event → multiple consumers


# Q31. AWS Kinesis

The JD mentions event-driven systems and AWS.

Q31. When would you use Kinesis?

For high-volume streaming data.

Example:

Transactions
     ↓
Kinesis Data Stream
     ↓
Consumers
     ↓
Analytics / Monitoring

Useful when you need continuous streams of events.


# Q32. Kinesis vs SQS

SQS:

Message queue

Typically a worker processes a message and removes it.

Kinesis:

Streaming platform

Multiple consumers can process the stream and records can be retained for a period.


# Q33. AWS CloudFormation

The JD explicitly requires:

Infrastructure as Code with AWS CloudFormation

Q32. What is CloudFormation?

CloudFormation allows infrastructure to be defined as code.

Instead of manually creating:

Lambda
SQS
DynamoDB
IAM
API Gateway

you define them in a template.

Resources:
  PaymentQueue:
    Type: AWS::SQS::Queue

Then AWS creates the infrastructure.

Benefits
Repeatability
Version control
Automation
Consistency
Easy environment creation


# Q34. Infrastructure as Code Real Example

You have:

DEV
QA
UAT
PROD

Instead of manually configuring each environment:

CloudFormation
      ↓
Same infrastructure template
      ↓
DEV
QA
UAT
PROD

with environment-specific parameters.

# Q35. ECS
Q33. What is ECS?

Elastic Container Service manages containerized applications.

For a .NET application:

.NET API
   ↓
Docker image
   ↓
ECR
   ↓
ECS
   ↓
Fargate


# Q36. ECR
Q34. What is ECR?

Elastic Container Registry stores Docker container images.

Docker build
    ↓
ECR
    ↓
ECS/Fargate

# Q37. ECS Fargate
Q35. What is Fargate?

Fargate lets you run containers without managing EC2 servers.

You specify:

CPU
Memory
Container image

AWS manages the underlying infrastructure.

# Q38. Kubernetes — Since JD explicitly mentions it
Q36. What is Kubernetes?

Kubernetes orchestrates containers.

It manages:

Deployment
Scaling
Networking
Service discovery
Health checks
Container lifecycle

AWS's managed Kubernetes service is:

EKS


# Q39. ECS vs EKS
ECS

AWS-native container orchestration.

EKS

Managed Kubernetes.

Senior answer:

If the organization is already standardized on Kubernetes and needs Kubernetes ecosystem capabilities, EKS can make sense. If the requirement is straightforward AWS-native container orchestration, ECS/Fargate can reduce operational complexity.


# Q40. AWS VPC
Q37. What is VPC?

Virtual Private Cloud provides an isolated network environment.

Example:

Internet
   ↓
ALB/API Gateway
   ↓
Public subnet
   ↓
Private subnet
   ↓
RDS

Databases should generally not be publicly accessible.

# Q41. Security Groups vs NACL
Security Group

Works at instance/resource level.

Stateful

NACL

Works at subnet level.

Stateless

Interview shortcut:

Security Groups are stateful and associated with resources, while NACLs are stateless and operate at subnet level.

# 42. NAT Gateway
Q38. Why use NAT Gateway?

Suppose your Lambda/ECS service is in a private subnet but needs to access the internet.

Private subnet
     ↓
NAT Gateway
     ↓
Internet

The resource can make outbound requests without being directly exposed to inbound internet traffic.

# 43. High Availability
Q39. How would you design a highly available AWS application?

I'd avoid single points of failure.

Example:

             Load Balancer
              /        \
          AZ-1          AZ-2
           ↓              ↓
        Service         Service
           \              /
              RDS

Use:

Multiple AZs
Auto Scaling
Managed services
Multi-AZ database
Health checks
Automated recovery
# 44. Scalability
Q40. How would you scale a payment system?

Instead of simply increasing server size:

Vertical scaling

I'd prefer horizontal scaling where appropriate:

             API
          /   |   \
       Service Service Service

For asynchronous workloads:

1000 payments
      ↓
     SQS
      ↓
Multiple consumers
# 45. Resilience
Q41. How do you make AWS applications resilient?

Use:

Retry policies
Exponential backoff
Dead-letter queues
Timeouts
Circuit breakers
Idempotency
Multi-AZ
Health checks
Graceful degradation
Excellent payment example

If the notification service is down, payment processing shouldn't necessarily fail. I would decouple notification using an asynchronous event. The payment succeeds, while notification processing can retry independently.

That's exactly the type of thinking Corpay is looking for.

# 46. Retry + Exponential Backoff
Q42. Why use exponential backoff?

Suppose an external FX service is temporarily unavailable.

Bad:

Retry immediately
Retry immediately
Retry immediately

This can overload the dependency.

Better:

1 sec
2 sec
4 sec
8 sec
16 sec

with jitter where appropriate.

# 47. Circuit Breaker
Q43. What is a circuit breaker?

If a downstream service repeatedly fails:

Service A → Service B ❌
Service A → Service B ❌
Service A → Service B ❌

Instead of continuously calling it:

Circuit OPEN

Requests fail fast or use fallback behavior.

After some time:

Half Open
     ↓
Test request
     ↓
Success
     ↓
Closed

Very relevant to distributed payment systems.

# 48. Observability
Q44. What does observability mean?

Three major pillars:

Logs
Metrics
Traces

For example:

Payment API
   ↓
Trace ID = ABC123
   ↓
Payment Service
   ↓
SQS
   ↓
Fraud Service

You can trace one transaction across services.

# 49. AWS X-Ray
Q45. What is AWS X-Ray?

X-Ray helps trace requests across distributed applications.

Example:

API Gateway
    ↓
Lambda
    ↓
DynamoDB
    ↓
SQS

You can identify where latency or failures occur.

# 50. CloudWatch Metrics

Possible metrics:

Lambda Errors
Lambda Duration
Lambda Throttles
SQS ApproximateNumberOfMessagesVisible
API Gateway 4xx
API Gateway 5xx
RDS CPU
RDS Connections
Senior answer

I wouldn't monitor only infrastructure metrics. I would also monitor business metrics such as payment failure rate, transaction processing latency, reconciliation failures and DLQ depth.

This is an excellent Corpay answer.

# 51. AWS Cost Optimization
Q46. How would you reduce AWS cost?

I'd look at:

Right-sizing
Serverless for suitable workloads
Auto Scaling
S3 lifecycle policies
Avoiding unnecessary data transfer
CloudWatch retention
Reserved/Savings Plans where appropriate
DynamoDB capacity model
Removing unused resources
# 52. Disaster Recovery
Q47. How would you design disaster recovery?

Depending on RTO/RPO requirements:

Primary Region
     ↓
Backup/Replication
     ↓
Secondary Region

Possible approaches:

Backup and restore
Pilot light
Warm standby
Active-active

For financial systems, the correct strategy depends heavily on business requirements and compliance.

# 53. AWS Deployment Pipeline
Q48. Explain how you would deploy a .NET AWS application.

A typical pipeline:

Developer
    ↓
Git
    ↓
CI Build
    ↓
Unit Tests
    ↓
Security Scan
    ↓
Build
    ↓
Deploy
    ↓
AWS
    ↓
Smoke Tests

For containers:

.NET
 ↓
Docker
 ↓
ECR
 ↓
ECS

For serverless:

.NET
 ↓
Package
 ↓
CloudFormation/SAM
 ↓
Lambda
# 54. Blue-Green Deployment
Q49. What is blue-green deployment?
BLUE = Current production
GREEN = New version

Deploy new version to GREEN.

Test it.

Then switch traffic:

BLUE
 ↓
GREEN

If something goes wrong:

GREEN
 ↓
BLUE

This reduces deployment risk.

# 55. Canary Deployment

Gradually release:

1% → 10% → 25% → 50% → 100%

Monitor errors/latency at every stage.

This is particularly useful for critical financial applications.

# 56. AWS Authentication in .NET
Q50. How does your .NET application authenticate to AWS services?

Prefer IAM roles.

For example:

.NET Lambda
     ↓
Execution Role
     ↓
DynamoDB

No credentials stored in source code.

If running locally, AWS SDK can use configured developer credentials/profile.

# 57. AWS SDK for .NET
Q51. How have you integrated AWS with .NET?

You can say:

In .NET applications, I use the AWS SDK for .NET to interact with AWS services. For example, I can use SDK clients for S3, DynamoDB, SQS, Lambda and other services and register them through dependency injection where appropriate.

Example architecture:

Controller
    ↓
Service
    ↓
AWS SDK client
    ↓
SQS/S3/DynamoDB

Avoid putting AWS SDK calls directly inside controllers.

# 58. Scenario: Payment API Suddenly Gets 10x Traffic
Q52. What would you do?

A strong answer:

First I'd identify whether the workload is synchronous or asynchronous. For synchronous APIs I'd use API Gateway/load balancing and horizontal scaling. For asynchronous workloads I'd buffer requests through SQS. I'd also check downstream database capacity because scaling the application alone could overload the database.

Then:

Traffic spike
    ↓
API Gateway
    ↓
Auto scaling
    ↓
SQS
    ↓
Consumers
    ↓
Database

Monitor:

Latency
Errors
Throttling
Queue depth
DB connections
# 59. Scenario: SQS Queue Suddenly Has 1 Million Messages
Q53. What would you investigate?

I'd check:

Producer rate
Consumer rate
Lambda concurrency
Lambda errors
Processing duration
Database bottleneck
Downstream dependency
DLQ
Visibility timeout

Then scale consumers carefully.

Important: Don't blindly increase concurrency if the database is already the bottleneck.

# 60. Scenario: Duplicate Payment
Q54. How would you prevent duplicate payments?

This is a must-prepare Corpay question.

Answer:

I would implement idempotency at the payment boundary and also design downstream consumers to be idempotent.

Example:

Client
 ↓
Idempotency-Key: ABC123
 ↓
Payment API
 ↓
Check idempotency store
 ↓
Already processed?
   ↓
YES → return existing result

For asynchronous processing:

Payment event
     ↓
Consumer
     ↓
Check event/payment ID
     ↓
Already processed?
     ↓
Skip
# 61. Scenario: AWS Service Goes Down
Q55. How would you handle a downstream AWS dependency failure?

Don't immediately fail the entire system.

Use:

Timeout
   ↓
Retry
   ↓
Exponential backoff
   ↓
Circuit breaker
   ↓
Fallback / Queue

For example, if notification processing fails:

Payment completed
       ↓
EventBridge
       ↓
Notification queue
       ↓
Notification service ❌
       ↓
Retry
       ↓
DLQ

Payment doesn't need to be rolled back simply because notification failed.

# 62. Scenario: Database Is Slow
Q56. Your API latency increased from 200ms to 5 seconds. What do you do?

I'd investigate from top to bottom:

API
 ↓
Application logs
 ↓
Database latency
 ↓
Query execution plan
 ↓
Connection pool
 ↓
CPU/Memory
 ↓
Locks

Also check:

CloudWatch metrics
recent deployments
traffic increase
slow queries
connection exhaustion
downstream APIs

Don't immediately increase EC2/Lambda memory without identifying the bottleneck.

# 63. Scenario: Security Breach
Q57. An AWS access key is accidentally committed to Git. What do you do?

Senior answer:

First I would treat the credential as compromised. I would immediately revoke/rotate it, identify where it was used, review CloudTrail activity, remove the secret from the repository, and replace the credential with IAM roles or Secrets Manager depending on the workload. I would also determine whether additional remediation is required.

# 64. CloudTrail
Q58. What is CloudTrail?

CloudTrail records AWS API activity.

For example:

Who changed an S3 bucket?
Who deleted an EC2 instance?
Who modified IAM?

CloudTrail helps with:

Auditing
Security investigation
Compliance
Troubleshooting

Very relevant to financial services.

# 65. KMS
Q59. What is AWS KMS?

Key Management Service manages encryption keys.

Used for encrypting:

S3 objects
databases
secrets
other AWS resources

For financial applications, encryption at rest is important.

# 66. Encryption at Rest vs Transit
At rest

Data stored in:

S3
RDS
DynamoDB

should be encrypted.

In transit

Use:

HTTPS/TLS

between services and clients.

# 67. AWS Shared Responsibility Model
Q60. What is shared responsibility?

AWS is responsible for security of the cloud.

Customer is responsible for security in the cloud, depending on the service.

Example:

AWS:

Physical data center
Infrastructure

Customer:

IAM permissions
Application code
Data
Security configuration
# 68. Architecture Question They Could Ask You
Q61. Design a payment processing system using AWS.

This is probably the single most important architecture question to practice.

I'd answer:

                 Client
                   ↓
              API Gateway
                   ↓
             .NET Payment API
                   ↓
              RDS/DynamoDB
                   ↓
          PaymentInitiated Event
                   ↓
              EventBridge
             ↙     ↓      ↘
           SQS    SQS     Lambda
            ↓      ↓
         Fraud   Audit
            ↓
        Payment Processor
            ↓
          Settlement

Then explain:

API Gateway

Entry point.

.NET service

Business logic and validation.

RDS

Transactional payment data.

EventBridge

Event distribution.

SQS

Reliable asynchronous processing.

Lambda

Event consumers.

DLQ

Failed messages.

CloudWatch

Monitoring.

IAM

Authorization.

KMS

Encryption.

Secrets Manager

Credentials.


# AWS Developer Services Cheat Sheet

## Related AWS Services and Important Features

| Service | Important feature to remember | Common development use |
| --- | --- | --- |
| IAM | Roles, policies, least privilege, temporary credentials | Secure access between applications and AWS services |
| KMS | Customer managed keys, encryption at rest, key policies, rotation | Encrypt S3, RDS, DynamoDB, and application secrets |
| Secrets Manager | Secret rotation and encrypted secret storage | Store database passwords, API keys, and tokens |
| Systems Manager Parameter Store | Configuration values and SecureString parameters | Store environment configuration and non-secret settings |
| VPC | Subnets, route tables, Internet Gateway, NAT Gateway, security groups, and NACLs | Isolate applications and control network traffic |
| API Gateway | Authentication, throttling, stages, authorizers, and request routing | Expose Lambda or container APIs |
| Lambda | Event-driven serverless compute, automatic scaling, retries, and concurrency controls | Run short-lived APIs, workers, and event consumers |
| ECS | Task definitions, services, deployments, and service auto scaling | Run Docker containers on AWS |
| Fargate | Serverless container runtime without managing EC2 instances | Run ECS tasks with reduced infrastructure management |
| ECR | Private container image registry, image scanning, and lifecycle policies | Store images used by ECS, EKS, and batch workloads |
| EC2 | Instance types, AMIs, user data, EBS, and Auto Scaling Groups | Run full-control virtual machines |
| S3 | Object storage, versioning, lifecycle rules, encryption, and event notifications | Store documents, backups, static files, and reports |
| SQS | Durable queue, visibility timeout, retries, DLQ, and FIFO ordering | Buffer work and decouple producers from consumers |
| SNS | Pub/sub topics, fanout, subscription filtering, and FIFO topics | Send one event to multiple subscribers |
| EventBridge | Event buses, event patterns, rules, targets, archives, and cross-account routing | Route events between decoupled services |
| Step Functions | State machines, retries, catches, waits, parallel branches, and orchestration | Coordinate multi-step business workflows |
| DynamoDB | Partition key design, GSIs, LSIs, TTL, streams, and conditional writes | Low-latency key-value or document workloads |
| RDS | Transactions, SQL, Multi-AZ, read replicas, backups, and parameter groups | Relational transactional data |
| Aurora | Managed relational database compatible with MySQL or PostgreSQL | High-performance relational workloads |
| ElastiCache | Redis or Memcached in-memory caching | Reduce database load and improve response time |
| Kinesis Data Streams | Shards, ordered records, replay, and multiple consumers | Process high-volume real-time streams |
| CloudWatch | Logs, metrics, alarms, dashboards, and log insights | Monitor applications and trigger operational alerts |
| CloudTrail | AWS API audit history | Investigate changes, access, and security incidents |
| X-Ray | Distributed tracing and service maps | Find latency and failures across services |
| CloudFormation | Infrastructure as Code, stacks, parameters, and change sets | Recreate consistent environments safely |
| EKS | Managed Kubernetes control plane | Run Kubernetes workloads when the ecosystem is required |

## Important Resource Limits and Quotas

These are common default or service limits used in interviews. Account, Region, API type, and quota increases can change some values. Confirm production values in AWS Service Quotas and the service documentation.

| Service/resource | Important limit or quota | Interview point |
| --- | --- | --- |
| Lambda timeout | Maximum 15 minutes (900 seconds) | Use ECS, Fargate, or Step Functions for long-running work |
| Lambda memory | 128 MB to 10,240 MB | More memory also provides more CPU |
| Lambda temporary storage | `/tmp` from 512 MB to 10,240 MB | Temporary only; do not treat it as durable storage |
| Lambda synchronous payload | 6 MB request and 6 MB response | Use S3 for large request or response data |
| Lambda asynchronous payload | 256 KB | Put large events in S3 and send a reference |
| Lambda concurrency | 1,000 concurrent executions per Region by default | Reserved concurrency protects dependencies; quota can be increased |
| API Gateway timeout | About 29 seconds for Regional and private REST APIs | Do not keep clients waiting for long background work |
| API Gateway payload | 10 MB for REST API requests | Upload large files directly to S3 with presigned URLs |
| SQS message size | 256 KB | Store large payloads in S3 and send an object reference |
| SQS message retention | 1 minute to 14 days | Configure retention and a DLQ for failed messages |
| SQS visibility timeout | 0 seconds to 12 hours | Set it longer than normal processing time and extend when needed |
| SQS batch size | Up to 10 messages for `SendMessageBatch` and standard Lambda event source mapping | Batch processing improves throughput but requires idempotency |
| SNS message size | 256 KB | Use fanout for notifications and SQS for durable processing |
| EventBridge event size | 256 KB | Keep events small and place large data in S3 |
| EventBridge archive retention | Configurable; use archives and replay for recovery | Event buses route events, while archives provide replay capability |
| Step Functions Standard execution | Maximum execution duration of 1 year | Suitable for durable workflows and human approval steps |
| Step Functions Express execution | Maximum execution duration of 5 minutes | Suitable for high-volume short workflows |
| Step Functions state input/output | 256 KB per state input or output | Store large workflow data in S3 |
| DynamoDB item size | 400 KB, including attribute names | Store large documents in S3 and keep metadata in DynamoDB |
| DynamoDB Query/Scan page | 1 MB per response page | Paginate results and avoid unbounded scans |
| DynamoDB transaction | Up to 100 unique items or 4 MB | Keep transactions small and design for access patterns |
| S3 object size | Up to 5 TB | Use multipart upload for large objects |
| S3 single PUT upload | Up to 5 GB | Use multipart upload above this size |
| Kinesis record size | Up to 1 MB | Split or reference larger records |
| Kinesis shard write throughput | 1 MB/second or 1,000 records/second per shard | Increase shards or use on-demand capacity for higher throughput |
| Kinesis shard read throughput | 2 MB/second per shard for shared throughput | Use enhanced fan-out when consumers need dedicated read throughput |
| RDS storage | Engine and configuration dependent | Check engine-specific storage and IOPS limits before sizing |
| ECR image size | Container image size depends on the runtime and deployment service | Keep images small with multi-stage builds and lifecycle cleanup |
| CloudFormation template size | 51,200 bytes when uploaded directly | Upload larger templates to S3 |
| IAM policy size | Policy size quotas vary by policy type | Use managed policies carefully and keep permissions least-privileged |

## Quick Selection Rules

| Requirement | Prefer |
| --- | --- |
| Synchronous API endpoint | API Gateway + Lambda, ECS, or Fargate |
| Short event-driven function | Lambda |
| Long-running container or worker | ECS/Fargate or EKS |
| Durable asynchronous work queue | SQS |
| One message delivered to many subscribers | SNS fanout |
| Content-based event routing | EventBridge |
| Ordered stream with replay and multiple consumers | Kinesis Data Streams |
| Multi-step workflow with retries and branching | Step Functions |
| Key-value access at very high scale | DynamoDB |
| Relational transactions and joins | RDS or Aurora |
| Object and document storage | S3 |
| Secrets and credential rotation | Secrets Manager |
| Application configuration | Systems Manager Parameter Store |
| Metrics, logs, alarms, and dashboards | CloudWatch |
| Audit trail of AWS API calls | CloudTrail |