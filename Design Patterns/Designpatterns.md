# **1. SOLID Principles** #


# S — Single Responsibility Principle (SRP)
Definition

A class should have one reason to change.

It doesn't necessarily mean a class should contain only one method.

Bad example

Imagine a payment service:

public class PaymentService
{
    public void ProcessPayment(Payment payment)
    {
        // Process payment
    }

    public void ValidatePayment(Payment payment)
    {
        // Validate payment
    }

    public void SavePayment(Payment payment)
    {
        // Save to database
    }

    public void SendEmail(Payment payment)
    {
        // Send email
    }
}

This class has multiple responsibilities:

Payment processing
Validation
Database persistence
Notification

If email requirements change, PaymentService changes.

If database logic changes, PaymentService changes.

If validation changes, PaymentService changes.

That's a violation of SRP.

Better design
public class PaymentValidator
{
    public bool Validate(Payment payment)
    {
        return payment.Amount > 0;
    }
}
public class PaymentRepository
{
    public void Save(Payment payment)
    {
        // Save payment
    }
}
public class PaymentNotificationService
{
    public void SendConfirmation(Payment payment)
    {
        // Send notification
    }
}

And orchestration:

public class PaymentService
{
    private readonly PaymentValidator _validator;
    private readonly PaymentRepository _repository;
    private readonly PaymentNotificationService _notification;

    public PaymentService(
        PaymentValidator validator,
        PaymentRepository repository,
        PaymentNotificationService notification)
    {
        _validator = validator;
        _repository = repository;
        _notification = notification;
    }

    public void Process(Payment payment)
    {
        if (!_validator.Validate(payment))
            throw new Exception("Invalid payment");

        _repository.Save(payment);

        _notification.SendConfirmation(payment);
    }
}
Corpay example

Think of a payment flow:

PaymentController
       ↓
PaymentService
       ↓
PaymentValidator
       ↓
PaymentProcessor
       ↓
PaymentRepository
       ↓
NotificationService

Each component has a focused responsibility.

Interview answer

"I apply SRP by separating validation, payment processing, persistence and notification responsibilities. For example, a payment service should orchestrate the payment flow rather than directly containing SQL, validation and email logic."


# 2. Open/Closed Principle
Definition

Software entities should be open for extension but closed for modification.

Meaning:

You should be able to add new behavior without modifying existing stable code.

Payment example

Suppose Corpay supports:

Credit Card
ACH
Wire Transfer
Virtual Card

Bad design:

public class PaymentProcessor
{
    public void Process(string paymentType)
    {
        if (paymentType == "Card")
        {
            // card processing
        }
        else if (paymentType == "ACH")
        {
            // ACH processing
        }
        else if (paymentType == "Wire")
        {
            // wire processing
        }
    }
}

If tomorrow we add:

Virtual Card

we have to modify the existing class.

Better design

Create an abstraction:

public interface IPaymentProcessor
{
    void Process(Payment payment);
}

Implement different processors:

public class CardPaymentProcessor : IPaymentProcessor
{
    public void Process(Payment payment)
    {
        // Card processing
    }
}
public class AchPaymentProcessor : IPaymentProcessor
{
    public void Process(Payment payment)
    {
        // ACH processing
    }
}
public class WirePaymentProcessor : IPaymentProcessor
{
    public void Process(Payment payment)
    {
        // Wire processing
    }
}

Now adding Virtual Card:

public class VirtualCardPaymentProcessor : IPaymentProcessor
{
    public void Process(Payment payment)
    {
        // Virtual card processing
    }
}

Existing implementations don't need modification.

Interview answer

"OCP means I should be able to introduce a new payment method by adding a new implementation rather than modifying the existing payment-processing logic. Interfaces combined with Strategy or Factory patterns are commonly used to achieve this."



# 3. Liskov Substitution Principle
Definition

A derived class should be substitutable for its base class without breaking the application's expected behavior.

The Liskov Substitution Principle (LSP) states that objects of a superclass should be replaceable with objects of a subclass without breaking the application.

This is one of the most confusing SOLID principles.

Simple example

Suppose:

public interface IPaymentProcessor
{
    void Process(Payment payment);
}

Every implementation should genuinely be capable of processing a payment.

If we create:

public class RefundOnlyProcessor : IPaymentProcessor
{
    public void Process(Payment payment)
    {
        throw new NotSupportedException();
    }
}

That's a bad abstraction.

The application expects:

processor.Process(payment);

but one implementation cannot actually fulfill the contract.

Better design

Separate responsibilities:

public interface IPaymentProcessor
{
    void Process(Payment payment);
}
public interface IRefundProcessor
{
    void Refund(Payment payment);
}

Now classes implement only what they support.

Interview answer

"LSP means implementations of an abstraction should honor the contract of that abstraction. If my application expects every IPaymentProcessor to process payments, I shouldn't create an implementation that throws NotSupportedException for Process."



# 4. Interface Segregation Principle
Definition

Clients should not be forced to depend on methods they don't use.

Bad
public interface IPaymentService
{
    void ProcessPayment();
    void RefundPayment();
    void GenerateReport();
    void SendNotification();
}

A class that only processes payments is forced to implement everything.

Better
public interface IPaymentProcessor
{
    void ProcessPayment();
}
public interface IRefundService
{
    void RefundPayment();
}
public interface IPaymentReportService
{
    void GenerateReport();
}

Now consumers depend only on what they need.

Corpay example

You might have:

IPaymentProcessor
IRefundProcessor
IPaymentStatusService
IPaymentNotificationService
IPaymentRepository

instead of one giant:

IPaymentService
Interview answer

"ISP helps prevent large, bloated interfaces. In a payment system, processing, refund, reporting and notification are different capabilities, so I would expose smaller interfaces based on consumer requirements."


# 5. Dependency Inversion Principle
Definition

High-level modules should not depend directly on low-level implementation details. Both should depend on abstractions.

This is extremely important in .NET interviews.

Bad
public class PaymentService
{
    private readonly SqlPaymentRepository _repository;

    public PaymentService()
    {
        _repository = new SqlPaymentRepository();
    }
}

PaymentService is tightly coupled to SQL implementation.

Better
public interface IPaymentRepository
{
    Task SaveAsync(Payment payment);
}
public class SqlPaymentRepository : IPaymentRepository
{
    public async Task SaveAsync(Payment payment)
    {
        // SQL implementation
    }
}

Then:

public class PaymentService
{
    private readonly IPaymentRepository _repository;

    public PaymentService(IPaymentRepository repository)
    {
        _repository = repository;
    }
}

Now:

PaymentService
      ↓
IPaymentRepository
      ↑
SqlPaymentRepository

The high-level business logic doesn't care how persistence happens.


# 6. Dependency Injection

DI is closely related to DIP.

Definition

Dependency Injection is the technique of providing an object's dependencies from outside instead of the object creating them itself.

Without DI
public class PaymentService
{
    private SqlPaymentRepository _repository =
        new SqlPaymentRepository();
}
With DI
public class PaymentService
{
    private readonly IPaymentRepository _repository;

    public PaymentService(IPaymentRepository repository)
    {
        _repository = repository;
    }
}

Register:

builder.Services.AddScoped<IPaymentRepository, SqlPaymentRepository>();
builder.Services.AddScoped<PaymentService>();

ASP.NET Core creates the dependency.

Why use DI?
Loose coupling
Unit testing
Easy replacement of implementations
Better maintainability
Centralized configuration
Unit testing benefit

You can inject:

FakePaymentRepository

instead of hitting a real database.

Important interview distinction

DIP = principle

DI = implementation technique

You can say:

"Dependency Inversion is the architectural principle, while Dependency Injection is one of the common techniques used to implement it."


# 7. Repository Pattern
Definition

Repository provides an abstraction over data access and separates business logic from persistence logic.

Example:

public interface IPaymentRepository
{
    Task<Payment?> GetByIdAsync(int id);
    Task AddAsync(Payment payment);
    Task UpdateAsync(Payment payment);
}

Implementation:

public class PaymentRepository : IPaymentRepository
{
    private readonly PaymentDbContext _context;

    public PaymentRepository(PaymentDbContext context)
    {
        _context = context;
    }

    public async Task<Payment?> GetByIdAsync(int id)
    {
        return await _context.Payments
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task AddAsync(Payment payment)
    {
        await _context.Payments.AddAsync(payment);
        await _context.SaveChangesAsync();
    }
}

Business service:

public class PaymentService
{
    private readonly IPaymentRepository _repository;

    public PaymentService(IPaymentRepository repository)
    {
        _repository = repository;
    }
}
Corpay scenario

Imagine:

Payment API
     ↓
Payment Service
     ↓
Payment Repository
     ↓
SQL Server

The service shouldn't contain:

SELECT ...
INSERT ...
UPDATE ...

That responsibility belongs to the data-access layer.

Interview caveat

Don't blindly say:

"Repository is always required with EF Core."

EF Core's DbContext and DbSet already provide repository/unit-of-work-like behavior.

A strong answer:

"I use Repository when it provides a meaningful abstraction around persistence or when the architecture requires it. With EF Core, I avoid adding an unnecessary generic repository layer that only wraps DbSet methods."

That's a good senior-level answer.


# 8. Factory Pattern
Definition

Factory encapsulates object creation so the consumer doesn't need to know which concrete implementation to instantiate.

Payment example
public interface IPaymentProcessor
{
    Task ProcessAsync(Payment payment);
}

Implementations:

public class CardProcessor : IPaymentProcessor
{
    public Task ProcessAsync(Payment payment)
    {
        return Task.CompletedTask;
    }
}
public class AchProcessor : IPaymentProcessor
{
    public Task ProcessAsync(Payment payment)
    {
        return Task.CompletedTask;
    }
}

Factory:

public class PaymentProcessorFactory
{
    public IPaymentProcessor Create(string paymentType)
    {
        return paymentType switch
        {
            "CARD" => new CardProcessor(),
            "ACH" => new AchProcessor(),
            _ => throw new ArgumentException("Unsupported payment type")
        };
    }
}

Consumer:

var processor =
    factory.Create(payment.PaymentType);

await processor.ProcessAsync(payment);

Instead of:

if (...)
    new CardProcessor();
else
    new AchProcessor();

being scattered throughout the application.

Corpay example

Payment method could determine processor:

Payment Type
     ↓
Factory
     ↓
 ┌───────────────┐
 │               │
Card           ACH
 │               │
Processor      Processor


# 12. CQRS
Definition

CQRS = Command Query Responsibility Segregation.

It means separating:

Commands

Change state.

CreatePayment
ProcessPayment
CancelPayment
RefundPayment
Queries

Read state.

GetPayment
GetPaymentHistory
GetPaymentStatus
Traditional approach

One service may do everything:

PaymentService
 ├── Create
 ├── Update
 ├── Delete
 ├── Get
 └── Search

CQRS separates them.

                 Payment API
                     |
          ┌──────────┴──────────┐
          ↓                     ↓
      Commands                Queries
          ↓                     ↓
  PaymentCommandHandler   PaymentQueryHandler
          ↓                     ↓
       Write DB             Read Model
Command
public record ProcessPaymentCommand(
    decimal Amount,
    string AccountNumber);

Handler:

public class ProcessPaymentHandler
{
    public async Task Handle(
        ProcessPaymentCommand command)
    {
        // Validate
        // Process payment
        // Save payment
    }
}
Query
public record GetPaymentQuery(Guid PaymentId);

Handler:

public class GetPaymentHandler
{
    public async Task<PaymentDto> Handle(
        GetPaymentQuery query)
    {
        // Read payment
        return new PaymentDto();
    }
}
Why CQRS can help payments

Payment systems often have:

Writes

Process payment
Refund
Cancel
Update status

and huge numbers of:

Reads

Payment status
Payment history
Transaction search
Reports
Dashboard

Read and write workloads can have different scalability requirements.

Important

Don't say:

"CQRS means two databases."

That's not necessarily true.

CQRS primarily means:

separating command and query responsibilities.

Separate databases/read models are an architectural option.


# Principle	Payment example #
SRP	Separate validation, processing, persistence and notification
OCP	Add new payment processor without modifying existing processors
LSP	Every payment processor must honor IPaymentProcessor contract
ISP	Separate payment/refund/report interfaces
DIP	Service depends on IPaymentRepository, not SQL implementation
DI	ASP.NET Core injects repository/payment services
Repository	Encapsulates persistence
Factory	Selects/creates appropriate payment processor
Strategy	Different payment/fee/routing algorithms
Decorator	Add logging/retry/audit around payment processing
Adapter	Normalize different external payment provider APIs
CQRS	Separate payment commands from payment queries
Mediator	Decouple controller from command/query handlers


**Very Important Interview Questions**
# Q1. What's the difference between Factory and Strategy?

Answer:

"Factory is mainly concerned with object creation. Strategy is concerned with selecting behavior or an algorithm. For example, a PaymentProcessorFactory can create a Card or ACH processor, while a fee calculation strategy can determine how the fee is calculated for each payment type."

# Q2. What's the difference between Adapter and Decorator?

Answer:

"Adapter changes the interface of an existing component so it can work with our application. Decorator keeps the same interface but adds additional behavior."

Easy way:

Adapter:
Different interface → Compatible interface

Decorator:
Same interface → Additional behavior
# Q3. DI vs Dependency Inversion?

"Dependency Inversion is a SOLID design principle where high-level code depends on abstractions. Dependency Injection is a technique for supplying those dependencies from outside the class."

# Q4. Why use Repository?

"Repository provides an abstraction over persistence and keeps database-specific logic away from business logic. However, with EF Core, I wouldn't introduce a repository purely as a wrapper around DbSet unless the abstraction provides real value."

# Q5. Why CQRS?

"CQRS separates read and write responsibilities. In a payment system, processing transactions and querying transaction history can have different performance, scalability and consistency requirements, so separating those responsibilities can make the system easier to scale and evolve."

# Q6. Why Mediator?

"Mediator reduces direct dependencies between controllers and application components. The controller sends a command or query to the mediator, which routes it to the appropriate handler. This works particularly well with CQRS."


# 1. Singleton Pattern
Definition

Ensures that only one instance of a class exists and provides a common access point to it.

Example
public sealed class PaymentConfiguration
{
    private static readonly PaymentConfiguration _instance =
        new PaymentConfiguration();

    private PaymentConfiguration()
    {
    }

    public static PaymentConfiguration Instance => _instance;

    public string Currency { get; set; }
}

Usage:

var config = PaymentConfiguration.Instance;
Real payment example

You might have application-wide configuration:

PaymentConfiguration
    ↓
Currency
Timeout
Provider URL
Retry settings
Feature flags
But in ASP.NET Core

Don't immediately implement Singleton manually.

You can use:

builder.Services.AddSingleton<PaymentConfiguration>();
Interview point

"Singleton can be useful for stateless shared services or configuration, but I need to be careful because shared mutable state can create concurrency problems. In ASP.NET Core I would normally use the built-in DI container rather than manually implementing Singleton."


# 5. Observer Pattern ⭐
Definition

When an object's state changes, interested subscribers are notified automatically.

Payment systems are event-heavy.

Example:

Payment Completed
       ↓
 ┌─────┼──────────┐
 ↓     ↓          ↓
Email  Audit    Analytics

The payment service doesn't need to directly call all of them.

Example:

public interface IPaymentObserver
{
    Task OnPaymentCompleted(Payment payment);
}

Observers:

public class EmailObserver : IPaymentObserver
{
    public Task OnPaymentCompleted(Payment payment)
    {
        // Send email
        return Task.CompletedTask;
    }
}
public class AuditObserver : IPaymentObserver
{
    public Task OnPaymentCompleted(Payment payment)
    {
        // Write audit log
        return Task.CompletedTask;
    }
}
Real distributed architecture

Instead of in-memory Observer, a payment system may use:

Payment Service
      ↓
Event
      ↓
AWS SQS / SNS / EventBridge
      ↓
 ┌────┼───────┐
 ↓    ↓       ↓
Audit Email Analytics

This becomes event-driven architecture.


# 1. KISS
Full form

KISS = Keep It Simple, Stupid

More professionally, you can say:

Keep It Simple and Straightforward.

Definition

KISS means:

Prefer the simplest solution that correctly solves the problem.

The idea is to avoid unnecessary complexity.

Bad example

Suppose we only need to calculate a payment fee:

public decimal CalculateFee(decimal amount)
{
    return amount * 0.02m;
}

Someone creates:

PaymentFeeFacade
       ↓
PaymentFeeFactory
       ↓
PaymentFeeStrategyFactory
       ↓
PaymentFeeStrategy
       ↓
FeeCalculationService
       ↓
FeeRepository

For a simple 2% calculation, this is excessive.

KISS approach
public decimal CalculateFee(decimal amount)
{
    return amount * 0.02m;
}

If the business becomes more complicated later, then introduce Strategy/Factory/etc.

KISS in a Corpay Payment System

Imagine you need:

POST /payments

The requirement is simply:

Validate payment
      ↓
Process payment
      ↓
Save result

Don't immediately create:

CQRS
Mediator
Factory
Strategy
Repository
Decorator
Adapter
Event Bus
Saga
Microservice

just because these are available.

Instead, start with the simplest architecture that satisfies the requirement.

Interview answer

"I follow KISS by avoiding unnecessary abstractions and design patterns. I choose the simplest maintainable solution that satisfies the current business requirement and introduce additional complexity when there is a real need."

# 2. YAGNI
Full form

YAGNI = You Aren't Gonna Need It

Definition

Don't implement functionality until it is actually required.

This is especially important in software development.

Example

Suppose Corpay currently supports:

ACH
Credit Card

You don't need to build:

ACH
Credit Card
Wire
Virtual Card
Crypto
Apple Pay
Google Pay
FuturePaymentType

just because they might be needed someday.

Implement the requirements you actually have.

Bad example

Current requirement:

Process ACH payments.

Developer creates:

public interface IPaymentProcessor
{
    void ProcessAch();
    void ProcessCard();
    void ProcessWire();
    void ProcessCrypto();
    void ProcessFuturePayment();
}

Most of these aren't required.

That's a YAGNI violation.

Better
public interface IPaymentProcessor
{
    Task ProcessAsync(Payment payment);
}

Implement what's actually needed.

When a new payment type becomes a real requirement, extend the design appropriately.


# KISS vs YAGNI

This is a useful interview distinction.

Principle	Meaning
KISS	Don't make the solution unnecessarily complicated
YAGNI	Don't build functionality that isn't currently required
Simple way to remember

KISS → Don't overcomplicate.

YAGNI → Don't overbuild.



# S1. DRY — Don't Repeat Yourself

Avoid duplicating the same business logic in multiple places.

Bad:

// Controller 1
if (amount > 10000)
    throw new Exception();

// Controller 2
if (amount > 10000)
    throw new Exception();

Better:

public class PaymentValidator
{
    public void Validate(Payment payment)
    {
        if (payment.Amount > 10000)
            throw new Exception("Limit exceeded");
    }
}

Interview phrase:

"I avoid duplication of business rules by extracting reusable logic into appropriate services or components."


# Design Patterns Developer Cheat Sheet

## Related Design Patterns and Important Features

| Pattern or principle | Important feature to remember | Common development service use |
| --- | --- | --- |
| SOLID | Design guidance for maintainability, loose coupling, and focused responsibilities | Shape service, domain, and repository boundaries |
| Single Responsibility | A class should have one reason to change | Separate validation, payment processing, persistence, and notification |
| Open/Closed | Extend behavior without modifying stable code | Add payment or notification strategies through new implementations |
| Liskov Substitution | Implementations must honor the abstraction contract | Replace providers without breaking callers |
| Interface Segregation | Prefer small client-specific interfaces | Keep consumers dependent only on required operations |
| Dependency Inversion | High-level policy depends on abstractions | Inject repositories, gateways, and external service clients |
| Strategy | Encapsulate interchangeable algorithms behind an interface | Select payment, pricing, routing, or retry behavior at runtime |
| Factory Method | Delegate object creation to a method or factory | Create provider-specific clients or domain objects |
| Abstract Factory | Create related objects as a compatible family | Build complete provider-specific integrations |
| Builder | Construct complex objects step by step | Create requests, reports, configuration, or immutable objects |
| Singleton | One shared instance for a process or scope | Use sparingly for stateless infrastructure; prefer DI singleton registration |
| Adapter | Convert one interface into another expected by clients | Wrap legacy APIs, SDKs, or third-party providers |
| Facade | Provide a simpler interface over multiple components | Expose one application service over several domain or infrastructure services |
| Decorator | Add behavior without changing the wrapped implementation | Add logging, caching, authorization, metrics, or retry behavior |
| Proxy | Control access to another object or service | Lazy loading, access checks, remote calls, and caching |
| Composite | Treat individual and groups uniformly | Build trees, menus, permission groups, or workflow steps |
| Repository | Abstract persistence operations | Encapsulate database queries and aggregate persistence decisions |
| Unit of Work | Coordinate changes and commit them together | Save related entity changes in one transaction |
| Specification | Encapsulate reusable business predicates or queries | Compose filtering rules for search and domain validation |
| Chain of Responsibility | Pass a request through ordered handlers | Middleware, validation pipelines, authorization, and approvals |
| Command | Represent an action as an object | Queue work, audit commands, retry operations, or undo actions |
| Mediator | Centralize communication between components | Reduce controller-to-service coupling and implement request pipelines |
| Observer / Pub-Sub | Notify many subscribers when state or events change | Domain events, notifications, messaging, and cache invalidation |
| Template Method | Define an algorithm skeleton with customizable steps | Standardize workflows while allowing provider-specific steps |
| State | Change behavior based on current state | Payment lifecycle, order status, or workflow transitions |
| State Machine | Make valid transitions explicit | Prevent invalid payment or approval status changes |
| Retry with Backoff | Retry transient failures with delay and jitter | External APIs, queues, and temporary infrastructure failures |
| Circuit Breaker | Fail fast when a dependency is unhealthy | Protect services from cascading failures |
| Bulkhead | Isolate resource pools or failure domains | Prevent one dependency from consuming all threads or connections |
| Outbox | Store an event with the transaction before publishing it | Prevent lost integration events after database commits |
| Saga | Coordinate a distributed transaction with compensating actions | Multi-service payment or order workflows |
| Strangler Fig | Replace a legacy system incrementally | Move endpoints or capabilities to a new service safely |

## Development Service Mapping

| Development requirement | Useful pattern or principle | Important feature |
| --- | --- | --- |
| HTTP request pipeline | Chain of Responsibility, Decorator | Order matters; short-circuit unauthorized or invalid requests |
| Dependency injection | Dependency Inversion, Factory | Prefer constructor injection and explicit lifetimes |
| Database access | Repository, Unit of Work, Specification | Keep query and transaction decisions close to persistence boundaries |
| External API integration | Adapter, Facade, Decorator, Circuit Breaker | Hide vendor-specific contracts and protect the application from outages |
| Multiple payment providers | Strategy, Factory, Adapter | Add providers without changing the payment workflow |
| Background processing | Command, Queue, Consumer | Make work idempotent because delivery may be repeated |
| Notifications | Observer, Pub-Sub, Outbox | Decouple email, SMS, and push delivery from the main transaction |
| Multi-step workflow | State Machine, Saga, Orchestrator | Define retries, compensation, and valid transitions explicitly |
| Caching | Proxy, Decorator, Cache-Aside | Define expiration and invalidation rules before caching |
| Logging and metrics | Decorator, Middleware, Observer | Add telemetry without placing logging in every business method |
| Authorization | Chain of Responsibility, Policy, Specification | Keep authorization rules centralized and testable |
| Testing | Dependency Inversion, Factory, Builder, Test Double | Replace external services with deterministic fakes or mocks |
| Legacy migration | Adapter, Facade, Strangler Fig | Introduce a stable boundary before replacing internals |

## Important Design and Resource Limits

Patterns do not remove runtime limits. These are practical constraints to consider when applying them; exact quotas depend on the runtime, database, hosting platform, and provider.

| Resource or concern | Important limit or risk | Interview point |
| --- | --- | --- |
| Singleton state | One instance may be shared by all requests in a process | Avoid mutable state and ensure thread safety |
| Scoped service | Usually one instance per request scope | Do not capture a scoped dependency in a singleton |
| Transient service | A new instance can be created for every resolution | Avoid expensive construction and excessive allocations |
| Decorator depth | No fixed limit, but many nested decorators increase latency and debugging cost | Keep the chain intentional and observable |
| Middleware/pipeline length | No universal limit | Order security, exception handling, routing, and telemetry deliberately |
| Strategy count | No fixed limit | Use a registry or keyed factory when implementations become numerous |
| Factory complexity | A large conditional factory becomes a maintenance bottleneck | Split creation by bounded context or use registration-based lookup |
| Repository queries | No fixed count, but unbounded queries cause memory and database pressure | Paginate, project, and avoid loading entire tables |
| Unit of Work size | Large transactions hold locks and tracked entities longer | Keep transactions short and commit bounded batches |
| Observer subscribers | More subscribers increase fanout work and failure handling | Use durable messaging when delivery cannot be lost |
| In-process events | Lost when the process crashes or restarts | Use an outbox or durable broker for important events |
| Queue consumers | Concurrency is limited by CPU, connections, downstream capacity, and quotas | Scale gradually and apply backpressure |
| Retry attempts | Unlimited retries can create retry storms | Use maximum attempts, exponential backoff, jitter, and a DLQ |
| Circuit breaker state | Incorrect thresholds can hide recovery or cause unnecessary failures | Tune failure ratio, open duration, and half-open probes |
| Cache size | Limited by process memory or cache configuration | Set expiration, eviction, and maximum size policies |
| Object graph depth | Deep graphs increase serialization and mapping cost | Prefer DTOs and prevent circular serialization |
| Event payload size | Broker and transport-specific limits apply | Keep events small and store large data externally |
| Thread pool usage | Blocking work can exhaust available worker threads | Use async I/O and bound CPU-intensive work |
| Connection pools | Database and HTTP pools have provider-specific limits | Reuse clients, dispose correctly, and avoid unbounded parallelism |
| Distributed locks | Leaked or long-held locks block other work | Use expiry, ownership, and a recovery strategy |
| Saga compensation | Compensation is not the same as a database rollback | Make each step and compensating action idempotent |
| Shared mutable state | Race conditions increase with concurrent requests | Prefer immutability, synchronization, or actor/message ownership |

## Quick Pattern Selection Rules

| Problem | Prefer |
| --- | --- |
| Choose one algorithm at runtime | Strategy |
| Hide complex object creation | Factory or Builder |
| Wrap an incompatible external API | Adapter |
| Simplify several services behind one entry point | Facade |
| Add logging, caching, or metrics around a service | Decorator |
| Process a request through ordered checks | Chain of Responsibility |
| Publish one event to many consumers | Pub-Sub or Observer |
| Track valid lifecycle transitions | State Machine |
| Coordinate multiple service operations | Saga or Orchestrator |
| Guarantee an event is not lost after a database commit | Transactional Outbox |
| Protect a failing dependency | Circuit Breaker plus Timeout |
| Prevent one dependency from exhausting resources | Bulkhead plus bounded concurrency |
| Isolate persistence from business logic | Repository or ports-and-adapters boundary |
| Replace dependencies in tests | Dependency Inversion plus Test Doubles |
| Replace a legacy capability gradually | Strangler Fig |
| Avoid premature abstractions | KISS and YAGNI |