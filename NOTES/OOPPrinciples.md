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

## Object Relationships

## 1. Composition

**Definition:** A "whole-part" relationship where the part cannot exist without the whole. When the whole is destroyed, its parts are also destroyed.

**Real-world Example:** A House and its Rooms - Rooms cannot exist without the house.

```csharp
public class Room
{
    public string Name { get; set; }
    public int SquareFootage { get; set; }

    public Room(string name, int squareFootage)
    {
        Name = name;
        SquareFootage = squareFootage;
    }
}

public class House
{
    private List<Room> _rooms;

    public House()
    {
        _rooms = new List<Room>();
    }

    public void AddRoom(string name, int squareFootage)
    {
        _rooms.Add(new Room(name, squareFootage));
    }

    // If House is destroyed, all its Rooms cease to exist
}

// Usage example:
House myHouse = new House();
myHouse.AddRoom("Living Room", 300);
myHouse.AddRoom("Bedroom", 200);
```

## 2. Aggregation

**Definition:** A "whole-part" relationship where the part can exist independently of the whole. Parts can be shared among multiple wholes.

**Real-world Example:** A Library and Books - Books can exist without a specific library.

```csharp
public class Book
{
    public string Title { get; set; }
    public string Author { get; set; }

    public Book(string title, string author)
    {
        Title = title;
        Author = author;
    }
}

public class Library
{
    private List<Book> _books;

    public Library()
    {
        _books = new List<Book>();
    }

    public void AddBook(Book book)
    {
        _books.Add(book);
    }

    public void RemoveBook(Book book)
    {
        _books.Remove(book);
    }

    // Books can exist independently, even if the library is destroyed
}

// Usage example:
Book book1 = new Book("Clean Code", "Robert Martin");
Book book2 = new Book("Design Patterns", "Gang of Four");

Library library1 = new Library();
library1.AddBook(book1);
library1.AddBook(book2);

// The same book could be in multiple libraries
Library library2 = new Library();
library2.AddBook(book1);
```
