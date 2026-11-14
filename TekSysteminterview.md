# self-introduction
II have around 5 years of experience in software engineering, specializing in C#, .NET Core, AWS, SQL, NoSQL, REST APIs, and frontend technologies like Angular.

I started my career at **Novac Technology Solutions**, where I worked on building a serverless e-learning application using AWS. I developed REST APIs using AWS Lambda and API Gateway. For database management, we used DynamoDB, while SQS and SNS were used for asynchronous communication. We also used EventBridge for event-driven workflows and Step Functions for workflow orchestration.

My responsibilities included developing highly available, scalable, secure, and cost-optimized REST APIs by integrating various AWS services. I was also involved in migrating data from a SQL database to NoSQL using an ETL process.

Currently, I am working at **EPAM Systems** for the Thomson Reuters client in the Tax and Accounting domain.

We are modernizing a legacy monolithic application by migrating it to a microservices-based architecture using newer .NET technologies. My responsibilities include migrating the existing codebase to .NET 10, following clean code principles and design patterns such as Repository and CQRS.

I also work on application performance improvements, database query optimization, troubleshooting, and migrating existing stored procedures to LINQ queries using Entity Framework Core. Through these modernization and optimization efforts, we achieved approximately 60% performance improvement compared to the legacy monolithic application.

Additionally, we developed a .NET add-in application that allows customers to work directly with Excel and Word documents with interactive features. I also contribute to Angular development by implementing new features and supporting end-to-end client issues.

On the professional development side, I have completed the **AWS Certified Developer – Associate** certification. I actively use AI-powered development tools such as Claude Code and GitHub Copilot to improve my development productivity. I am also continuously learning about Generative AI and emerging cloud technologies to strengthen my technical skills.

# Abstract class vs interface	Differences and use cases

abstract classes and interfaces are both used to achieve abstraction, but they serve different purposes.

The most important interview distinction is:
- Abstract class: Used when multiple related classes share common functionality and state.
- Interface: Used when different classes need to follow the same contract, even if they are unrelated.
Let's understand this with C# examples.


1. Abstract class
An abstract class is a class that cannot be instantiated directly. It can contain both implemented methods and abstract methods (methods without implementation).

Why use an abstract class?
- To share common code between related classes.
- To maintain common properties and fields.
- To provide default implementations.
- To force derived classes to implement specific methods.

2. Interface
An interface defines a contract that implementing classes must follow.

It specifies what a class should do, rather than providing shared instance state.

Why use an interface?
- To achieve loose coupling.
- To support multiple implementations.
- To enable dependency injection.
- To define common contracts across unrelated classes.
- To make unit testing and mocking easier.



# Value types vs reference types	Stack/heap nuance, copying, boxing

1. Value types
A value type stores its actual value directly within its storage location(STACK).

Examples:
- int
- double
- float
- bool
- char
- decimal
- struct
- enum

2. Reference types
A reference type variable stores a reference to an object rather than storing the object itself(HEAP).

Examples:
- class
- string
- array
- object
- delegate
- interface

3. Stack vs Heap – The important nuance

You might hear:
- Value types are stored on the stack.
- Reference types are stored on the heap.
This is an oversimplification and is not always correct in C#.

The actual rule is:
- Value types are stored wherever their containing storage is located.
- Reference-type objects are generally allocated on the managed heap.
- A reference variable itself can be stored on the stack, in an object, in a register, or elsewhere depending on its context and JIT optimizations.


Value types store their data directly, whereas reference types store a reference to an object. When we assign a value type, its data is copied, so both variables are independent. When we assign a reference type, only the reference is copied, so both variables can point to the same object.

In terms of memory, we shouldn't say all value types are stored on the stack and all reference types on the heap. Value types are stored wherever their containing storage is located. Reference-type objects are generally allocated on the managed heap. Boxing occurs when a value type is converted into an object or interface reference, creating a boxed object and potentially causing additional memory allocations.


# IEnumerable vs IQueryable	In-memory vs provider-side execution

The key difference:
- IEnumerable: Used to work with data in memory. LINQ operations are generally executed by .NET.
- IQueryable: Used to build queries that a query provider can translate and execute, often against a database.


1. IEnumerable
IEnumerable<T> is an interface that allows you to iterate over a collection of elements.

It is commonly used for in-memory data processing.


How does it work?
1. Data is available in memory.
2. Where() builds a deferred LINQ operation.
3. When we iterate using foreach, the filtering executes in .NET.
4. Matching elements are returned.
Important: IEnumerable does not necessarily mean the entire collection is copied into memory. It can enumerate a source lazily.



2. IQueryable
IQueryable<T> is designed to build queries that can be translated by a query provider.

A common example is Entity Framework Core.

Consider an Employees database table.

What's happening?
1. EF Core builds an expression tree representing the query.
2. The query is not necessarily executed immediately.
3. When the query is enumerated, EF Core translates the expression into SQL.
4. The database executes the SQL and returns matching records.


3. Real-world example: IEnumerable vs IQueryable
Imagine your Employees table contains 100,000 records.

You only need employees whose salary is above ₹50,000.
Approach 1: IEnumerable
IEnumerable<Employee> employees =
    _context.Employees.AsEnumerable()
        .Where(x => x.Salary > 50000);


What happens?
- AsEnumerable() switches subsequent LINQ operations to LINQ-to-Objects.
- When enumeration starts, EF Core retrieves the rows from the database.
- The salary filtering happens in application memory.
Potentially, all 100,000 employees are retrieved.
Approach 2: IQueryable
IQueryable<Employee> employees =
    _context.Employees
        .Where(x => x.Salary > 50000);


What happens?
- EF Core translates the filter into SQL.
- The database filters the records.
- Only matching employees are returned.

5. Deferred execution vs immediate execution
This is another frequently asked interview question.

Both IEnumerable and IQueryable commonly support deferred execution.
Deferred execution
The query executes only when you enumerate or otherwise consume it.


Immediate execution
Some LINQ methods trigger execution immediately.

Examples:
- ToList()
- ToArray()
- Count()
- First()
- FirstOrDefault()
- Any()

IEnumerable is used to iterate over collections and perform LINQ operations, typically in memory. IQueryable extends IEnumerable and provides an expression tree that allows query providers like Entity Framework Core to translate LINQ expressions into SQL.

For example, when querying a large employee table, I prefer IQueryable so filtering, sorting, and pagination can happen at the database level. Once I call ToListAsync(), the query executes and the results are materialized into memory. From that point onward, additional LINQ operations execute in memory.


# Task vs Thread	Asynchronous programming
Thread: An actual execution path managed by the operating system.

Task: A representation of an asynchronous operation that may produce a result in the future.

1. What is a Thread?
A thread is the basic unit of execution within a process.
Important characteristics:
- A thread is an actual execution resource.
- It has its own call stack.
- It is scheduled by the operating system.
- Creating and managing threads has overhead.
- Multiple threads can execute concurrently.

2. What is a Task?
A Task represents an operation that may complete in the future.

It belongs to the System.Threading.Tasks namespace.

Here:
1. Task.Run() schedules work on the ThreadPool.
2. The task represents that work.
3. await asynchronously waits for completion.
4. The method resumes after the task completes.
Important: A Task is an abstraction for asynchronous work, not a thread itself.


4. Synchronous vs Asynchronous programming
This is where the actual concept becomes important.

Synchronous programming
The next operation waits until the current operation completes.

Asynchronous programming
The method can wait for an operation to complete without blocking its thread.


6. CPU-bound vs I/O-bound operations
This distinction is extremely important in real-world .NET applications.
A. I/O-bound operations
Examples:
- Calling external APIs.
- Database queries.
- Reading files.
- Sending HTTP requests.
- Accessing cloud services such as AWS S3.
Use asynchronous APIs directly.

Here, EF Core performs asynchronous database I/O.

We generally don't need Task.Run() for this.


B. CPU-bound operations
Examples:
- Image processing.
- Complex calculations.
- Data compression.
- Large computational operations.
For CPU-intensive work, Task.Run() can move execution to a ThreadPool thread.


8. CancellationToken
In real-world ASP.NET Core applications, cancellation is important.

For example, if a client disconnects from an API request, you may want to stop unnecessary database or HTTP operations.

Important:
- Cancellation is cooperative.
- A CancellationToken requests cancellation.
- It does not forcibly terminate a thread.
- The operation must observe and respond to the token.


A Thread is an actual execution unit managed by the operating system, whereas a Task is a higher-level abstraction representing an asynchronous operation.

A Task may use a ThreadPool thread for CPU-bound work, but asynchronous I/O operations such as database calls or HTTP requests don't need to occupy a thread while waiting.

In ASP.NET Core, I generally prefer Task-based asynchronous programming because it improves scalability by avoiding unnecessary thread blocking. For example, I use EF Core's ToListAsync() for database operations rather than wrapping synchronous database calls inside Task.Run().


# `async` / `await` Non-blocking I/O

1. What is async / await?
async and await are C# keywords used to write asynchronous code.
- async: Marks a method as asynchronous and allows it to use await.
- await: Asynchronously waits for an operation to complete without blocking the executing thread while an incomplete operation is pending.

Async state machine – Simplified
1. Method starts
   
   Executes statements before await.
2. Await encountered
   
   Checks whether the operation is complete.
3. Operation incomplete
   
   Saves the state and registers a continuation. The method returns an incomplete Task.
4. I/O completes
   
   The continuation is scheduled.
5. Method resumes
   
   Executes the remaining statements and completes its Task.


# ref vs out	Parameter passing

Both ref and out allow a method to work with a variable through a reference to its storage location, rather than receiving an independent copy of its value.

The main difference:
- ref: The variable must be initialized before passing it. The method can read and modify its existing value.
- out: The variable doesn't need to be initialized before passing it. The method must assign it before returning.

- in: passes an argument by reference but makes the parameter readonly inside the method.

Both ref and out are used to pass parameters by reference in C#.

The main difference is that ref requires the variable to be initialized before passing it, and the method can read and modify its existing value.

With out, the caller doesn't need to initialize the variable, but the method must assign a value before returning.

A practical example of out is int.TryParse(), where the method returns a Boolean indicating success and provides the converted value through an out parameter.


# `const` vs `readonly` Compile-time vs runtime initialization

const vs readonly in C#
The main difference is when the value is assigned and whether it can change afterward.
- const → Value must be known at compile time.
- readonly → Value can be assigned at runtime, but only during declaration or inside a constructor.


# Delegates and events	Callback and event handling

Delegate: A type-safe reference to a method. It allows you to pass a method as an argument or store it in a variable.

Event: A mechanism built on delegates that allows one object to notify other objects when something happens.

# Dependency Injection	Lifetime and registration

Dependency Injection is a design pattern where a class receives the objects it depends on from an external source instead of creating those objects itself.

It helps achieve:
- Loose coupling.
- Better unit testing.
- Easier maintenance.
- Better separation of responsibilities.



3. Dependency Injection lifetimes
This is the most important interview topic.

ASP.NET Core provides three primary service lifetimes:

- Transient
AddTransient

A new instance is created every time the service is requested from the container.

Example: lightweight stateless helper services.


- Scoped
AddScoped

One instance per DI scope. In a typical ASP.NET Core web request, this means one instance per request.

Example: EF Core DbContext, business services working within a request.


- Singleton
AddSingleton

One instance for the lifetime of the service provider (typically the application).

Example: thread-safe shared caches and application-wide services.



# Garbage Collection Generations, IDisposable

Garbage Collection (GC) is the automatic memory management mechanism in .NET that identifies objects no longer reachable by an application and reclaims their managed memory.


2. Generational Garbage Collection

.NET divides managed objects into three main generations:
.NET GC generations
- Generation 0
Young objects

Newly allocated, short-lived objects.

Collected frequently.

- Generation 1
Survivors

Objects that survived a Gen 0 collection.

Acts as a buffer between short-lived and long-lived objects.

- Generation 2
Long-lived objects

Objects that survived previous collections.

Collected less frequently because these collections are generally more expensive.

4. What is IDisposable?
Now let's understand a different but closely related concept.

GC manages managed memory automatically, but it does not guarantee timely cleanup of external resources.

Examples:
- Database connections.
- File handles.
- Network connections.
- Streams.
- Native operating system resources.
This is where IDisposable comes in.

IDisposable is an interface that provides a deterministic way to release resources when they are no longer needed.

# LINQ in C# – Deferred Execution, Filtering and Projection

LINQ (Language Integrated Query) is a feature in C# that allows you to query and manipulate data collections using a consistent syntax.

It allows you to query different data sources, such as:
- Collections (List, arrays).
- Databases (using Entity Framework Core).
- XML documents.
- Other data providers.

For .NET interviews, focus on these concepts:
1. Filtering – Where()
2. Projection – Select()
3. Deferred execution – when a query actually executes.
4. Immediate execution – ToList(), ToArray(), Count().
5. IEnumerable vs IQueryable.
6. Method syntax vs query syntax.


# Collections	Dictionary, HashSet, List
List<T> – Stores an ordered collection of elements.
Dictionary<TKey, TValue> – Stores key-value pairs for fast lookup.
HashSet<T> – Stores unique elements without duplicates.
Collection	Small Definition


Array	Stores a fixed number of elements of the same type.
List	Stores elements dynamically; size can grow or shrink.
Dictionary	Stores data as key-value pairs.
HashSet	Stores unique elements; does not allow duplicates.
Queue	Follows FIFO (First In, First Out).
Stack	Follows LIFO (Last In, First Out).
LinkedList	Stores elements as nodes connected to each other.
SortedList	Stores key-value pairs sorted by key.
SortedDictionary	Stores key-value pairs in sorted key order.
SortedSet	Stores unique elements in sorted order.
Hashtable	Stores key-value pairs without generic type safety.
ConcurrentDictionary	Thread-safe dictionary for multiple threads.
ConcurrentQueue	Thread-safe FIFO collection.
ConcurrentStack	Thread-safe LIFO collection.


# Explain the ASP.NET Core request pipeline.
Interview answer:

The ASP.NET Core request pipeline is a sequence of middleware components that handle incoming HTTP requests and outgoing HTTP responses.

Each middleware can:
- Process the incoming request.
- Pass the request to the next middleware using next().
- Modify the response.
- Stop further request processing.
Middleware is configured in Program.cs using app.Use(), app.Run(), and app.Map().

# What is middleware?
Middleware is a component in ASP.NET Core that handles HTTP requests and responses as part of the request pipeline.

Each middleware can perform specific tasks such as:
- Authentication
- Authorization
- Logging
- Exception handling
- HTTPS redirection
- Routing
Middleware executes in the order it is registered in Program.cs.


# Middleware vs filters?

he main difference is that middleware works at the HTTP request pipeline level, whereas filters work within the MVC/Web API action execution pipeline.


When to use what?
- Middleware: Logging, global exception handling, CORS, authentication.
- Filters: Action validation, action-level logging, modifying action results.

# How does Dependency Injection work in ASP.NET Core?

How it works:
1. We register the service in the DI container.
2. ASP.NET Core creates the required service object.
3. When the controller is created, the DI container injects the service through its constructor.
4. The controller uses the injected service.

# How do you implement global exception handling?

Global exception handling is a mechanism to catch and handle unhandled exceptions across the application in one central place instead of writing try-catch blocks in every controller or service.

In ASP.NET Core, we can implement global exception handling using:
1. Built-in exception handling middleware.
2. Custom exception-handling middleware.
3. IExceptionHandler (available in .NET 8 and later).

# How do you implement JWT authentication?
JWT (JSON Web Token) authentication is a token-based authentication mechanism used to securely authenticate users in Web APIs.

When a user logs in with valid credentials, the server generates a JWT token and sends it to the client.

The client sends this token in the Authorization header for subsequent API requests. The server validates the token before allowing access to protected endpoints.


# Authentication vs authorization?

Authentication and Authorization are security mechanisms used to protect applications and APIs.
- Authentication: Verifies who the user is.
- Authorization: Determines what the authenticated user is allowed to access.

# How do you implement API versioning?

API versioning allows us to maintain multiple versions of an API without breaking existing clients.

When we introduce new features or change API functionality, we can create a new version while keeping the older version available for existing applications.

Approach	Example
URL versioning	/api/v1/employees
Query string	/api/employees?api-version=1.0
Header versioning	api-version: 1.0
Media type versioning	Accept: application/json;v=1.0

# How do you secure REST APIs in ASP.NET Core?
We can secure REST APIs using multiple security mechanisms to prevent unauthorized access, protect sensitive data, and defend against common attacks.

In ASP.NET Core, I use the following approaches:
1. Authentication
2. Authorization
3. HTTPS
4. Input Validation
5. CORS (Cross-Origin Resource Sharing)
6. Rate Limiting
7. Secure Sensitive Information
8. Exception Handling

# How do you implement logging?
Logging is used to record application events, errors, warnings, and debugging information.

ASP.NET Core provides a built-in logging framework through ILogger<T>.

We can use logging to:
- Track application behavior.
- Identify and troubleshoot errors.
- Monitor application performance.
- Maintain application activity records.


ILogger<T> = Logging interface.



LogInformation() = General information.



LogWarning() = Warning messages.



LogError() = Error messages.



Serilog = Structured logging framework.


# How do you handle API Performance in ASP.NET Core?

API performance optimization means improving response time, reducing resource consumption, and handling more concurrent requests efficiently.

1. Asynchronous Programming
Use async and await for database calls and external API requests to avoid blocking threads while waiting for I/O operations.

2. Database Optimization
- Use proper database indexes.
- Avoid unnecessary database queries.
- Optimize SQL queries and stored procedures.
- Use pagination for large datasets.
- Select only required columns.

3. Caching
Cache frequently accessed data to reduce repeated database calls.

ASP.NET Core supports in-memory caching and distributed caching.


4. Pagination
Avoid returning thousands of records in a single API response.


5. Reduce Unnecessary Data Transfer
- Use DTOs.
- Return only required fields.
- Enable response compression where appropriate.
- Avoid unnecessarily large API responses.


6. Database Query Optimization
Use EF Core performance features:
- AsNoTracking() for read-only queries.
- Avoid N+1 query problems.
- Use projection with Select().
- Avoid unnecessary Include() statements.


7. Background Processing
Move long-running operations outside the API request-response cycle.

Examples:
- Sending emails.
- Generating reports.
- Processing large files.
We can use background services, message queues such as AWS SQS, or hosted services.


8. Monitoring and Logging
Monitor API response times, database execution times, exceptions, and resource utilization.

Tools include:
- AWS CloudWatch.
- Application Insights.
- Serilog.


# What is Model Validation in ASP.NET Core?
Model validation is the process of checking incoming request data to ensure it satisfies predefined rules before processing it in the application.

ASP.NET Core supports model validation using Data Annotations such as [Required], [StringLength], [Range], and [EmailAddress].

It helps prevent invalid data from entering the application.

# 1. Monolithic vs Microservices Architecture
Monolithic Architecture:

A monolithic architecture is an application where all components, such as business logic, APIs, and database access, are developed and deployed as a single application.

Microservices Architecture:

Microservices architecture divides an application into small, independent services. Each service handles a specific business functionality and can be developed, deployed, and scaled independently.
Monolithic	Microservices
Single application	Multiple independent services
Single deployment	Independent deployment
Tightly coupled components (typically)	Loosely coupled services
Scaling the entire application	Scaling individual services
Simpler initial development	More operational complexity
Shared database is common	Independent databases are preferred

Example: In an e-commerce application, User, Product, Order, and Payment functionalities can be separate microservices.


# 2. Why did your project migrate to microservices?
We migrated from a legacy monolithic architecture to microservices because the existing application had tightly coupled components.

The main challenges were:
- Changes in one module could affect other modules.
- Difficult to scale individual functionalities.
- Deployment of the entire application was required.
- Maintenance and development became complex as the application grew.
By migrating to microservices, we achieved better modularity, independent deployment, scalability, and maintainability.


# 3. How do microservices communicate?
Microservices communicate mainly through two approaches:

1. Synchronous Communication
- Uses HTTP/REST APIs or gRPC.
- One service sends a request and waits for a response.
- Example: Order Service calls Payment Service through REST API.
2. Asynchronous Communication
- Uses message brokers such as AWS SQS or RabbitMQ.
- Services communicate through messages without waiting for an immediate response.
- Example: Order Service publishes an order-processing message to SQS, and another service processes it.

# 4. Synchronous vs Asynchronous Communication
Synchronous	Asynchronous
Request-response communication	Message-based communication
Caller waits for response	Caller doesn't wait for processing
REST API, gRPC	AWS SQS, RabbitMQ
Tighter runtime dependency	Looser coupling
Suitable for immediate responses	Suitable for background processing

Example from my project:

We used REST APIs for synchronous communication and AWS SQS for asynchronous processing between services.


# 5. What is API Gateway?
An API Gateway is a single entry point for clients to access multiple microservices.

Instead of calling individual services directly, clients send requests to the API Gateway, which routes them to the appropriate service.

Responsibilities:
- Request routing.
- Authentication and authorization.
- Rate limiting.
- Load balancing.
- Logging and monitoring.
Example: A client sends /api/orders to the API Gateway, which forwards the request to Order Service.

In .NET, we can implement an API Gateway using YARP or Ocelot.


# 6. What is Service Discovery?
Service discovery is a mechanism that helps microservices find the network locations of other services dynamically.

Instead of hardcoding IP addresses and ports, services discover each other through a service registry or platform.

Types:
- Client-side service discovery.
- Server-side service discovery.
Examples: Consul, Eureka, Kubernetes Service Discovery.

It helps manage dynamic service instances when services scale up or down.


# 7. How do you handle distributed transactions?
A distributed transaction involves multiple services or databases participating in a single business operation.

In microservices, we generally avoid traditional database transactions across multiple services.

Instead, we use:
- Saga Pattern.
- Event-driven communication.
- Compensating transactions.
- Outbox Pattern for reliable event publishing.
Example: An order involves Order Service, Payment Service, and Inventory Service. If payment fails, the order can be cancelled.


# 8. What is the Saga Pattern?
The Saga Pattern manages distributed transactions by dividing them into multiple local transactions.

Each service executes its own transaction. If one step fails, compensating transactions are executed to undo the previous operations logically.

Example:
1. Order Service

Create order


2. Payment Service

Process payment


3. Inventory Service

Reserve inventory


Success
Order confirmed

Failure example
If inventory reservation fails, execute a compensating transaction to refund the payment and cancel the order.

Types:
1. Choreography – Services communicate through events without a central coordinator.
2. Orchestration – A central orchestrator controls the transaction flow.


# 9. What is the Circuit Breaker Pattern?
Circuit Breaker is a fault-tolerance pattern that prevents repeated calls to a failing service.

It has three states:
- Closed: Requests are allowed normally.
- Open: Requests are blocked temporarily because the service is failing.
- Half-Open: A limited number of requests are allowed to check whether the service has recovered.
Example: If Payment Service is unavailable, Order Service temporarily stops calling it instead of continuously retrying and increasing the load.

In .NET, we can implement this using Polly or .NET resilience pipelines.


# 10. How do you handle failures in microservices?
We handle failures using different fault-tolerance techniques:
1. Retry policies for temporary failures.
2. Circuit Breaker to prevent repeated calls to failing services.
3. Timeout policies to avoid waiting indefinitely.
4. Dead Letter Queues (DLQ) for messages that cannot be processed.
5. Exception handling and structured logging.
6. Health checks to monitor service availability.
7. Fallback mechanisms where appropriate.
Example: If processing an SQS message fails repeatedly, it can be moved to a Dead Letter Queue for investigation.


# 11. How do you implement centralized logging?
Centralized logging collects logs from multiple microservices into a single platform.

Each service generates structured logs containing information such as:
- Timestamp.
- Log level.
- Service name.
- Correlation ID.
- Exception details.
These logs are sent to a centralized logging platform.

Tools:
- ELK Stack (Elasticsearch, Logstash, Kibana).
- Grafana Loki.
- AWS CloudWatch.
In .NET, we can use ILogger with structured logging and appropriate logging providers.

Example: A Correlation ID helps trace a single request across multiple microservices.
# 12. How do you ensure idempotency?
Idempotency means executing the same operation multiple times produces the same final result as executing it once.

It is especially important in distributed systems where messages or requests may be delivered more than once.

Example: If a payment request is received twice, we should not charge the customer twice.

Implementation:
1. Generate a unique idempotency key for each operation.
2. Store processed request IDs in a database.
3. Check whether the request has already been processed.
4. Return the previous result instead of processing it again.
For AWS SQS, we should account for at-least-once message delivery and implement deduplication at the application level.


# 1. What is AWS Lambda?

AWS Lambda is a serverless compute service that allows us to run code without managing servers. We upload our code, and AWS automatically executes it when triggered by an event.

# 2. What is serverless architecture?

Serverless architecture is an approach where developers don't need to manage servers. AWS handles infrastructure, scaling, and availability, while developers focus on application logic.

# 3. What triggers a Lambda function?

A Lambda function can be triggered by different AWS services, such as:
- API Gateway
- S3
- SQS
- SNS
- EventBridge
- DynamoDB Streams
- CloudWatch Events (now part of EventBridge)


# 4. What is a cold start?

A cold start happens when AWS needs to initialize a new Lambda execution environment before running the function. This can increase the initial execution time.

# 5. How does Lambda scale?

Lambda automatically creates multiple execution environments based on incoming requests. It can handle multiple requests concurrently without manually managing servers.

# 6. What happens when a Lambda execution fails?

The behavior depends on the invocation type:
- Synchronous invocation: The caller receives an error. The caller can retry.
- Asynchronous invocation: Lambda retries failed executions automatically, typically twice, before discarding or routing the event according to configuration.
- SQS event source: Messages become visible again after the visibility timeout and can be processed again until they reach the configured retry limit and DLQ.


# 7. What are Lambda execution limits?

Important Lambda limits include:
- Maximum execution time: 15 minutes.
- Memory: 128 MB to 10,240 MB.
- Deployment package and concurrency limits apply.
- Temporary storage: 512 MB to 10,240 MB.


# 8. How do you connect Lambda with DynamoDB and SQS?
- DynamoDB: Use the AWS SDK to perform operations such as GetItem, PutItem, UpdateItem and DeleteItem.
- SQS: Use the AWS SDK to send and receive messages, or configure Lambda as an SQS event source to process messages automatically.
For secure access, assign an IAM execution role to Lambda.


# 1. What is Amazon API Gateway?

Amazon API Gateway is a managed AWS service used to create, publish, secure, and manage REST APIs, HTTP APIs, and WebSocket APIs.

# 2. How does API Gateway communicate with Lambda?

API Gateway receives an HTTP request and invokes the configured Lambda function. Lambda processes the request and returns a response to API Gateway, which sends it back to the client.

# 3. How do you secure API Gateway?

We can secure API Gateway using:
- IAM authentication.
- JWT authorizers.
- Amazon Cognito.
- Lambda authorizers.
- Resource policies.
- AWS WAF for additional web application protection.


# 4. What is throttling?

Throttling controls the number of requests an API can handle within a specific time. It protects backend services from excessive traffic and helps prevent overload.

# 5. What is the difference between API Gateway and a load balancer?
API Gateway	Load Balancer
Manages API requests	Distributes traffic
Supports API authentication	Supports traffic distribution
Provides API throttling	Provides health checks
Supports request/response transformation	Routes traffic to targets
Commonly used for serverless APIs	Commonly used for EC2 and container applications

# 1. What is Amazon SQS?

Amazon Simple Queue Service (SQS) is a fully managed message queuing service that allows applications to communicate asynchronously by storing messages in a queue.

2. What is the difference between Standard and FIFO queues?
Standard Queue	FIFO Queue
High throughput	Ordering is guaranteed within a message group
At-least-once delivery	Exactly-once processing support through deduplication
Messages may arrive out of order	Messages are processed in order
Suitable for general workloads	Suitable for order-sensitive workloads

# 3. What is a visibility timeout?

Visibility timeout is the period during which a message becomes invisible after a consumer receives it. If the consumer successfully processes and deletes the message, it is removed from the queue. Otherwise, it becomes visible again after the timeout.

# 4. What is a Dead Letter Queue (DLQ)?

A DLQ is a separate queue used to store messages that cannot be successfully processed after a configured number of attempts. It helps with debugging and failure analysis.

# 5. What happens when an SQS message fails?

If processing fails and the message is not deleted, it becomes visible again after the visibility timeout. It can be processed again. After reaching the configured maximum receive count, it is moved to the DLQ if one is configured.

# 6. What is the difference between SQS and SNS?
SQS	SNS
Message queue service	Publish-subscribe messaging service
Messages are consumed by polling	Messages are pushed to subscribers
Usually used for one consumer processing each message	Can distribute messages to multiple subscribers
Supports message retention	Delivers notifications to subscribers

# 7. What is message retention?

Message retention is the duration for which SQS stores messages before automatically deleting them.
- Minimum: 1 minute.
- Maximum: 14 days.
- Default: 4 days.


# 8. How do you prevent duplicate message processing?

We can use idempotent processing by maintaining a unique message ID or transaction ID. Before processing, check whether the message has already been processed. If it has, skip duplicate processing.

Other options include:
- FIFO queues with deduplication.
- Database unique constraints.
- Conditional writes.
- Proper visibility timeout configuration.

# 1. Code First vs Database First
Code First:

In Code First, we create entity classes and configure the database using C# code. EF Core generates database tables using migrations.

Database First:

In Database First, we already have a database, and EF Core generates entity classes and DbContext from the existing database.
Code First	Database First
Start with C# classes	Start with an existing database
Uses migrations	Uses scaffolding
Suitable for new applications	Suitable for existing databases
Code defines the model	Database defines the model

Example:
Code First
dotnet ef migrations add InitialCreate
dotnet ef database update

Database First
dotnet ef dbcontext scaffold "ConnectionString" Microsoft.EntityFrameworkCore.SqlServer


# 2. What are DbContext and DbSet?
DbContext:

DbContext is the main class in EF Core that manages database connections, queries, entity tracking, and saving changes.

DbSet:

DbSet represents a collection of entities that can be queried and modified in the database.

Example:
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Employee> Employees { get; set; }
}


Here:
- AppDbContext manages database operations.
- Employees represents the Employee table.


# 3. Tracking vs AsNoTracking()
Tracking:

EF Core tracks changes to entities retrieved from the database. When we call SaveChanges(), it automatically detects and saves those changes.

AsNoTracking():

It retrieves entities without tracking them. It improves performance for read-only operations.

Example:
// Tracking
var employee = await context.Employees
    .FirstOrDefaultAsync(x => x.Id == 1);

employee.Name = "John";

await context.SaveChangesAsync();

// No Tracking
var employees = await context.Employees
    .AsNoTracking()
    .ToListAsync();


When to use?
- Tracking: When we need to update or delete entities.
- AsNoTracking: When we only need to read data.

# 4. Eager Loading vs Lazy Loading vs Explicit Loading
These are three approaches to loading related entities.

Eager Loading:

Loads related data along with the main entity using Include().
var employees = await context.Employees
    .Include(x => x.Department)
    .ToListAsync();


Lazy Loading:

Related data is loaded automatically when we access a navigation property, provided lazy-loading proxies or another supported lazy-loading mechanism are configured.
var employee = await context.Employees
    .FirstOrDefaultAsync(x => x.Id == 1);

var department = employee.Department;


Explicit Loading:

We manually load related data when required.
var employee = await context.Employees
    .FirstOrDefaultAsync(x => x.Id == 1);

await context.Entry(employee)
    .Reference(x => x.Department)
    .LoadAsync();


Eager Loading	Lazy Loading	Explicit Loading
Loads immediately	Loads when accessed	Loads when requested
Uses Include()	Uses navigation properties	Uses Entry().Load()
Predictable queries	Can cause extra queries	Developer controls loading

# 5. What are Include() and ThenInclude()?
Include():

Used to load related entities along with the main entity.

ThenInclude():

Used to load nested related entities.

Example:
var employees = await context.Employees
    .Include(x => x.Department)
        .ThenInclude(x => x.Manager)
    .ToListAsync();


Here:
- Include() loads Department.
- ThenInclude() loads the Manager associated with that Department.
# 6. What are EF Core Migrations?
Migrations help us manage database schema changes based on changes in our entity classes and EF Core model.

For example, when we add a new property to an Employee class, we can create a migration to add a corresponding database column.

Common commands:
# Create migration
dotnet ef migrations add AddEmployeeEmail

# Apply migration
dotnet ef database update

# Remove the last migration (if not applied)
dotnet ef migrations remove

# List migrations
dotnet ef migrations list


# Why use migrations?
- Track database schema changes.
- Maintain database version history.
- Apply schema changes across environments.
- Keep the database synchronized with the EF Core model.

# 7. What is the N+1 Query Problem?
The N+1 problem happens when an application executes one query to retrieve the main records and then executes additional queries for each record to retrieve related data.

Example:
var employees = await context.Employees.ToListAsync();

foreach (var employee in employees)
{
    Console.WriteLine(employee.Department.Name);
}


If lazy loading is enabled, this could execute:
- 1 query to retrieve employees.
- N additional queries to retrieve departments.
Total: N+1 queries.

How to solve it?

Use eager loading with Include():
var employees = await context.Employees
    .Include(x => x.Department)
    .ToListAsync();


We can also use projections with Select() to retrieve only the required data.


# 8. What are Transactions in EF Core?
A transaction ensures that multiple database operations are executed as a single unit.

If all operations succeed, the transaction is committed. If something fails, the transaction can be rolled back.

Example:
using var transaction =
    await context.Database.BeginTransactionAsync();

try
{
    context.Employees.Add(employee);
    await context.SaveChangesAsync();

    context.Departments.Add(department);
    await context.SaveChangesAsync();

    await transaction.CommitAsync();
}
catch
{
    await transaction.RollbackAsync();
    throw;
}


Why use transactions?

To maintain data consistency and ensure that related database operations either all succeed or are rolled back together.

Note: A single SaveChanges() call is transactional by default when the provider supports transactions.


# 9. How do you optimize LINQ queries in EF Core?
We can optimize LINQ queries using the following approaches:
1. Use AsNoTracking() for read-only queries.
2. Use Select() to retrieve only required columns.
3. Avoid unnecessary Include() statements.
4. Avoid N+1 query problems.
5. Use pagination with Skip() and Take().
6. Use asynchronous methods such as ToListAsync().
7. Ensure appropriate database indexes exist.
8. Avoid loading unnecessary records into memory.
9. Check generated SQL queries.
10. Use compiled queries for frequently executed queries when profiling justifies them.
Example:

Without optimization:
var employees = await context.Employees
    .ToListAsync();


Optimized:
var employees = await context.Employees
    .AsNoTracking()
    .Where(x => x.IsActive)
    .Select(x => new
    {
        x.Id,
        x.Name
    })
    .ToListAsync();


The optimized query retrieves only the required columns and avoids change tracking.
# 10. Stored Procedures vs EF Core
Stored Procedures:

Stored procedures are SQL queries stored and executed inside the database.

Entity Framework Core:

EF Core is an ORM that allows developers to interact with databases using C# objects and LINQ instead of writing SQL for every operation.
Stored Procedures	EF Core
Written in SQL	Written using C# and LINQ
Execute inside database	Generates SQL queries
Gives direct control over SQL	Simplifies database operations
Useful for complex database operations	Useful for CRUD operations
Database-specific implementation	Supports multiple database providers

Can EF Core execute stored procedures?

Yes. EF Core supports executing stored procedures.

Example:
var employees = await context.Employees
    .FromSqlRaw(
        "EXEC GetEmployeesByDepartment @p0",
        departmentId)
    .ToListAsync();


For non-query operations:
await context.Database.ExecuteSqlRawAsync(
    "EXEC UpdateEmployeeStatus @p0, @p1",
    employeeId,
    true);


When should we use each?
- EF Core: Regular CRUD operations and maintainable application development.
- Stored Procedures: Complex SQL operations, database-side processing, or when existing database logic needs to be reused.


# 1. INNER JOIN vs LEFT JOIN
INNER JOIN:

Returns only matching records from both tables.

LEFT JOIN:

Returns all records from the left table and matching records from the right table. If there is no match, right-side columns return NULL.

Example:
-- INNER JOIN
SELECT e.Name, d.DepartmentName
FROM Employees e
INNER JOIN Departments d
ON e.DepartmentId = d.Id;

-- LEFT JOIN
SELECT e.Name, d.DepartmentName
FROM Employees e
LEFT JOIN Departments d
ON e.DepartmentId = d.Id;


INNER JOIN	LEFT JOIN
Only matching records	All left table records
Excludes unmatched records	Includes unmatched records
Returns matching data	Returns NULL for missing right-side data

# 2. Clustered vs Non-Clustered Indexes
Clustered Index:

A clustered index determines the logical order of data rows in a table based on the index key. A table can have only one clustered index.

Non-Clustered Index:

A non-clustered index is a separate index structure that stores indexed values and pointers to the corresponding table rows.
Clustered Index	Non-Clustered Index
Determines row organization	Separate index structure
Only one per table	Multiple per table
Stores actual table rows at leaf level	Stores index keys and row locators
Good for range queries	Good for searching specific columns

Example:
-- Clustered Index
CREATE CLUSTERED INDEX IX_Employees_Id
ON Employees(Id);

-- Non-Clustered Index
CREATE NONCLUSTERED INDEX IX_Employees_Email
ON Employees(Email);


# 3. What are Stored Procedures?
A stored procedure is a collection of SQL statements stored in the database that can be executed whenever required.

Advantages:
- Reusability.
- Better control over database operations.
- Supports parameters.
- Can reduce network round trips.
- Supports transaction handling.
Example:
CREATE PROCEDURE GetEmployeeById
    @EmployeeId INT
AS
BEGIN
    SELECT *
    FROM Employees
    WHERE Id = @EmployeeId;
END;


Execute:
EXEC GetEmployeeById @EmployeeId = 101;


How do you pass parameters to a stored procedure?

We can pass input parameters using @ variables. We can also use output parameters to return values.


# 4. What is a CTE (Common Table Expression)?
A CTE is a temporary named result set that makes complex SQL queries easier to read and maintain.

It exists only for the duration of the statement that follows it.

Example:
WITH ActiveEmployees AS
(
    SELECT *
    FROM Employees
    WHERE IsActive = 1
)
SELECT *
FROM ActiveEmployees;


Advantages:
- Improves readability.
- Simplifies complex queries.
- Supports recursive queries.
- Helps avoid repeating the same query logic.


# 5. What are Window Functions?
Window functions perform calculations across a set of related rows without combining them into a single row.

They are commonly used for ranking, running totals, and comparisons.

Example:
SELECT
    Name,
    Salary,
    AVG(Salary) OVER() AS AverageSalary
FROM Employees;


Here, the average salary is calculated across all employees while retaining individual employee records.

Common window functions:
- ROW_NUMBER()
- RANK()
- DENSE_RANK()
- LAG()
- LEAD()
- SUM() OVER()
- AVG() OVER()

# 6. What is ROW_NUMBER()?
ROW_NUMBER() assigns a unique sequential number to each row based on the specified ordering.

Example:
SELECT
    Name,
    Salary,
    ROW_NUMBER() OVER(
        ORDER BY Salary DESC
    ) AS RowNum
FROM Employees;


Practical use: Find the second-highest salary.
WITH SalaryRanking AS
(
    SELECT
        Salary,
        ROW_NUMBER() OVER(
            ORDER BY Salary DESC
        ) AS RowNum
    FROM Employees
)
SELECT Salary
FROM SalaryRanking
WHERE RowNum = 2;


Note: This finds the second row by salary, not necessarily the second distinct salary. For the second distinct salary, use DENSE_RANK().

Difference between ROW_NUMBER(), RANK(), and DENSE_RANK():
Function	Behavior
ROW_NUMBER()	Unique sequential numbers
RANK()	Same rank for ties, skips next ranks
DENSE_RANK()	Same rank for ties, doesn't skip ranks

# 7. What is a Query Execution Plan?
A query execution plan shows how SQL Server executes a query internally.

It helps identify performance issues and expensive operations.

Important operators:
- Index Seek: Finds specific records using an index.
- Index Scan: Reads index entries.
- Table Scan: Reads the entire table.
- Key Lookup: Retrieves additional columns from the clustered index.
- Sort: Sorts records.
- Nested Loops: Joins tables using repeated lookups.
- Hash Match: Uses hashing for joins or aggregations.
How do you check an execution plan?

In SQL Server Management Studio (SSMS):
1. Open the query.
2. Select Include Actual Execution Plan.
3. Execute the query.
4. Review the execution plan and expensive operators.
Shortcut: Ctrl + M enables the actual execution plan.
# 8. What is a Deadlock?
A deadlock occurs when two or more transactions wait for resources locked by each other, and none can proceed.

Example:
- Transaction 1 locks Employee table and waits for Department table.
- Transaction 2 locks Department table and waits for Employee table.
- Both transactions wait indefinitely.
SQL Server detects the deadlock and selects one transaction as the victim, rolling it back.

How do you prevent deadlocks?
1. Access tables in a consistent order.
2. Keep transactions short.
3. Create appropriate indexes.
4. Avoid unnecessary locks.
5. Use appropriate transaction isolation levels.
6. Implement retry logic for deadlock victim errors when safe.
9. What is SQL Index Optimization?
Index optimization means creating and maintaining appropriate indexes to improve query performance.

Common techniques:
1. Create indexes on frequently filtered columns.
2. Create indexes on JOIN columns.
3. Use composite indexes when queries filter by multiple columns.
4. Avoid unnecessary indexes.
5. Use covering indexes with INCLUDE columns.
6. Monitor index fragmentation where relevant.
7. Review missing and unused index information.
8. Analyze execution plans.
Example:
CREATE NONCLUSTERED INDEX IX_Employees_DepartmentId
ON Employees(DepartmentId);


What is a Composite Index?

An index created using multiple columns.
CREATE NONCLUSTERED INDEX IX_Employees_Dept_Salary
ON Employees(DepartmentId, Salary);


This can help queries filtering by DepartmentId, or by DepartmentId and Salary, depending on the query.

What is a Covering Index?

A covering index contains all the columns required by a particular query, so SQL Server may retrieve the results directly from the index without an additional lookup.

Example:
CREATE NONCLUSTERED INDEX IX_Employees_Department
ON Employees(DepartmentId)
INCLUDE (Name, Salary);

# 1. What is xUnit?
xUnit is a unit testing framework for .NET applications. It helps developers write and execute automated tests to verify application functionality.

Important attributes:
- [Fact] – Used for a test method without parameters.
- [Theory] – Used for parameterized test methods.
- [InlineData] – Provides test input values.
- [BeforeAfterTest] – Not a standard xUnit attribute; use constructor and IDisposable for setup and cleanup.

# 3. What is Moq?
Moq is a mocking framework for .NET used to create mock objects for dependencies during unit testing.

It allows us to simulate dependency behavior without calling actual databases, APIs, or external services.

Example:
var mockRepository = new Mock<IEmployeeRepository>();

mockRepository
    .Setup(x => x.GetByIdAsync(1))
    .ReturnsAsync(new Employee
    {
        Id = 1,
        Name = "John"
    });


Here, we configure the mock repository to return an employee without accessing the actual database.

Important Moq methods:
Method	Purpose
Setup()	Configure mock behavior
Returns()	Return a value
ReturnsAsync()	Return an asynchronous value
Throws()	Simulate an exception
Verify()	Verify that a method was called
It.IsAny<T>()	Match any argument of a type

# 4. What is Arrange, Act, Assert (AAA)?
AAA is a common testing pattern that organizes test cases into three sections.
1. Arrange – Prepare test data and dependencies.
2. Act – Execute the method being tested.
3. Assert – Verify the expected result.
Example:
[Fact]
public void Add_ShouldReturnCorrectResult()
{
    // Arrange
    var calculator = new Calculator();

    // Act
    var result = calculator.Add(10, 20);

    // Assert
    Assert.Equal(30, result);
}


It makes tests readable, organized, and easy to maintain.