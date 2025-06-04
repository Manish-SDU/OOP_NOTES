# SOLID Principles

## 1. Single Responsibility Principle (SRP)

**Definition:** A class should have only one reason to change, meaning it should have only one responsibility.

**How to identify violations:** Look for classes that perform multiple unrelated tasks. For example, a `User` class that handles both user authentication and generating PDF reports.

```csharp
// Violation of SRP
public class User
{
    public void SaveToDatabase() => Console.WriteLine("Saving user to database...");
    public void GenerateReport() => Console.WriteLine("Generating user report..."); 
    public bool Authenticate(string password) => true;
    public void SendEmail(string message) => Console.WriteLine($"Sending email: {message}");
}

// Following SRP
public class User
{
    public void SaveToDatabase() => Console.WriteLine("Saving user to database...");
}

public class ReportGenerator
{
    public void GenerateUserReport(User user) => Console.WriteLine("Generating user report...");
}

public class AuthenticationService
{
    public bool Authenticate(User user, string password) => true;
}

public class EmailService
{
    public void SendEmail(string recipient, string message) => Console.WriteLine($"Sending email: {message}");
}
```

## 2. Open/Closed Principle (OCP)

**Definition:** Software entities should be open for extension but closed for modification.

**How to identify violations:** When adding new functionality requires changing existing code rather than just extending it. For example, a `PaymentProcessor` class that needs to be modified every time a new payment method is added.

```csharp
// Violation of OCP
public class PaymentProcessor
{
    public void ProcessPayment(string paymentType, double amount)
    {
        if (paymentType == "CreditCard")
        {
            ProcessCreditCard(amount);
        }
        else if (paymentType == "PayPal")
        {
            ProcessPayPal(amount);
        }
        // Need to modify this class when adding new payment methods
    }

    private void ProcessCreditCard(double amount) => Console.WriteLine($"Processing credit card payment: ${amount}");
    private void ProcessPayPal(double amount) => Console.WriteLine($"Processing PayPal payment: ${amount}");
}

// Following OCP
public abstract class PaymentProcessor
{
    public abstract void ProcessPayment(double amount);
}

public class CreditCardProcessor : PaymentProcessor
{
    public override void ProcessPayment(double amount) => Console.WriteLine($"Processing credit card payment: ${amount}");
}

public class PayPalProcessor : PaymentProcessor
{
    public override void ProcessPayment(double amount) => Console.WriteLine($"Processing PayPal payment: ${amount}");
}

// Adding a new payment method doesn't require changing existing code
public class BitcoinProcessor : PaymentProcessor
{
    public override void ProcessPayment(double amount) => Console.WriteLine($"Processing Bitcoin payment: ${amount}");
}
```

## 3. Liskov Substitution Principle (LSP) -> TOO COMPLEX

**Definition:** Objects of a superclass should be replaceable with objects of its subclasses without affecting the correctness of the program.

**How to identify violations:** When a subclass doesn't work correctly when used in place of its parent class. For example, a `Square` class that inherits from `Rectangle` but breaks assumptions about independent width and height.

```csharp
// Violation of LSP
public class Rectangle
{
    public virtual int Width { get; set; }
    public virtual int Height { get; set; }

    public int CalculateArea() => Width * Height;
}

public class Square : Rectangle
{
    private int _side;
    
    public override int Width
    {
        get => _side;
        set
        {
            _side = value;
            Height = value; // Forces Height to change when Width changes
        }
    }
    
    public override int Height
    {
        get => _side;
        set
        {
            _side = value;
            Width = value; // Forces Width to change when Height changes
        }
    }
}

// This breaks client code that expects to work with Rectangle
public void ClientCode(Rectangle rectangle)
{
    rectangle.Width = 5;
    rectangle.Height = 10;
    
    // With a Rectangle, area should be 50
    // With a Square, area will be 100 (unexpected!)
    Console.WriteLine($"Area: {rectangle.CalculateArea()}");
}

// Following LSP
public interface IShape
{
    int CalculateArea();
}

public class Rectangle : IShape
{
    public int Width { get; set; }
    public int Height { get; set; }
    
    public int CalculateArea() => Width * Height;
}

public class Square : IShape
{
    public int Side { get; set; }
    
    public int CalculateArea() => Side * Side;
}
```

## 4. Interface Segregation Principle (ISP)

**Definition:** Clients should not be forced to depend on interfaces they do not use.

**How to identify violations:** Large interfaces that force implementing classes to provide methods they don't need. For example, a single `Worker` interface that has both `work()` and `eat()` methods when some workers don't need to eat.

```csharp
// Violation of ISP
public interface IWorker
{
    void Work();
    void Eat();
}

public class Human : IWorker
{
    public void Work() => Console.WriteLine("Working...");
    public void Eat() => Console.WriteLine("Eating lunch...");
}

public class Robot : IWorker
{
    public void Work() => Console.WriteLine("Working efficiently...");
    public void Eat() => throw new NotImplementedException("Robots don't eat!"); // Forced to implement unnecessary method
}

// Following ISP
public interface IWorkable
{
    void Work();
}

public interface IEatable
{
    void Eat();
}

public class Human : IWorkable, IEatable
{
    public void Work() => Console.WriteLine("Working...");
    public void Eat() => Console.WriteLine("Eating lunch...");
}

public class Robot : IWorkable
{
    public void Work() => Console.WriteLine("Working efficiently...");
    // No need to implement Eat()
}
```

## 5. Dependency Inversion Principle (DIP)

**Definition:** High-level modules should not depend on low-level modules; both should depend on abstractions.

**How to identify violations:** Direct instantiation of concrete classes rather than depending on interfaces or abstract classes. For example, a `NotificationService` directly instantiating an `EmailSender` rather than accepting an `INotifier` interface.

```csharp
// Violation of DIP
public class NotificationService
{
    private readonly EmailSender _emailSender;
    
    public NotificationService()
    {
        _emailSender = new EmailSender(); // Direct dependency on concrete class
    }
    
    public void Notify(string message)
    {
        _emailSender.SendEmail(message);
    }
}

public class EmailSender
{
    public void SendEmail(string message) => Console.WriteLine($"Sending email: {message}");
}

// Following DIP
public interface INotifier
{
    void Send(string message);
}

public class EmailSender : INotifier
{
    public void Send(string message) => Console.WriteLine($"Sending email: {message}");
}

public class SMSSender : INotifier
{
    public void Send(string message) => Console.WriteLine($"Sending SMS: {message}");
}

public class NotificationService
{
    private readonly INotifier _notifier;
    
    public NotificationService(INotifier notifier)
    {
        _notifier = notifier; // Dependency on abstraction, not concrete class
    }
    
    public void Notify(string message)
    {
        _notifier.Send(message);
    }
}

// Usage
var emailService = new NotificationService(new EmailSender());
var smsService = new NotificationService(new SMSSender());
```
