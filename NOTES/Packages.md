# .NET Development Resources

This README contains commands and packages for various .NET development scenarios.

## NuGet Packages

### Unit Testing

```bash
dotnet add package xunit
dotnet add package xunit.runner.visualstudio
dotnet add package Microsoft.NET.Test.Sdk
```

### Data Processing

```bash
# Reading and Handling JSON
dotnet add package System.Text.Json

# CSV Processing
dotnet add package CsvHelper
```

### AVALONIA UI Framework

```bash
# Core Packages
dotnet add package Avalonia
dotnet add package Avalonia.ReactiveUI

# Templates
dotnet new install Avalonia.Templates
dotnet new avalonia.mvvm

# Charts
dotnet add package LiveChartsCore.SkiaSharpView.Avalonia
```

#### Important XAML Namespace for LiveCharts

Add the following namespace to your XAML files when using LiveCharts:

```xaml

xmlns:lvc="clr-namespace:LiveChartsCore.SkiaSharpView.Avalonia;assembly=LiveChartsCore.SkiaSharpView.Avalonia"
```
