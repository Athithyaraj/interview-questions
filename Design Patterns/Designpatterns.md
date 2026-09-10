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