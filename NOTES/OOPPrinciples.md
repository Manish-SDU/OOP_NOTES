# Four OOP Principles

## 1. Encapsulation

**Definition:** Restrict access to internal data using access modifiers (like private) and expose behavior via methods.

```csharp
public class Account
{
    private decimal _balance;

    public void Deposit(decimal amount)
    {
        if (amount > 0) _balance += amount;
    }

    public decimal GetBalance() => _balance;
}
```

## 2. Abstraction

**Definition:** Hide complex implementation details, exposing only what's necessary.

```csharp
public abstract class Shape
{
    public abstract double Area();
}

public class Circle : Shape
{
    private double _radius;

    public Circle(double radius) => _radius = radius;

    public override double Area() => Math.PI * _radius * _radius;
}
```

## 3. Inheritance

**Definition:** One class inherits from another to reuse code.

```csharp
public class Animal
{
    public void Eat() => Console.WriteLine("Eating...");
}

public class Dog : Animal
{
    public void Bark() => Console.WriteLine("Barking...");
}
```

## 4. Polymorphism

**Definition:** Ability to treat objects of different subclasses through a common interface or base class.

```csharp
public class Animal
{
    public virtual void Speak() => Console.WriteLine("Animal sound");
}

public class Cat : Animal
{
    public override void Speak() => Console.WriteLine("Meow");
}

public class Dog : Animal
{
    public override void Speak() => Console.WriteLine("Woof");
}

// Usage example:
Animal myAnimal = new Cat();
myAnimal.Speak(); // Output: Meow
```
