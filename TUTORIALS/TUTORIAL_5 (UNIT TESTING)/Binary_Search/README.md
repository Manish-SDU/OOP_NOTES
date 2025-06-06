# Unit Testing:
You are handed over a C# project, comprising 2 folders, `employee_details` & `binary_search`

## Task a
Folder `employee_details` contains 2 classes, i.e., `Employee` and `EmployeeBusinessLogic`.

- Create a Test Class, `EmployeeTest` (should be placed in Binary_Search.Tests->employee_details folder)
- Write NUnit tests for the following methods in the test class:  
  - `TestCalculateAppraisal()`  // Test to check appraisal  
  - `TestCalculateYearlySalary()`   // Test to check yearly salary  
- You can use this Employee as test input:
  - `Employee _employee = new Employee("John", 8000, 25);`

## Task b

Folder `binary_search` contains 2 classes for BinarySearch in both a recursive- and an iterative-version.

- Create 2 Test Classes, one for IterativeBinSearch `IterativeBinSearchTest` and the other for 
RecursiveBinSearch `RecursiveBinSearchTest` (should be placed in Binary_Search.Tests->binary_search folder)
- Write NUnit tests for the following methods in both test classes:  
  - `ShouldFindIndexOfNumber()` // Find present number  
  - `ShouldReturnNegativeInsertionPointWhenNotFound()` // Find not-present number  
- You can use this array as test input:   
  - `private static readonly int[] Fibos = [1, 1, 2, 3, 5, 8, 13, 21, 34, 55];`