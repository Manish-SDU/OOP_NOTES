# README - Guide to Unit Testing with xUnit in C#

## 1. Project Setup
- Create a test project:
  ```bash
  dotnet new xunit -n MyApp.Tests
  cd MyApp.Tests
  ```
- Add reference to the project under test:
  ```bash
  dotnet add reference ../MyApp/MyApp.csproj
  ```
- Install NuGet packages:
  ```bash
  dotnet add package xunit.runner.visualstudio
  dotnet add package Moq
  ```

## 2. Basic xUnit Test Structure
```csharp
using Xunit;

public class CalculatorTests
{
    [Fact]
    public void Add_TwoPositiveNumbers_ReturnsSum()
    {
        // Arrange
        // Act
        // Assert
    }
}
```
- `[Fact]`: single test.
- `[Theory]` + `[InlineData]`: parameterized tests.

## 3. Arrange - Act - Assert (AAA) Pattern
1. **Arrange**  
   Prepare the SUT, inputs, and mocks.
2. **Act**  
   Call the method under test.
3. **Assert**  
   Verify the expected result.

## 4. Example Test Class
```csharp
using Xunit;
using Moq;

public class OrderServiceTests
{
    [Fact]
    public void PlaceOrder_ValidOrder_CallsRepositoryAndReturnsTrue()
    {
        // Arrange
        var mockRepo = new Mock<IOrderRepository>();
        var service  = new OrderService(mockRepo.Object);
        var order    = new Order { Id = 1, Total = 100m };

        // Act
        bool result = service.PlaceOrder(order);

        // Assert
        mockRepo.Verify(r => r.Save(order), Times.Once);
        Assert.True(result);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-10)]
    public void PlaceOrder_InvalidTotal_ThrowsArgumentException(decimal invalidTotal)
    {
        // Arrange
        var mockRepo = new Mock<IOrderRepository>();
        var service  = new OrderService(mockRepo.Object);
        var order    = new Order { Id = 1, Total = invalidTotal };

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => service.PlaceOrder(order));
        Assert.Contains("Total must be positive", ex.Message);
    }
}
```

## 5. Best Practices
- One behavior per test.
- Clear naming: `Method_State_ExpectedBehavior()`.
- Isolate dependencies with mocks/stubs.
- Use `IClassFixture<T>` for shared setup.
- Teardown with `IDisposable` or `IAsyncLifetime`.
- Parameterized tests with `[Theory]`.
- Cover edge cases (zero, negatives, exceptions).

## 6. Common Assert Methods and Explanations
- `Assert.Equal(expected, actual)`: Verifies that the actual value equals the expected value.
- `Assert.NotEqual(expected, actual)`: Verifies that the actual value does not equal the expected value.
- `Assert.True(condition)`: Verifies that a boolean condition is true.
- `Assert.False(condition)`: Verifies that a boolean condition is false.
- `Assert.Null(object)`: Verifies that the object is null.
- `Assert.NotNull(object)`: Verifies that the object is not null.
- `Assert.Same(expected, actual)`: Verifies that both references point to the same object instance.
- `Assert.NotSame(expected, actual)`: Verifies that the references point to different object instances.
- `Assert.Contains(substring, string)`: Verifies that the string contains the specified substring.
- `Assert.DoesNotContain(substring, string)`: Verifies that the string does not contain the specified substring.
- `Assert.Contains(item, collection)`: Verifies that the collection contains the specified item.
- `Assert.DoesNotContain(item, collection)`: Verifies that the collection does not contain the specified item.
- `Assert.StartsWith(prefix, string)`: Verifies that the string starts with the given prefix.
- `Assert.EndsWith(suffix, string)`: Verifies that the string ends with the given suffix.
- `Assert.InRange(value, low, high)`: Verifies that a value is within the specified inclusive range.
- `Assert.NotInRange(value, low, high)`: Verifies that a value is outside the specified inclusive range.
- `Assert.Single(collection)`: Verifies that the collection contains exactly one element.
- `Assert.Empty(collection)`: Verifies that the collection is empty.
- `Assert.NotEmpty(collection)`: Verifies that the collection is not empty.
- `Assert.Collection(collection, params Action<T>[] inspectors)`: Verifies both the order and content of items with provided inspectors.
- `Assert.All(collection, Action<T> action)`: Applies an action to every element and verifies no exceptions are thrown.
- `Assert.Throws<TException>(() => method())`: Verifies that the method throws a specific exception.
- `Assert.ThrowsAsync<TException>(async () => await methodAsync())`: Verifies that an async method throws a specific exception.
- `Assert.IsType<T>(object)`: Verifies that the object is exactly of the specified type.
- `Assert.IsNotType<T>(object)`: Verifies that the object is not of the specified type.
