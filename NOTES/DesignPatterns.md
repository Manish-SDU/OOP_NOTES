# Design Patterns and OOP Principles

## Quick Pattern Identification Guide

| Pattern | Key Characteristics | When to Use |
|---------|-------------------|------------|
| **Singleton** | Private constructor, static instance, static accessor | When exactly one instance of a class is needed |
| **Factory Method** | Abstract creator class, concrete subclasses creating objects | When object creation logic should be separated from usage |
| **Facade** | Simple interface to complex subsystem | When you need to simplify and unify a complex system |
| **Strategy** | Multiple interchangeable algorithms, behavior injection | When you have multiple ways to perform an operation |
| **Adapter** | Wrapper that makes incompatible interfaces work together | When interfaces don't match but need to work together |
| **Command** | Encapsulates requests as objects | When you need to parameterize, queue, or log operations |

## Design Patterns

### 1. Singleton Pattern

**Purpose:** Ensures only one instance of a class exists and provides a global point of access to it.

```csharp
public class Singleton
{
    private static Singleton _instance;
    private static readonly object _lock = new object();

    private Singleton() { }

    public static Singleton Instance
    {
        get
        {
            lock (_lock)
            {
                return _instance ??= new Singleton();
            }
        }
    }

    public void DoSomething() => Console.WriteLine("Singleton in action!");
}
```

**How to recognize in code:**

- Private constructor
- Static instance
- Static accessor method (Instance)

### 2. Factory Method Pattern

**Purpose:** Delegates the creation of objects to subclasses.

```csharp
public abstract class Product
{
    public abstract void Show();
}

public class ConcreteProduct : Product
{
    public override void Show() => Console.WriteLine("ConcreteProduct created");
}

public abstract class Creator
{
    public abstract Product FactoryMethod();
}

public class ConcreteCreator : Creator
{
    public override Product FactoryMethod() => new ConcreteProduct();
}
```

**How to recognize:**

- Abstract class with a FactoryMethod
- Subclasses override the method to return different types

### 3. Facade Pattern

**Purpose:** Simplifies a complex subsystem by providing a unified interface.

```csharp
public class SubsystemA
{
    public void OperationA() => Console.WriteLine("SubsystemA");
}

public class SubsystemB
{
    public void OperationB() => Console.WriteLine("SubsystemB");
}

public class Facade
{
    private readonly SubsystemA _a = new();
    private readonly SubsystemB _b = new();

    public void Operation() {
        _a.OperationA();
        _b.OperationB();
    }
}
```

**How to recognize:**

- A wrapper class calls multiple subsystem classes
- Client only interacts with the wrapper

### 4. Strategy Pattern

**Purpose:** Defines a family of algorithms, encapsulates each, and makes them interchangeable.

```csharp
public interface IStrategy
{
    void Execute();
}

public class StrategyA : IStrategy
{
    public void Execute() => Console.WriteLine("Strategy A");
}

public class StrategyB : IStrategy
{
    public void Execute() => Console.WriteLine("Strategy B");
}

public class Context
{
    private IStrategy _strategy;

    public Context(IStrategy strategy) => _strategy = strategy;

    public void SetStrategy(IStrategy strategy) => _strategy = strategy;

    public void DoWork() => _strategy.Execute();
}
```

**How to recognize:**

- Interface or abstract class for a behavior
- Multiple implementations injected into a context class

### 5. Adapter Pattern

**Purpose:** Converts the interface of a class into another interface clients expect.

```csharp
// Adaptee (incompatible)
public class OldSystem
{
    public void SpecificRequest() => Console.WriteLine("Old system");
}

// Target interface
public interface INewSystem
{
    void Request();
}

// Adapter
public class Adapter : INewSystem
{
    private readonly OldSystem _oldSystem = new();

    public void Request() => _oldSystem.SpecificRequest();
}
```

**How to recognize:**

- Adapts one interface to another
- Uses composition or inheritance

### 6. Command Pattern

**Purpose:** Encapsulates a request as an object, letting you parameterize clients with commands.

```csharp
public interface ICommand
{
    void Execute();
}

public class Light
{
    public void On() => Console.WriteLine("Light On");
}

public class LightOnCommand : ICommand
{
    private readonly Light _light;

    public LightOnCommand(Light light) => _light = light;

    public void Execute() => _light.On();
}

public class RemoteControl
{
    private ICommand _command;

    public void SetCommand(ICommand command) => _command = command;
    public void PressButton() => _command.Execute();
}
```

**How to recognize:**

- Commands implement an interface
- There's an invoker (e.g., RemoteControl) and a receiver (e.g., Light)
