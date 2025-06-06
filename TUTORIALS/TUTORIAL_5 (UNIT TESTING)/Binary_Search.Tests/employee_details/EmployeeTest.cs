using Binary_Search.employee_details;

namespace Binary_Search.Tests.employee_details;

public class EmployeeTest
{   
    [Fact]
    public void TestCalculateAppraisal()
    {
        //Arrange
        Employee employee = new ("John", 8000, 25);
        EmployeeBusinessLogic employeeBusinessLogic = new();

        //Act
        var employeeAppraisal = employeeBusinessLogic.CalculateAppraisal(employee);
        
        //Assert
        Assert.Equal(500, employeeAppraisal);
    }

    [Fact]
    public void TestCalculateYearlySalary()
    {
        //Arrange
        Employee employee = new ("John", 8000, 25);
        EmployeeBusinessLogic employeeBusinessLogic = new();

        //Act
        var yearlySalary = employeeBusinessLogic.CalculateYearlySalary(employee);
        var twelveMonthSalary=employee.MonthlySalary * 12;

        //Assert
        Assert.Equal(twelveMonthSalary, yearlySalary);
    }
}